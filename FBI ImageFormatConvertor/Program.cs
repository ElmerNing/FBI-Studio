using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ImageFormatConvertor
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] _ags)
        {
            ags = _ags;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static public string[] ags = null;
    }
}
