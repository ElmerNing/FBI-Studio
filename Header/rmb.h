#ifndef _RMB_H_
#define _RMB_H_

#include "..\Header\Focus.h"

//#define CHARPDLL
#ifndef CHIP_DM642
	#include "..\Header\Win32rts.h"
	#define DllExport __declspec(dllexport)
#else
	#define DllExport
#endif


#ifdef __cplusplus
extern "C"{
#endif

extern int CurrentImageCount;

extern int RMB_EWidth;
extern int RMB_EHeight; 

//Utility
extern DllExport int ImageCheck(const short* Points,int PointsNum,int turn,int picIndex);
extern DllExport void GetRectImage(unsigned char* Img , int x,int y,int width,int height,int turn,int picIndex);
extern DllExport int MinDistanceSearch(unsigned char* srcImg,int srcWidth,int srcHeight,unsigned char* model,int modelWidth,int modelHeight);
extern DllExport void AdaptiveThreshold(unsigned char* srcImg,int width,int height);
extern DllExport int ImgDistance(unsigned char* srcImg,int srcWidth,int x,int y,int modelWidth,int modelHeight,unsigned char* model);
extern DllExport unsigned char GetRotatePixel(int w,int h,int picIndex);
extern DllExport short GetRotate5Pixel(int w,int h,int picIndex);
extern DllExport short GetFivePointsAverageGrey(short x, short y,int picIndex);
extern DllExport void GetImgRectFast(unsigned char* restrict srcImg,int deno, int x0, int y0, int width, int height,int index0);
extern DllExport void GetImgRectFastByCol(unsigned char* restrict srcImg, int deno, int y0, int x0, int height, int width,int index0);
extern DllExport void GetRotateImageByMul(unsigned char* restrict img, int x, int y, int width, int height,int face);

extern DllExport unsigned char GetRotatePixelByMul(int width, int height);
extern DllExport void GetHist(int* _hist,int picIndex);
extern DllExport void HistEqualFast(int picIndex);
extern DllExport int PreProcessInit(IMG_BuffObj imageObj, int processIndex , int isReflect);
extern DllExport int RMB_PreProcess();
extern DllExport int RMB_GetMillimeterWidth();

#ifndef CHIP_DM642
extern DllExport int   RMB_getEWidth(int Index);
extern DllExport int RMB_getEHeight(int Index);
extern DllExport int   RMB_getLeanFlag();
extern DllExport short GetFivePointsAverageGrey(short x, short y,int picIndex);
extern DllExport int WinImageInitPlus(unsigned char* pData[3], int iDatLen[3],short sDataNum, int processIndex, int isReflect);
extern DllExport void GetHist(int* _hist,int picIndex);
extern DllExport void GetPoints(FocusPoint* tl,FocusPoint* tr,FocusPoint* bl,FocusPoint* br);
extern DllExport void RmbGetReviseImg(unsigned char* img, int x, int y, int width, int height,int picIndex);
extern DllExport void RmbGetReviseImgByStride(unsigned char* img,int x, int y, int width, int height, int stride,int picIndex);
#endif

#ifdef __cplusplus
};
#endif

#endif

