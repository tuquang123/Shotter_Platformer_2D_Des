using System;
using System.IO;
using System.Xml.Serialization;

public static class ByteConverter
{
    private static byte[] emptyData = new byte[0];

    public static byte[] SerializeObject<T>(T serializableObject)
    {
        if (serializableObject == null)
        {
            DebugCustom.Log("Serialize Object is NULL");
            return emptyData;
        }
        else
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer x = new XmlSerializer(typeof(T));
                x.Serialize(stream, serializableObject);

                return stream.ToArray();
            }
        }
    }

    public static T DeserializeObject<T>(byte[] serilizedBytes)
    {
        XmlSerializer x = new XmlSerializer(typeof(T));

        using (MemoryStream stream = new MemoryStream(serilizedBytes))
        {
            try
            {
                return (T)x.Deserialize(stream);
            }
            catch (Exception e)
            {
                DebugCustom.LogError("Read data error: " + e.Message);
                return default(T);
            }
        }
    }
}