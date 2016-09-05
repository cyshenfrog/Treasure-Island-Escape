using UnityEngine;
using System.Collections.Generic;
using System;

public class WorldController : MonoBehaviour
{
    void Awake ()
    {
        textures = new Texture2D[] { Sea, Grassland, Forest, Desert, Marsh, Snowfield, Volcano };

        /*
        sprites = new Sprite[materialTypeAmount];
        for (int i = 0; i < materialTypeAmount; ++i)
            sprites[i] = Sprite.Create(textures[i], new Rect(20, 20, CellWidth, CellHeight), Vector2.one / 2);
        */
        
        int widthCount = WorldWidth / CellWidth, heightCount = WorldHeight / CellHeight;
        
        //to get random world
        WorldRandomer wr = new WorldRandomer(widthCount, heightCount, DistanceThreshold);
        worldData = wr.WorldData;

        //Textures Processing
        for (int i = 0; i < landformTypeAmount; ++i)
            textures[i] = ResizeCanvas(textures[i], WorldWidth, WorldHeight);

        //Sprite[] grasslands = Resources.LoadAll<Sprite>(MapConstants.LandformPath);


        
        //to display the random map
        
        //1 pixel = 0.01 in world coordinates
        float cellWidthInWC = CellWidth * .01f, cellHeightInWC = CellHeight * .01f, halfCellWidthInWC = cellWidthInWC * .5f, halfCellHeightInWC = cellHeightInWC * .5f;
        Transform tf = ((GameObject)Resources.Load(@"Map\Tile")).transform;
        SpriteRenderer sr;

        int displayWidth = SightWidth / CellWidth + 1, displayHeight = SightHeight / CellHeight + 1;
        float halfDisplayWidth = displayWidth / 2f, halfDisplayHeight = displayHeight / 2f;

        displayWorld = new Transform[displayHeight][];
        for (int i = 0; i < displayHeight; ++i)
            displayWorld[i] = new Transform[displayWidth];



        for(int i = 0; i < displayHeight; ++i)
        {
            for(int j = 0; j < displayWidth; ++j)
            {
                displayWorld[i][j] = tf = Instantiate(tf);
                tf.parent = Role;

                tf.localPosition = (i - halfDisplayWidth) * Vector3.right + (j - halfDisplayHeight) * Vector3.up;
                tf.localScale = Vector3.one;

                //tf.GetComponent<SpriteRenderer>().sprite = ChooseSprite(tf.position.x, tf.position.y);
            }
        }



        /*
        displayWorld = new Transform[heightCount][];
        for (int i = 0; i < heightCount; ++i)
            displayWorld[i] = new Transform[widthCount];
        */

        /*
        for (int i = 0; i < heightCount; ++i)
        {
            //displayWorld[i] = new Transform[widthCount];
            for (int j = 0; j < widthCount; ++j)
            {
                displayWorld[i][j] = tf = Instantiate(tf);
                sr = tf.GetComponent<SpriteRenderer>();

                tf.parent = WorldList;
                tf.localScale = Vector3.one;
                //the local position is at center;
                tf.localPosition = new Vector3(j * cellWidthInWC, i * cellHeightInWC);
                tf.name = "Tile " + j + ", " + i;

                TileData td = worldData[i][j];

                //Can an edge have two materialTypes only?
                if (td.MaterialTypes[1] == MapConstants.LandformType.None)
                {
                    //this is a simple materialType
                    int type = (int)td.MaterialTypes[0];

                    sr.sprite = Sprite.Create(textures[type], new Rect(j * CellWidth, i * CellHeight, CellWidth, CellHeight), Vector2.zero);
                    //sr.sprite = Sprite.Create(textures[type], new Rect(0, 0, CellWidth, CellHeight), Vector2.zero);

                    //sr.sprite = sprites[(int)td.MaterialTypes[0]];
                    tf.name += " " + td.MaterialTypes[0];
                }
                else
                {
                    //this is an edge
                    tf.name += " edge with " + td.MaterialTypes[0] + " and " + td.MaterialTypes[1];

                    int type = (int)td.MaterialTypes[0];

                    //noise!!
                    float[][] noise = GenerateWhiteNoise(CellHeight, CellWidth);
                    float[][] perlinNoise = GeneratePerlinNoise(noise, 6);

                    //to blend two textures
                    Texture2D t0 = textures[(int)td.MaterialTypes[0]], t1 = textures[(int)td.MaterialTypes[1]], blendedimage = new Texture2D(CellHeight, CellWidth);
                    int firstPixelX = j * CellWidth, firstPixelY = i * CellHeight;
                    for (int k = 0; k < CellHeight; ++k)
                        for (int l = 0; l < CellWidth; ++l)
                            blendedimage.SetPixel(l, k, Interpolate(t0.GetPixel(firstPixelX + l, firstPixelY + k), t1.GetPixel(firstPixelX + l, firstPixelY + k), perlinNoise[l][k]));

                    blendedimage.Apply();
                    sr.sprite = Sprite.Create(blendedimage, new Rect(0, 0, CellWidth, CellHeight), Vector2.zero);
                }
            }
        }
        */
        
        
        List<TileData>[] landformList = wr.LandformList;

        worldNoise = GenerateWhiteNoise(WorldHeight, WorldWidth);
        worldPerlinNoise = GeneratePerlinNoise(worldNoise, 6);

        /*
        Debug.Log("x length = " + landformList[0].Count);

        for (int i = 0; i < landformList.Length; ++i)
            Debug.Log(landformList[i].Count);
        */

        for (int i = 0; i < landformList.Length; ++i)
        {
            for(int j = 0; j < landformList[i].Count; ++j)
            {
                TileData td = landformList[i][j];
                Vector3 position = td.Position;

                displayWorld[(int)position.y][(int)position.x] = tf = Instantiate(tf);
                sr = tf.GetComponent<SpriteRenderer>();

                tf.parent = WorldList;
                tf.localScale = Vector3.one;
                //the local position is at center;
                tf.localPosition = new Vector3(position.x * cellWidthInWC, position.y * cellHeightInWC);
                tf.name = "Tile " + position.x + ", " + position.y;

                //Can an edge have two materialTypes only?
                if (td.MaterialTypes[1] == MapConstants.LandformType.None)
                {
                    //this is a simple materialType
                    int type = (int)td.MaterialTypes[0];


                    if(type != (int)MapConstants.LandformType.Grassland)
                    {
                        sr.sprite = Sprite.Create(textures[type], new Rect((int)position.x * CellWidth, (int)position.y * CellHeight, CellWidth, CellHeight), Vector2.zero);
                    }
                    else
                    {
                        Texture2D t = new Texture2D(CellWidth, CellHeight);

                        for(int k = 0; k < CellHeight; ++k)
                        {
                            for(int l = 0; l < CellWidth; ++l)
                            {
                                t.SetPixel(l, k, ChooseColor(type, worldPerlinNoise[(int)position.x * CellWidth + l][(int)position.y * CellHeight + k]));
                            }
                        }
                        t.Apply();

                        sr.sprite = Sprite.Create(t, new Rect(0, 0, CellWidth, CellHeight), Vector2.zero);

                        //Debug.Log(j + " " + i);
                        //sr.sprite = grasslands[992 - (int)position.y * 32 + (int)position.x];
                        //sr.sprite.
                        //tf.name += " " + position.x + " " + position.y;
                    }


                    //sr.sprite = Sprite.Create(textures[type], new Rect(0, 0, CellWidth, CellHeight), Vector2.zero);
                    
                    //sr.sprite = sprites[(int)td.MaterialTypes[0]];
                    tf.name += " " + td.MaterialTypes[0];
                }
                else
                {
                    //this is an edge
                    tf.name += " edge with " + td.MaterialTypes[0] + " and " + td.MaterialTypes[1];

                    int type = (int)td.MaterialTypes[0];

                    //noise!!
                    noise = GenerateWhiteNoise(CellHeight, CellWidth);
                    perlinNoise = GeneratePerlinNoise(noise, 6);

                    //to blend two textures
                    Texture2D t0 = textures[(int)td.MaterialTypes[0]], t1 = textures[(int)td.MaterialTypes[1]], blendedimage = new Texture2D(CellWidth, CellHeight);
                    int firstPixelX = (int)position.x * CellWidth, firstPixelY = (int)position.y * CellHeight;
                    for (int k = 0; k < CellHeight; ++k)
                        for (int l = 0; l < CellWidth; ++l)
                            blendedimage.SetPixel(l, k, Interpolate(t0.GetPixel(firstPixelX + l, firstPixelY + k), t1.GetPixel(firstPixelX + l, firstPixelY + k), perlinNoise[l][k]));

                    blendedimage.Apply();
                    sr.sprite = Sprite.Create(blendedimage, new Rect(0, 0, CellWidth, CellHeight), Vector2.zero);
                }
            }
        }
        

        /*
        //to find the max and min x , y coordinates in respective landform
        Vector2[][] boundaries = new Vector2[landformTypeAmount][];
        for (int i = 0; i < landformTypeAmount; ++i)
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

        List<TileData>[] landformList = wr.LandformList;

        for(int i = 0; i < landformTypeAmount; ++i)
        {
            int count = landformList[i].Count;
            for(int j = 0; j < count; ++j)
            {
                TileData td = landformList[i][j];
                Vector2 position = td.Position;

                boundaries[i][0] = boundaries[i][0].x < position.x ? position : boundaries[i][0];
                boundaries[i][1] = boundaries[i][1].x > position.x ? position : boundaries[i][1];
                boundaries[i][2] = boundaries[i][2].y < position.y ? position : boundaries[i][2];
                boundaries[i][3] = boundaries[i][3].y > position.y ? position : boundaries[i][3];
            }
        }

        for(int i = 0; i < landformTypeAmount; ++i)
        {
            int reWidth = (int)(boundaries[i][0].x - boundaries[i][1].x + 1) * CellWidth, reHeight = (int)(boundaries[i][2].y - boundaries[i][3].y + 1) * CellHeight;
            ResizeCanvas(textures[i], reWidth, reHeight);

            
            //int tWidth = textures[i].width, tHeight = textures[i].height;
            
            //float times = reWidth / tWidth > reHeight / tHeight ? reWidth / tWidth : reHeight / tHeight;
            
            //Debug.Log(textures[i].Resize(tWidth * ((int)times + 1), tHeight * ((int)times + 1)));
            //textures[i].Apply();
            
        }
        

        Vector2[] coordinates = new Vector2[landformTypeAmount];
        for(int i = 0; i < landformTypeAmount; ++i)
        {
            coordinates[i] = new Vector2(boundaries[i][1].x, boundaries[i][3].y);
            Debug.Log(coordinates[i]);
        }
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

    
    //used to change the size of texture 
    public static Texture2D ResizeCanvas(Texture2D texture, int width, int height)
    {
        float oldWidth = texture.width, oldHeight = texture.height;
        float newWR = width / oldWidth, newHR = height / oldHeight, times = newWR > newHR ? newWR : newHR;
        times = (int)times + 1;

        var newPixels = times == 1f ? texture.GetPixels32() : ResizeCanvas(texture.GetPixels32(), (int)oldWidth, (int)oldHeight, (int)(oldWidth * times), (int)(oldHeight * times), (int)times);

        //texture.Resize((int)(oldWidth * times), (int)(oldHeight * times));
        //texture.SetPixels32(newPixels);
        //texture.Apply();
        
        //to create a new texture directly
        Texture2D newT = new Texture2D((int)(oldWidth * times), (int)(oldHeight * times));
        newT.SetPixels32(newPixels);
        newT.Apply();
        return newT;
    }

    private static Color32[] ResizeCanvas(IList<Color32> pixels, int oldWidth, int oldHeight, int width, int height, int times)
    {
        var newPixels = new Color32[(width * height)];

        for (int i = 0; i < oldHeight; ++i)
        {
            for (int j = 0; j < oldWidth; ++j)
            {
                int newIndex = i * width * times + j * times;
                int oldIndex = i * oldWidth + j;

                Color32 c = pixels[i * oldWidth + j];

                newPixels[newIndex] = c;

                int rightTemp, upTemp;
                for (int k = 1; k < times; ++k)
                {
                    //the rightmost
                    rightTemp = newIndex + 1 * k;
                    newPixels[rightTemp] = c;
                    //the upmost
                    upTemp = newIndex + width * k;
                    newPixels[upTemp] = c;

                    for (int l = k - 1; l > 0; --l)
                    {
                        rightTemp += width;
                        newPixels[rightTemp] = c;

                        upTemp += 1;
                        newPixels[upTemp] = c;
                    }

                    //the rightmost and upmost 
                    rightTemp = newIndex + width * k + 1 * k;
                    newPixels[rightTemp] = c;
                }
            }
        }

        return newPixels;
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

    /*
    Sprite ChooseSprite(float positionX, float positionY)
    {
        int x = (int)positionX * 100, y = (int)positionY * 100;

        int type = (int)worldData[y][x].MaterialTypes[0];
        Vector3 position = worldData[y][x].Position;

        if(type != (int)MapConstants.LandformType.Grassland)
        {
            return Sprite.Create(textures[type], new Rect((int)position.x * CellWidth, (int)position.y * CellHeight, CellWidth, CellHeight), Vector2.zero);
        }
        else
        {
            
            Texture2D t = new Texture2D(CellWidth, CellHeight);

            for (int k = 0; k < CellHeight; ++k)
            {
                for (int l = 0; l < CellWidth; ++l)
                {
                    t.SetPixel(l, k, ChooseColor(type, worldPerlinNoise[(int)position.x * CellWidth + l][(int)position.y * CellHeight + k]));
                }
            }
            t.Apply();

            sr.sprite = Sprite.Create(t, new Rect(0, 0, CellWidth, CellHeight), Vector2.zero);
        }
    }
    */

    void Update()
    {
        Vector3 rolePosition = Role.transform.localPosition;


    }
    

    /*
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
    */

    public Transform WorldList, Role;
    public Texture2D Sea, Grassland, Forest, Desert, Marsh, Snowfield, Volcano;
    public Sprite gra;
    public int WorldWidth, WorldHeight, CellWidth, CellHeight, SightWidth, SightHeight;
    public float DistanceThreshold;

    Sprite[] sprites;
    Texture2D[] textures;
    Transform[][] displayWorld;
    TileData[][] worldData;
    float[][] noise;
    float[][] perlinNoise;
    float[][] worldNoise;
    float[][] worldPerlinNoise;
    int landformTypeAmount = MapConstants.LandformTypeAmount;
}