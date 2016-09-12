using UnityEngine;
using System.Collections.Generic;
using System;

//ex: TileData td = GroundController.GetTileDataByWorldPosition(position);
public class GroundController : MonoBehaviour
{
    //Its parent must be world;

    void Awake ()
    {
        StaticCellWidth = CellWidth;
        StaticCellHeight = CellHeight;
        tile = Resources.Load<GameObject>(@"Map\Tile").transform;

        worldWidthCount = WorldWidth / CellWidth;
        worldHeightCount = WorldHeight / CellHeight;

        //to get random world
        groundData = GroundRandomer.Create(worldWidthCount, worldHeightCount, DistanceThreshold).GroundData;
        
        //to display the random map

        //to calculate how many go do this world have to

        //1 pixel = 0.01 in world coordinates
        cellWidthInWC = CellWidth * .01f;
        cellHeightInWC = CellHeight * .01f;
        sightWidthCount = SightWidth / CellWidth;
        sightHeightCount = SightHeight / CellHeight;

        minSightWidthBoundary = SightWidth * .01f;
        maxSightWidthBoundary = (WorldWidth - SightWidth) * .01f;
        minSightHeightBoundary = SightHeight * .01f;
        maxSightHeightBoundary = (WorldHeight - SightHeight) * .01f;

        //to create the noise
        worldNoise = GenerateWhiteNoise(WorldWidth, WorldHeight);
        worldPerlinNoise = GeneratePerlinNoise(worldNoise, 6);

        //Display(false);
        Display(true);
        /*
        foreach(Transform[] ta in displaySight)
        {
            foreach(Transform t in ta)
            {
                t.gameObject.SetActive(true);
            }
        }*/
        
        //to set the nowTileData below the Role
        nowTileData = GetTileDataByWorldPosition(Role.position);

        Debug.Log("groundcontroller is " + transform.parent);
    }

    public static TileData GetTileDataByWorldPosition(Vector3 worldPosition)
    {
        //cast should be done last
        //"+1" is to ensure the correctness of getting GroundData
        float pixelX = worldPosition.x * 100 + 1, pixelY = worldPosition.y * 100 + 1;

        try
        {
            return GroundRandomer.Self.GroundData[(int)(pixelX) / StaticCellWidth][(int)(pixelY) / StaticCellHeight];
        }
        catch(Exception e)
        {
            Debug.LogError("GetTileDataByWorldPosition Error: " + e);
            return null;
        }
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
                    catch(IndexOutOfRangeException e)
                    {
                        t1.SetPixel(k, l, Color.white);
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
                    catch(IndexOutOfRangeException e)
                    {
                        t1.SetPixel(k, l, Color.white);
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

    void Display(bool isSight)
    {
        Transform parent = isSight ? Role : transform;
        int widthCount = isSight ? sightWidthCount : worldWidthCount, heightCount = isSight ? sightHeightCount : worldHeightCount;
        float halfWidthCount = isSight ? sightWidthCount * .5f : 0f, halfHeightCount = isSight ? sightHeightCount * .5f : 0f;
        
        //to display
        Transform[][] display = new Transform[widthCount][];

        //to decide the detail of new go

        isNotRoleNearBoundary = Role.position.x > minSightWidthBoundary && Role.position.x < maxSightWidthBoundary && Role.position.y > minSightHeightBoundary && Role.position.y < maxSightHeightBoundary;

        if (IsNotRoleNearBoundary)
        {
            for (int i = 0; i < widthCount; ++i)
            {
                display[i] = new Transform[heightCount];

                for (int j = 0; j < heightCount; ++j)
                {
                    //to initialize
                    display[i][j] = tile = Instantiate(tile);
                    tile.parent = parent;
                    tile.localPosition = (i - halfWidthCount) * Vector3.right * cellWidthInWC + (j - halfHeightCount) * Vector3.up * cellHeightInWC;
                    tile.localScale = Vector3.one;
                    tile.name = "Tile " + i.ToString() + ',' + j.ToString();

                    tile.GetComponent<SpriteRenderer>().sprite = MakeSprite(tile.position);
                }
            }
        }
        else
        {
            for (int i = 0; i < widthCount; ++i)
            {
                display[i] = new Transform[heightCount];

                for (int j = 0; j < heightCount; ++j)
                {
                    //to initialize
                    display[i][j] = tile = Instantiate(tile);
                    tile.parent = parent;
                    tile.localPosition = (i - halfWidthCount) * Vector3.right * cellWidthInWC + (j - halfHeightCount) * Vector3.up * cellHeightInWC;
                    tile.localScale = Vector3.one;
                    tile.name = "Tile " + i.ToString() + ',' + j.ToString();

                    tile.GetComponent<SpriteRenderer>().sprite = MakeSprite(tile.position);
                }
            }
        }

        if (isSight)
            displaySight = display;
        else
            displayWorld = display;
    }

    void DisplaySight()
    {

    }

    void DisplayAllWorld()
    {
        float halfWorldWidth = worldWidthCount * .5f, halfWorldHeight = worldHeightCount * .5f;

        //to display
        displayWorld = new Transform[worldWidthCount][];

        //to decide the detail of new go
        for (int i = 0; i < worldWidthCount; ++i)
        {
            displayWorld[i] = new Transform[worldHeightCount];

            for (int j = 0; j < worldHeightCount; ++j)
            {
                //to initialize
                displayWorld[i][j] = tile = Instantiate(tile);
                tile.parent = transform;
                tile.localPosition = (i - halfWorldWidth) * Vector3.right * cellWidthInWC + (j - halfWorldHeight) * Vector3.up * cellHeightInWC;
                tile.localScale = Vector3.one;
                tile.name = "Tile " + i.ToString() + ',' + j.ToString();

                tile.GetComponent<SpriteRenderer>().sprite = MakeSprite(tile.position);

                tile.gameObject.SetActive(false);
            }
        }
    }

    void RefreshMap()
    {
        //to refresh the displayWorld

        if(IsNotRoleNearBoundary = Role.position.x > minSightWidthBoundary && Role.position.x < maxSightWidthBoundary && Role.position.y > minSightHeightBoundary && Role.position.y < maxSightHeightBoundary)
        {
            //walking speed cannot be too fast!?
            TileData newTileData = GetTileDataByWorldPosition(Role.position);

            if (nowTileData != newTileData)
            {
                //to decide the moving direction

                Vector2 from = nowTileData.Position, to = newTileData.Position;
                float distanceX = to.x - from.x, distanceY = to.y - from.y;
                int tempX = sightWidthCount - 1, tempY = sightHeightCount - 1;

                //one direction
                //two directions redundancy?

                if (distanceX > 0f)
                {
                    //right direction

                    //transit
                    for (int i = 1; i < sightWidthCount; ++i)
                        for (int j = 0; j < sightHeightCount; ++j)
                            displaySight[i - 1][j].GetComponent<SpriteRenderer>().sprite = displaySight[i][j].GetComponent<SpriteRenderer>().sprite;

                    //new sight
                    for (int i = 0; i < sightHeightCount; ++i)
                        displaySight[tempX][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(displaySight[tempX][i].position);
                }
                else if (distanceX < 0f)
                {
                    //left direction

                    //transit
                    for (int i = sightWidthCount - 2; i >= 0; --i)
                        for (int j = 0; j < sightHeightCount; ++j)
                            displaySight[i + 1][j].GetComponent<SpriteRenderer>().sprite = displaySight[i][j].GetComponent<SpriteRenderer>().sprite;

                    //new sight
                    for (int i = 0; i < sightHeightCount; ++i)
                        displaySight[0][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(displaySight[0][i].position);
                }

                if (distanceY > 0f)
                {
                    //up direction

                    //transit
                    for (int i = 0; i < sightWidthCount; ++i)
                        for (int j = 1; j < sightHeightCount; ++j)
                            displaySight[i][j - 1].GetComponent<SpriteRenderer>().sprite = displaySight[i][j].GetComponent<SpriteRenderer>().sprite;

                    //new sight
                    for (int i = 0; i < sightWidthCount; ++i)
                        displaySight[i][tempY].GetComponent<SpriteRenderer>().sprite = MakeSprite(displaySight[i][tempY].position);
                }
                else if (distanceY < 0f)
                {
                    //down direction

                    //transit
                    for (int i = 0; i < sightWidthCount; ++i)
                        for (int j = sightHeightCount - 2; j >= 0; --j)
                            displaySight[i][j + 1].GetComponent<SpriteRenderer>().sprite = displaySight[i][j].GetComponent<SpriteRenderer>().sprite;

                    //new sight
                    for (int i = 0; i < sightWidthCount; ++i)
                        displaySight[i][0].GetComponent<SpriteRenderer>().sprite = MakeSprite(displaySight[i][0].position);
                }

                nowTileData = newTileData;
            }
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

    public static int StaticCellWidth, StaticCellHeight;

    public Transform Role;
    public int WorldWidth, WorldHeight, CellWidth, CellHeight, SightWidth, SightHeight;
    public float DistanceThreshold;

    Transform[][] displaySight, displayWorld;
    TileData[][] groundData;
    TileData nowTileData;
    Transform tile;

    bool IsNotRoleNearBoundary
    {
        get { return isNotRoleNearBoundary; }

        set
        {
            if(isNotRoleNearBoundary != value)
            {
                //bug: cannot init at small/large posiiton 

                Transform tf, parent = value ? Role : transform.parent;

                for (int i = 0; i < sightWidthCount; ++i)
                    for (int j = 0; j < sightHeightCount; ++j)
                    {
                        tf = displaySight[i][j];
                        
                        tf.parent = parent;
                        tf.localScale = Vector3.one;
                    }

                Debug.Log(value ? "displaySight is moving" : "displaySight is stand still");
                isNotRoleNearBoundary = value;
            }
        }
    }

    float[][] worldNoise, worldPerlinNoise;
    float cellWidthInWC, cellHeightInWC;
    //InWC
    float minSightWidthBoundary, maxSightWidthBoundary, minSightHeightBoundary, maxSightHeightBoundary;
    int worldWidthCount, worldHeightCount, sightWidthCount, sightHeightCount, landformTypeAmount = MapConstants.LandformTypeAmount;
    bool isNotRoleNearBoundary;
}