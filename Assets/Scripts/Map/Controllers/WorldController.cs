using UnityEngine;
using System.Collections.Generic;

public class WorldController : MonoBehaviour
{
    void Awake ()
    {
        textures = new Texture2D[] { Volcano, Snowfield, Marsh, Desert, Forest, Grassland, Sea, };
        
        sprites = new Sprite[materialTypeAmount];
        for (int i = 0; i < materialTypeAmount; ++i)
            sprites[i] = Sprite.Create(textures[i], new Rect(20, 20, CellWidth, CellHeight), Vector2.one / 2);

        int widthCount = WorldWidth / CellWidth, heightCount = WorldHeight / CellHeight;
        //to get random world
        worldData = new WorldRandomer(widthCount, heightCount, DistanceThreshold).WorldData;

        //to find the max and min x , y coordinates in respective landform
        Vector2[][] boundaries = new Vector2[materialTypeAmount][];
        for (int i = 0; i < materialTypeAmount; ++i)
        {
            boundaries[i] = new Vector2[4];
            for(int j = 0; j < 4; ++j)
            {
                //0 => max X, 1 => min X, 2 => max Y, 3 => min Y
                boundaries[i][0] = Vector2.left;
                boundaries[i][1] = Vector2.right * (widthCount + 1);
                boundaries[i][2] = Vector2.down;
                boundaries[i][3] = Vector2.up * (heightCount + 1);
            }
        }

        for(int i = 0; i < widthCount; ++i)
        {
            for(int j = 0; j < heightCount; ++j)
            {
                TileData td = worldData[i][j];
                Vector2 position = new Vector2(i, j);
                int type = (int)td.MaterialTypes[0];

                boundaries[type][0] = boundaries[type][0].x < i ? position : boundaries[type][0];
                boundaries[type][1] = boundaries[type][1].x > i ? position : boundaries[type][1];
                boundaries[type][2] = boundaries[type][2].y < j ? position : boundaries[type][2];
                boundaries[type][3] = boundaries[type][3].y > j ? position : boundaries[type][3];
            }
        }

        for(int i = 0; i < materialTypeAmount; ++i)
        {
            int reWidth = (int)(boundaries[i][0].x - boundaries[i][1].x), reHeight = (int)(boundaries[i][2].y - boundaries[i][3].y);
            ResizeCanvas(textures[i], reWidth * 100, reHeight * 100);

            /*
            int tWidth = textures[i].width, tHeight = textures[i].height;
            
            float times = reWidth / tWidth > reHeight / tHeight ? reWidth / tWidth : reHeight / tHeight;
            
            Debug.Log(textures[i].Resize(tWidth * ((int)times + 1), tHeight * ((int)times + 1)));
            textures[i].Apply();
            */
        }

        Vector2[] coordinates = new Vector2[materialTypeAmount];
        for(int i = 0; i < materialTypeAmount; ++i)
        {
            coordinates[i] = new Vector2(boundaries[i][1].x, boundaries[i][3].y);
            Debug.Log(coordinates[i]);
        }
        

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
                    sr.sprite = Sprite.Create(textures[(int)td.MaterialTypes[0]], new Rect((i - coordinates[(int)td.MaterialTypes[0]].x) * 100, (j - coordinates[(int)td.MaterialTypes[0]].y) * 100, CellWidth, CellHeight), Vector2.one / 2);
                    
                    //sr.sprite = sprites[(int)td.MaterialTypes[0]];
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

    //test
    public static Color32[] ResizeCanvas(Texture2D texture, int width, int height)
    {
        int tWidth = texture.width, tHeight = texture.height;
        float times = width / tWidth > height / tHeight ? width / tWidth : height / tHeight;

        var newPixels = ResizeCanvas(texture.GetPixels32(), tWidth, tHeight, tWidth * ((int)times + 1), tHeight * ((int)times + 1));
        
        texture.Resize(tWidth * ((int)times + 1), tHeight * ((int)times + 1));
        texture.SetPixels32(newPixels);
        texture.Apply();
        return newPixels;
    }

    private static Color32[] ResizeCanvas(IList<Color32> pixels, int oldWidth, int oldHeight, int width, int height)
    {
        var newPixels = new Color32[(width * height)];
        var wBorder = (width - oldWidth) / 2;
        var hBorder = (height - oldHeight) / 2;

        for (int r = 0; r < height; r++)
        {
            var oldR = r - hBorder;
            if (oldR < 0) { continue; }
            if (oldR >= oldHeight) { break; }

            for (int c = 0; c < width; c++)
            {
                var oldC = c - wBorder;
                if (oldC < 0) { continue; }
                if (oldC >= oldWidth) { break; }

                var oldI = oldR * oldWidth + oldC;
                var i = r * width + c;
                newPixels[i] = pixels[oldI];
            }
        }

        return newPixels;
    }

    public Transform WorldList;
    public Texture2D Volcano, Snowfield, Marsh, Desert, Forest, Grassland, Sea;
    public int WorldWidth, WorldHeight, CellWidth, CellHeight;
    public float DistanceThreshold;

    Sprite[] sprites;
    Texture2D[] textures;
    Transform[][] displayWorld;
    TileData[][] worldData;
    int materialTypeAmount = (int)MapConstants.LandformType.Sea + 1;
}