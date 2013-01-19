using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Focus
{
    partial class C54XXAPI
    {
        [DllImport("C54XX_IMAGE_PREPROCESS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void C5409_Init(byte[] irDat, UInt32 irDatLen);

        [DllImport("C54XX_IMAGE_PREPROCESS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float C5409_GetW();

        [DllImport("C54XX_IMAGE_PREPROCESS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float C5409_GetH();

        [DllImport("C54XX_IMAGE_PREPROCESS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void C5409_GetImg(byte[] img, int x, int y, int width, int height, int stride);

        [DllImport("C54XX_IMAGE_PREPROCESS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void C5409_GethistequalImg(byte[] img, int x, int y, int width, int height, int stride);
    }
    partial class DM642API
    {
        [DllImport("DM642_IMAGE_PREPROCESS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WinImageInitPlus(IntPtr[] pData, int[] iDatLen, short sDataNum, int processIndex, int isReflect);
        [DllImport("DM642_IMAGE_PREPROCESS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RMB_getEHeight(int Index);
        [DllImport("DM642_IMAGE_PREPROCESS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RMB_getEWidth(int Index);
        [DllImport("DM642_IMAGE_PREPROCESS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RmbGetReviseImgByStride(byte[] img, int x, int y, int width, int height, int stride, int picIndex);
        [DllImport("DM642_IMAGE_PREPROCESS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short GetFivePointsAverageGrey(short x, short y, int picIndex);
        [DllImport("DM642_IMAGE_PREPROCESS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetHist(int[] hist, int picIndex);
    }


}
