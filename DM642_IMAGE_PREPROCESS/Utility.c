#include "..\Header\rmb.h"

#define MISMATCHPOINT 9

#ifdef  CHIP_DM642
#pragma CODE_SECTION(ImageCheck,".rmbProcessSec");
#endif
int ImageCheck(const short* Points,int PointsNum,int turn,int picIndex);

#ifdef  CHIP_DM642
#pragma CODE_SECTION(GetRectImage,".rmbProcessSec");
#endif
void GetRectImage(unsigned char* Img , int x,int y,int width,int height,int turn,int picIndex);

#ifdef  CHIP_DM642
#pragma CODE_SECTION(AdaptiveThreshold,".rmbProcessSec");
#endif
void AdaptiveThreshold(unsigned char* srcImg,int width,int height);

#ifdef  CHIP_DM642
#pragma CODE_SECTION(MinDistanceSearch,".rmbProcessSec");
#endif
int MinDistanceSearch(unsigned char* srcImg,int srcWidth,int srcHeight,unsigned char* model,int modelWidth,int modelHeight);

#ifdef  CHIP_DM642
#pragma CODE_SECTION(ImgDistance,".rmbProcessSec");
#endif
int ImgDistance(unsigned char* srcImg,int srcWidth,int x,int y,int modelWidth,int modelHeight,unsigned char* model);

/// <summary>
/// ��ֵ��ĳһ���򣬲���ģ��ƥ�䷽���ҳ����ƥ��
/// </summary>
/// <param name="srcImg">��������</param>
/// <param name="srcWidth">��������Ŀ��</param>
/// <param name="srcHeight">��������ĸ߶�</param>
/// <param name="model">ģ��</param>
/// <param name="modelWidth">ģ�͵Ŀ��</param>
/// <param name="modelHeight">ģ�͵ĸ߶�</param>
void GetRectImage(unsigned char* Img , int x,int y,int width,int height,int turn,int picIndex)
{
	int h,w;
	for(h = 0; h < height; ++h)
		for(w = 0; w < width; ++w)
		{
			if(turn  == 0 )
				Img[h*width+w] = GetRotatePixel(w + x,h + y,picIndex); 
			else
				Img[h*width+w] = GetRotatePixel(RMB_EWidth - (w+x) - 1,RMB_EHeight - (h+y) - 1,picIndex);
		}
}

/// <summary>
/// ��ֵ��ĳһ���򣬲���ģ��ƥ�䷽���ҳ����ƥ��
/// </summary>
/// <param name="srcImg">��������</param>
/// <param name="srcWidth">��������Ŀ��</param>
/// <param name="srcHeight">��������ĸ߶�</param>
/// <param name="model">ģ��</param>
/// <param name="modelWidth">ģ�͵Ŀ��</param>
/// <param name="modelHeight">ģ�͵ĸ߶�</param>
/// <returns>��С����</returns>
int MinDistanceSearch(unsigned char* srcImg,int srcWidth,int srcHeight,unsigned char* model,int modelWidth,int modelHeight)
{
	int x,y;//,w,h;
	int mindistance = 0x7fffffff;
	AdaptiveThreshold(srcImg,srcWidth,srcHeight);
	for (y = 0; y < srcHeight - modelHeight; ++y)
	{
		for (x = 0; x < srcWidth - modelWidth; ++x)
		{
			int distance = ImgDistance(srcImg,srcWidth,x,y,modelWidth,modelHeight,model);
			if (distance < mindistance)
			{
				mindistance = distance;
//				w = x;
//				h = y;
			}
		}
	}
	return mindistance;
}

/// <summary>
/// �������������ģ��֮��ľ���,���к�ɫ�����ľ������2
/// </summary>
/// <param name="srcImg">��������</param>
/// <param name="srcWidth">��������Ŀ��</param>
/// <param name="xx">����ڴ����������ʼ����X</param>
/// <param name="yy">����ڴ����������ʼ����Y</param>
/// <param name="modelWidth">ģ�͵Ŀ��</param>
/// <param name="modelHeight">ģ�͵ĸ߶�</param>
/// <param name="model">ģ��</param>
/// <returns>����</returns>
int ImgDistance(unsigned char* srcImg,int srcWidth,int xx,int yy,int modelWidth,int modelHeight,unsigned char* model)
{
	int x,y,Dis = 0;
	int	index1,index2;
	for (y = 0; y < modelHeight; y++)
	{
		for (x = 0; x < modelWidth; x++)
		{
			index1 = y*modelWidth+x;
			index2 = srcWidth * (y+yy) + x + xx;
			if (model[index1] != srcImg[index2])
				Dis = Dis - model[index1] + 2;
		}
	}
	return Dis;
}

/// <summary>
/// ����Ӧ��ֵ��
/// </summary>
/// <param name="srcImg">input:ԭ���ؾ���,output:��ֵ�����ؾ���</param>
/// <param name="width">���</param>
/// <param name="height">�߶�</param>
void AdaptiveThreshold(unsigned char* srcImg,int width,int height)
{
	unsigned int* integraImg = (unsigned int*)malloc(width*height*sizeof(int));

	int i, j;
	//int S = width >> 3;
	int S = 16;
	int s2 = S / 2;
	//float T = 0.5f;

	for (i = 0; i < width; i++)
	{
		int sum = 0;
		for (j = 0; j < height; j++)
		{
			int index = j * width + i;
			sum += srcImg[index];
			if (i == 0)
			{
				integraImg[index] = sum;
			}
			else
			{
				integraImg[index] = integraImg[index - 1] + sum;
			}
		}
	}

	for (i = 0; i < width; i++)
	{
		for (j = 0; j < height; j++)
		{
			int count, sum;
			int index = j * width + i;
			int x1 = i - s2;
			int x2 = i + s2;
			int y1 = j - s2;
			int y2 = j + s2;
			if (x1 < 0) x1 = 0;
			if (x2 >= width) x2 = width - 1;
			if (y1 < 0) y1 = 0;
			if (y2 >= height) y2 = height - 1;
			count = (x2 - x1) * (y2 - y1);
			sum = integraImg[y2 * width + x2] -
				integraImg[y1 * width + x2] -
				integraImg[y2 * width + x1] +
				integraImg[y1 * width + x1];
			if (srcImg[index] * 4 * count < sum * 3)
				srcImg[index] = 0;
			else
				srcImg[index] = 1;

		}
	}
	free(integraImg);
}

/// <summary>
/// ���ݵ㼯ȥʶ��ָ��·�����ļ��Ƿ�ƥ��
/// </summary>
/// <param name="Points">�㼯</param>
/// <param name="PointsNum">����</param>
/// <returns>��ƥ�����</returns>
int ImageCheck(const short* Points,int PointsNum,int turn,int picIndex)
{
	int mismatchPt = 0,i;
	short x,y,g,isWhite,ag;
	for (i = 0; i < PointsNum; ++i)
	{
		x = Points[3*i];
		y = Points[3*i+1];
		g = Points[3*i+2];
		if(g>=256){
			g-=256;
			isWhite = 0;
		}else isWhite = 1;

		if(turn==1){
			ag =  GetFivePointsAverageGrey(RMB_EWidth - x - 1 ,RMB_EHeight - y - 1,picIndex);
		}else 
			ag = GetFivePointsAverageGrey(x, y,picIndex);
		if(ag >= 256 || (isWhite == 1 && ag < g) || (isWhite == 0 && ag > g) )
			++mismatchPt;
		if(mismatchPt >= MISMATCHPOINT + 3) return mismatchPt;		//OPT
	}

	return mismatchPt;
}

