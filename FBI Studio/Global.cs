using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBI_Studio
{
    public enum PointFormat
    {
        DM642,
        C54XX,
    }

    public class Global
    {
        //工作目录文件管理以及图片索引
        public static FileManage WorkFileManage = null;
        public static int WorkPicIndex = 0; //0:Ir, 1 Gr
        //辅助目录文件管理以及图片索引
        public static FileManage SuplyFileManage = null;
        public static int SuplyPicIndex = 0;//0:Ir, 1:Gr

        //标志当前点文件保存格式!
        public static PointFormat CurrentPointFormat = PointFormat.DM642;
    }
}
