namespace Shade.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    public static class ExtensionsSerialization
    {
        public static string ToJson(this object instance, int recursionlimit = 100)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 1000000000;
            serializer.RecursionLimit = recursionlimit;
            return serializer.Serialize(instance);
        }

        public static void ToJsonFile(this object instance, string filepath, int recursionlimit = 100)
        {
            string json = instance.ToJson(recursionlimit);
            StreamWriter writer = new StreamWriter(filepath);
            writer.Write(json);
            writer.Flush();
            writer.Close();
        }

        public static T ToObjectFromJson<T>(this string json)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 1000000000;
            return serializer.Deserialize<T>(json);
        }

        public static TType ToObjectFromFileJson<TType>(this string filename)
        {
            string json = string.Empty;

            using (StreamReader reader = new StreamReader(filename))
            {
                json = reader.ReadToEnd();
                reader.Close();
            }

            return (TType)json.ToObjectFromJson<TType>();
        }

        /// <summary>
        /// Deserializes a string of xml into an object og TType
        /// </summary>
        /// <typeparam name="TType">the target type</typeparam>
        /// <param name="xml">the xml data</param>
        /// <returns></returns>
        public static TType ToObjectFromXml<TType>(this string xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(TType));
            MemoryStream memoryStream = new MemoryStream(xml.ToUTF8ByteArray());
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (TType)xs.Deserialize(memoryStream);
        }

        /// <summary>
        /// Serializes an object into a string of xml
        /// </summary>
        /// <param name="instance">the object to serialize</param>
        /// <param name="humanreadable">if set to false then the resulting xml will be on a single line</param>
        /// <returns></returns>
        public static string ToXml(this object instance, bool humanreadable = true)
        {
            try
            {
                string xmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(instance.GetType());

                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Indent = false,
                    NewLineOnAttributes = false
                };

                XmlWriter writer = XmlWriter.Create(memoryStream, settings);

                xs.Serialize(writer, instance);
                xmlizedString = memoryStream.ToArray().ToStringUTF8();
                return xmlizedString;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return null;
            }
        }

        public static void ToXmlFile(this object instance, string filename, bool humanreadable = true)
        {
            string xml = instance.ToXml(humanreadable);
            StreamWriter writer = new StreamWriter(filename);
            writer.Write(xml);
            writer.Flush();
            writer.Close();
        }

        public static TType ToObjectFromFileXml<TType>(this string filename)
        {
            string xml = string.Empty;

            using (StreamReader reader = new StreamReader(filename))
            {
                xml = reader.ReadToEnd();
                reader.Close();
            }

            return (TType)xml.ToObjectFromXml<TType>();
        }

        public static string ToStringUTF8(this byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return constructedString;
        }

        public static byte[] ToUTF8ByteArray(this string pxmlstring)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pxmlstring);
            return byteArray;
        }

        /// <summary>
        /// Serializes an object to a binary file. The object must be tagged [Serializable()] attribute 
        /// </summary>
        /// <param name="obj">the instance to serialize, must be tagged with [Serializable()] attribute</param>
        /// <param name="path">the path of the output file</param>
        public static void ToBinary(this object obj, string path)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, obj);
                stream.Close();
            }
        }

        /// <summary>
        /// Creates a TType Object from a binaryfile using deserialization
        /// </summary>
        /// <typeparam name="TType">the type of the object</typeparam>
        /// <param name="binarypath">the path of the binary file</param>
        /// <returns></returns>
        public static TType ToObjectFromBinaryFile<TType>(this string binarypath)
        {
            using (Stream stream = File.Open(binarypath, FileMode.Open))
            {
                BinaryFormatter bformatter = new BinaryFormatter();

                TType obj = (TType)bformatter.Deserialize(stream);
                stream.Close();

                return obj;
            }
        }

        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            bf.Serialize(stream, obj);
            return stream.ToArray();
        }

        public static TType ToObjectFromByteArray<TType>(this byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            TType obj = (TType)bf.Deserialize(stream);
            stream.Dispose();
            return obj;
        }
    }
}
