using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour
{
    void Awake ()
    {
        textures = new Texture2D[] { Volcano, Snowfield, Marsh, Desert, Forest, Grasslands, Vestige, Sea, };

        sprites = new Sprite[materialTypeAmount];
        for (int i = 0; i < materialTypeAmount; ++i)
            sprites[i] = Sprite.Create(textures[i], new Rect(20, 20, CellWidth, CellHeight), Vector2.one / 2);

        int widthCount = WorldWidth / CellWidth, heightCount = WorldHeight / CellHeight;
        //to get random world
        worldData = new WorldRandomer(widthCount, heightCount, DistanceThreshold).WorldData;
        
        //to display the random map
        float cellWidthInWC = CellWidth / 100f, cellHeightInWC = CellHeight / 100f, halfCellWidthInWC = cellWidthInWC / 2, halfCellHeightInWC = cellHeightInWC / 2;
        displayWorld = new Transform[widthCount][];

        for (int i = 0; i < widthCount; ++i)
        {
            displayWorld[i] = new Transform[heightCount];
            for (int j = 0; j < heightCount; ++j)
            {
                Transform tf = displayWorld[i][j] = new GameObject().transform;
                tf.parent = WorldList;
                tf.localScale = Vector3.one;
                //1 pixel = 0.01 in world coordinates
                //the local position is at center;
                tf.localPosition = new Vector3(i * cellWidthInWC + halfCellWidthInWC, j * cellHeightInWC + halfCellHeightInWC);

                TileData td = worldData[i][j];
                SpriteRenderer sr = tf.gameObject.AddComponent<SpriteRenderer>();
                tf.name = "Tile" + i + ", " + j;

                //Can an edge have two materialTypes only?
                if (td.MaterialTypes[1] == MapConstants.LandformType.Sea)
                {
                    //this is a simple materialType
                    sr.sprite = sprites[(int)td.MaterialTypes[0]];
                    tf.name += " " + td.MaterialTypes[0];
                }
                else
                {
                    //this is an edge
                    tf.name += " edge with " + td.MaterialTypes[0] + " and " + td.MaterialTypes[1];

                    //noise!!
                    float[][] noise = GenerateWhiteNoise(CellWidth, CellHeight);
                    float[][] perlinNoise = GeneratePerlinNoise(noise, 6);

                    //to blend two textures
                    Texture2D t0 = textures[(int)td.MaterialTypes[0]], t1 = textures[(int)td.MaterialTypes[1]], blendedimage = new Texture2D(CellWidth, CellHeight);
                    for (int k = 0; k < CellWidth; ++k)
                        for (int l = 0; l < CellHeight; ++l)
                            blendedimage.SetPixel(k, l, Interpolate(t0.GetPixel(k, l), t1.GetPixel(k, l), perlinNoise[k][l]));

                    blendedimage.Apply();
                    sr.sprite = Sprite.Create(blendedimage, new Rect(0, 0, CellWidth, CellHeight), Vector2.one / 2);
                }
            }
        }
    }
    
    float[][] GenerateWhiteNoise(int width, int height)
    {
        float[][] noise = new float[width][];
        for (int i = 0; i < width; ++i)
        {
            noise[i] = new float[height];
            for (int j = 0; j < height; ++j)
                noise[i][j] = Random.Range(0f, 1f);
        }

        return noise;
    }

    float[][] GenerateSmoothNoise(float[][] baseNoise, int octave)
    {
        int width = baseNoise.Length, height = baseNoise[0].Length;

        float[][] smoothNoise = new float[width][];

        int samplePeriod = 1 << octave;
        float sampleFrequency = 1f / samplePeriod;

        for (int i = 0; i < width; ++i)
        {
            smoothNoise[i] = new float[height];

            //to calculate the horizontal sampling indices
            int sample_i0 = (i / samplePeriod) * samplePeriod;
            int sample_i1 = (sample_i0 + samplePeriod) % width;
            float horizontal_blend = (i - sample_i0) * sampleFrequency;

            for (int j = 0; j < height; ++j)
            {
                //to calculate the vertical sampling indices
                int sample_j0 = (j / samplePeriod) * samplePeriod;
                int sample_j1 = (sample_j0 + samplePeriod) % height;
                float vertical_blend = (j - sample_j0) * sampleFrequency;

                //to blend the top of two corners
                float top = Interpolate(baseNoise[sample_i0][sample_j0], baseNoise[sample_i1][sample_j0], horizontal_blend);
                //to blend the bottom two corners
                float bottom = Interpolate(baseNoise[sample_i0][sample_j1], baseNoise[sample_i1][sample_j1], horizontal_blend);

                //final blend
                smoothNoise[i][j] = Interpolate(top, bottom, vertical_blend);
            }
        }

        return smoothNoise;
    }

    float Interpolate(float x0, float x1, float alpha)
    {
        return x0 * (1 - alpha) + x1 * alpha;
    }

    Color Interpolate(Color c0, Color c1, float alpha)
    {
        
        if (alpha > .5f)
            return c1;
        else
            return c0;
        
        //return c0 * (1 - alpha) + c1 * alpha;

        /*
        float u = 1f - alpha;
        return new Color(c0.r * u + c1.r * alpha, c0.g * u + c1.g * alpha, c0.b * u + c1.b * alpha);
        */
    }

    float[][] GeneratePerlinNoise(float[][] baseNoise, int octaveCount)
    {
        int width = baseNoise.Length, height = baseNoise[0].Length;

        float[][][] smoothNoise = new float[octaveCount][][];
        float persistance = .5f;

        //to generate smooth noise
        for (int i = 0; i < octaveCount; ++i)
            smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);

        float[][] perlinNoise = new float[width][];
        for(int i = 0; i < width; ++i)
            perlinNoise[i] = new float[height];

        float amplitude = 1f, totalAmplitude = 0f;

        //to blend noise together
        for (int octave = octaveCount - 1; octave >= 0; --octave)
        {
            amplitude *= persistance;
            totalAmplitude += amplitude;

            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                    perlinNoise[i][j] += smoothNoise[octave][i][j] * amplitude;
        }

        //normalization
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                perlinNoise[i][j] /= totalAmplitude;
            }
        }

        return perlinNoise;
    }
    
    public Transform WorldList;
    public Texture2D Volcano, Snowfield, Marsh, Desert, Forest, Grasslands, Vestige, Sea;
    public int WorldWidth, WorldHeight, CellWidth, CellHeight;
    public float DistanceThreshold;

    Sprite[] sprites;
    Texture2D[] textures;
    Transform[][] displayWorld;
    TileData[][] worldData;
    int materialTypeAmount = (int)MapConstants.LandformType.Sea;
}