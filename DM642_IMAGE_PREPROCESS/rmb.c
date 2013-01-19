#include "..\Header\rmb.h"

/*
 * 图像算法模块
 */

#define MAXLINE 600                                                          //图象行数的上限
#define DispersionRMB_Width 20                                                //边界搜索横向步长(搜索左边界时的“横向”指的是竖直方向步长)
#define SearchStepofHeight 5                                                //边界搜索纵向步长(搜索左边界时的“纵向”指的是竖水平向步长)

#define maxNodesCount  240                                                    //确定边界所用点的最大值
#define Y_OFFSET       3                                                      //对原始图像上下边界裁掉的距离
#define RMB_inclinationThreshold 10                                           //左边线和上边线夹角的正弦值上限，小于这个值说明发生了横向侧移

#define MAXDATLENGTH IMG_FRAME_SIZE*MAXLINE

#ifdef  CHIP_DM642
#pragma DATA_SECTION(SrcColGrey, ".internal");
#endif
short SrcColGrey[IMG_FRAME_SIZE];

#ifndef CHIP_DM642
unsigned char srcImg[3][MAXDATLENGTH];
int deno = 0xA;
#endif

int RMB_L_X_OFFSET;		//对原始图像左右边界裁掉的距离
int RMB_R_X_OFFSET;		//对原始图像左右边界裁掉的距离
int RMB_upThresholdGrey ;		//边界阀值上限 
int RMB_downThresholdGrey ;	//边界阀值下限
int ReflectThr = 15;

/// <summary>
/// 图像数目,默认情况下只有1,2两种
/// </summary>
int CurrentImageCount;

FocusPoint RMB_Top;

/// 是否为反射图像，不是则为透射图像
static int isCurrentReflect = 0;

int ImageRotateIndex = 0;
unsigned char* image;				//用来处理边界的图像

#ifdef  CHIP_DM642
#pragma DATA_SECTION(focusImage, ".internal");
#endif
FocusImage focusImage[3];

/// <summary>
/// 水平与坚直方向的斜率
/// </summary>
double RMB_horizontal_rate;
double RMB_vertical_rate;

FocusPoint  RMB_pointUpMid,RMB_pointDownMid,RMB_pointLeftMid,RMB_pointRightMid;

#ifdef  CHIP_DM642
#pragma DATA_SECTION(PtCorrectFirstHeight, ".internal");
#endif
FocusPoint  PtCorrectFirstHeight[IMG_FRAME_SIZE];			//第一行图像在校正前图像中的坐标   

#ifdef  CHIP_DM642
#pragma DATA_SECTION(PtCorrectFirstWidth,".internal");
#endif
FocusPoint PtCorrectFirstWidth[MAXLINE];             //第一列图像在校正前图像中的坐标 

double RMB_cosa = 1;                                              //倾斜角度余弦值 
double RMB_sina = 0;                                              //倾斜角度正弦值 

double RMB_cosa400 = 1;
double RMB_sina400 = 0;

int    RMB_fixcosa = 1;
int	   RMB_fixcosa400 = 1;

int    RMB_fixsina = 0;
int	   RMB_fixsina400 = 0;

double RMB_inclination;                                           //左边线和上边线夹角的正弦

#ifdef  CHIP_DM642
#pragma DATA_SECTION(RMB_xx,".internal");
#endif
int  RMB_xx[maxNodesCount];

#ifdef  CHIP_DM642
#pragma DATA_SECTION(RMB_yy,".internal");
#endif
int  RMB_yy[maxNodesCount];

int ScanNodesCount = 0;

#ifdef  CHIP_DM642
#pragma DATA_SECTION(HorizontalScanNode,".internal");
#endif
VDISNODE HorizontalScanNode[maxNodesCount];

#ifdef  CHIP_DM642
#pragma CODE_SECTION(RMB_getIrPixel,".rmbProcessSec");
#endif
UINT8 RMB_getIrPixel(int row,int col);

#ifdef  CHIP_DM642
#pragma CODE_SECTION(GetRotatePixel,".rmbProcessSec");
#endif
UINT8 GetRotatePixel(int w,int h,int picIndex);

#ifdef  CHIP_DM642
#pragma CODE_SECTION(GetRotate5Pixel,".rmbProcessSec");
#endif
short GetRotate5Pixel(int w,int h,int picIndex);

#ifdef  CHIP_DM642
#pragma CODE_SECTION(GetFivePointsAverageGrey,".rmbProcessSec");
#endif
short GetFivePointsAverageGrey(short x, short y,int picIndex);

#ifdef  CHIP_DM642
#pragma CODE_SECTION(SetTheOtherFocusImageParams,".rmbProcessSec");
#endif
void SetTheOtherFocusImageParams();

#ifdef  CHIP_DM642
#pragma CODE_SECTION(GetRotatePixelByMul,".rmbProcessSec");
#endif
UINT8 GetRotatePixelByMul(int w,int h);

#ifdef  CHIP_DM642
#pragma CODE_SECTION(RMB_GetRawPixel,".rmbProcessSec");
#endif
UINT8 RMB_GetRawPixel(int row, int col);

#ifdef  CHIP_DM642
#pragma CODE_SECTION(RMB_PreProcess,".rmbProcessSec");
#endif
int   RMB_PreProcess();

#ifdef  CHIP_DM642
#pragma CODE_SECTION(RMB_FindXoffset,".rmbProcessSec");
#endif
/// <summary>
/// 去除左右两黑边
/// <summary>
int   RMB_FindXoffset();

#ifdef  CHIP_DM642
#pragma CODE_SECTION(RMB_FindAcme,".rmbProcessSec");
#endif
int   RMB_FindAcme();

#ifdef  CHIP_DM642
#pragma CODE_SECTION(RMB_CalculateLineKB,".rmbProcessSec");
#endif
int   RMB_CalculateLineKB(int*  xList, int  * yList, int lCount,  double *k );

#ifdef  CHIP_DM642
#pragma CODE_SECTION(RMB_OrientationHorizontal,".rmbProcessSec");
#endif
int   RMB_OrientationHorizontal();

#ifdef  CHIP_DM642
#pragma CODE_SECTION(RMB_OrientationVertical,".rmbProcessSec");
#endif
int   RMB_OrientationVertical();

#ifdef  CHIP_DM642
#pragma CODE_SECTION(RMB_GetMillimeterWidth,".rmbProcessSec");
#endif
int   RMB_GetMillimeterWidth();

#ifdef  CHIP_DM642
#pragma CODE_SECTION(GetRotateImageByMul,".rmbProcessSec");
#endif
void GetRotateImageByMul(unsigned char* img, int x, int y, int width, int height,int face);

/// <summary>
/// 将长度从像素计量转换为实际的毫米计量大小
/// </summary>
/// <returns>长度:毫米单位</returns>
int RMB_GetMillimeterWidth()
{
    return (int)( ( (double)RMB_EWidth * 25.4 ) / 100);
}

#ifdef  CHIP_DM642
#pragma CODE_SECTION(PreProcessInit,".rmbProcessSec");
#endif
int PreProcessInit(IMG_BuffObj imageObj, int processIndex , int isReflect)
{
	short i;
	CurrentImageCount = imageObj.sBuffNum;
#ifndef CHIP_DM642
	memcpy_s(srcImg[0],imageObj.iBuffLength[0],imageObj.pBuff[0],imageObj.iBuffLength[0]);
	if(CurrentImageCount == 2 ) 
		memcpy_s(srcImg[1],imageObj.iBuffLength[1],imageObj.pBuff[1],imageObj.iBuffLength[1]);
	if (CurrentImageCount == 3)
	{
		memcpy_s(srcImg[1],imageObj.iBuffLength[1],imageObj.pBuff[1],imageObj.iBuffLength[1]);
		memcpy_s(srcImg[2],imageObj.iBuffLength[2],imageObj.pBuff[2],imageObj.iBuffLength[2]);
	}
#endif

	if (processIndex >= imageObj.sBuffNum )
	{
		ImageRotateIndex = 0;
	}else
		ImageRotateIndex = processIndex;
	isCurrentReflect = isReflect;
	
	memset(RMB_xx,0,sizeof(RMB_xx));
	memset(RMB_yy,0,sizeof(RMB_yy));
	memset(PtCorrectFirstHeight,0,sizeof(PtCorrectFirstHeight));
	memset(PtCorrectFirstWidth,0,sizeof(PtCorrectFirstWidth));

	ScanNodesCount = 0;
	RMB_horizontal_rate=0;
	RMB_vertical_rate=0;
	RMB_pointUpMid.X=0;
	RMB_pointUpMid.Y=0; 
	RMB_pointDownMid.X=0;
	RMB_pointDownMid.Y=0;
	RMB_pointLeftMid.X=0;
	RMB_pointLeftMid.Y=0;
	RMB_pointRightMid.X=0;
	RMB_pointRightMid.Y=0;
	RMB_Top.X=0;
	RMB_Top.Y=0;
	RMB_cosa = 1;
	RMB_sina = 0;   
	RMB_inclination=0;

	memset(focusImage,0,sizeof(focusImage));
	for (i = 0; i < CurrentImageCount; ++i)
	{
		focusImage[i].srcWidth = IMG_FRAME_SIZE;
		focusImage[i].srcHeight = imageObj.iBuffLength[i] / IMG_FRAME_SIZE;
		focusImage[i].iBuffLength = imageObj.iBuffLength[i];
		focusImage[i].pBuff = imageObj.pBuff[i];
	}

	focusImage[0].il = Focus400;
	if(CurrentImageCount == 2){
		focusImage[0].il = Focus200;
		focusImage[1].il = Focus200;
		if(focusImage[0].srcHeight == (focusImage[1].srcHeight<<1) )
		{
			focusImage[0].il = Focus400;
		}
	}

	image = focusImage[ImageRotateIndex].pBuff;
	
	return 1;
}

/// <summary>
/// 得到旋转校正后的图像
/// </summary>
/// <param name="w">X坐标值</param>
/// <param name="h">Y坐标值</param>
/// <param name="picIndex">标志第几幅图，从下标0开始</param>
/// <return>校正后坐标的灰度值</return>
UINT8 GetRotatePixel(int w,int h,int picIndex)
{
	int x=0, y=0,x1=0,y1=0,pos=0,h0=0;
	h0 = h;
	if ((picIndex==0||picIndex==2)&&focusImage[picIndex].iBuffLength<(focusImage[picIndex].iBuffLength+focusImage[1].iBuffLength-720*4)/2)
	{
		h<<=1;
	}
	x1 = RMB_Top.X + w;
	y1 = RMB_Top.Y + h;
	x = x1;
	y = y1;
	if(x1 >= focusImage[ImageRotateIndex].srcWidth || y1 >= focusImage[ImageRotateIndex].srcHeight )
		return 255;

	if (focusImage[ImageRotateIndex].Lean != 0)
	{         
		x=(PtCorrectFirstWidth[y1].X - PtCorrectFirstWidth[RMB_Top.Y].X);
		x+= PtCorrectFirstHeight[x1].X;
		y=(PtCorrectFirstWidth[y1].Y - PtCorrectFirstWidth[RMB_Top.Y].Y);
		y+= PtCorrectFirstHeight[x1].Y;
	}
	
	if(focusImage[0].iBuffLength == (focusImage[1].iBuffLength<<1))
		y <<= 1;
	if ((picIndex==0||picIndex==2)&&focusImage[picIndex].iBuffLength<(focusImage[picIndex].iBuffLength+focusImage[1].iBuffLength-720*4)/2)
	{
		y>>=1;
	}
	pos = y * focusImage[picIndex].srcWidth + x;

	if (pos < focusImage[picIndex].iBuffLength && pos >=0)
	{
#ifndef CHIP_DM642
		return srcImg[picIndex][pos];
#else
		return focusImage[picIndex].pBuff[pos];
#endif
	}
	else
	{
		return 255;
	}
}

UINT8 RMB_getIrPixel(int row,int col)
{
	int x=0, y=0,x1=0,y1=0,pos=0;
	x1 = RMB_Top.X + col;
	y1 = RMB_Top.Y + row;
	x = x1;
	y = y1;
	x1=(x1>= focusImage[1].srcWidth -1)?(focusImage[1].srcWidth-1):x1;
	y1=(y1>= focusImage[1].srcHeight -1)?( focusImage[1].srcHeight -1):y1;
	if ( focusImage[ImageRotateIndex].Lean != 0)
	{         
		x=(PtCorrectFirstWidth[y1].X - PtCorrectFirstWidth[RMB_Top.Y].X);
		x+= PtCorrectFirstHeight[x1].X;
		y=(PtCorrectFirstWidth[y1].Y - PtCorrectFirstWidth[RMB_Top.Y].Y);
		y+= PtCorrectFirstHeight[x1].Y;
	}
	pos = y * focusImage[1].srcWidth + x;
	if (pos < focusImage[1].iBuffLength && pos >=0)
	{
		return focusImage[1].pBuff[pos];
	}
	else
	{
		return 255;
	}
}

UINT8 RMB_GetRawPixel(int h, int w)
{
	if ( h< 8 || h >= focusImage[ImageRotateIndex].srcHeight - 1 || w < RMB_L_X_OFFSET || w > RMB_R_X_OFFSET  )
	{
		return 255;
	}
#ifndef CHIP_DM642
	return srcImg[ImageRotateIndex][h * focusImage[ImageRotateIndex].srcWidth + w];
#else
	return image[h * focusImage[ImageRotateIndex].srcWidth + w];
#endif
}

#ifndef CHIP_DM642
#include <Windows.h>
#include <WinBase.h>
#include <stdio.h>
#endif
/*
 * 预处理，旋转校正
 */
int   RMB_PreProcess()
{
#ifndef CHIP_DM642
	LARGE_INTEGER st,en;
	long time1,time2 = 0;
#endif
	int success = 0;
	if ( !WITHIN( focusImage[ImageRotateIndex].srcHeight*((CurrentImageCount>2)?2:CurrentImageCount),200,MAXLINE) )
		return -1; 
	success = RMB_FindXoffset();        //去黑边处理
	if (success == 0)
		return -2;//图像超出边界	
#ifndef CHIP_DM642
	QueryPerformanceCounter(&st);
#endif
	success = RMB_OrientationHorizontal();
#ifndef CHIP_DM642
	QueryPerformanceCounter(&en);
	time1 = (en.LowPart - st.LowPart);
#endif
	if (success == 0) 
		return -3;//图像超出边界
#ifndef CHIP_DM642
	QueryPerformanceCounter(&st);
#endif
	success = RMB_OrientationVertical();
#ifndef CHIP_DM642
	QueryPerformanceCounter(&en);
	time2 = (en.LowPart - st.LowPart);
#endif
	if (success == 0)
		return -4;//图像超出边界
	success = RMB_FindAcme();
	if (success == 0)
		return -5;//图像超出边界
	SetTheOtherFocusImageParams();
	return 1;
}

int RMB_EWidth;
int RMB_EHeight; 

void SetTheOtherFocusImageParams()
{
	short indexofOther = (ImageRotateIndex+1)&1;
	if(CurrentImageCount == 2)
	{
		focusImage[indexofOther].Lean = focusImage[ImageRotateIndex].Lean;
		focusImage[indexofOther].Center = focusImage[ImageRotateIndex].Center;
		focusImage[indexofOther].TopLeft = focusImage[ImageRotateIndex].TopLeft;
		focusImage[indexofOther].TopRight = focusImage[ImageRotateIndex].TopRight;
		focusImage[indexofOther].BottomLeft = focusImage[ImageRotateIndex].BottomLeft;
		focusImage[indexofOther].BottomRight = focusImage[ImageRotateIndex].BottomRight;
		focusImage[indexofOther].FixedCenterX = focusImage[ImageRotateIndex].FixedCenterX;
		focusImage[indexofOther].FixedCenterY = focusImage[ImageRotateIndex].FixedCenterY;
		focusImage[indexofOther].Width = focusImage[ImageRotateIndex].Width;
		focusImage[indexofOther].Height = focusImage[ImageRotateIndex].Height;
		if(focusImage[0].iBuffLength == (focusImage[1].iBuffLength<<1) )
		{
			focusImage[0].Center.Y <<= 1;
			focusImage[0].FixedCenterY <<=1;
			focusImage[0].TopLeft.Y <<= 1;
			focusImage[0].TopRight.Y <<= 1;
			focusImage[0].BottomLeft.Y <<= 1;
			focusImage[0].BottomRight.Y <<= 1;
			focusImage[0].Height <<= 1;
		}
		if(focusImage[0].iBuffLength < (focusImage[1].iBuffLength+focusImage[0].iBuffLength-720*4)/2 )
		{
			focusImage[0].Center.Y >>= 1;
			focusImage[0].FixedCenterY >>=1;
			focusImage[0].TopLeft.Y >>= 1;
			focusImage[0].TopRight.Y >>= 1;
			focusImage[0].BottomLeft.Y >>= 1;
			focusImage[0].BottomRight.Y >>= 1;
			focusImage[0].Height >>= 1;
		}
	}
	if(CurrentImageCount == 3)
	{
		focusImage[0].Lean = focusImage[ImageRotateIndex].Lean;
		focusImage[0].Center = focusImage[ImageRotateIndex].Center;
		focusImage[0].TopLeft = focusImage[ImageRotateIndex].TopLeft;
		focusImage[0].TopRight = focusImage[ImageRotateIndex].TopRight;
		focusImage[0].BottomLeft = focusImage[ImageRotateIndex].BottomLeft;
		focusImage[0].BottomRight = focusImage[ImageRotateIndex].BottomRight;
		focusImage[0].FixedCenterX = focusImage[ImageRotateIndex].FixedCenterX;
		focusImage[0].FixedCenterY = focusImage[ImageRotateIndex].FixedCenterY;
		focusImage[0].Width = focusImage[ImageRotateIndex].Width;
		focusImage[0].Height = focusImage[ImageRotateIndex].Height;
		focusImage[2].Lean = focusImage[ImageRotateIndex].Lean;
		focusImage[2].Center = focusImage[ImageRotateIndex].Center;
		focusImage[2].TopLeft = focusImage[ImageRotateIndex].TopLeft;
		focusImage[2].TopRight = focusImage[ImageRotateIndex].TopRight;
		focusImage[2].BottomLeft = focusImage[ImageRotateIndex].BottomLeft;
		focusImage[2].BottomRight = focusImage[ImageRotateIndex].BottomRight;
		focusImage[2].FixedCenterX = focusImage[ImageRotateIndex].FixedCenterX;
		focusImage[2].FixedCenterY = focusImage[ImageRotateIndex].FixedCenterY;
		focusImage[2].Width = focusImage[ImageRotateIndex].Width;
		focusImage[2].Height = focusImage[ImageRotateIndex].Height;
		if(focusImage[0].iBuffLength < (focusImage[1].iBuffLength+focusImage[0].iBuffLength-720*4)/2 )
		{
			focusImage[0].Center.Y >>= 1;
			focusImage[0].FixedCenterY >>=1;
			focusImage[0].TopLeft.Y >>= 1;
			focusImage[0].TopRight.Y >>= 1;
			focusImage[0].BottomLeft.Y >>= 1;
			focusImage[0].BottomRight.Y >>= 1;
			focusImage[0].Height >>= 1;
		}
		if(focusImage[2].iBuffLength < (focusImage[1].iBuffLength+focusImage[2].iBuffLength-720*4)/2 )
		{
			focusImage[2].Center.Y >>= 1;
			focusImage[2].FixedCenterY >>=1;
			focusImage[2].TopLeft.Y >>= 1;
			focusImage[2].TopRight.Y >>= 1;
			focusImage[2].BottomLeft.Y >>= 1;
			focusImage[2].BottomRight.Y >>= 1;
			focusImage[2].Height >>= 1;
		}
	}
}

int   RMB_FindXoffset()
{
	short SrcWidth = focusImage[ImageRotateIndex].srcWidth;
	if (isCurrentReflect == 1)
	{
		int h,w,pos ;
		RMB_downThresholdGrey = 55;
		RMB_upThresholdGrey = 256;
		RMB_L_X_OFFSET = 5;
		RMB_R_X_OFFSET = SrcWidth - 3;
		memset(SrcColGrey,0,sizeof(SrcColGrey));
		for (h = 1; h < 9; ++h)
		{
			pos = h * SrcWidth ;
			for (w = 0; w < SrcWidth ; ++w)
			{
				SrcColGrey[ w ] += image[pos + w];
			}
		}
		for (w = 0; w < SrcWidth ; ++w)
		{
			SrcColGrey[ w ] >>= 3;
		}
	}else{		// 透射图像
		int v = 0;
		int whitegroundpixel = 250;
		int lx=10,y = 5,rx;
		RMB_upThresholdGrey = 150;
		if(CurrentImageCount == 3 ) 
			RMB_upThresholdGrey = 150;
		RMB_downThresholdGrey = 1;

		for (lx = 10; lx < 50; ++lx)
		{
			v = (int)image[y * SrcWidth + lx];
			if (v > whitegroundpixel)
				break;
		}
		RMB_L_X_OFFSET = lx;

		for (rx = SrcWidth - 1 - 10 ; rx > SrcWidth - 1 - 50; --rx)
		{
			v = (int)image[y * SrcWidth + rx];
			if (v > whitegroundpixel)
				break;
		}
		RMB_R_X_OFFSET = rx;
	}
    return 1;
}

int   RMB_FindAcme()
 {
    int i = 0, x = 0, y = 0;
    double k1 = 0, b1 = 0, k2 = 0, b2 = 0, temp1 = 0, temp2 = 0;
	double c_k2;

    //RMB_inclination = fabs(RMB_horizontalrate - RMB_verticalRMB_rate);
    RMB_inclination /= fabs(1 + RMB_horizontal_rate / RMB_vertical_rate);
	c_k2 = RMB_vertical_rate;
    if ( focusImage[ImageRotateIndex].Lean == 0)
    {
        focusImage[ImageRotateIndex].TopLeft.X = RMB_pointLeftMid.X;
        focusImage[ImageRotateIndex].TopLeft.Y = RMB_pointUpMid.Y;

        focusImage[ImageRotateIndex].TopRight.X = RMB_pointRightMid.X;
        focusImage[ImageRotateIndex].TopRight.Y = RMB_pointUpMid.Y;

        focusImage[ImageRotateIndex].BottomLeft.X = RMB_pointLeftMid.X;
        focusImage[ImageRotateIndex].BottomLeft.Y = RMB_pointDownMid.Y;

        focusImage[ImageRotateIndex].BottomRight.X = RMB_pointRightMid.X;
        focusImage[ImageRotateIndex].BottomRight.Y = RMB_pointDownMid.Y;

        RMB_EWidth = (focusImage[ImageRotateIndex].TopRight.X - focusImage[ImageRotateIndex].TopLeft.X);
        RMB_EWidth += (RMB_pointRightMid.X - RMB_pointLeftMid.X);
        RMB_EWidth += (focusImage[ImageRotateIndex].BottomRight.X - focusImage[ImageRotateIndex].BottomLeft.X);
        RMB_EWidth /= 3;
        if (RMB_inclination < RMB_inclinationThreshold)
        {
            k1 = RMB_horizontal_rate;
            temp1 = k1 * RMB_pointUpMid.X;
            b1 = RMB_pointUpMid.Y - temp1;
            temp1 = RMB_horizontal_rate * RMB_pointDownMid.X;
            temp1 -= RMB_pointDownMid.Y;
            temp1 += b1;
            temp1 = fabs(temp1);
            temp2 = 1 + RMB_horizontal_rate * RMB_horizontal_rate;
            temp2 = sqrt(temp2);
            temp1 /= temp2;
            RMB_EHeight = (int)temp1;
        }
        else
        {
            RMB_EHeight = ( focusImage[ImageRotateIndex].BottomLeft.Y - focusImage[ImageRotateIndex].TopLeft.Y);
            RMB_EHeight += (RMB_pointDownMid.Y - RMB_pointUpMid.Y);
            RMB_EHeight += ( focusImage[ImageRotateIndex].BottomRight.Y - focusImage[ImageRotateIndex].TopRight.Y);
            RMB_EHeight /= 3;
        }

		RMB_cosa = RMB_cosa400 = 1;
		RMB_sina = RMB_sina400 = 0;
		RMB_fixcosa400 = RMB_fixcosa = 32768;
		RMB_fixsina400 = RMB_fixsina = 0;
    }
    else
    {
        k1 = RMB_horizontal_rate;
        temp1 = k1 * RMB_pointUpMid.X;
        b1 = RMB_pointUpMid.Y - temp1;
        k2 = 1/RMB_vertical_rate;
        temp1 = k2 * RMB_pointLeftMid.X;
        b2 = RMB_pointLeftMid.Y - temp1;
        temp1 = (b2 - b1);
        temp2 = (k1 - k2);
        temp1 /= temp2;
        focusImage[ImageRotateIndex].TopLeft.X = (int)temp1;
        temp1 = k2 * b1;
        temp1 -= k1 * b2;
        temp2 = (k2 - k1);
        temp1 /= temp2;
        focusImage[ImageRotateIndex].TopLeft.Y = (int)temp1;

        temp1 = k2 * RMB_pointRightMid.X;
        b2 = RMB_pointRightMid.Y - temp1;
        temp1 = (b2 - b1);
        temp2 = (k1 - k2);
        temp1 /= temp2;
        focusImage[ImageRotateIndex].TopRight.X = (int)temp1;
        temp1 = k2 * b1;
        temp1 -= k1 * b2;
        temp2 = (k2 - k1);
        temp1 /= temp2;
        focusImage[ImageRotateIndex].TopRight.Y = (int)temp1;


        temp1 = k1 * RMB_pointDownMid.X;
        b1 = RMB_pointDownMid.Y - temp1;
        temp1 = (b2 - b1);
        temp2 = (k1 - k2);
        temp1 /= temp2;
        focusImage[ImageRotateIndex].BottomRight.X = (int)temp1;
        temp1 = k2 * b1;
        temp1 -= k1 * b2;
        temp2 = (k2 - k1);
        temp1 /= temp2;
        focusImage[ImageRotateIndex].BottomRight.Y = (int)temp1;

        temp1 = k2 * RMB_pointLeftMid.X;
        b2 = RMB_pointLeftMid.Y - temp1;
        temp1 = (b2 - b1);
        temp2 = (k1 - k2);
        temp1 /= temp2;
        focusImage[ImageRotateIndex].BottomLeft.X = (int)temp1;
        temp1 = k2 * b1;
        temp1 -= k1 * b2;
        temp2 = (k2 - k1);
        temp1 /= temp2;
        focusImage[ImageRotateIndex].BottomLeft.Y = (int)temp1;

        temp1 = ( focusImage[ImageRotateIndex].TopLeft.X - focusImage[ImageRotateIndex].TopRight.X);
        temp1 *= ( focusImage[ImageRotateIndex].TopLeft.X - focusImage[ImageRotateIndex].TopRight.X);
        temp2 = ( focusImage[ImageRotateIndex].TopLeft.Y - focusImage[ImageRotateIndex].TopRight.Y);
        temp2 *= ( focusImage[ImageRotateIndex].TopLeft.Y - focusImage[ImageRotateIndex].TopRight.Y);
        temp1 += temp2;
        temp1 = sqrt(temp1);
        RMB_EWidth = (int)temp1;
        temp1 = (focusImage[ImageRotateIndex].BottomLeft.X - focusImage[ImageRotateIndex].BottomRight.X);
        temp1 *= (focusImage[ImageRotateIndex].BottomLeft.X - focusImage[ImageRotateIndex].BottomRight.X);
        temp2 = (focusImage[ImageRotateIndex].BottomLeft.Y - focusImage[ImageRotateIndex].BottomRight.Y);
        temp2 *= (focusImage[ImageRotateIndex].BottomLeft.Y - focusImage[ImageRotateIndex].BottomRight.Y);
        temp1 += temp2;
        temp1 = sqrt(temp1);
        RMB_EWidth += (int)temp1;
        RMB_EWidth /= 2;


        k1 = RMB_horizontal_rate;
        temp1 = k1 * RMB_pointUpMid.X;
        b1 = RMB_pointUpMid.Y - temp1;
        temp1 = RMB_horizontal_rate * RMB_pointDownMid.X;
        temp1 -= RMB_pointDownMid.Y;
        temp1 += b1;
        temp1 = fabs(temp1);
        temp2 = 1 + RMB_horizontal_rate * RMB_horizontal_rate;
        temp2 = sqrt(temp2);
        temp1 /= temp2;
        RMB_EHeight = (int)temp1;
        RMB_cosa = 1.0 / temp2;
        RMB_sina = RMB_cosa * RMB_horizontal_rate;
		RMB_fixcosa=(int)(RMB_cosa*32768);
		RMB_fixsina=(int)(RMB_sina*32768);
		{
			temp2 = 1 + 4 * RMB_horizontal_rate * RMB_horizontal_rate;
			temp2 = sqrt(temp2);
			RMB_cosa400 = 1.0 / temp2;
			RMB_sina400 = RMB_cosa400 * RMB_horizontal_rate*2;
			RMB_fixcosa400 = (int)(RMB_cosa400*32768);
			RMB_fixsina400 = (int)(RMB_sina400*32768);
		}
    }

	focusImage[ImageRotateIndex].Width = RMB_EWidth;
	focusImage[ImageRotateIndex].Height = RMB_EHeight;
 
    focusImage[ImageRotateIndex].Center.X = (focusImage[ImageRotateIndex].TopLeft.X + focusImage[ImageRotateIndex].BottomRight.X) / 2;
    focusImage[ImageRotateIndex].Center.X += (focusImage[ImageRotateIndex].TopRight.X + focusImage[ImageRotateIndex].BottomLeft.X) / 2;
    focusImage[ImageRotateIndex].Center.X /= 2;
    focusImage[ImageRotateIndex].Center.Y = ( focusImage[ImageRotateIndex].TopLeft.Y + focusImage[ImageRotateIndex].BottomRight.Y) / 2;
    focusImage[ImageRotateIndex].Center.Y += ( focusImage[ImageRotateIndex].TopRight.Y + focusImage[ImageRotateIndex].BottomLeft.Y) / 2;
    focusImage[ImageRotateIndex].Center.Y /= 2;
    focusImage[ImageRotateIndex].FixedCenterX = focusImage[ImageRotateIndex].Center.X*32768;
	focusImage[ImageRotateIndex].FixedCenterY = focusImage[ImageRotateIndex].Center.Y*32768;

	/*
	假设对图片上任意点(x,y)，绕一个坐标点(rx0,ry0)逆时针旋转RotaryAngle角度后的新的坐标设为(x', y')，有公式： 
	(x平移rx0,y平移ry0,角度a对应-RotaryAngle ， 带入方程(7)、(8)后有： )  
	x'= (x - rx0)*cos(RotaryAngle) + (y - ry0)*sin(RotaryAngle) + rx0 ;
	y'=-(x - rx0)*sin(RotaryAngle) + (y - ry0)*cos(RotaryAngle) + ry0 ;*/
    if (focusImage[ImageRotateIndex].Lean != 0)
    {
        x = ( focusImage[ImageRotateIndex].TopLeft.X - focusImage[ImageRotateIndex].Center.X);
        y = ( focusImage[ImageRotateIndex].TopLeft.Y - focusImage[ImageRotateIndex].Center.Y);

        RMB_Top.X = (short)( x * RMB_cosa + y * RMB_sina + focusImage[ImageRotateIndex].Center.X + 0.5 );
		RMB_Top.Y = (short)( y * RMB_cosa - x * RMB_sina + focusImage[ImageRotateIndex].Center.Y + 0.5 );
    }
    else
    {
        RMB_Top.X = focusImage[ImageRotateIndex].TopLeft.X;
        RMB_Top.Y = focusImage[ImageRotateIndex].TopLeft.Y;
    }
	
    if (RMB_Top.X < 0)
    {
        RMB_Top.X = 0;
	}

	/*
	假设对图片上任意点(x,y)，绕一个坐标点(rx0,ry0)逆时针旋转RotaryAngle角度后的新的坐标设为(x', y')，有公式： 
	(x平移rx0,y平移ry0,角度a对应-RotaryAngle ， 带入方程(7)、(8)后有： )  
	x'= (x - rx0)*cos(RotaryAngle) + (y - ry0)*sin(RotaryAngle) + rx0 ;
	y'=-(x - rx0)*sin(RotaryAngle) + (y - ry0)*cos(RotaryAngle) + ry0 ;

	那么，根据新的坐标点求源坐标点的公式为：
	x=(x'- rx0)*cos(RotaryAngle) - (y'- ry0)*sin(RotaryAngle) + rx0 ;
	y=(x'- rx0)*sin(RotaryAngle) + (y'- ry0)*cos(RotaryAngle) + ry0 ;
	*/
    if (focusImage[ImageRotateIndex].Lean != 0)//求第一行第一列在原始图像中的位置，用于倾斜校正
    {
        temp1 = k1 * RMB_pointUpMid.X;
        b1 = RMB_pointUpMid.Y - temp1;
        temp2 = k2 * RMB_pointLeftMid.X;
        b2 = RMB_pointLeftMid.Y - temp2;

		PtCorrectFirstHeight[RMB_Top.X].X = focusImage[ImageRotateIndex].TopLeft.X;
        PtCorrectFirstHeight[RMB_Top.X].Y = focusImage[ImageRotateIndex].TopLeft.Y;
        for (i = 0; i < RMB_EWidth; i++)
        {
			x = RMB_Top.X + i - focusImage[ImageRotateIndex].Center.X;
			y = RMB_Top.Y  - focusImage[ImageRotateIndex].Center.Y;
			PtCorrectFirstHeight[RMB_Top.X+i].X = (short)( x * RMB_cosa - y * RMB_sina + focusImage[ImageRotateIndex].Center.X + 0.5);
			//PtCorrectFirstHeight[RMB_Top.X+i].Y = (short)( x * RMB_sina + y * RMB_cosa + Center.Y + 0.5);
			temp1 = k1*PtCorrectFirstHeight[RMB_Top.X+i].X+b1;
			PtCorrectFirstHeight[RMB_Top.X + i].Y = (int)temp1;
        }
				
		PtCorrectFirstWidth[RMB_Top.Y].X = focusImage[ImageRotateIndex].TopLeft.X;
        PtCorrectFirstWidth[RMB_Top.Y].Y = focusImage[ImageRotateIndex].TopLeft.Y;
        for (i = 0; i < RMB_EHeight; i++)
        {
			x = RMB_Top.X - focusImage[ImageRotateIndex].Center.X;
			y = RMB_Top.Y + i - focusImage[ImageRotateIndex].Center.Y;
			PtCorrectFirstWidth[RMB_Top.Y + i].Y = (short)( x * RMB_sina + y * RMB_cosa + focusImage[ImageRotateIndex].Center.Y + 0.5);
			temp1=( PtCorrectFirstWidth[RMB_Top.Y + i].Y - b2 ) * c_k2;
			PtCorrectFirstWidth[RMB_Top.Y + i].X = (int)temp1;
			//PtCorrectFirstWidth[RMB_Top.Y+i].X = (short)( x * RMB_cosa - y * RMB_sina + Center.X + 0.5);
        }
    }

    return 1;
}

int   RMB_CalculateLineKB(int*  xList, int  * yList, int lCount,  double *k )
{
    //最小二乘法直线拟合
    //拟合直线方程(Y=kX+b) 
    double mX,mY,mRMB_xx,mXY;       
    int iCount = 0;
	int n=lCount;
    if (lCount == 0) return 0;
    if(lCount<2)return 0;
        
    mX=mY=mRMB_xx=mXY=0;
         
    while(iCount<n)
    {
          
    mX+=xList[iCount];
    mY+=yList[iCount];
    mRMB_xx += xList[iCount] * xList[iCount];
    mXY += xList[iCount] * yList[iCount];
    iCount++;
    }
    if(mX*mX-mRMB_xx*n==0)return 0;
    *k=(mXY*n-mY*mX)/(mRMB_xx*n-mX*mX);
	// *b=(mY-mX*(*k))/n;
    return 1;
}

int   RMB_OrientationHorizontal()
{
	int temp1=0;
	int i, j,k;

	int WR_end=0;
	int H_break=0,lastCount1=0,lastCount2=0;
	unsigned short bPt[6];
	int topFlinecount = 0;
	int bottomFlinecount = 0;
	FocusLine topFLine[10];
	FocusLine bottomFLine[10];
	int bLineEnd = 0;
	//////////////////////////////////////////////////////////
	int HStartyTop = 0;
	int HStartyBottom = focusImage[ImageRotateIndex].srcHeight - 1;
	int distance =0;
	int keyflag=0,iCount=0,maxcount=0,temp0=0,temp2=0;
	int distanceCount = 0;
	int SrcWidth = focusImage[ImageRotateIndex].srcWidth;
	HDISNODE distanceNode[100];
	memset(distanceNode,0,sizeof(distanceNode));
	memset(topFLine,0,sizeof(topFLine));
	memset(bottomFLine,0,sizeof(bottomFLine));

	H_break=( focusImage[ImageRotateIndex].srcWidth >> 1);
	WR_end=RMB_R_X_OFFSET;
	j = RMB_L_X_OFFSET+2;
	//寻找上下边界点
	while ( j <WR_end)
	{
		HStartyTop = focusImage[ImageRotateIndex].srcHeight;
		HStartyBottom = 0;
		bPt[1]=WITHIN( image[5 * SrcWidth + j],RMB_downThresholdGrey,RMB_upThresholdGrey) ;
		bPt[2]=WITHIN( image[6 * SrcWidth + j],RMB_downThresholdGrey,RMB_upThresholdGrey) ;
		for (i = 5; i < focusImage[ImageRotateIndex].srcHeight - 2 ; ++i)
		{
			if( isCurrentReflect == 0){
				bPt[0] = bPt[1];
				bPt[1] = bPt[2];
				bPt[2] = WITHIN(image[j+(i+2)*SrcWidth],RMB_downThresholdGrey,RMB_upThresholdGrey);
				if( bPt[0] && bPt[1] && bPt[2]) {
					HStartyTop = i ; break;
				}
			}else
			{
				if( SrcColGrey[j] + ReflectThr  < image[j+(i+2)*SrcWidth] && 
					SrcColGrey[j] + ReflectThr  < image[j+(i+1)*SrcWidth] && 
					SrcColGrey[j] + ReflectThr  < image[j+i*SrcWidth] ){
						HStartyTop = i; break;						
				}
			}
		}
		bPt[4]=WITHIN( image[( focusImage[ImageRotateIndex].srcHeight -1) * SrcWidth + j],RMB_downThresholdGrey,RMB_upThresholdGrey) ;
		bPt[5]=WITHIN( image[( focusImage[ImageRotateIndex].srcHeight -2) * SrcWidth + j],RMB_downThresholdGrey,RMB_upThresholdGrey) ;
		for (k = focusImage[ImageRotateIndex].srcHeight -1; k > i+1 ; --k)
		{
			if( isCurrentReflect == 0){
				bPt[3] = bPt[4];
				bPt[4] = bPt[5];
				bPt[5] = WITHIN(image[j+(k-2)*SrcWidth],RMB_downThresholdGrey,RMB_upThresholdGrey);
				if( bPt[3] && bPt[4] && bPt[5]) {
					HStartyBottom = k ;break;					
				}
			}else
			{
				if( SrcColGrey[j] + ReflectThr  < image[j+(k-2)*SrcWidth] && 
					SrcColGrey[j] + ReflectThr  < image[j+(k-1)*SrcWidth] && 
					SrcColGrey[j] + ReflectThr  < image[j+k*SrcWidth] ){
						HStartyBottom = k ;break;						
				}
			}
		}
		if( HStartyBottom != 0 ){
			distanceNode[distanceCount].dis= HStartyBottom - HStartyTop + 1;
			distanceNode[distanceCount].top.X=j;
			distanceNode[distanceCount].top.Y=HStartyTop;
			distanceNode[distanceCount].bottom.X=j;
			distanceNode[distanceCount++].bottom.Y=HStartyBottom;		
		}else if (j>H_break)	
			break;
		j+=15;
	}
	//寻找上下边界中点
	temp0=2*distanceCount/5;
	distance=distanceNode[temp0].dis;
	keyflag=0;
	for (i=temp0;i<(distanceCount-temp0/2);i++)
	{
		if (i > temp0)
		{
			temp1 = abs(distanceNode[i].dis - distance);
			if (temp1 < 0.01)continue;
		}
		iCount = 0;
		for (j = 0 ; j < distanceCount; j++)
		{
			temp1 = abs(distanceNode[i].dis - distanceNode[j].dis);
			if (temp1 < 0.01 )
				iCount++;
		}
		if (iCount > maxcount)
		{
			distance = distanceNode[i].dis;
			maxcount = iCount;
			keyflag = i;
			if(maxcount>=(distanceCount>>1))break;
		}
	}
	RMB_pointUpMid.X = distanceNode[keyflag].top.X;
	RMB_pointUpMid.Y = distanceNode[keyflag].top.Y;
	RMB_pointDownMid.X = distanceNode[keyflag].bottom.X;
	RMB_pointDownMid.Y = distanceNode[keyflag].bottom.Y;
	//求上边斜率
	lastCount1 = 2;
	for ( i = 0 ; i < distanceCount ; ++i)
	{
		if(topFLine[topFlinecount].count == 0){
			RMB_xx[topFLine[topFlinecount].count] = distanceNode[i].top.X;
			RMB_yy[topFLine[topFlinecount].count] = distanceNode[i].top.Y;
			topFLine[topFlinecount].count ++;
			continue;
		} 
		temp1 = distanceNode[i].top.Y - distanceNode[i - 1].top.Y;
		if(  abs(temp1) <= 10 )
		{
			if(topFLine[topFlinecount].count == 1 ||WITHIN2( abs( topFLine[topFlinecount].differ -(temp1+10)*
				(topFLine[topFlinecount].count - 1) ) ,0 , (13 * ( topFLine[topFlinecount].count - 1 ) )>>3 ) )
			{
				RMB_xx[topFLine[topFlinecount].count] = distanceNode[i].top.X;
				RMB_yy[topFLine[topFlinecount].count] = distanceNode[i].top.Y;
				topFLine[topFlinecount].count ++;
				topFLine[topFlinecount].differ += temp1 + 10 ;
				if( i == distanceCount - 1)
					bLineEnd = 1;
			}else
				bLineEnd = 1;
		}else
			bLineEnd = 1;

		if(bLineEnd == 1)
		{
			if(topFLine[topFlinecount].count >lastCount1 )
			{
				lastCount1=topFLine[topFlinecount].count;				
				RMB_CalculateLineKB(RMB_xx,RMB_yy , topFLine[topFlinecount].count , &topFLine[topFlinecount].rate) ;
				RMB_horizontal_rate = topFLine[topFlinecount].rate;
				++topFlinecount;
				if(topFlinecount > 8) break;
			}
			topFLine[topFlinecount].count = 0;
			topFLine[topFlinecount].differ = 0;
			bLineEnd = 0;
			if(lastCount1>=(distanceCount-i))break;
		}
	}
	//求下边斜率
	lastCount2=lastCount1;
	temp2=9*distanceCount/10;
	for ( i = 0 ; i < distanceCount ; ++i)
	{
		if(lastCount1>=temp2)break;
		if(bottomFLine[bottomFlinecount].count == 0){
			RMB_xx[bottomFLine[bottomFlinecount].count] = distanceNode[i].bottom.X;
			RMB_yy[bottomFLine[bottomFlinecount].count] = distanceNode[i].bottom.Y;
			bottomFLine[bottomFlinecount].count ++;
			continue;
		} 
		temp1 = distanceNode[i].bottom.Y - distanceNode[i - 1].bottom.Y;
		if(  abs(temp1) <= 10 )
		{
			if(bottomFLine[bottomFlinecount].count == 1 ||WITHIN2( abs( bottomFLine[bottomFlinecount].differ - (temp1+10) *
				(bottomFLine[bottomFlinecount].count - 1) ) ,0 , (13* (bottomFLine[bottomFlinecount].count - 1))>>3 ) )
			{
				RMB_xx[bottomFLine[bottomFlinecount].count] = distanceNode[i].bottom.X;
				RMB_yy[bottomFLine[bottomFlinecount].count] = distanceNode[i].bottom.Y;
				bottomFLine[bottomFlinecount].count ++;
				bottomFLine[bottomFlinecount].differ += temp1 + 10 ;
				if( i == distanceCount - 1)
					bLineEnd = 1;
			}else
				bLineEnd = 1;
		}else
			bLineEnd = 1;

		if(bLineEnd == 1)
		{
			if(bottomFLine[bottomFlinecount].count>lastCount2 )
			{
				lastCount2=bottomFLine[bottomFlinecount].count;
				RMB_CalculateLineKB(RMB_xx,RMB_yy , bottomFLine[bottomFlinecount].count , &bottomFLine[bottomFlinecount].rate) ;
				RMB_horizontal_rate = bottomFLine[bottomFlinecount].rate;
				if(bottomFlinecount > 8)break;
				++bottomFlinecount;
			}
			bottomFLine[bottomFlinecount].count = 0;
			bottomFLine[bottomFlinecount].differ = 0;
			bLineEnd = 0;
			if(lastCount2>=(distanceCount-i))break;
		}
	}
	//判断图像歪曲程度与方向
	if (RMB_horizontal_rate >0.001)
	{
		focusImage[ImageRotateIndex].Lean = 2;
	}
	else
	{
		if (RMB_horizontal_rate < -0.001)
		{
			focusImage[ImageRotateIndex].Lean = 1;
		}
		else
		{
			focusImage[ImageRotateIndex].Lean = 0;
		}
	}

	return 1;
} 

int   RMB_OrientationVertical()
{
	int indexOfH = 0;
	int i, j, k;
	int temp1=0,temp0=0,temp2=0;
	int lastCount1 = 0,lastCount2 = 0;
	double rightRMB_rate=0;                     //右边界直线倾斜率
	double leftRMB_rate=0;                      //左边界直线倾斜率
	int keyflag=0,iCount=0,maxcount=0;      
	int SrcWidth = focusImage[ImageRotateIndex].srcWidth;
	int VStartxLeft = SrcWidth,VStartxRight = 0;
	int distance =0;
	int leftFlinecount = 0;
	int rightFlinecount = 0;
	FocusLine leftFLine[10];
	FocusLine rightFLine[10];
	int bLineEnd = 0;
	short bPixelInImage[6];

	ScanNodesCount = 0;
	memset(HorizontalScanNode,0,sizeof(HorizontalScanNode));
	memset(leftFLine,0,sizeof(leftFLine));
	memset(rightFLine,0,sizeof(rightFLine));
	//寻找左右边界点
	j = 8;		// start from 8th line
	while ( j >= 0 && j< focusImage[ImageRotateIndex].srcHeight )
	{
		// DO SOME SEARCH
		indexOfH = j * SrcWidth;
		VStartxLeft = SrcWidth;
		VStartxRight = 0;
		bPixelInImage[1] = WITHIN(image[indexOfH + RMB_L_X_OFFSET],RMB_downThresholdGrey,RMB_upThresholdGrey);
		bPixelInImage[2] = WITHIN(image[indexOfH + RMB_L_X_OFFSET + 1],RMB_downThresholdGrey,RMB_upThresholdGrey);
		for (i = RMB_L_X_OFFSET; i < RMB_R_X_OFFSET-2 ; ++i)
		{
			if( isCurrentReflect == 0){
				bPixelInImage[0] = bPixelInImage[1];
				bPixelInImage[1] = bPixelInImage[2];
				bPixelInImage[2] = WITHIN(image[indexOfH+i+2],RMB_downThresholdGrey,RMB_upThresholdGrey);
				if( bPixelInImage[0] && bPixelInImage[1] && bPixelInImage[2] ){
					VStartxLeft = i ; break;
				}
			}else{
				if(SrcColGrey[i] + ReflectThr < image[indexOfH + i ] && SrcColGrey[i+1] + ReflectThr 
					< image[indexOfH + i + 1] &&  SrcColGrey[i+2] + ReflectThr < image[indexOfH+i+2]){
						VStartxLeft = i ; break;
				}
			}
		}
		bPixelInImage[4] = WITHIN(image[indexOfH + RMB_R_X_OFFSET],RMB_downThresholdGrey,RMB_upThresholdGrey);
		bPixelInImage[5] = WITHIN(image[indexOfH + RMB_R_X_OFFSET - 1],RMB_downThresholdGrey,RMB_upThresholdGrey);
		for (k = RMB_R_X_OFFSET; k > i+1 ; --k)
		{
			if( isCurrentReflect == 0){
				bPixelInImage[3] = bPixelInImage[4];
				bPixelInImage[4] = bPixelInImage[5];
				bPixelInImage[5] = WITHIN(image[indexOfH+k-2],RMB_downThresholdGrey,RMB_upThresholdGrey);
				if( bPixelInImage[3] && bPixelInImage[4] && bPixelInImage[5] ){
					VStartxRight = k ;break;
				}
			}else{
				if(SrcColGrey[k] + ReflectThr < image[indexOfH + k] && SrcColGrey[k-1] + ReflectThr 
					< image[indexOfH + k - 1] &&  SrcColGrey[k-2] + ReflectThr < image[indexOfH+k-2]){
						VStartxRight = k ;break;
				}
			}
		}
		if( VStartxRight != 0 ){
			HorizontalScanNode[ScanNodesCount].dis = VStartxRight - VStartxLeft + 1;
			HorizontalScanNode[ScanNodesCount].left.X=VStartxLeft;
			HorizontalScanNode[ScanNodesCount].left.Y=j;
			HorizontalScanNode[ScanNodesCount].right.X=VStartxRight;
			HorizontalScanNode[ScanNodesCount++].right.Y=j;
		}
		j = j + ( SearchStepofHeight << ((CurrentImageCount>1 )?0:1) );
	}
	//寻找左右边界中点
	temp0 = 2 * ScanNodesCount / 5 ;
	distance=HorizontalScanNode[temp0].dis;
	keyflag  =  0 ;
	for (i=temp0;i<(ScanNodesCount-temp0/2);i++)
	{
		if (i > temp0)
		{
			temp1 = abs(HorizontalScanNode[i].dis - distance);
			if (temp1 < 0.01)continue;
		}
		iCount = 0;
		for (j = 0 ; j < ScanNodesCount; j++)
		{
			temp1 = abs(HorizontalScanNode[i].dis - HorizontalScanNode[j].dis);
			if (temp1 < 0.01 )
				iCount++;
		}
		if (iCount > maxcount)
		{
			distance = HorizontalScanNode[i].dis;
			maxcount = iCount;
			keyflag = i;
			if(maxcount>=(ScanNodesCount>>1))break;
		}
	}
	if (HorizontalScanNode[keyflag].left.X<RMB_L_X_OFFSET)
		RMB_pointLeftMid.X=RMB_L_X_OFFSET;//图像嵌入左边界
	else
		RMB_pointLeftMid.X = HorizontalScanNode[keyflag].left.X;
	RMB_pointLeftMid.Y = HorizontalScanNode[keyflag].left.Y;
	if(HorizontalScanNode[keyflag].right.X>RMB_R_X_OFFSET)
		RMB_pointRightMid.X = RMB_R_X_OFFSET;//图像嵌入右边界
	else
		RMB_pointRightMid.X = HorizontalScanNode[keyflag].right.X;
	RMB_pointRightMid.Y = HorizontalScanNode[keyflag].right.Y;
	//求左边线斜率
	lastCount1=2;
	for ( i = 0 ; i < ScanNodesCount ; ++i)
	{
		if( HorizontalScanNode[i].left.X == RMB_L_X_OFFSET )
			bLineEnd = 1;
		else {
			if(leftFLine[leftFlinecount].count == 0){
				RMB_xx[leftFLine[leftFlinecount].count] = HorizontalScanNode[i].left.X;
				RMB_yy[leftFLine[leftFlinecount].count] = HorizontalScanNode[i].left.Y;
				leftFLine[leftFlinecount].count ++;
				continue;
			} 
			temp1 = HorizontalScanNode[i].left.X - HorizontalScanNode[i - 1].left.X;
			if(  abs(temp1) <= 6 )
			{
				if(leftFLine[leftFlinecount].count == 1 || WITHIN2( abs( leftFLine[leftFlinecount].differ - (temp1+6) *
					(leftFLine[leftFlinecount].count - 1) ) ,0 , (13 * (leftFLine[leftFlinecount].count - 1))>>3 ) )
				{
					RMB_xx[leftFLine[leftFlinecount].count] = HorizontalScanNode[i].left.X;
					RMB_yy[leftFLine[leftFlinecount].count] = HorizontalScanNode[i].left.Y;
					leftFLine[leftFlinecount].count ++;
					leftFLine[leftFlinecount].differ += temp1 + 6 ;
					if( i == ScanNodesCount - 1)
						bLineEnd = 1;
				}else
					bLineEnd = 1;
			}else
				bLineEnd = 1;
		}		
		if(bLineEnd == 1)
		{
			if(leftFLine[leftFlinecount].count >lastCount1 )
			{
				lastCount1=leftFLine[leftFlinecount].count;
				RMB_CalculateLineKB(RMB_yy,RMB_xx , leftFLine[leftFlinecount].count , &leftFLine[leftFlinecount].rate) ;
				leftRMB_rate=leftFLine[leftFlinecount].rate;
				if(!WITHIN(leftRMB_rate,-0.3,0.3)&&lastCount1>6){
					RMB_pointLeftMid.X = RMB_xx[leftFLine[leftFlinecount].count>>1];
					RMB_pointLeftMid.Y = RMB_yy[leftFLine[leftFlinecount].count>>1];
				}
				++leftFlinecount;
				if(leftFlinecount > 8) break;
			}
			leftFLine[leftFlinecount].count = 0;
			leftFLine[leftFlinecount].differ = 0;
			bLineEnd = 0;
			if(lastCount1>=(ScanNodesCount-i))break;
		}
	}
	//求右边线斜率
	if(!WITHIN(leftRMB_rate,-0.3,0.3))
		lastCount2 = 2;
	else
		lastCount2 = lastCount1;
	temp2 = 9 * ScanNodesCount / 10;
	for ( i = 0 ; i < ScanNodesCount ; ++i)
	{
		if(lastCount1>=temp2)
			break;
		if( HorizontalScanNode[i].right.X == RMB_R_X_OFFSET )
			bLineEnd = 1;
		else {
			if(rightFLine[rightFlinecount].count == 0){
				RMB_xx[rightFLine[rightFlinecount].count] = HorizontalScanNode[i].right.X;
				RMB_yy[rightFLine[rightFlinecount].count] = HorizontalScanNode[i].right.Y;
				rightFLine[rightFlinecount].count ++;
				continue;
			} 
			temp1 = HorizontalScanNode[i].right.X - HorizontalScanNode[i - 1].right.X;
			if(  abs(temp1) <= 6 )
			{
				if(rightFLine[rightFlinecount].count == 1 ||WITHIN2( abs( rightFLine[rightFlinecount].differ - (temp1+6) *
					(rightFLine[rightFlinecount].count - 1) ) ,0 , (13 * (rightFLine[rightFlinecount].count - 1))>>3 ) )
				{
					RMB_xx[rightFLine[rightFlinecount].count] = HorizontalScanNode[i].right.X;
					RMB_yy[rightFLine[rightFlinecount].count] = HorizontalScanNode[i].right.Y;
					rightFLine[rightFlinecount].count ++;
					rightFLine[rightFlinecount].differ += temp1 + 6 ;
					if( i == ScanNodesCount - 1)
						bLineEnd = 1;
				}else
					bLineEnd = 1;
			}else
				bLineEnd = 1;
		}
		if(bLineEnd == 1)
		{
			if(rightFLine[rightFlinecount].count >lastCount2 )
			{
				lastCount2=rightFLine[rightFlinecount].count;
				RMB_CalculateLineKB(RMB_yy,RMB_xx , rightFLine[rightFlinecount].count , &rightFLine[rightFlinecount].rate) ;
				rightRMB_rate=rightFLine[rightFlinecount].rate;
				if(!WITHIN(rightRMB_rate,-0.3,0.3)&&lastCount2>6 ){
					RMB_pointRightMid.X = RMB_xx[leftFLine[leftFlinecount].count>>1];
					RMB_pointRightMid.Y = RMB_yy[leftFLine[leftFlinecount].count>>1];
				}
				if(rightFlinecount > 8) break;
				++rightFlinecount;
			}
			rightFLine[rightFlinecount].count = 0;
			rightFLine[rightFlinecount].differ = 0;
			bLineEnd = 0;
			if(lastCount2>=(ScanNodesCount-i))break;
		}
	}

	if ( lastCount1 < lastCount2)
	{
		if( fabs(rightRMB_rate ) < 1e-2 )
			RMB_vertical_rate = 1e-2;
		else RMB_vertical_rate = rightRMB_rate;
	}else
	{
		if( fabs(leftRMB_rate ) < 1e-2 )
			RMB_vertical_rate = 1e-2;
		else RMB_vertical_rate = leftRMB_rate;
	}

	return 1;
}

/// <summary>
/// 邻域近似平均
/// </summary>
/// <param name="w">X坐标值</param>
/// <param name="h">Y坐标值</param>
/// <param name="picIndex">标志第几幅图，从下标0开始</param>
/// <return>校正后坐标的灰度值</return>
short GetRotate5Pixel(int w,int h,int picIndex)
{
	
	int x=0, y=0,x1=0,y1=0,pos=0;
	int SrcWidth = focusImage[picIndex].Width;
	int SrcHeight = focusImage[picIndex].Height;

	if (w < 1 || w > SrcWidth - 1 || h < 1 && h > SrcHeight - 1)
	{
		return 256;
	}
	x1 = RMB_Top.X + w;
	y1 = RMB_Top.Y + h;
	x = x1;
	y = y1;
	if(x1 >= focusImage[picIndex].srcWidth || y1 >= focusImage[picIndex].srcHeight )
		return 256;

	if (focusImage[ImageRotateIndex].Lean != 0)
	{         
		x=(PtCorrectFirstWidth[y1].X - PtCorrectFirstWidth[RMB_Top.Y].X);
		x+= PtCorrectFirstHeight[x1].X;
		y=(PtCorrectFirstWidth[y1].Y - PtCorrectFirstWidth[RMB_Top.Y].Y);
		y+= PtCorrectFirstHeight[x1].Y;
	}

	if(focusImage[0].iBuffLength == (focusImage[1].iBuffLength<<1))
		y <<= 1;
	pos = y * focusImage[picIndex].srcWidth + x;

	if (pos + focusImage[picIndex].srcWidth  < focusImage[picIndex].iBuffLength 
		&& pos - focusImage[picIndex].srcWidth >=0)
	{
#ifndef CHIP_DM642
		int v = ( (int)srcImg[picIndex][pos] + (int)srcImg[picIndex][pos - 1] + (int)srcImg[picIndex][pos+1] + (int)srcImg[picIndex][pos+focusImage[picIndex].srcWidth]
		+ (int)srcImg[picIndex][pos-focusImage[picIndex].srcWidth])/5;
		return v;
#else
		int v= ( (int)focusImage[picIndex].pBuff[pos] + (int)focusImage[picIndex].pBuff[pos - 1] + (int)focusImage[picIndex].pBuff[pos + 1]
			+ (int)focusImage[picIndex].pBuff[pos - focusImage[picIndex].srcWidth] + (int)focusImage[picIndex].pBuff[pos + focusImage[picIndex].srcWidth]) / 5;
		return v;
#endif
	}
	else
	{
		return 256;
	}
}

#ifdef  CHIP_DM642
/// <summary>
/// 得到五点直方图后邻域的平均灰度
/// </summary>
/// <param name="x">X坐标</param>
/// <param name="y">Y坐标</param>
/// <returns>五点直方图后邻域的平均灰度,如果超出图片范围返回256</returns>
short GetFivePointsAverageGrey(short x, short y,int picIndex)
{
	if (x >= 1 && x < focusImage[picIndex].Width - 1 && y >= 1 && y < focusImage[picIndex].Height - 1)
	{
		int centerP = focusImage[picIndex].Hist[GetRotatePixel(x, y , picIndex)];
		int leftP = focusImage[picIndex].Hist[GetRotatePixel(x - 1, y,picIndex)];
		int rightP = focusImage[picIndex].Hist[GetRotatePixel(x + 1, y,picIndex)];
		int topP = focusImage[picIndex].Hist[GetRotatePixel(x, y - 1,picIndex)];
		int bottomP = focusImage[picIndex].Hist[GetRotatePixel(x, y + 1,picIndex)];
		return (short)((centerP + leftP + rightP + topP + bottomP) / 5);
	}
	return 256;
}
#endif

#ifdef  CHIP_DM642
#pragma CODE_SECTION(HistEqualFast,".rmbProcessSec");
#endif
void HistEqualFast(int picIndex){
	int w, h, total = 0,i;

	memset( focusImage[picIndex].Hist,0,sizeof(focusImage[picIndex].Hist));

	for ( i = 0 ; i < ScanNodesCount; ++i)
	{
		h = HorizontalScanNode[i].left.Y;
		if((picIndex == 0||picIndex==2)&& (focusImage[0].iBuffLength< ((focusImage[0].iBuffLength+focusImage[1].iBuffLength-720*4)>> 1)) )
			h >>= 1;
		if(picIndex == 0 && (focusImage[0].iBuffLength == focusImage[1].iBuffLength << 1) )
			h <<= 1;
		for( w = HorizontalScanNode[i].left.X + 2 ; w < HorizontalScanNode[i].right.X - 2 ; w += 4)
		{
			focusImage[picIndex].Hist[ focusImage[picIndex].pBuff[h * focusImage[picIndex].srcWidth + w] ]++;
			++total;
		}
	}

	if(total <= 10)		return;

	for (i = 1; i < HIST_SIZE; ++i)
	{
		focusImage[picIndex].Hist[i] += focusImage[picIndex].Hist[i - 1];
		focusImage[picIndex].Hist[i - 1] = (HIST_SIZE - 1) * focusImage[picIndex].Hist[i - 1] / total;
	}
	focusImage[picIndex].Hist[HIST_SIZE - 1] = HIST_SIZE - 1;	
}


UINT8 GetRotatePixelByMul(int w,int h)
{
	int x, y, pos;

	//x = (w - focusImage[0].Width / 2) * RMB_cosa400 - (h - focusImage[0].Height / 2) * RMB_sina400 + focusImage[0].Center.X ;
	//y = (w - focusImage[0].Width / 2) * RMB_sina400 + (h - focusImage[0].Height / 2) * RMB_cosa400 + focusImage[0].Center.Y ;

	x = ( (w - focusImage[0].Width / 2) * RMB_fixcosa400 - (h - focusImage[0].Height / 2) * RMB_fixsina400 + focusImage[0].FixedCenterX ) >>15;
	y = ( (w - focusImage[0].Width / 2) * RMB_fixsina400 + (h - focusImage[0].Height / 2) * RMB_fixcosa400 + focusImage[0].FixedCenterY ) >> 15;

	pos = y * focusImage[0].srcWidth + x;
	if (pos < focusImage[0].iBuffLength && pos >= 0)
	{
		return focusImage[0].pBuff[pos];
	}
	else
	{
		return 255;
	}
}

void GetRotateImageByMul(unsigned char* restrict img, int x, int y, int width, int height,int face)
{
	int index = 0,w,h;
	int x0 , y0 , pos , tW ,tH;
	int stepW, stepH , w0, h0;
	int TOPW0;

	h = y;
	TOPW0 = x;
	stepH = stepW = 1;
	if(face == 1 || face == 3)
	{
		h = focusImage[0].Height - y - 1;
		stepH = -1;
	}
	if(face == 1 ||face == 2)
	{
		TOPW0 = focusImage[0].Width - x - 1;
		stepW = -1;
	}

	for ( h0 = 0 ; h0 < height; ++h0 )
	{
		h += stepH;
		w = TOPW0;
		tW = - (focusImage[0].Width >> 1) * RMB_fixcosa400 + ( (focusImage[0].Height >>1) - h ) * RMB_fixsina400 + focusImage[0].FixedCenterX;
		tH = - (focusImage[0].Width >> 1) * RMB_fixsina400 + ( h - (focusImage[0].Height >>1) ) * RMB_fixcosa400 + focusImage[0].FixedCenterY;
		for ( w0 = 0; w0 < width; ++w0 )
		{
			x0 = ( w * RMB_fixcosa400 + tW ) >>15;
			y0 = ( w * RMB_fixsina400 + tH ) >> 15;

			pos = y0 * focusImage[0].srcWidth  + x0;
			if (pos < focusImage[0].iBuffLength && pos >= 0)
			{
				img[index++] = focusImage[0].pBuff[pos];
			}
			else
			{
				img[index++] =	255;
			}
			w+=stepW;
		}
	}
}

#ifdef CHIP_DM642
#pragma CODE_SECTION(GetImgRectFast,".rmbProcessSec")
#endif
void GetImgRectFast(unsigned char* restrict srcImg,int deno, int x0, int y0, int width, int height,int index0)
{
	int index = 0,macWidth,topXY;
	int maxCount = focusImage[index0].iBuffLength;
	int specialWidth = focusImage[index0].srcWidth - 1 ;
	int specialHeight = focusImage[ImageRotateIndex].srcHeight - 1 ;
	int rmbwidth = focusImage[index0].srcWidth ;
	int topX = RMB_Top.X;
	int topY = RMB_Top.Y;
	int *correctFirstCol = (int*)PtCorrectFirstWidth;
	int *correctFirstRow = (int*)PtCorrectFirstHeight;
	int i,j,directX=1,directY=1;
	FocusPoint pt;
	unsigned int mask1 = 0xFFFFFFFF; 
	unsigned int mask2 = 0xFFFFFFFF; 
	//400行
	if (index0 == 0 && focusImage[0].iBuffLength == (focusImage[1].iBuffLength << 1))
	{
		rmbwidth *= 2;
	}
	//100行
	if ((index0==0||index0==2)&&focusImage[index0].iBuffLength<(focusImage[index0].iBuffLength+focusImage[1].iBuffLength-720*4)/2)
	{
		y0*=2;
		rmbwidth /= 2;
		directY=2;
		mask1 = 0xFFFEFFFF; 
		mask2 = 0xFFFFFFFE; 
	}
	pt.X = 1;
	pt.Y = rmbwidth;
	macWidth = *(int*)(&pt);
	pt.X = PtCorrectFirstWidth[RMB_Top.Y].X;
	pt.Y = PtCorrectFirstWidth[RMB_Top.Y].Y;
	topXY = *(int*)(&pt);
	if(deno == 0xB || deno == 0xD)
	{
		y0 = RMB_EHeight - y0 - 1;
		directY *= -1;
	}
	if(deno == 0xB ||deno == 0xC)
	{
		x0 = RMB_EWidth - x0 - 1;
		directX = -1;
	}
	if( focusImage[index0].Lean !=0)
	{	
		int y1 = topY+y0;
		for (j=0;j<height; j++)
		{
			int y2,xy0, x1 = topX+x0;
			y2 = _min2(specialHeight,y1);
			y2 = _max2(0, y2);
			xy0 = _sub2(correctFirstCol[y2],topXY);
			for (i=0; i<width; i++)
			{
				int xy,pos,x2; 
				x2 = _min2(specialWidth,x1);
				x2 = _max2(0,x2);
				xy = _add2(correctFirstRow[x2],xy0);
				xy &= mask1;
				pos = _dotp2(macWidth,xy);
				if (pos <maxCount && pos >=0)
				{
					srcImg[index++] = focusImage[index0].pBuff[pos];
				}
				else
				{
					srcImg[index++] = 255;
				}
				x1 += directX;
			}
			y1 += directY;
		}	
	}
	else
	{
		int y1 = topY+y0;
		for (j=0;j<height; j++)
		{
			int y2, x1 = topX+x0;
			y2 = _min2(specialHeight,y1);
			y2 = _max2(0, y2);
			y2 &= mask2;
			for (i=0; i<width; i++)
			{
				int pos,x2; 
				x2 = _min2(specialWidth,x1);
				x2 = _max2(0,x2);
				pos = y2*rmbwidth + x2;
				if (pos <maxCount && pos >=0)
				{
					srcImg[index++] = focusImage[index0].pBuff[pos];
				}
				else
				{
					srcImg[index++] = 255;
				}
				x1 += directX;
			}
			y1 += directY;
		}	
	}
}


#ifdef CHIP_DM642
#pragma CODE_SECTION(GetImgRectFastByCol,".rmbProcessSec")
#endif
void GetImgRectFastByCol(unsigned char* restrict srcImg, int deno, int y0, int x0, int height, int width,int index0)
{
	int index = 0,macWidth,topXY;
	int maxCount = focusImage[index0].iBuffLength ;
	int specialWidth = focusImage[index0].srcWidth - 1;
	int specialHeight = focusImage[index0].srcHeight -1;
	int rmbwidth = focusImage[index0].srcWidth;
	int topX = RMB_Top.X;
	int topY = RMB_Top.Y;
	int *correctFirstCol = (int*)PtCorrectFirstWidth;
	int *correctFirstRow = (int*)PtCorrectFirstHeight;
	int i,j,directX=1,directY=1;
	FocusPoint pt;
	unsigned int mask1 = 0xFFFFFFFF; 
	unsigned int mask2 = 0xFFFFFFFF; 
	//400行
	if (index0 == 0 && focusImage[0].iBuffLength == (focusImage[1].iBuffLength << 1))
	{
		rmbwidth *= 2;
	}
	//100行
	if ((index0==0||index0==2)&&focusImage[index0].iBuffLength<(focusImage[index0].iBuffLength+focusImage[1].iBuffLength-720*4)/2)
	{
		y0*=2;
		rmbwidth /= 2;
		directY=2;
		mask1 = 0xFFFEFFFF; 
		mask2 = 0xFFFFFFFE; 
	}
	pt.X = 1;
	pt.Y = rmbwidth;
	macWidth = *(int*)(&pt);
	pt.X = PtCorrectFirstWidth[RMB_Top.Y].X;
	pt.Y = PtCorrectFirstWidth[RMB_Top.Y].Y;
	topXY = *(int*)(&pt);
	if(deno == 0xB || deno == 0xD)
	{
		y0 = RMB_EHeight - y0 - 1;
		directY *= -1;
	}
	if(deno == 0xB ||deno == 0xC)
	{
		x0 = RMB_EWidth - x0 - 1;
		directX = -1;
	}
	if(focusImage[index0].Lean != 0)
	{	
		int x1 = topX+x0;
		for (i=0; i<width; i++)
		{
			int x2, y1 = topY+y0;
			x2 = _min2(specialWidth,x1);
			x2 = _max2(0,x2);
			for (j=0;j<height; j++)
			{
				int xy,pos,y2; 
				y2 = _min2(specialHeight,y1);
				y2 = _max2(0, y2);
				xy = _sub2(correctFirstCol[y2],topXY);
				xy = _add2(correctFirstRow[x2],xy);
				xy &= mask1;
				pos = _dotp2(macWidth,xy);
				if (pos <maxCount && pos >=0)
				{
					srcImg[index++] = focusImage[index0].pBuff[pos];
				}
				else
				{
					srcImg[index++] = 255;
				}
				y1 += directY;
			}
			x1 += directX;
		}	
	}
	else
	{
		int x1 = topX+x0 ;
		for (i=0; i<width; i++)
		{
			int x2, y1 = topY+y0;
			x2 = _min2(specialWidth,x1);
			x2 = _max2(0,x2);
			for (j=0;j<height; j++)
			{
				int pos,y2; 
				y2 = _min2(specialHeight,y1);
				y2 = _max2(0, y2);
				y2 &= mask2;
				pos = y2*rmbwidth + x2;
				if (pos <maxCount && pos >=0)
				{
					srcImg[index++] = focusImage[index0].pBuff[pos];
				}
				else
				{
					srcImg[index++] = 255;
				}
				y1 += directY;
			}
			x1 += directX;
		}	
	}
}

#ifdef CHIP_DM642
#pragma CODE_SECTION(GetHist,".rmbProcessSec")
void GetHist(int* _hist,int picIndex)
{	
	int i;
	for (i = 0; i < HIST_SIZE; ++i)
	{
		_hist[i] = focusImage[picIndex].Hist[i];
	}
}
#endif

#ifndef CHIP_DM642
extern DllExport int  RMB_getEWidth(int Index)
{
	return focusImage[Index].Width;
}

extern DllExport int  RMB_getEHeight(int Index)
{
	return focusImage[Index].Height;
}

extern DllExport int  RMB_getLeanFlag()
{
	return focusImage[ImageRotateIndex].Lean;
}

extern DllExport short GetFivePointsAverageGrey(short x, short y,int picIndex)
{
	if (x >= 1 && x < focusImage[picIndex].Width - 1 && y >= 1 && y < focusImage[picIndex].Height - 1)
	{
		int centerP = focusImage[picIndex].Hist[GetRotatePixel(x, y , picIndex)];
		int leftP = focusImage[picIndex].Hist[GetRotatePixel(x - 1, y,picIndex)];
		int rightP = focusImage[picIndex].Hist[GetRotatePixel(x + 1, y,picIndex)];
		int topP = focusImage[picIndex].Hist[GetRotatePixel(x, y - 1,picIndex)];
		int bottomP = focusImage[picIndex].Hist[GetRotatePixel(x, y + 1,picIndex)];
		return (short)((centerP + leftP + rightP + topP + bottomP) / 5);
	}
	return 256;
}

extern DllExport int WinImageInitPlus(unsigned char* pData[3], int iDatLen[3],short  sDataNum,
	int processIndex, int isReflect)
{
	
	//LARGE_INTEGER st,en;
	//long time1,time2 = 0;
	
	int suc = 0;
	IMG_BuffObj obj;

	obj.pBuff[0] = pData[0]+4;
	obj.iBuffLength[0] = iDatLen[0];
	obj.sBuffNum = 1;
	if(sDataNum==2 ) {
		obj.pBuff[1] = pData[1]+4;
		obj.iBuffLength[1] = iDatLen[1];
		obj.sBuffNum = 2;
	}
	if(sDataNum==3 ) {
		obj.pBuff[1] = pData[1]+4;
		obj.iBuffLength[1] = iDatLen[1];
		obj.pBuff[2] = pData[2]+4;
		obj.iBuffLength[2] = iDatLen[2];
		obj.sBuffNum = 3;
	}

	suc = PreProcessInit(obj,processIndex,isReflect);
	if (!suc) return 0;
	if ( 1 != RMB_PreProcess() ) return 0;			//RMB_PreProcess成功返回1， 失败的话根据失败类型会返回-5 至 0

	//QueryPerformanceCounter(&st);
	//HistEqual(0);
	//QueryPerformanceCounter(&en);
	//time1 = en.LowPart - st.LowPart;
	//QueryPerformanceCounter(&st);
	HistEqualFast(0);
	//QueryPerformanceCounter(&en);
	//time2 = en.LowPart - st.LowPart;
	if(sDataNum==2)
		HistEqualFast(1);
	if (sDataNum==3)
	{
		HistEqualFast(1);
		HistEqualFast(2);
	}
	return 1;
}

extern DllExport void GetHist(int* _hist,int picIndex)
{	
	int i;
	for (i = 0; i < HIST_SIZE; ++i)
	{
		_hist[i] = focusImage[picIndex].Hist[i];
	}
}

extern DllExport void GetPoints(FocusPoint* tl,FocusPoint* tr,FocusPoint* bl,FocusPoint* br)
{
	*tl = focusImage[ImageRotateIndex].TopLeft;
	*tr = focusImage[ImageRotateIndex].TopRight;
	*bl = focusImage[ImageRotateIndex].BottomLeft;
	*br = focusImage[ImageRotateIndex].BottomRight;
}

extern DllExport void RmbGetReviseImg(unsigned char* img, int x, int y, int width, int height,int picIndex)
{
	int index = 0,i,j,xi,yj;
	for (j=y; j<y+height; j++)
	{
		for (i=x; i<x+width; i++)
		{
			xi = i;
			yj = j;
			if (deno == 0xB || deno == 0xD)
			{
				yj = RMB_EHeight - yj;
			}
			if (deno == 0xB || deno == 0xC)
			{
				xi = RMB_EWidth - xi;
			}
			img[index++] = GetRotatePixel(xi, yj,picIndex);
		}
	}
}

extern DllExport void RmbGetReviseImgByStride(unsigned char* img,int x, int y, int width, int height, int stride,int picIndex)
{	
	int index = 0, i, j, xi, yj;
	for ( j=y; j<height; j++)
	{
		for ( i=x; i<width; i++)
		{
			 xi = i;
			 yj = j;
			if (deno == 0xB || deno == 0xD)
			{
				yj = RMB_EHeight - yj;
			}
			if (deno == 0xB || deno == 0xC)
			{
				xi = RMB_EWidth - xi;
			}
// 			if (picIndex==0)
// 			{
// 				yj*=2;
// 			}
			img[index++] = GetRotatePixel(xi, yj,picIndex);
		}
		index += stride-width;
	}
}
#endif



