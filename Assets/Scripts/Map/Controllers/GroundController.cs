using UnityEngine;
using System.Collections;
using System;

//ex: TileData td = GroundController.GetTileDataByWorldPosition(position);
public class GroundController : MonoBehaviour
{
    //Its parent must be world;

    void Awake ()
    {
        //to initialize some value
        StaticCellWidth = CellWidth;
        StaticCellHeight = CellHeight;
        StaticSightWidth = SightWidth;
        StaticSightHeight = SightHeight;
        StaticWorldWidth = WorldWidth;
        StaticWorldHeight = WorldHeight;
        StaticWorldWidthInWC = WorldWidth * .01f;
        StaticWorldHeightInWC = WorldHeight * .01f;

        MapConstants.MaxStack = MaxStack;

        preSightRange = PreSight + 2;

        //to calculate how many tileData do this world have to
        worldWidthCount = WorldWidth / CellWidth;
        worldHeightCount = WorldHeight / CellHeight;

        //to calculate how many go do sightWorld have to
        sightWidthCount = SightWidth / CellWidth;
        sightHeightCount = SightHeight / CellHeight;
        halfSightWidthCount = (int)(sightWidthCount * .5f);
        halfSightHeightCount = (int)(sightHeightCount * .5f);

        //1 pixel = 0.01 in world coordinates
        CellWidthInWC = CellWidth * .01f;
        CellHeightInWC = CellHeight * .01f;
        
        tile = Resources.Load<GameObject>(@"Map\Tile").transform;
        //tile.GetComponent<BoxCollider>().size = cellWidthInWC * Vector3.right + cellHeightInWC * Vector3.up;
        //tile.GetComponent<BoxCollider>().center = cellWidthInWC * .5f * Vector3.right + cellHeightInWC * .5f * Vector3.up;

        //boundary
        minSightWidthBoundary = SightWidth * .01f;
        maxSightWidthBoundary = (WorldWidth - SightWidth) * .01f;
        minSightHeightBoundary = SightHeight * .01f;
        maxSightHeightBoundary = (WorldHeight - SightHeight) * .01f;
        
        //to get random world
        groundData = GroundRandomer.Create(worldWidthCount, worldHeightCount, DistanceThreshold).GroundData;

        //to display the random map

        //to create the noise
        //worldNoise = GenerateWhiteNoise(WorldWidth, WorldHeight);
        //worldPerlinNoise = GeneratePerlinNoise(worldNoise, 6);
        worldPerlinNoise = GenerateWhiteNoise(WorldWidth, WorldHeight);

        //to load the landform sprites
        spriteWidth = LandformTextureWidth / CellWidth;
        spriteHeight = LandformTextureHeight / CellHeight;
        int spriteCount = spriteWidth * spriteWidth;
        landformSprites = new Sprite[landformTypeAmount, spriteCount];
        Debug.Log("landformsprites length = " + landformSprites.Length + " spriteWidth = " + spriteWidth + " spriteHeight = " + spriteHeight);

        for(int i = 0; i < 2; ++i)
        {
            string path = landformSpriteDirectoryPath + ((MapConstants.LandformType)i).ToString() + @"Slices\" + ((MapConstants.LandformType)i).ToString() + '_';
            for (int j = 0; j < spriteCount; ++j)
            {
                landformSprites[i, j] = Resources.Load<Sprite>(path + j.ToString());
            }
        }



        /*
        //for all
        for (int i = 0; i < landformTypeAmount; ++i)
        {
            string path = landformSpriteDirectoryPath + ((MapConstants.LandformType)i).ToString() + @"Slices\" + ((MapConstants.LandformType)i).ToString() + '_';
            for (int j = 0; j < spriteCount; ++j)
            {
                landformSprites[i, j] = Resources.Load<Sprite>(path + j.ToString());
            }
        }
        */


        //Display(false);
        //Display();
        DisplayWorld();

        
        for (int i = 0; i < landformTypeAmount; ++i)
            Debug.Log((MapConstants.LandformType)i + ": " + GroundRandomer.Self.LandformList[i].Count);

        /*
        foreach(Transform[] ta in displaySight)
        {
            foreach(Transform t in ta)
            {
                t.gameObject.SetActive(true);
            }
        }*/

            //to set the nowTileData below the Role
            //nowTileData = GetTileDataByWorldPosition(Role.position);

            /*
            //the max tile
            mapPool[worldWidthCount - 1, worldHeightCount - 1] = tile = Instantiate(tile);
            tile.parent = SightList;
            tile.position = (worldWidthCount - 1) * Vector3.right * cellWidthInWC + (worldHeightCount - 1) * Vector3.up * cellHeightInWC;
            tile.localScale = Vector3.one;
            tile.name = "Tile " + (worldWidthCount - 1).ToString() + ',' + (worldHeightCount - 1).ToString();
            */


        //Debug.Log(mapPool[0, 0].GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
    }

    public static TileData GetTileDataByWorldPosition(Vector3 worldPosition)
    {
        //cast should be done last

        try
        {
            //"+1" is to ensure the correctness of getting GroundData
            return GroundRandomer.Self.GroundData[(int)(worldPosition.x * 100 + 1) / StaticCellWidth][(int)(worldPosition.y * 100 + 1) / StaticCellHeight];
        }
        catch(Exception e)
        {
            Debug.Log(worldPosition);
            Debug.LogError("GetTileDataByWorldPosition Error: " + e);
            return null;
        }
    }

    public static Transform GetMapPoolByTileData(TileData td)
    {
        //cast should be done last

        Debug.Log((int)(td.Position.x / CellWidthInWC) + " " + (int)(td.Position.y / CellHeightInWC));

        try
        {
            return mapPool[(int)(td.Position.x / CellWidthInWC), (int)(td.Position.y / CellHeightInWC)];
        }
        catch(Exception e)
        {
            Debug.LogError("GetMapPoolByTileData Error: " + e);
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

        if(type == MapConstants.LandformType.Grassland || type == MapConstants.LandformType.Forest)
        {
            if(td.MaterialTypes[1] == MapConstants.LandformType.None)
            {
                int width = (int)tdPosition.x % spriteHeight;
                int height = (int)tdPosition.y % spriteWidth;
                return landformSprites[(int)type, height * spriteWidth + width];
            }
            else
            {
                //important!! This should create more 2 width and height for SpriteCreate! Otherwise, the sprite will have some strange edges
                Texture2D t1 = new Texture2D(CellWidth + 2, CellHeight + 2);

                int type0 = (int)td.MaterialTypes[0], type1 = (int)td.MaterialTypes[1];

                if (type1 != (int)MapConstants.LandformType.Grassland && type1 != (int)MapConstants.LandformType.Forest)
                    return null;

                int width = (int)tdPosition.x % spriteHeight;
                int height = (int)tdPosition.y % spriteWidth;
                Sprite s0 = landformSprites[type0, height * spriteWidth + width];
                Sprite s1 = landformSprites[type1, height * spriteWidth + width];

                //this is an edge

                //Can an edge have two materialTypes only?

                //temp
                for (int k = 0; k < CellWidth; ++k)
                {
                    for (int l = 0; l < CellHeight; ++l)
                    {
                        try
                        {
                            t1.SetPixel(k, l, Interpolate(s0.texture.GetPixel(k, l), s1.texture.GetPixel(k, l), worldPerlinNoise[(int)tdPosition.x * CellWidth + k - 1][(int)tdPosition.y * CellHeight + l - 1]));
                            //t1.SetPixel(k, l, ChooseColor((int)type, worldPerlinNoise[(int)tdPosition.x * CellWidth + k - 1][(int)tdPosition.y * CellHeight + l - 1]));
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            t1.SetPixel(k, l, Color.white);
                        }
                    }
                }
                t1.Apply();

                return Sprite.Create(t1, new Rect(1, 1, CellWidth, CellHeight), Vector2.zero);
            }
        }
        else
        {
            return null;
        }

        /*
        else
        {
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
                        catch (IndexOutOfRangeException e)
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
                        catch (IndexOutOfRangeException e)
                        {
                            t1.SetPixel(k, l, Color.white);
                        }
                    }
                }
                t1.Apply();
            }

            return Sprite.Create(t1, new Rect(1, 1, CellWidth, CellHeight), Vector2.zero);
        }
        */
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

    void Display()
    {
        //to create the mapPool
        mapPool = new Transform[worldWidthCount, worldHeightCount];
        sightBottomLeft = new Vector3[preSightRange * preSightRange];

        //to decide the detail of new go

        bool isNotLeft = Role.position.x > minSightWidthBoundary, isNotRight = Role.position.x < maxSightWidthBoundary, isNotDown = Role.position.y > minSightHeightBoundary, isNotUp = Role.position.y < maxSightHeightBoundary;

        //overhead??
        //                                                                                 normal situation special situation: role is near boundary
        //SightList.parent = (isNotRoleNearBoundary = isNotLeft && isNotRight && isNotDown && isNotUp) ? Role : transform;
        //isNotRoleNearBoundary = isNotLeft && isNotRight && isNotDown && isNotUp;
        //int x = IsNotRoleNearBoundary ? -halfSightWidthCount : isNotLeft ? isNotRight ? (int)(Role.position.x) : worldWidthCount - sightWidthCount : 0;
        //int y = IsNotRoleNearBoundary ? -halfSightHeightCount : isNotDown ? isNotUp ? (int)(Role.position.y) : worldHeightCount - sightHeightCount : 0;
        int x = -halfSightWidthCount;
        int y = -halfSightHeightCount;
        //int nowXCount = (int)(Role.position.x * 100 + 1) / CellWidth, nowYCount = (int)(Role.position.y * 100 + 1) / CellHeight;
        //to plus x and y to ensure that Role will be surrounded by these tiles
        int startX = (int)(Role.position.x * 100 + 1) / CellWidth + x - PreSight * sightWidthCount, startY = (int)(Role.position.y * 100 + 1) / CellHeight + y - PreSight * sightHeightCount, tileX, tileY;
        int sightX, sightY;

        //to display the 9 sights
        
        for(int i = 0; i < preSightRange; ++i)
        {
            for(int j = 0; j < preSightRange; ++j)
            {
                sightX = startX + i * sightWidthCount;
                sightY = startY + j * sightHeightCount;

                for (int k = 0; k < sightWidthCount; ++k)
                {
                    for (int l = 0; l < sightHeightCount; ++l)
                    {
                        tileX = sightX + k;
                        tileY = sightY + l;

                        if (tileX >= 0 && tileX < worldWidthCount && tileY >= 0 && tileY < worldHeightCount)
                        {
                            //to initialize
                            tile = Instantiate(tile);
                            tile.parent = SightList;
                            tile.position = tileX * CellWidthInWC * Vector3.right + tileY * CellHeightInWC * Vector3.up;
                            tile.localScale = Vector3.one;
                            tile.name = "Tile " + tileX.ToString() + ',' + tileY.ToString();

                            tile.GetComponent<SpriteRenderer>().sprite = MakeSprite(tile.position);
                            Debug.Log(tile.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);

                            //to put tile into mapPool
                            mapPool[tileX, tileY] = tile;
                        }
                    }
                }

                sightBottomLeft[i + j * preSightRange] = sightX * CellWidthInWC * Vector3.right + sightY * CellHeightInWC * Vector3.up;
            }
        }
        
        InvokeRepeating("RefreshMap", 0f, MapRefreshTime);
    }

    void DisplayWorld()
    {        
        //to create the mapPool
        mapPool = new Transform[worldWidthCount, worldHeightCount];
        
        //to display world
        for (int i = 0; i < worldWidthCount; ++i)
        {
            for (int j = 0; j < worldHeightCount; ++j)
            {
                {
                    if(groundData[i][j] != null)
                    {
                        //to initialize
                        tile = Instantiate(tile);
                        tile.parent = SightList;
                        tile.position = i * CellWidthInWC * Vector3.right + j * CellHeightInWC * Vector3.up;
                        tile.localScale = Vector3.one;

                        //redundent
                        TileData td = GetTileDataByWorldPosition(tile.position);
                        tile.name = "Tile " + i.ToString() + ',' + j.ToString() + ' ' + td.MaterialTypes[0];

                        if(td.MaterialTypes[1] != MapConstants.LandformType.None)
                        {
                            tile.name += td.MaterialTypes[1];
                        }

                        tile.GetComponent<SpriteRenderer>().sprite = MakeSprite(tile.position);

                        //to put tile into mapPool
                        mapPool[i, j] = tile;
                    }
                    //else
                    //{
                        //Debug.Log("tiledata null");
                    //}
                }
            }
        }
    }

    void RefreshMap()
    {
        //to decide the moving direction
        Vector3 from = sightBottomLeft[(int)sightDirection.Center], to = Role.position;
        //Debug.Log(from + " " + to);
        float distanceX = to.x - from.x, distanceY = to.y - from.y;
        bool right = distanceX > sightWidthCount * CellWidthInWC, left = distanceX < 0, up = distanceY > sightHeightCount * CellHeightInWC, down = distanceY < 0; 

        //TileData newTileData = GetTileDataByWorldPosition(Role.position);
        //if (nowTileData != newTileData)
        if (true)
        {
            //bool isNotLeft = Role.position.x > minSightWidthBoundary, isNotRight = Role.position.x < maxSightWidthBoundary, isNotDown = Role.position.y > minSightHeightBoundary, isNotUp = Role.position.y < maxSightHeightBoundary;

            //to refresh the displayWorld
            //if (IsNotRoleNearBoundary = isNotLeft && isNotRight && isNotDown && isNotUp)
            if(right || left || up || down)
            {
                //Debug.Log("in");
                //normal situation

                //to decide the moving direction
                //Vector2 from = nowTileData.Position, to = newTileData.Position;
                //float distanceX = to.x - from.x, distanceY = to.y - from.y;

                //int nowXCount = (int)(Role.position.x * 100 + 1) / CellWidth, nowYCount = (int)(Role.position.y * 100 + 1) / CellHeight;

                int nowXCount = (int)(from.x * 100 + 1) / CellWidth, nowYCount = (int)(from.y * 100 + 1) / CellHeight;
                int newXCount = right ? nowXCount + (PreSight + 1) * sightWidthCount : left ? nowXCount - (PreSight + 1) * sightWidthCount : nowXCount - sightWidthCount;
                int oldXCount = right ? nowXCount - PreSight * sightWidthCount : left ? nowXCount + PreSight * sightWidthCount : 0;
                int newYCount = up ? nowYCount + (PreSight + 1) * sightHeightCount : down ? nowYCount - (PreSight + 1) * sightHeightCount : nowYCount - sightHeightCount;
                int oldYCount = up ? nowYCount - PreSight * sightHeightCount : down ? nowYCount + PreSight * sightHeightCount : 0;

                //two dimensions
                if ((right || left) && (up || down))
                    Debug.Log("twe dimensions");

                int indexX, indexY, sumX, sumY, cancelX, cancelY;

                //moving in x direction
                if (right || left)
                {
                    Vector3 newXVector = newXCount * CellWidthInWC * Vector3.right;
                    Vector3 newYUpVector = (newYCount + 2 * sightHeightCount) * CellHeightInWC * Vector3.up;
                    Vector3 newYMiddleVector = (newYCount + sightHeightCount) * CellHeightInWC * Vector3.up;
                    Vector3 newYDownVector = newYCount * CellHeightInWC * Vector3.up;

                    if (right)
                    {
                        sightBottomLeft[6] = sightBottomLeft[7]; sightBottomLeft[7] = sightBottomLeft[8]; sightBottomLeft[8] = newXVector + newYUpVector;
                        sightBottomLeft[3] = sightBottomLeft[4]; sightBottomLeft[4] = sightBottomLeft[5]; sightBottomLeft[5] = newXVector + newYMiddleVector;
                        sightBottomLeft[0] = sightBottomLeft[1]; sightBottomLeft[1] = sightBottomLeft[2]; sightBottomLeft[2] = newXVector + newYDownVector;
                    }
                    else
                    {
                        //left
                        sightBottomLeft[8] = sightBottomLeft[7]; sightBottomLeft[7] = sightBottomLeft[6]; sightBottomLeft[6] = newXVector + newYUpVector;
                        sightBottomLeft[5] = sightBottomLeft[4]; sightBottomLeft[4] = sightBottomLeft[3]; sightBottomLeft[3] = newXVector + newYMiddleVector;
                        sightBottomLeft[2] = sightBottomLeft[1]; sightBottomLeft[1] = sightBottomLeft[0]; sightBottomLeft[0] = newXVector + newYDownVector;
                    }

                    //Debug.Log(newXCount + " " + newYCount + " " + oldXCount);

                    indexY = newYCount;
                    for (int i = 0; i < 3; ++i)
                    {
                        for (int j = 0; j < sightWidthCount; ++j)
                        {
                            sumX = newXCount + j;
                            cancelX = oldXCount + j;

                            for (int k = 0; k < sightHeightCount; ++k)
                            {
                                sumY = indexY + k;

                                try
                                {
                                    //to display
                                    if (mapPool[sumX, sumY] != null)
                                    {
                                        mapPool[sumX, sumY].gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        //to create new tile into mapPool
                                        mapPool[sumX, sumY] = tile = Instantiate(tile);
                                        tile.parent = SightList;
                                        tile.position = sumX * CellWidthInWC * Vector3.right + sumY * CellHeightInWC * Vector3.up;
                                        tile.localScale = Vector3.one;
                                        tile.name = "Tile " + sumX.ToString() + ',' + sumY.ToString();

                                        tile.GetComponent<SpriteRenderer>().sprite = MakeSprite(tile.position);
                                    }
                                }
                                catch(IndexOutOfRangeException e)
                                {
                                    Debug.Log("RefreshMap: Display OutOfRange\n" + e);
                                }

                                try
                                {
                                    //to cancel
                                    mapPool[cancelX, sumY].gameObject.SetActive(false);
                                }
                                catch(IndexOutOfRangeException e)
                                {
                                    Debug.Log("RefreshMap: Cancel OutOfRange\n" + e);
                                }
                            }
                        }
                        indexY += sightHeightCount;
                    }
                }

                
                //moving in y direction
                if (up || down)
                {
                    Vector3 newYVector = newYCount * CellHeightInWC * Vector3.up;
                    Vector3 newXRightVector = (newXCount + 2 * sightWidthCount) * CellWidthInWC * Vector3.right;
                    Vector3 newXMiddleVector = (newXCount + sightWidthCount) * CellWidthInWC * Vector3.right;
                    Vector3 newXLeftVector = newXCount * CellWidthInWC * Vector3.right;

                    if (up)
                    {
                        sightBottomLeft[0] = sightBottomLeft[3]; sightBottomLeft[1] = sightBottomLeft[4]; sightBottomLeft[2] = sightBottomLeft[5];
                        sightBottomLeft[3] = sightBottomLeft[6]; sightBottomLeft[4] = sightBottomLeft[7]; sightBottomLeft[5] = sightBottomLeft[8];
                        sightBottomLeft[6] = newXLeftVector + newYVector; sightBottomLeft[7] = newXMiddleVector + newYVector; sightBottomLeft[8] = newXRightVector + newYVector;
                    }
                    else
                    {
                        //down
                        sightBottomLeft[6] = sightBottomLeft[3]; sightBottomLeft[7] = sightBottomLeft[4]; sightBottomLeft[8] = sightBottomLeft[5];
                        sightBottomLeft[3] = sightBottomLeft[0]; sightBottomLeft[4] = sightBottomLeft[1]; sightBottomLeft[5] = sightBottomLeft[2];
                        sightBottomLeft[0] = newXLeftVector + newYVector; sightBottomLeft[1] = newXMiddleVector + newYVector; sightBottomLeft[2] = newXRightVector + newYVector;
                    }

                    //Debug.Log(newXCount + " " + newYCount + " " + oldYCount);

                    indexX = newXCount;
                    for (int i = 0; i < 3; ++i)
                    {
                        for (int j = 0; j < sightWidthCount; ++j)
                        {
                            sumX = indexX + j;

                            for (int k = 0; k < sightHeightCount; ++k)
                            {
                                sumY = newYCount + k;
                                cancelY = oldYCount + k;

                                try
                                {
                                    //to display
                                    if (mapPool[sumX, sumY] != null)
                                    {
                                        mapPool[sumX, sumY].gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        //to create new tile into mapPool
                                        mapPool[sumX, sumY] = tile = Instantiate(tile);
                                        tile.parent = SightList;
                                        tile.position = sumX * CellWidthInWC * Vector3.right + sumY * CellHeightInWC * Vector3.up;
                                        tile.localScale = Vector3.one;
                                        tile.name = "Tile " + sumX.ToString() + ',' + sumY.ToString();

                                        tile.GetComponent<SpriteRenderer>().sprite = MakeSprite(tile.position);
                                    }
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                    Debug.Log("RefreshMap: Display OutOfRange\n" + e);
                                }

                                try
                                {
                                    //to cancel
                                    mapPool[sumX, cancelY].gameObject.SetActive(false);
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                    Debug.Log(sumX + " " + cancelY + " " + sumY);
                                    Debug.Log("RefreshMap: Cancel OutOfRange\n" + e);
                                }
                                catch (NullReferenceException ne)
                                {
                                    Debug.Log(sumX + " " + cancelY + " " + sumY);
                                    Debug.Log("RefreshMap: Cancel Null\n" + ne);
                                }
                            }
                        }

                        indexX += sightWidthCount;
                    }
                }
            }

            //nowTileData = newTileData;
        }

        /*
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
                            sight[i - 1][j].GetComponent<SpriteRenderer>().sprite = sight[i][j].GetComponent<SpriteRenderer>().sprite;

                    //new sight
                    for (int i = 0; i < sightHeightCount; ++i)
                        sight[tempX][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(sight[tempX][i].position);
                }
                else if (distanceX < 0f)
                {
                    //left direction

                    //transit
                    for (int i = sightWidthCount - 2; i >= 0; --i)
                        for (int j = 0; j < sightHeightCount; ++j)
                            sight[i + 1][j].GetComponent<SpriteRenderer>().sprite = sight[i][j].GetComponent<SpriteRenderer>().sprite;

                    //new sight
                    for (int i = 0; i < sightHeightCount; ++i)
                        sight[0][i].GetComponent<SpriteRenderer>().sprite = MakeSprite(sight[0][i].position);
                }

                if (distanceY > 0f)
                {
                    //up direction

                    //transit
                    for (int i = 0; i < sightWidthCount; ++i)
                        for (int j = 1; j < sightHeightCount; ++j)
                            sight[i][j - 1].GetComponent<SpriteRenderer>().sprite = sight[i][j].GetComponent<SpriteRenderer>().sprite;

                    //new sight
                    for (int i = 0; i < sightWidthCount; ++i)
                        sight[i][tempY].GetComponent<SpriteRenderer>().sprite = MakeSprite(sight[i][tempY].position);
                }
                else if (distanceY < 0f)
                {
                    //down direction

                    //transit
                    for (int i = 0; i < sightWidthCount; ++i)
                        for (int j = sightHeightCount - 2; j >= 0; --j)
                            sight[i][j + 1].GetComponent<SpriteRenderer>().sprite = sight[i][j].GetComponent<SpriteRenderer>().sprite;

                    //new sight
                    for (int i = 0; i < sightWidthCount; ++i)
                        sight[i][0].GetComponent<SpriteRenderer>().sprite = MakeSprite(sight[i][0].position);
                }

                nowTileData = newTileData;
            }
        }
        */
    }

    float Interpolate(float x0, float x1, float alpha)
    {
        return x0 * (1 - alpha) + x1 * alpha;
    }

    Color Interpolate(Color c0, Color c1, float alpha)
    {
        //return alpha > .5f ? c1 : c0;

        return c0 * (1 - alpha) + c1 * alpha;
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
        //RefreshMap();
    }

    public static float StaticWorldWidthInWC, StaticWorldHeightInWC, CellWidthInWC, CellHeightInWC;
    public static int StaticCellWidth, StaticCellHeight, StaticSightWidth, StaticSightHeight, StaticWorldWidth, StaticWorldHeight;

    //SightList localPosition must be 0, 0, 0
    public Transform Role, SightList;
    public int WorldWidth, WorldHeight, CellWidth, CellHeight, SightWidth, SightHeight, PreSight, MaxStack, LandformTextureWidth, LandformTextureHeight;
    public float DistanceThreshold, MapRefreshTime;

    static Transform[,] mapPool;

    bool IsNotRoleNearBoundary
    {
        get { return isNotRoleNearBoundary; }

        set
        {
            /*
            if(isNotRoleNearBoundary != value)
            {
                SightList.parent = value ? Role : transform;

                
                for (int i = 0; i < sightWidthCount; ++i)
                    for (int j = 0; j < sightHeightCount; ++j)
                    {
                        tf = sight[i][j];
                        
                        tf.parent = parent;
                        tf.localScale = Vector3.one;
                    }
                

                Debug.Log(value ? "displaySight is moving" : "displaySight is stand still");
                isNotRoleNearBoundary = value;
            }
            */
            isNotRoleNearBoundary = value;
        }
    }

    enum sightDirection
    {
        BottomLeft,
        Bottom,
        BottomRight,
        Left,
        Center,
        Right,
        TopLeft,
        Top,
        TopRight
    }

    Transform[][] sight, map;
    TileData[][] groundData;
    Sprite[,] landformSprites;
    Vector3[] sightBottomLeft;
    float[][] worldNoise, worldPerlinNoise;

    //TileData nowTileData;
    Transform tile;
    string landformSpriteDirectoryPath = @"Map\";
    //InWC
    float minSightWidthBoundary, maxSightWidthBoundary, minSightHeightBoundary, maxSightHeightBoundary;
    int worldWidthCount, worldHeightCount, sightWidthCount, sightHeightCount, halfSightWidthCount, halfSightHeightCount, preSightRange, landformTypeAmount = MapConstants.LandformTypeAmount;
    int spriteWidth, spriteHeight;
    bool isNotRoleNearBoundary;
}