using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldRandomer : MonoBehaviour
{
    public int Width, Height;
    public float DistanceThreshold;
    public Sprite Image0, Image1, Image2, Image3, Image4;
    public Texture2D Image00, Image11, Image22, Image33, Image44;

    void Awake()
    {
        widthcount = Width / 128;
        heightcount = Height / 128;
        Vector2[] centers = RandomSites();
        TileData2[][] world = new TileData2[widthcount][];
        Texture2D[] textures = new Texture2D[] { Image00, Image11, Image22, Image33, Image44 };
        //Sprite[] images = new Sprite[] { Image0, Image1, Image2, Image3, Image4 };
        List<TileData2> selectedtds = new List<TileData2>();

        //world initialization
        for(int i = 0; i < widthcount; ++i)
        {
            world[i] = new TileData2[heightcount];
            for(int j = 0; j < heightcount; ++j)
            {
                world[i][j] = new TileData2(new Vector2(i, j));
            }
        }

        Sprite[] images = new Sprite[5];
        for (int i = 0; i < 5; ++i)
        {
            images[i] = Sprite.Create(textures[i], new Rect(200, 200, 128, 128), Vector2.one / 2);
        }

        //dfs generator
        //first selectedtds are center points
        for (int i = 0; i < 5; ++i)
        {
            selectedtds.Add(world[(int)centers[i].x][(int)centers[i].y]);
            selectedtds[i].MaterialTypes[0] = (MapConstants.MaterialType)i;
        }

        int selected = 0;
        while (selectedtds.Count != 0)
        {
            if (TileData2.DFS(selectedtds, selected, world))
            {
                //success
                if (++selected >= selectedtds.Count)
                {
                    selected = 0;
                }
            }
            else
            {
                //failure
                selectedtds.RemoveAt(selected);
                if(selected >= selectedtds.Count)
                {
                    selected = 0;
                }
            }
        }


        //to display the random map
        for(int i = 0; i < widthcount; ++i)
        {
            for(int j = 0; j < heightcount; ++j)
            {
                GameObject go = new GameObject();
                go.transform.parent = transform;
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = new Vector3(i * 1.28f + .64f, j * 1.28f + .64f);

                TileData2 td = world[i][j];
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                go.name = "Tile" + i + ", " + j;
                if(td.MaterialTypes[1] != MapConstants.MaterialType.None)
                {
                    //this is an edge
                    go.name += " edge";
                    
                    //noise!!
                    float[][] noise = GenerateWhiteNoise(128, 128);
                    float[][] perlinNoise = GeneratePerlinNoise(noise, 6);
                    Texture2D t0 = textures[(int)td.MaterialTypes[0]], t1 = textures[(int)td.MaterialTypes[1]];

                    Texture2D blendedimage = new Texture2D(128, 128);
                    for(int k = 0; k < 128; ++k)
                    {
                        for(int l = 0; l < 128; ++l)
                        {
                            blendedimage.SetPixel(k, l, Interpolate(t0.GetPixel(k, l), t1.GetPixel(k, l), perlinNoise[k][l]));
                        }
                    }

                    blendedimage.Apply();
                    sr.sprite = Sprite.Create(blendedimage, new Rect(0, 0, 128, 128), Vector2.one / 2);
                }
                else
                {
                    sr.sprite = images[(int)td.MaterialTypes[0]];
                }
            }
        }
        

        /*
        Texture2D map = new Texture2D(Width, Height);
        for (int i = 0; i < Width; ++i)
        {
            for(int j = 0; j < Height; ++j)
            {
                map.SetPixel(i, j, MapConstants.materialColors[(int)world[i][j].MaterialTypes[0]]);
                //map.LoadImage(Image.bytes);
            }
        }

        map.Apply();
        Sprite s = Sprite.Create(map, new Rect(0, 0, Width, Height), new Vector2(.5f, .5f), 40f);
        GetComponent<SpriteRenderer>().sprite = s;
        */
    }

    Vector2[] RandomSites()
    {
        Vector2[] centers = new Vector2[5];
        bool conti = true;

        while(conti)
        {
            Debug.Log("random...");
            //to create sites randomly
            for (int i = 0; i < 5; ++i)
            {
                centers[i] = new Vector2(Random.Range(0, widthcount), Random.Range(0, heightcount));
            }
            
            //to check if these sites is too close
            conti = ReasonalDistance(centers);
        }

        return centers;
    }

    bool ReasonalDistance(Vector2[] centers)
    {
        for (int i = 0; i < 5; ++i)
        {
            for (int j = i + 1; j < 5; ++j)
            {
                if (Vector2.Distance(centers[i], centers[j]) < DistanceThreshold)
                {
                    return true;
                }
            }
        }

        return false;
    }

    float[][] GenerateWhiteNoise(int width, int height)
    {
        float[][] noise = new float[width][];
        for (int i = 0; i < width; ++i)
            noise[i] = new float[height];

        for(int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                noise[i][j] = Random.Range(0f, 1f);
            }
        }

        return noise;
    }

    float[][] GenerateSmoothNoise(float[][] baseNoise, int octave)
    {
        int width = baseNoise.Length, height = baseNoise[0].Length;

        float[][] smoothNoise = new float[width][];
        for (int i = 0; i < width; ++i)
            smoothNoise[i] = new float[height];

        int samplePeriod = 1 << octave;
        float sampleFrequency = 1f / samplePeriod;

        for(int i = 0; i < width; ++i)
        {
            //to calculate the horizontal sampling indices
            int sample_i0 = (i / samplePeriod) * samplePeriod;
            int sample_i1 = (sample_i0 + samplePeriod) % width;
            float horizontal_blend = (i - sample_i0) * sampleFrequency;

            for(int j = 0; j < height; ++j)
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
        float u = 1 - alpha;
        return new Color(c0.r * u + c1.r * alpha, c0.g * u + c1.g * alpha, c0.b * u + c1.b * alpha);
    }

    float[][] GeneratePerlinNoise(float[][] baseNoise, int octaveCount)
    {
        int width = baseNoise.Length, height = baseNoise[0].Length;

        float[][][] smoothNoise = new float[octaveCount][][];
        float persistance = .5f;

        //to generate smooth noise
        for(int i = 0; i < octaveCount; ++i)
        {
            smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
        }

        float[][] perlinNoise = new float[width][];
        for (int i = 0; i < width; ++i)
            perlinNoise[i] = new float[height];

        float amplitude = 1f, totalAmplitude = 0f;

        //to blend noise together
        for(int octave = octaveCount - 1; octave >= 0; --octave)
        {
            amplitude *= persistance;
            totalAmplitude += amplitude;

            for(int i = 0; i < width; ++i)
            {
                for(int j = 0; j < height; ++j)
                {
                    perlinNoise[i][j] += smoothNoise[octave][i][j] * amplitude;
                }
            }
        }

        //normalization
        for(int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                perlinNoise[i][j] /= totalAmplitude;
            }
        }

        return perlinNoise;
    }

    int widthcount, heightcount;
}
