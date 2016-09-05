using UnityEngine;
using System.Collections.Generic;
using System;

public class Test : MonoBehaviour
{
	void Awake ()
    {
        int widthCount = WorldWidth / CellWidth, heightCount = WorldHeight / CellHeight;

        //to get random world
        WorldRandomer wr = new WorldRandomer(widthCount, heightCount, DistanceThreshold);
        worldData = wr.WorldData;

        //to display the random map

        //something for new go
        Transform tf = ((GameObject)Resources.Load(@"Map\Tile")).transform;
        //SpriteRenderer sr;

        //to calculate how many go do this world have to
        float cellWidthInWC = CellWidth * .01f, cellHeightInWC = CellHeight * .01f, halfCellWidthInWC = cellWidthInWC * .5f, halfCellHeightInWC = cellHeightInWC * .5f;
        int displayWidth = SightWidth / CellWidth + 2, displayHeight = SightHeight / CellHeight + 2;
        float halfDisplayWidth = displayWidth / 2f, halfDisplayHeight = displayHeight / 2f;

        //to create the noise
        worldNoise = GenerateWhiteNoise(WorldWidth, WorldHeight);
        worldPerlinNoise = GeneratePerlinNoise(worldNoise, 6);

        //to display
        displayWorld = new Transform[displayWidth][];
        for (int i = 0; i < displayWidth; ++i)
            displayWorld[i] = new Transform[displayHeight];

        t = new Texture2D(WorldWidth, WorldHeight);

        for (int k = 0; k < WorldHeight; ++k)
        {
            for (int l = 0; l < WorldWidth; ++l)
            {
                t.SetPixel(l, k, ChooseColor(0, worldPerlinNoise[l][k]));
            }
        }
        t.Apply();
        BigTile.GetComponent<SpriteRenderer>().sprite = Sprite.Create(t, new Rect(0, 0, WorldWidth, WorldHeight), Vector2.zero);

        //to decide the detail of new go
        for (int i = 0; i < displayHeight; ++i)
        {
            for (int j = 0; j < displayWidth; ++j)
            {
                displayWorld[j][i] = tf = Instantiate(tf);
                tf.parent = Role;

                tf.localPosition = (j - halfDisplayWidth) * Vector3.right * cellWidthInWC  + (i - halfDisplayHeight) * Vector3.up * cellHeightInWC;
                tf.localScale = Vector3.one;
                tf.GetComponent<SpriteRenderer>().sprite = ChooseSprite1(tf.position.x, tf.position.y);
            }
        }

        for (int i = 0; i < displayHeight; ++i)
        {
            for (int j = 0; j < displayWidth; ++j)
            {
                tf = Instantiate(tf);
                tf.parent = Role1;

                tf.localPosition = (j - halfDisplayWidth) * Vector3.right * cellWidthInWC + (i - halfDisplayHeight) * Vector3.up * cellHeightInWC;
                tf.localScale = Vector3.one;
                tf.GetComponent<SpriteRenderer>().sprite = ChooseSprite(tf.position.x, tf.position.y);
            }
        }

    }

    Color ChooseColor(int lt, float value)
    {
        int index = -1;

        if (value < .2f)
            index = 0;
        else if (value < .35f)
            index = 1;
        else if (value < .5f)
            index = 2;
        else if (value < .65f)
            index = 3;
        else if (value < .8f)
            index = 4;
        else
            index = 5;

        return MapConstants.GrasslandColor[index];
    }

    Sprite ChooseSprite(float positionX, float positionY)
    {
        int pixelX = (int)(positionX * 100), pixelY = (int)(positionY * 100);
        int widthCount = pixelX / CellWidth, heightCount = pixelY / CellHeight;
        
        int type = (int)worldData[heightCount][widthCount].MaterialTypes[0];
        Vector3 position = worldData[heightCount][widthCount].Position;
        
        Texture2D t = new Texture2D(CellWidth, CellHeight);
        
        for (int k = 0; k < CellHeight; ++k)
        {
            for (int l = 0; l < CellWidth; ++l)
            {
                t.SetPixel(l, k, ChooseColor(type, worldPerlinNoise[(int)position.x * CellWidth + l][(int)position.y * CellHeight + k]));
            }
        }
        t.Apply();
        
        return Sprite.Create(t, new Rect(0, 0, CellWidth, CellHeight), Vector2.zero);
    }

    Sprite ChooseSprite1(float positionX, float positionY)
    {
        int pixelX = (int)(positionX * 100), pixelY = (int)(positionY * 100);
        int widthCount = pixelX / CellWidth, heightCount = pixelY / CellHeight;

        int type = (int)worldData[heightCount][widthCount].MaterialTypes[0];
        Vector3 position = worldData[heightCount][widthCount].Position;

        Texture2D t = new Texture2D(CellWidth, CellHeight);

        for (int k = 0; k < CellHeight; ++k)
        {
            for (int l = 0; l < CellWidth; ++l)
            {
                t.SetPixel(l, k, ChooseColor(type, worldPerlinNoise[(int)position.x * CellWidth + l][(int)position.y * CellHeight + k]));
            }
        }
        t.Apply();

        return Sprite.Create(t, new Rect(0, 0, CellWidth, CellHeight), Vector2.zero);
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
    }

    float[][] GenerateWhiteNoise(int width, int height)
    {
        float[][] noise = new float[width][];
        for (int i = 0; i < width; ++i)
        {
            noise[i] = new float[height];
            for (int j = 0; j < height; ++j)
                noise[i][j] = UnityEngine.Random.Range(0f, 1f);
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

    float[][] GeneratePerlinNoise(float[][] baseNoise, int octaveCount)
    {
        int width = baseNoise.Length, height = baseNoise[0].Length;

        float[][][] smoothNoise = new float[octaveCount][][];
        float persistance = .5f;

        //to generate smooth noise
        for (int i = 0; i < octaveCount; ++i)
            smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);

        float[][] perlinNoise = new float[width][];
        for (int i = 0; i < width; ++i)
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

    void Update ()
    {
	
	}

    public Transform Role, Role1, BigTile;
    public int WorldWidth, WorldHeight, CellWidth, CellHeight, SightWidth, SightHeight;
    public float DistanceThreshold;
    
    Transform[][] displayWorld;
    TileData[][] worldData;
    Texture2D t;
    float[][] noise;
    float[][] perlinNoise;
    float[][] worldNoise;
    float[][] worldPerlinNoise;
    int landformTypeAmount = MapConstants.LandformTypeAmount;
}
