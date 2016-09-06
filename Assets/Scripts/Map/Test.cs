using UnityEngine;
using System.Collections.Generic;
using System.IO;

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
        for (int i = 0; i < displayWidth; ++i)
            displayWorld[i] = new Transform[displayHeight];
        
        //to decide the detail of new go
        for (int i = 0; i < displayHeight; ++i)
        {
            for (int j = 0; j < displayWidth; ++j)
            {
                displayWorld[j][i] = tf = Instantiate(tf);
                tf.parent = Role;
                tf.localPosition = (j - halfDisplayWidth) * Vector3.right * cellWidthInWC  + (i - halfDisplayHeight) * Vector3.up * cellHeightInWC;
                tf.localScale = Vector3.one;

                tf.GetComponent<SpriteRenderer>().sprite = MakeSprite(tf.position);
            }
        }

        nowTileData = GetTileDataByWorldPosition(Role.position);
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

    Sprite MakeSprite(Vector3 worldPosition)
    {
        //to decide which TileData do we want
        TileData td = GetTileDataByWorldPosition(worldPosition);
        int type = (int)td.MaterialTypes[0];
        Vector3 position = td.Position;
        
        //important!! This should create more 2 width and height for SpriteCreate! Otherwise, the sprite will have some strange edges
        Texture2D t1 = new Texture2D(CellWidth + 2, CellHeight + 2);

        for (int k = 0; k < CellHeight + 2; ++k)
        {
            for (int l = 0; l < CellWidth + 2; ++l)
            {
                try
                {
                    t1.SetPixel(l, k, ChooseColor(type, worldPerlinNoise[(int)position.x * CellWidth + l - 1][(int)position.y * CellHeight + k - 1]));
                }
                catch
                {
                    Debug.LogError("MakeSprite Error: OutOfRange");
                }
            }
        }
        t1.Apply();

        return Sprite.Create(t1, new Rect(1, 1, CellWidth, CellHeight), Vector2.zero);
    }

    TileData GetTileDataByWorldPosition(Vector3 worldPosition)
    {
        //cast should be done last
        float pixelX = worldPosition.y * 100, pixelY = worldPosition.x * 100;

        return worldData[(int)(pixelX) / CellHeight][(int)(pixelY) / CellWidth];
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

            if (distanceX > 0f)
            {
                //right direction

                //transit
                for (int i = 0; i < displayHeight; ++i)
                    for (int j = 1; j < displayWidth; ++j)
                        displayWorld[j - 1][i].GetComponent<SpriteRenderer>().sprite = displayWorld[j][i].GetComponent<SpriteRenderer>().sprite;

                //new sight
                for (int i = 0; i < displayHeight; ++i)
                    displayWorld[tempX][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[tempX][i].position);
            }
            else if (distanceX < 0f)
            {
                //left direction

                //transit
                for (int i = 0; i < displayHeight; ++i)
                    for (int j = displayWidth - 2; j >= 0; --j)
                        displayWorld[j + 1][i].GetComponent<SpriteRenderer>().sprite = displayWorld[j][i].GetComponent<SpriteRenderer>().sprite;

                //new sight
                for (int i = 0; i < displayHeight; ++i)
                    displayWorld[0][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[0][i].position);
            }

            if (distanceY > 0f)
            {
                //up direction

                //transit
                for (int i = 1; i < displayHeight; ++i)
                    for (int j = 0; j < displayWidth; ++j)
                        displayWorld[j][i - 1].GetComponent<SpriteRenderer>().sprite = displayWorld[j][i].GetComponent<SpriteRenderer>().sprite;

                //new sight
                for (int i = 0; i < displayWidth; ++i)
                    displayWorld[i][tempY].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[i][tempY].position);
            }
            else if (distanceY < 0f)
            {
                //down direction

                //transit
                for (int i = displayHeight - 2; i >= 0; --i)
                    for (int j = 0; j < displayWidth; ++j)
                        displayWorld[j][i + 1].GetComponent<SpriteRenderer>().sprite = displayWorld[j][i].GetComponent<SpriteRenderer>().sprite;

                //new sight
                for (int i = 0; i < displayWidth; ++i)
                    displayWorld[i][0].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[i][0].position);
            }

            nowTileData = newTileData;
        }
    }

    void Update ()
    {
        /*
        if (distanceX != 0f && distanceY != 0f)
        {
            Debug.Log("Two directions");
            //two directions need!?
            if (distanceX > 0f && distanceY > 0f)
            {
                //right up direction

                //transit
                for (int i = 1; i < displayHeight; ++i)
                    for (int j = 1; j < displayWidth; ++j)
                        displayWorld[j - 1][i - 1].GetComponent<SpriteRenderer>().sprite = displayWorld[j][i].GetComponent<SpriteRenderer>().sprite;

                //new sightX
                for (int i = 0; i < displayWidth; ++i)
                    displayWorld[i][tempY].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[i][tempY].position);

                //new sightY
                for (int i = 0; i < tempY; ++i)
                    displayWorld[tempX][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[tempX][i].position);


            }
            else if (distanceX > 0f && distanceY < 0f)
            {
                //right down direction

                //transit
                for (int i = displayHeight - 2; i >= 0; --i)
                    for (int j = 1; j < displayWidth; ++j)
                        displayWorld[j - 1][i + 1].GetComponent<SpriteRenderer>().sprite = displayWorld[j][i].GetComponent<SpriteRenderer>().sprite;

                //new sightX
                for (int i = 0; i < displayWidth; ++i)
                    displayWorld[i][0].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[i][0].position);

                //new sightY
                for (int i = 0; i < tempY; ++i)
                    displayWorld[tempX][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[tempX][i].position);
            }
            else if (distanceX < 0f && distanceY > 0f)
            {
                //left up direction

                //transit
                for (int i = 0; i < displayHeight; ++i)
                    for (int j = displayWidth - 2; j >= 0; --j)
                        displayWorld[j + 1][i - 1].GetComponent<SpriteRenderer>().sprite = displayWorld[j][i].GetComponent<SpriteRenderer>().sprite;

                //new sightX
                for (int i = 0; i < displayWidth; ++i)
                    displayWorld[i][tempY].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[i][tempY].position);

                //new sightY
                for (int i = 0; i < tempY; ++i)
                    displayWorld[0][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[0][i].position);
            }
            else if (distanceX < 0f && distanceY < 0f)
            {
                //left down direction

                //transit
                for (int i = displayHeight - 2; i >= 0; --i)
                    for (int j = displayWidth - 2; j >= 0; --j)
                        displayWorld[j + 1][i + 1].GetComponent<SpriteRenderer>().sprite = displayWorld[j][i].GetComponent<SpriteRenderer>().sprite;

                //new sightX
                for (int i = 0; i < displayWidth; ++i)
                    displayWorld[i][0].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[i][0].position);

                //new sightY
                for (int i = 0; i < tempY; ++i)
                    displayWorld[0][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[0][i].position);
            }
        }
        */

        /*
        for (int i = 0; i < displayHeight; ++i)
        {
            for (int j = 0; j < displayWidth; ++j)
            {
                Debug.Log(j + " " + i + " " + displayWorld[j][i].localPosition.ToString());
                displayWorld[j][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(displayWorld[j][i].position);
            }
        }
        */

    }

    public Transform Role;
    public int WorldWidth, WorldHeight, CellWidth, CellHeight, SightWidth, SightHeight;
    public float DistanceThreshold;
    
    Transform[][] displayWorld;
    TileData[][] worldData;
    Texture2D t;
    TileData nowTileData;

    float[][] worldNoise, worldPerlinNoise;
    float cellWidthInWC, cellHeightInWC, halfDisplayWidth, halfDisplayHeight;
    int displayWidth, displayHeight, landformTypeAmount = MapConstants.LandformTypeAmount;
}
