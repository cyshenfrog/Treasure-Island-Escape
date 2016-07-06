using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public static class AutoSerializer {
    public static void Save<T>(object obj, string path)
    {
        CreateXML(SerializeObject<T>(obj), path);
    }
    public static T Load<T>(string path, Type type)
    {
        string data = LoadXML(path);
        if (data == null)
        {
            return default(T);
        }
        return (DeserializeObject<T>(data, type));
    }

    public static string SerializeObject<T>(object pObject)
    {
        string XmlizedString = null;
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(T));//需要更改類別
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }
    public static T DeserializeObject<T>(string pXmlizedString, Type type)
    {
        XmlSerializer xs;
        MemoryStream memoryStream;
        object o;
        try
        {
            xs = new XmlSerializer(type);//需要更改類別
            memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
            o = xs.Deserialize(memoryStream);
        }
        catch { return default(T); }
        // XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        return (T)o;
    }

    //-----------------------------基礎內部function
    private static string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }
    private static byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }
    private static void CreateXML(string data, string fileLocation)
    {
        StreamWriter writer;
        FileInfo t = new FileInfo(fileLocation);
        if (!t.Exists)
        {
            writer = t.CreateText();
        }
        else
        {
            t.Delete();
            writer = t.CreateText();
        }
        writer.Write(data);
        writer.Close();

    }
    private static string LoadXML(string fileLocation)
    {
        if (File.Exists(fileLocation))
        {
            StreamReader r = File.OpenText(fileLocation);
            string info = r.ReadToEnd();
            r.Close();

            return info;
        }
        return null;
    }
}
