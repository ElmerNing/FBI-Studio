using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Focus
{
    public enum ConfigFormat { DM642, C54XX, FOCUS };   //3种格式
    public enum ConfigLightNum { Single, Double, Triple };  //三光源，双光源和单光源
    public enum ConfigOutput { Ir, Green,Uv, All };    //Ir，Green，Uv代表只转换一种光源的图片， All转换所有光源的图片
    public enum ConfigReviseMode { Penetrate, Reflect };    //透射校正算法, 反射校正算法
    public enum ConfigBasicImg { Ir, Green };   //双光源时, 校正基准图

    public abstract class OpenConfig : XmlSerializerBase
    {
        public ConfigFormat format = ConfigFormat.DM642;
        public ConfigLightNum lightNum = ConfigLightNum.Double;
        public ConfigOutput Output = ConfigOutput.All;
        public ConfigReviseMode reviseMode = ConfigReviseMode.Reflect;
        public ConfigBasicImg basicImg = ConfigBasicImg.Ir;

        //配置双光源红外图像或单光源图像的取点模式
        public bool irEqual = true;         //均衡化
        public bool irNeighbor = true;  //领域均值

        //配置双光源和三光源下绿光图像的取点模式
        public bool uvgrEqual = true; 
        public bool uvgrNeighbor = true;


        public abstract override void SerializeXML();
        public abstract override Object DeserializeXML();
    }

    public class DM642OpenConfig : OpenConfig
    {
        public DM642OpenConfig()
        {
            this.format = ConfigFormat.DM642;
            this.lightNum = ConfigLightNum.Double;
            this.Output = ConfigOutput.All;
            this.reviseMode = ConfigReviseMode.Penetrate;
            this.basicImg = ConfigBasicImg.Ir;
            this.irEqual = true;
            this.irNeighbor = true;
            this.uvgrEqual = true;
            this.uvgrNeighbor = true;
        }

        public override void SerializeXML()
        {
            if (!Directory.Exists("Config"))
                Directory.CreateDirectory("Config");
            SerializeXML("Config\\DM642OpenConfig.xml");
        }

        public override object DeserializeXML()
        {
            if (!Directory.Exists("Config"))
                Directory.CreateDirectory("Config");
            return DeserializeXML("Config\\DM642OpenConfig.xml");
        }
    }

    public class C54XXOpenConfig : OpenConfig
    {
        public C54XXOpenConfig()
        {
            this.format = ConfigFormat.C54XX;
            this.lightNum = ConfigLightNum.Single;
            this.Output = ConfigOutput.All;
            this.reviseMode = ConfigReviseMode.Penetrate;
            this.basicImg = ConfigBasicImg.Ir;
            this.irEqual = true;
            this.irNeighbor = true;
            this.uvgrEqual = true;
            this.uvgrNeighbor = true;
        }

        public override void SerializeXML()
        {
            if (!Directory.Exists("Config"))
                Directory.CreateDirectory("Config");
            SerializeXML("Config\\C54XXOpenConfig.xml");
        }

        public override object DeserializeXML()
        {
            if (!Directory.Exists("Config"))
                Directory.CreateDirectory("Config");
            return DeserializeXML("Config\\C54XXOpenConfig.xml");
        }
    }



}
