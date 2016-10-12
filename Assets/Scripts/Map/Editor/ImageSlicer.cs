using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public static class ImageSlicer
{
    [MenuItem("Editor/ImageSlicer/Process to Sprites")]
    static void ProcessToSprite()
    {
        //to get the current selected object
        Texture2D image = Selection.activeObject as Texture2D;
        //to get the directory path
        string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(image));

        Debug.Log(rootPath);
        //to get the name of image
        string name = image.name;
        //to get the path of image
        string path = rootPath + "/" + name + ".png";
        //to get the slices directory
        string slicesDirectory = name + "Slices";

        //to get the access of image
        TextureImporter texImp = (TextureImporter)AssetImporter.GetAtPath(path);

        //to create the direction folder
        if(!Directory.Exists(rootPath + @"\" + slicesDirectory))
        {
            Debug.Log("slicesDirectory does not exist");
            AssetDatabase.CreateFolder(rootPath, slicesDirectory);
        }

        //to search metadata in image to find all spriteslices
        int length = texImp.spritesheet.Length;
        //to ensure that length is square
        int width = (int)Mathf.Sqrt(length);
        int maxRow = width - 1;

        for(int i = 0; i < length; ++i)
        {
            SpriteMetaData metaData = texImp.spritesheet[i];

            //to create new image slices
            Texture2D myimage = new Texture2D((int)metaData.rect.width, (int)metaData.rect.height);

            for (int y = (int)metaData.rect.y; y < metaData.rect.y + metaData.rect.height; y++)//Y轴像素
            {
                for (int x = (int)metaData.rect.x; x < metaData.rect.x + metaData.rect.width; x++)
                    myimage.SetPixel(x - (int)metaData.rect.x, y - (int)metaData.rect.y, image.GetPixel(x, y));
            }

            //to ensure that new image slices can encode to png
            if (myimage.format != TextureFormat.ARGB32 && myimage.format != TextureFormat.RGB24)
            {
                Texture2D newTexture = new Texture2D(myimage.width, myimage.height);
                newTexture.SetPixels(myimage.GetPixels(0), 0);
                myimage = newTexture;
            }
            
            var pngData = myimage.EncodeToPNG();

            //to rename
            int no = int.Parse(metaData.name.Split('_')[1]);
            int newNo = (maxRow - no / width) * (width) + no % width;
            string newPath = rootPath + @"\" + slicesDirectory + @"\" + name + '_' + newNo.ToString() + ".png";

            //AssetDatabase.CreateAsset(myimage, rootPath + "/" + image.name + "/" + metaData.name + ".PNG");
            File.WriteAllBytes(newPath, pngData);


        }

        //to refresh the interface in unity
        AssetDatabase.Refresh();

        for (int i = 0; i < length; ++i)
        {
            //to change sprite pivot to bottom left
            texImp = (TextureImporter)AssetImporter.GetAtPath(rootPath + @"\" + slicesDirectory + @"\" + name + '_' + i.ToString() + ".png");
            texImp.textureType = TextureImporterType.Advanced;
            texImp.isReadable = true;
            TextureImporterSettings texSettings = new TextureImporterSettings();
            texImp.ReadTextureSettings(texSettings);
            //texSettings.textur
            texSettings.spriteAlignment = (int)SpriteAlignment.BottomLeft;
            texImp.SetTextureSettings(texSettings);
            
            texImp.SaveAndReimport();
            //Debug.Log(texImp.spritePivot);
        }

        //to refresh the interface in unity
        AssetDatabase.Refresh();


        /*
        foreach (SpriteMetaData metaData in texImp.spritesheet)
        {
            //to create new image slices
            Texture2D myimage = new Texture2D((int)metaData.rect.width, (int)metaData.rect.height);
            
            for (int y = (int)metaData.rect.y; y < metaData.rect.y + metaData.rect.height; y++)//Y轴像素
            {
                for (int x = (int)metaData.rect.x; x < metaData.rect.x + metaData.rect.width; x++)
                    myimage.SetPixel(x - (int)metaData.rect.x, y - (int)metaData.rect.y, image.GetPixel(x, y));
            }
            
            //to ensure that new image slices can encode to png
            if (myimage.format != TextureFormat.ARGB32 && myimage.format != TextureFormat.RGB24)
            {
                Texture2D newTexture = new Texture2D(myimage.width, myimage.height);
                newTexture.SetPixels(myimage.GetPixels(0), 0);
                myimage = newTexture;
            }
            var pngData = myimage.EncodeToPNG();


            //AssetDatabase.CreateAsset(myimage, rootPath + "/" + image.name + "/" + metaData.name + ".PNG");
            File.WriteAllBytes(rootPath + "/" + image.name + "/" + metaData.name + ".png", pngData);
            //to refresh the interface in unity
            AssetDatabase.Refresh();
        }
        */
    }
}
