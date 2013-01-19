using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace Focus
{
    [Serializable]
    public abstract class XmlSerializerBase
    {
        /// <summary>
        /// 提供一个默认路径的XML序列化抽象接口
        /// </summary>
        /// <returns></returns>
        public abstract void SerializeXML();

        /// <summary>
        /// 提供一个默认路径的XML反序列化抽象接口
        /// </summary>
        /// <returns></returns>
        public abstract Object DeserializeXML();

        public void SerializeXML(string path)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(this.GetType());
                Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                xs.Serialize(stream, this);
                stream.Close();
            }
            catch (System.Exception)
            {

            }
        }

        public Object DeserializeXML(string path)
        {
            Type thisType = this.GetType();
            object config;
            try
            {
                XmlSerializer xs = new XmlSerializer(thisType);
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                config = xs.Deserialize(stream);
                stream.Close();
                return config;
            }
            catch (System.Exception)
            {
                Assembly asm = Assembly.GetAssembly(this.GetType());
                config = asm.CreateInstance(this.GetType().ToString(), true);
                return config;
            }
        }
    }
}
