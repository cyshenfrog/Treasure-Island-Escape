using UnityEngine;
using System.Collections.Generic;
using System;

public class GroundController : MonoBehaviour
{
    void Awake ()
    {
        int widthCount = WorldWidth / CellWidth, heightCount = WorldHeight / CellHeight;

        //to get random world
        GroundRandomer gr = new GroundRandomer(widthCount, heightCount, DistanceThreshold);
        groundData = gr.GroundData;
        
        //to display the random map

        //new go
        Transform tf = ((GameObject)Resources.Load(@"Map\Tile")).transform;

        //to calculate how many go do this world have to

        //1 pixel = 0.01 in world coordinates
        cellWidthInWC = CellWidth * .01f;
        cellHeightInWC = CellHeight * .01f;
        displayWidth = SightWidth / CellWidth;
        displayHeight = SightHeight / CellHeight;
        halfDisplayWidth = displayWidth * .5f;
        halfDisplayHeight = displayHeight * .5f;

        //to create the noise
        worldNoise = GenerateWhiteNoise(WorldWidth, WorldHeight);
        worldPerlinNoise = GeneratePerlinNoise(worldNoise, 6);

        //to display
        displayWorld = new Transform[displayWidth][];

        
        //to decide the detail of new go
        for (int i = 0; i < displayWidth; ++i)
        {
            displayWorld[i] = new Transform[displayHeight];

            for (int j = 0; j < displayHeight; ++j)
            {
                //to initialize
                displayWorld[i][j] = tf = Instantiate(tf);
                tf.parent = Role;
                tf.localPosition = (i - halfDisplayWidth) * Vector3.right * cellWidthInWC + (j - halfDisplayHeight) * Vector3.up * cellHeightInWC;
                tf.localScale = Vector3.one;
                tf.name = "Tile " + i.ToString() + ',' + j.ToString();

                tf.GetComponent<SpriteRenderer>().sprite = MakeSprite(tf.position);
            }
        }

        //to set the nowTileData below the Role
        nowTileData = GetTileDataByWorldPosition(Role.position);
    }
    
    Color ChooseColor(int type, float value)
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
        
        return MapConstants.LandformColor[type][index];
    }

    TileData GetTileDataByWorldPosition(Vector3 worldPosition)
    {
        //cast should be done last
        float pixelX = worldPosition.x * 100, pixelY = worldPosition.y * 100;

        return groundData[(int)(pixelX) / CellWidth][(int)(pixelY) / CellHeight];
    }

    Sprite MakeSprite(Vector3 worldPosition)
    {
        //to decide which TileData do we want
        TileData td = GetTileDataByWorldPosition(worldPosition);
        MapConstants.LandformType type = td.MaterialTypes[0];
        Vector3 tdPosition = td.Position;

        //important!! This should create more 2 width and height for SpriteCreate! Otherwise, the sprite will have some strange edges
        Texture2D t1 = new Texture2D(CellWidth + 2, CellHeight + 2);

        if (td.MaterialTypes[1] == MapConstants.LandformType.None)
        {
            //this is a simple materialType
            for (int k = 0; k < CellWidth + 2; ++k)
            {
                for (int l = 0; l < CellHeight + 2; ++l)
                {
                    try
                    {
                        t1.SetPixel(k, l, ChooseColor((int)type, worldPerlinNoise[(int)tdPosition.x * CellWidth + k - 1][(int)tdPosition.y * CellHeight + l - 1]));
                    }
                    catch(Exception e)
                    {
                        Debug.LogError("MakeSprite Error: OutOfRange " + e);
                    }
                }
            }
            t1.Apply();
        }
        else
        {
            //this is an edge

            //Can an edge have two materialTypes only?

            //temp
            for (int k = 0; k < CellWidth + 2; ++k)
            {
                for (int l = 0; l < CellHeight + 2; ++l)
                {
                    try
                    {
                        t1.SetPixel(k, l, ChooseColor((int)type, worldPerlinNoise[(int)tdPosition.x * CellWidth + k - 1][(int)tdPosition.y * CellHeight + l - 1]));
                    }
                    catch(Exception e)
                    {
                        Debug.LogError("MakeSprite Error: OutOfRange " + e);
                    }
                }
            }
            t1.Apply();

            /*
            //to blend two textures
            Texture2D t0 = textures[(int)td.MaterialTypes[0]], t1 = textures[(int)td.MaterialTypes[1]], blendedimage = new Texture2D(CellHeight, CellWidth);
            int firstPixelX = j * CellWidth, firstPixelY = i * CellHeight;
            for (int k = 0; k < CellHeight; ++k)
                for (int l = 0; l < CellWidth; ++l)
                    blendedimage.SetPixel(l, k, Interpolate(t0.GetPixel(firstPixelX + l, firstPixelY + k), t1.GetPixel(firstPixelX + l, firstPixelY + k), perlinNoise[l][k]));

            blendedimage.Apply();
            sr.sprite = Sprite.Create(blendedimage, new Rect(0, 0, CellWidth, CellHeight), Vector2.zero);
            */
        }

        return Sprite.Create(t1, new Rect(1, 1, CellWidth, CellHeight), Vector2.zero);
    }

    void RefreshMap()
    {
        //to refresh the displayWorld
        //walking speed cannot be too fast!?
        TileData newTileData = GetTileDataByWorldPosition(Role.position);

        if (nowTileData != newTileData)
        {
            //to decide the moving direction

            Vector2 from = nowTileData.Position, to = newTileData.Position;
            float distanceX = to.x - from.x, distanceY = to.y - from.y;
            int tempX = displayWidth - 1, tempY = displayHeight - 1;

            //one direction
            //two directions redundancy?

            if (distanceX > 0f)
            {
                //right direction

                //transit
                for (int i = 1; i < displayWidth; ++i)
                    for (int j = 0; j < displayHeight; ++j)
                        displayWorld[i - 1][j].GetComponent<SpriteRenderer>().sprite = displayWorld[i][j].GetComponent<SpriteRenderer>().sprite;

                //new sight
                for (int i = 0; i < displayHeight; ++i)
                    displayWorld[tempX][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[tempX][i].position);
            }
            else if (distanceX < 0f)
            {
                //left direction

                //transit
                for (int i = displayWidth - 2; i >= 0; --i)
                    for (int j = 0; j < displayHeight; ++j)
                        displayWorld[i + 1][j].GetComponent<SpriteRenderer>().sprite = displayWorld[i][j].GetComponent<SpriteRenderer>().sprite;

                //new sight
                for (int i = 0; i < displayHeight; ++i)
                    displayWorld[0][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[0][i].position);
            }

            if (distanceY > 0f)
            {
                //up direction

                //transit
                for (int i = 0; i < displayWidth; ++i)
                    for (int j = 1; j < displayHeight; ++j)
                        displayWorld[i][j - 1].GetComponent<SpriteRenderer>().sprite = displayWorld[i][j].GetComponent<SpriteRenderer>().sprite;

                //new sight
                for (int i = 0; i < displayWidth; ++i)
                    displayWorld[i][tempY].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[i][tempY].position);
            }
            else if (distanceY < 0f)
            {
                //down direction

                //transit
                for (int i = 0; i < displayWidth; ++i)
                    for (int j = displayHeight - 2; j >= 0; --j)
                        displayWorld[i][j + 1].GetComponent<SpriteRenderer>().sprite = displayWorld[i][j].GetComponent<SpriteRenderer>().sprite;

                //new sight
                for (int i = 0; i < displayWidth; ++i)
                    displayWorld[i][0].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[i][0].position);
            }

            nowTileData = newTileData;
        }
    }

    float Interpolate(float x0, float x1, float alpha)
    {
        return x0 * (1 - alpha) + x1 * alpha;
    }

    Color Interpolate(Color c0, Color c1, float alpha)
    {
        return alpha > .5f ? c1 : c0;

        //return c0 * (1 - alpha) + c1 * alpha;
        /*
        float u = 1f - alpha;
        return new Color(c0.r * u + c1.r * alpha, c0.g * u + c1.g * alpha, c0.b * u + c1.b * alpha);
        */
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
    
    void Update()
    {
        RefreshMap();
    }

    public Transform Role;
    public int WorldWidth, WorldHeight, CellWidth, CellHeight, SightWidth, SightHeight;
    public float DistanceThreshold;
    
    Transform[][] displayWorld;
    TileData[][] groundData;
    TileData nowTileData;

    float[][] worldNoise, worldPerlinNoise;
    float cellWidthInWC, cellHeightInWC, halfDisplayWidth, halfDisplayHeight;
    int displayWidth, displayHeight, landformTypeAmount = MapConstants.LandformTypeAmount;
}