#include "simulator.h"
#include <stdio.h>
#include <math.h>
#include <string.h>
#include "KeyPoints.h"

#ifdef __cplusplus
#define DLLAPI extern "C" __declspec(dllexport)
#else
#define DLLAPI __declspec(dllexport)
#endif

unsigned short hasRead;
unsigned short StartPoint,EndPoint;
unsigned short iul;
//unsigned short * sa_point;
short i,j;
short MID[CASH_TYPE]={1,2,3,4,5,6,7,8,9};
short Lean;		//判断平衡
short lastface=8;
float rate;
float xshift,yshift;
float g_matrix[2][3];
short unMatch;
float TempFloat;
short Itmp1;
float square;
float EW,EH;		
long  PicSquare;				//钱币最大面积
float cosa,sina;
float x_center,y_center;
long  AverageGrey;
short lineNum;
short PicWidth;
short LeftPoint[2][2];		// []-> Top/Down [][]->x/y
short RightPoint[2][2];
float Ftmp1;
unsigned short  *RowIndex[MAX_HEIGHT];
unsigned short hist[HIST_SIZE];
short  startP[200],endP[200];

void LoadFileBuffer(unsigned short	* buffer)
{
	unsigned short bufLen;
	unsigned short *sa_point = buffer;

	bufLen = *((unsigned int*)sa_point);
	hasRead = sizeof(unsigned int)/sizeof(unsigned short);
	Set();

	while (hasRead < (unsigned short)(bufLen))
	{
		memcpy(&lineNum,sa_point +hasRead,sizeof(unsigned short));
		RowIndex[lineNum] = (unsigned short*)(sa_point+hasRead);
		hasRead += sizeof(unsigned short)/sizeof(unsigned short);
		memcpy(&StartPoint,sa_point+hasRead,sizeof(unsigned short));
		hasRead += sizeof(unsigned short)/sizeof(unsigned short);
		memcpy(&EndPoint,sa_point+hasRead,sizeof(unsigned short));
		hasRead += sizeof(unsigned short)/sizeof(unsigned short);

		startP[lineNum] = StartPoint; endP[lineNum] = EndPoint;
		for (iul =  hasRead; iul < hasRead + (EndPoint - StartPoint)/2 ; ++iul )
		{
		}
		if ((EndPoint - StartPoint)%2==1)	//偶数个
		{
		}
		hasRead += (EndPoint-StartPoint)/2 * sizeof(unsigned short)/sizeof(unsigned short) +1;

		{
			unsigned short temp = 0;
			unsigned short infosize = 0 ;
			int i = 0;
			memcpy(&temp,sa_point +hasRead,sizeof(unsigned short));		
			if (temp == 0xffff)
			{
				hasRead += sizeof(unsigned short)/sizeof(unsigned short);

				memcpy(&infosize,sa_point+hasRead,sizeof(unsigned short));
				if(infosize <= 10000){
					hasRead += sizeof(infosize);
					for(i = 0; i < infosize; ++i){
						hasRead += sizeof(unsigned short)/sizeof(unsigned short);
					}
				}else
					hasRead -= sizeof(unsigned short)/sizeof(unsigned short);
			}
		}

#ifdef CORNERRECOVERY
		if(lineNum>2 && lineNum < MAX_HEIGHT)//计算链码
		{
			Itmp1= startP[lineNum]-startP[lineNum-3];
			leftLinkCode[lineNum]=GetLinkCode3(Itmp1);	//统计链码
			Itmp1= endP[lineNum]- endP[lineNum-3];
			rightLinkCode[lineNum]=GetLinkCode3(Itmp1);	//统计链码
		} 
#endif
		if( startP[lineNum] <= LeftPoint[0][0])           //get left edge position
		{
			if (LeftPoint[0][0] != startP[lineNum])
			{
				LeftPoint[0][1] = lineNum;
			}
			LeftPoint[1][1] = lineNum;
			LeftPoint[0][0] = LeftPoint[1][0] =  startP[lineNum];
		}
		if( endP[lineNum] >= RightPoint[0][0])     //get Right edge position
		{  
			if (RightPoint[0][0] != endP[lineNum])
			{
				RightPoint[0][1] = lineNum;
			}
			RightPoint[1][1] = lineNum;
			RightPoint[0][0] = RightPoint[1][0] = endP[lineNum];
		}    
		PicSquare=PicSquare+endP[lineNum]-startP[lineNum]+1; 
		for (i = startP[lineNum] ; i <= endP[lineNum]; ++i)
		{
			unsigned short* Pointer;
			short ii = i-startP[lineNum];
			unsigned short g;
			Pointer = RowIndex[lineNum]+(ii>>1)+3;
			if ( (ii&1) != 0)
			{
				g = (*Pointer)&0xff;
			}else 
				g = ( (*Pointer)>>8 )&0xff;
			hist[g]++;
			AverageGrey += g;
		}
	}
}

void Set(){
	PicSquare=0;

	LeftPoint[0][0] = LeftPoint[1][0] = MAX_WIDTH;
	RightPoint[0][0] = RightPoint[1][0] = 0;

	LeftPoint[0][1] = LeftPoint[1][1] = 0;
	RightPoint[0][1] = RightPoint[1][1] = 0;
	unMatch = 0xff;
	lineNum = 0;
	EH = EW = 0.0f;

	AverageGrey = 0;	//平均灰度

	for(i=0;i<HIST_SIZE;++i)
	{
		hist[i] = 0;
	}
}

short Recognize(void)
{
	short found;
	short curID = 0;
	short checkIndex[CASH_TYPE];
	short curCount = 0;/*,lastface=8;*/
	AverageGrey = (long)( (AverageGrey*1.0) /(PicSquare*1.0)) ;
	unMatch = 0xFF;
	//如果在行数或是面积上有所不符合 直接报以图像3D信息不符
	if(lineNum<40){
		unMatch = 0xEC;
		return 0 ;
	}
	histEqual();			//直方图均衡化
	getWH();				//得到3D与旋转基本数据
#ifdef MOSTOPT
	GetFirstLine();			//得到第一行的值
#endif
	//如果长与宽计算并不是预算时的限制也报以3D信息不符的错误
	if( !( WITHIN(EW,117,153) && WITHIN(EH,56,80) && WITHIN(PicSquare,6660,12200) ) )
	{
		unMatch = 0x3D;
		return 0;
	}

	if ( WITHIN(EW,117,124) && WITHIN(EH,56,65))		// 5
	{
		checkIndex[curCount] = 0;
		curCount++;
	}

	if ( WITHIN(EW,123,128) && WITHIN(EH,58,68) )		// 10
	{
		checkIndex[curCount] = 1;
		curCount++;
	}

	if ( WITHIN(EW,127,133) && WITHIN(EH,60,69) )		// 20
	{
		checkIndex[curCount] = 2;
		curCount++;
	}

	if ( WITHIN(EW,131,137) && WITHIN(EH,62,71) )		// 50
	{
		checkIndex[curCount] = 3;
		curCount++;
	}

	if ( WITHIN(EW,135,141) && WITHIN(EH,64,73) )		// 100
	{
		checkIndex[curCount] = 4;
		curCount++;
	}

	if ( WITHIN(EW,139,145) && WITHIN(EH,66,75) )		// 200
	{
		checkIndex[curCount] = 5;
		curCount++;
	}

	if ( WITHIN(EW,143,148) && WITHIN(EH,68,77) )		// 500
	{
		checkIndex[curCount] = 6;
		curCount++;
	}

	if ( WITHIN(EW,147,153) && WITHIN(EH,70,80) )		// 1000
	{
		checkIndex[curCount] = 7;
		curCount++;
	}

	{
		short _WPT;
		short MissPt;
		short id,iface,ii;
		found = 0;
		for (ii = 0 ; ii < curCount; ++ii)
		{
			id = checkIndex[ii];
			if (curID == MID[id] || found > 1)
			{
				continue; 
			}
			MissPt = 0xff;
			for (iface = 0; iface < 4; ++iface)		// here can sure his face..
			{
				_WPT = ImageCheck(Point[id][iface*2+0],Threshold[id][iface*2+0],
					PointSize[id][iface*2+0],1,0,MISMATCHPOINT);
				if(_WPT < MISMATCHPOINT)		//白点小于的时候再进一步判断
					MissPt = ImageCheck(Point[id][iface*2+1],Threshold[id][iface*2+1],
					PointSize[id][iface*2+1],0,_WPT,MISMATCHPOINT);
				else MissPt = _WPT;

				if (MissPt < MISMATCHPOINT)
				{
					++found;
					unMatch = MissPt;
					curID = MID[id];
					lastface=iface;
					break;
				}
			}
		}
	}
	if(found == 1 )
	{
// 		if ( curID == 5 && (lastface == 0||lastface == 1))
// 		{
// 			short x,y,ii2,jj2;
// 			unsigned short srimg[2][36]={0};
// 			unsigned short dsimg,flag2=0;
// 			for (ii2=0;ii2<2;ii2++)
// 			{
// 				for (jj2=0;jj2<36;jj2++)
// 				{
// 					x=127*(1-lastface)+10*lastface+ii2;
// 					y=15+5*lastface+jj2;
// 					GetFixedPoint(x,y,&x,&y);
// 					srimg[ii2][jj2] = GetPointGrey(x,y);
// 				}
// 			}
// 			for(ii2=0;ii2<32;ii2++)
// 			{
// 				dsimg = abs(srimg[0][ii2+3]-srimg[0][ii2])
// 					+abs(srimg[1][ii2+3]-srimg[1][ii2]);
// 				if(dsimg>40)flag2++;
// 			}
// 			if (flag2<4)curID=0xFA;
// 		}
// 		
		return curID;
	}
	else
		if(found == 2)
			return 0 ;
	return 0;
}

void histEqual(void)
{
	float SIZEofPIXEL;
	short i;
	PicWidth = RightPoint[0][0] - LeftPoint[0][0] + 1;			//图片的宽度
	//直方图均衡化
	SIZEofPIXEL = (HIST_SIZE -1) * 1.0f / ( (float) (PicSquare*1.0f));
	for (i = 1; i < HIST_SIZE; ++i )
	{	
		hist[i]+= hist[i-1];
		hist[i - 1 ] *= SIZEofPIXEL;
	}
	hist[HIST_SIZE - 1 ] *= SIZEofPIXEL; 
}

float Sqrt(float a)
{
	int _temp;
	float _x,_y,_f;

	_f = 1.5f;
	_x = a * 0.5f;
	_y = a;

	_temp = *(int*)&_y;
	_temp = 0x5f3759df - (_temp>>1);

	_y = *(float*)&_temp;
	_y = _y*(_f - (_x*_y*_y));
	_y = _y*(_f - (_x*_y*_y));
	_y = _y*(_f - (_x*_y*_y));

	a = a * _y;

	return a;
}

void getWH(void )
{
	short i,j;
	int LineX,LineY,exChange;
	int tempHeights[LineCountFor3D];
	short newleft ;
#ifdef CORNERRECOVERY
	ALINE l1,l2,r1,r2;		//边
	short lr,rr;			//左边界及右边界的特征
	long Result1,tmpR1,tmpR2,tmpR3,tmpR4;
#endif
	newleft = LeftPoint[0][0];
	square = (float)(lineNum +1 ) * PicWidth ;
	rate = (PicSquare * 1.0f)/(float)(square * 1.0f);

	if( rate < SLOPE_RATE){
		if((startP[lineNum]- LeftPoint[0][0])<(RightPoint[0][0]-startP[lineNum]))//左倾
			Lean = 1;
		else Lean = 2;
	}else Lean = 0;

#ifdef CORNERRECOVERY

	l1.startPos = 0xff;
	r1.startPos = 0xff;
	l2.startPos = 0xff;
	r2.startPos = 0xff;

	flag.TOPLEFT	=	TRUE;
	flag.TOPRIGHT	=	TRUE;
	flag.BOTTOMLEFT	=	TRUE;
	flag.BOTTOMRIGHT	=	TRUE;

	lr = AnalyzeLinkCode(leftLinkCode,lineNum,&l1,&l2);
	rr = AnalyzeLinkCode(rightLinkCode,lineNum,&r1,&r2);

	/*
	 * 只找到一条边界的情况
	 */
	if (lr == 2)
	{
		if(lineNum - l1.endPos >= PosOffset)
			flag.BOTTOMLEFT = FALSE;
		if (l1.startPos >= PosOffset +3 )
			flag.TOPLEFT = FALSE;
	}
	if(rr == 2)
	{
		if (lineNum - r1.endPos >= PosOffset)
			flag.BOTTOMRIGHT = FALSE;
		if(r1.startPos >= PosOffset+3)
			flag.TOPRIGHT = FALSE;
	}
	
	/*
	 * 找到两条边
	 */

	if (lr == 1)
	{		
		if (Lean == 1)
		{
			if (lineNum - l2.endPos >= PosOffset)
				flag.BOTTOMLEFT = FALSE;
			if (l2.startPos - l1.endPos >= PosOffset)
				flag.TOPLEFT = FALSE;
		}else if (Lean == 2)
		{
			if (l1.startPos >= PosOffset+3)
				flag.TOPLEFT = FALSE;
			if(l2.startPos - l1.endPos >= PosOffset)
				flag.BOTTOMLEFT = FALSE;
		}
		if( !(flag.TOPLEFT&&flag.BOTTOMLEFT) )	{
			tmpR1 = startP[l1.endPos]*l1.startPos - startP[l1.startPos]*l1.endPos;
			tmpR2 = startP[l2.endPos]-startP[l2.startPos];
			tmpR3 = startP[l2.endPos]*l2.startPos - startP[l2.startPos]*l2.endPos;
			tmpR4 = startP[l1.endPos]-startP[l1.startPos];

			tmpR1 = tmpR1 * tmpR2;
			tmpR3 = tmpR3 * tmpR4;
			Result1 = tmpR1 - tmpR3;

			tmpR1 = l1.startPos-l1.endPos;
			tmpR1 = tmpR1 * tmpR2;

			tmpR3 = l2.startPos-l2.endPos;
			tmpR3 = tmpR3 * tmpR4;

			tmpR1 = tmpR1 - tmpR3;

			Result1 = Result1 / tmpR1;
			newleft = (short)Result1;

		}	
		/*    LeftPoint[0][0] = ((startP[l1.endPos]*l1.startPos - startP[l1.startPos]*l1.endPos)*(startP[l2.endPos]-startP[l2.startPos])
			-(startP[l2.endPos]*l2.startPos - startP[l2.startPos]*l2.endPos)*(startP[l1.endPos]-startP[l1.startPos]))/
			((l1.startPos-l1.endPos)*(startP[l2.endPos]-startP[l2.startPos])-(startP[l1.endPos]-startP[l1.startPos])*(l2.startPos-l2.endPos)); */
	}

	if(rr == 1){
		if (Lean == 1)
		{
			if (r2.startPos - r1.endPos >= PosOffset)
				flag.BOTTOMRIGHT = FALSE;
			if (r1.startPos >= PosOffset+3 )
				flag.TOPRIGHT = FALSE;
		}else if (Lean == 2)
		{
			if (lineNum - r2.endPos >= PosOffset)
				flag.BOTTOMRIGHT = FALSE;
			if(r2.startPos - r1.endPos >= PosOffset)
				flag.TOPRIGHT = FALSE;
		}
	}
#endif

	EW = EH = -1;
	if(lineNum <= 32 )	return;

#ifndef CORNERRECOVERY
    if(rate<WH_RATE)
	{
		if(Lean == 1)
		{	
			if(RightPoint[1][1] > 16)
			{
				LineX = lineNum - RightPoint[1][1];
				for (i = 0; i < LineCountFor3D ; ++i)
				{
					tempHeights[i] = endP[ RightPoint[1][1]/2 + 6 - 3 * i ] - startP[ (lineNum + LeftPoint[0][1] )/2 + 6 - 3 * i ];
				}
				for ( i = 0; i < LineCountFor3D - 1; ++i)
				{
					for ( j = i+1; j < LineCountFor3D ; ++j)
					{
						if (tempHeights[j]<tempHeights[i])
						{
							exChange = tempHeights[i];
							tempHeights[i] = tempHeights[j];
							tempHeights[j] = exChange;
						}
					}
				}

				LineY = tempHeights[LineCountFor3D/2];
				TempFloat = (float)(LineY*LineY + LineX*LineX);
				EW = Sqrt(TempFloat);
				
				EH = (PicSquare*1.0f)/EW;	
			}	
		}
		else
		{
			if (LeftPoint[1][1] > 16)
			{
				LineX = lineNum - LeftPoint[1][1];
				for ( i = 0; i < LineCountFor3D ; ++i)
				{
					tempHeights[i] = endP[ (lineNum + RightPoint[0][1] )/2 + 6 - 3 * i ] - startP[  LeftPoint[1][1] /2 + 6 - 3 * i ];
				}
				for ( i = 0; i < LineCountFor3D - 1; ++i)
				{
					for ( j = i+1; j < LineCountFor3D ; ++j)
					{					
						if (tempHeights[j]<tempHeights[i])
						{
							exChange = tempHeights[i];
							tempHeights[i] = tempHeights[j];
							tempHeights[j] = exChange;
						}
					}
				}

				LineY = tempHeights[LineCountFor3D/2];
				TempFloat = (float)(LineY*LineY + LineX*LineX);
				EW = Sqrt(TempFloat);
				EH = (PicSquare*1.0f)/EW;
			}
		}
	}
#else
	if(rate < WH_RATE){
		if (Lean == 1)
		{
			if (flag.TOPLEFT && flag.TOPRIGHT && flag.BOTTOMLEFT && flag.BOTTOMRIGHT)  //是否特别完整都用这个
			{
				LineX = lineNum - RightPoint[1][1];
				for (i = 0; i < LineCountFor3D ; ++i)
				{
					tempHeights[i] = endP[ RightPoint[1][1]/2 + 6 - 3 * i ] - startP[ (lineNum + LeftPoint[0][1] )/2 + 6 - 3 * i ];
				}
			}else{
				if(flag.BOTTOMLEFT && flag.BOTTOMRIGHT){
					LineX = lineNum - RightPoint[1][1];
					for (i = 0; i < LineCountFor3D ; ++i)
					{
						tempHeights[i] = endP[ RightPoint[1][1] - 15 + 3 * i ] - startP[ lineNum - 15 + 3 * i ];
					}
				}else if(flag.TOPLEFT && flag.TOPRIGHT){
					LineX = LeftPoint[0][1] ;
					for (i = 0; i < LineCountFor3D ; ++i)
					{
						tempHeights[i] = endP[ 3 + 3 * i ] - startP[ LeftPoint[0][1]+ 3 + 3 * i ];
					}
				}else {
					LineX = lineNum - RightPoint[1][1];
					for (i = 0; i < LineCountFor3D ; ++i)
					{
						tempHeights[i] = endP[ RightPoint[1][1]/2 + 6 - 3 * i ] - startP[ (lineNum + LeftPoint[0][1] )/2 + 6 - 3 * i ];
					}
				}
			}
		}else{
			if (flag.TOPLEFT && flag.TOPRIGHT && flag.BOTTOMLEFT && flag.BOTTOMRIGHT)
			{
				LineX = lineNum - LeftPoint[1][1];
				for (i = 0; i < LineCountFor3D ; ++i)
				{
					tempHeights[i] = endP[ (lineNum + RightPoint[0][1] )/2 + 6 - 3 * i ] - startP[  LeftPoint[1][1] /2 + 6 - 3 * i ];
				}
			}else{
				if(flag.BOTTOMLEFT && flag.BOTTOMRIGHT){
					LineX = lineNum - LeftPoint[1][1];
					for (i = 0; i < LineCountFor3D ; ++i)
					{
						tempHeights[i] = endP[ lineNum - 15 + 3 * i ] - startP[ LeftPoint[1][1] - 15 + 3 * i ];
					}
				}else if(flag.TOPLEFT && flag.TOPRIGHT){
					LineX = RightPoint[0][1] ;
					for (i = 0; i < LineCountFor3D ; ++i)
					{
						tempHeights[i] = endP[ RightPoint[0][1] + 3 + 3 * i ] - startP[ 3 + 3 * i ];
					}
				}else{
					LineX = lineNum - LeftPoint[1][1];
					for (i = 0; i < LineCountFor3D ; ++i)
					{
						tempHeights[i] = endP[ (lineNum + RightPoint[0][1] )/2 + 6 - 3 * i ] - startP[  LeftPoint[1][1] /2 + 6 - 3 * i ];
					}
				}
			}
		}
		for (i = 0; i < LineCountFor3D - 1; ++i)
		{
			for (j = i+1; j < LineCountFor3D ; ++j)
			{
				int exChange;
				if (tempHeights[j]<tempHeights[i])
				{
					exChange = tempHeights[i];
					tempHeights[i] = tempHeights[j];
					tempHeights[j] = exChange;
				}
			}
		}

		LineY = tempHeights[LineCountFor3D/2];
		TempFloat = (float)(LineY*LineY + LineX*LineX);
		EW = Sqrt(TempFloat);
		EH = (PicSquare*1.0f)/EW;
	}
#endif
	if (EW < 0)	       //平衡
	{   
		for ( i = 0; i < LineCountFor3D ; ++i)
		{
			tempHeights[i] = endP[ lineNum / 2 + 6 - 3 * i ] - startP[  lineNum /2 + 6 - 3 * i ];
		}
		for ( i = 0; i < LineCountFor3D - 1; ++i)
		{
			for ( j = i+1; j < LineCountFor3D ; ++j)
			{
				if (tempHeights[j]<tempHeights[i])
				{
					exChange = tempHeights[i];
					tempHeights[i] = tempHeights[j];
					tempHeights[j] = exChange;
				}
			}
		}

		EW = (float)(tempHeights[LineCountFor3D/2]);
		EH = (PicSquare*1.0f)/EW;
	}

	if(rate >= SLOPE_RATE)
	{
		cosa = 1.0f;
		sina = 0.0f;
		x_center = EW * 0.5f + newleft;
		y_center = EH * 0.5f ;
	}
	else
	{
		if(Lean == 1)
		{
#ifdef CORNERRECOVERY
			if( lr == 2 )
			{
				l2.startPos = l1.startPos;
				l2.endPos = l1.endPos;
			}else if(lr == 0){
				l2.startPos = LeftPoint[0][1];
				l2.endPos = lineNum - 3;
			}

			Ftmp1 = GetSlope1(startP,l2.startPos,l2.endPos);
#else
			Ftmp1 = GetSlope1(startP,LeftPoint[0][1],lineNum-3);
#endif
			cosa = 1.0 / Sqrt((Ftmp1*Ftmp1)+1);
			sina = Ftmp1 * cosa ;
	//		y_center = (INT16)(g_imHeight / 2*cosa + g_imWidth/2*sina);
			y_center = lineNum * 0.5f + 0.5f;
			x_center = EH * 0.5f *sina + EW * 0.5f * cosa + newleft;		
		}
		else
		{
#ifdef CORNERRECOVERY
			if( rr == 2 )
			{
				r2.startPos = r1.startPos;
				r2.endPos = r1.endPos ;
			}else if(rr == 0){
				r2.startPos = RightPoint[0][1];
				r2.endPos = lineNum - 3;
			}

			Ftmp1 = GetSlope1(endP,r2.startPos,r2.endPos);
#else
			Ftmp1 = GetSlope1(endP,RightPoint[0][1],lineNum - 3);
#endif
			cosa = 1.0 / Sqrt((Ftmp1*Ftmp1)+1);
			sina = -Ftmp1 * cosa ;
	//		y_center = (INT16)( g_imWidth  / 2 * (-sina) + g_imHeight /2*cosa);
			y_center = lineNum * 0.5f + 0.5f ;
			x_center = EH * 0.5f * (-sina) +  EW * 0.5f * cosa + newleft;		
		}
	}	
	xshift =  - EW * 0.5f + x_center;
	yshift =  - EH * 0.5f + y_center;

	g_matrix[0][0] = cosa;
	g_matrix[0][1]  = sina;
	g_matrix[0][2]  = (1.0-cosa)*x_center - sina*y_center;
	g_matrix[1][0]  = -sina;
	g_matrix[1][1]  = cosa;
	g_matrix[1][2]  = sina*x_center + (1.0-cosa)*y_center;
}

float GetSlope1(short* point, short iStart,short iEnd)
{
	long sumX = 0;
	long sumX2 = 0;
	long sumY = 0;
	long sumXY = 0;
	long den;
	short idx;//,step;
	float rt;
	//	short PointNum = (iEnd - iStart + 1)/2 + (iEnd - iStart + 1)%2;
	short PointNum=0;

	for(idx = iStart;idx<=iEnd;idx++)
	{
		sumX += idx;
		sumY += point[idx];
		sumX2 += idx*idx;
		sumXY += idx*(unsigned short)point[idx];
		PointNum++;
	}
	den = (sumX*sumX-(PointNum)*sumX2);
	if(den==0) 
	{
		return 0;
	}
	rt = fabs((float)(sumX*sumY - (PointNum)*sumXY)/(float)den);
	return rt ;//返回弧度
}

short ImageCheck(const unsigned short Point[],const unsigned short Threshold[],
	const unsigned short PointSize,short isWhite,short Count,short MaxReturn)
{
	short _h,_w,index1;
	short x,y;
	short threshold;
	short rt_g;

	for(index1=0;index1<PointSize;index1++)
	{
		x = ( (Point[index1])>>8 ) & 0xff;
		y = ( (Point[index1])&0xff);

		GetFixedPoint(x,y,&_w,&_h);	
		rt_g = GetPointGrey(_w,_h);
		if( ( index1&1 ) ==0)
		{
			threshold=( Threshold[index1>>1]>>8 ) & 0xff;
		}
		else
		{
			threshold=( Threshold[index1>>1] & 0xff);
		}
		if ( WITHIN(rt_g,0,256) )
		{
			if (isWhite)
			{
				if( hist[rt_g] <= threshold) ++Count;
			}else{
				if( hist[rt_g] >= threshold) ++Count;
			}
		}else
			++Count;
		if(Count >= MaxReturn) return Count;
	}
	return Count;
}

#ifdef CORNERRECOVERY
short GetLinkCode3( short del )
{
	//get link code
	if(del>0)
	{
		if(del>26) return  18;
		else  return leftCode[del];
	}else
	{
		del=-del;
		if(del>26) return 36;
		else    return rightCode[del];
	}
}

short AnalyzeLinkCode( short link[],short linkCount,ALINE * l1,ALINE * l2 )
{
	const int MIN_LINE_POINT = 4;
	const float MIN_DIFF = 2.3f;

	ALINE tempLine[LINE_COUNT];
	short isFind = FALSE;
	short sum=0,count=0;
	short i,j;

	l1->len=0;
	l2->len=0;

	for(i=0;i<LINE_COUNT;i++)  //初始化
	{
		tempLine[i].startPos=0;
		tempLine[i].endPos=0;
		tempLine[i].len=0;
		tempLine[i].value=0;
	}

	//link[linkCount]=1000;
	for(j=3,i=0;j<=linkCount;)//获取线特征
	{
		if(tempLine[i].startPos==0)//
		{
			tempLine[i].startPos=j;
			sum=link[j];
			count=1;
			tempLine[i].len=1;
			tempLine[i].value=sum;
			j++;
		}else{
			//if(abs(link[j]-tempLine[i].value)<=MIN_DIFF||abs(link[j+1]-tempLine[i].value )<=MIN_DIFF)//合条件，将点加到直线上
			if(WITHIN(link[j]-tempLine[i].value,-MIN_DIFF,MIN_DIFF))//合条件，将点加到直线上
			{
				tempLine[i].endPos=j;
				sum=sum+link[j];
				count++;
				//	tempLine[i].value=(int)(sum/(float)count+0.5);
				tempLine[i].value = (float)(sum*1.0f / (float)count );
				tempLine[i].len++;
				++j;
			}
			else
			{
				if(tempLine[i].len > MIN_LINE_POINT) //找到一行
				{ 
					if(i<LINE_COUNT-1)
					{
						i++;
						//	j++;
					}else break; 
				}
				else
				{
					tempLine[i].startPos=0;//重新开始
					tempLine[i].endPos=0;
					tempLine[i].len=0;
					tempLine[i].value=0;
					sum=0;
					count=0;
					//	j++;
				}
			}
		}
	}

	//寻找边的特征行
	for( i=0;i<LINE_COUNT;i++)
	{ 
		if(tempLine[i].startPos==0||tempLine[i].len<MIN_LINE_POINT)break;
		// value=tempLine[i].value;

		for(j=1;j<LINE_COUNT;j++)
		{
			if(tempLine[j].startPos==0||tempLine[j].len<MIN_LINE_POINT)break;
			if(abs(abs(tempLine[i].value-tempLine[j].value)-9)<3) //找到垂直的边
			{
				if(tempLine[i].len<10 && tempLine[j].len <10)	continue;			//太短了不要
				if(l1->len<=tempLine[i].len && l2->len<=tempLine[j].len)
				{
					l1->startPos=tempLine[i].startPos;
					l1->endPos=tempLine[i].endPos;
					l1->value=tempLine[i].value;
					l1->len=tempLine[i].len;

					l2->startPos=tempLine[j].startPos;
					l2->endPos=tempLine[j].endPos;
					l2->len=tempLine[j].len;
					l2->value=tempLine[j].value;
				}
				isFind = TRUE;
			}
		}
	}

	if(isFind) return 1;

	l1->len=0;
	for(i=0;i<LINE_COUNT;i++)
	{
		if(tempLine[i].startPos==0) break;
		if(WITHIN(tempLine[i].value-27,-2,2))//找到一条边是竖直的
		{  
			if (tempLine[i].len < 10)continue;
			if(l1->len<tempLine[i].len)
			{
				l1->startPos=tempLine[i].startPos;
				l1->endPos=tempLine[i].endPos;
				l1->value=tempLine[i].value;
				l1->len=tempLine[i].len;
			}
			isFind = TRUE;
		}
	}
	if(isFind) return 2;
	return 0;
}
#endif

void GetFixedPoint(short DstX,short DstY,short* SrcX,short* SrcY)
{
	if (Lean != 0)
	{
		*SrcX = (short) ( g_matrix[0][0]*(DstX + xshift)+g_matrix[0][1]*(DstY + yshift)+g_matrix[0][2]+0.5 );
		*SrcY = (short) ( g_matrix[1][0]*(DstX + xshift)+g_matrix[1][1]*(DstY + yshift)+g_matrix[1][2]+0.5 );
	}else{
		*SrcX = (short) (DstX + xshift);
		*SrcY = (short) (DstY + yshift);
	}
}

short GetPointGreySingleMode(short w,short h)
{
	short _w;
	unsigned short * Pointer;
	if ( !(WITHIN(h,0,lineNum+1) && WITHIN(w,startP[h],endP[h]+1) ) )
	{
		return -1;
	}
	_w = w-startP[h];
	Pointer = RowIndex[h]+(_w>>1)+3;
	if ( (_w&1) == 0)
	{
		return (*Pointer)&0xff;
	}else return ( (*Pointer)>>8 )&0xff;
}

short GetPointGrey(short w,short h)
{
	unsigned short * Current_g;	
	unsigned short * pointer_g;

	short index_g;
	short index2_g;
	short begin_h_g;
	short begin_w_g;
	short end_h_g;
	short end_w_g;
	short rt_g;
	short value_g,big_g;

	short sumGrey = 0;
	short pointCount = 0;

	if (h - 1 <0 || h + 1 > lineNum)
	{
		return -1;
	}
	begin_h_g = h -1;
	end_h_g = h + 1 ;

	pointer_g = RowIndex[begin_h_g];

	rt_g = 0; 
	for(index_g = begin_h_g;index_g<=end_h_g;index_g++) 
	{ 
		if (w - 1 < startP[index_g] || w+1 > endP[index_g])
		{
			return -1;
		}
		begin_w_g = w - 1;
		end_w_g = w + 1;
		Current_g = pointer_g+3+(begin_w_g - startP[index_g])/2; 
		if((begin_w_g - startP[index_g])%2 == 0) 
		{ 
			big_g = 0; 
		} 
		else  
		{ 
			big_g = 8; 
		} 
		for(index2_g=begin_w_g;index2_g<=end_w_g;index2_g++) 
		{ 
			value_g = ((*Current_g)>>big_g)&0xff; 
			if(big_g == 0) 
			{ 
				big_g = 8; 
			} 
			else 
			{ 
				big_g = 0; 
				Current_g++; 
			} 

			sumGrey+= value_g;
			++pointCount;

		} 
		pointer_g = pointer_g + 4 + ( (endP[index_g] - startP[index_g])>>1 ) ; 
	} 
	if(pointCount>0)
	{
		rt_g= (short) ( ((sumGrey*1.0f)/(pointCount*1.0f) + 0.5f) );
	}
	return rt_g;
}

DLLAPI void C5409_Init(unsigned char* dat, int length)
{
	LoadFileBuffer((unsigned short*)dat);
/*
	AverageGrey = (long)( (AverageGrey*1.0) /(PicSquare*1.0)) ;
	histEqual();			//直方图均衡化
	getWH();				//得到3D与旋转基本数据
*/
	Recognize();
	//deno[1]=unMatch;
}

DLLAPI float C5409_GetW()
{
	return EW;
}

DLLAPI float C5409_GetH()
{
	return EH;
}

DLLAPI void C5409_GethistequalImg(unsigned char* img,int x, int y, int width, int height, int stride)
{
	int index = 0;
	for (j=y; j<height; j++)
	{
		for (i=x; i<width; i++)
		{
			short _w=0, _h=0, rt_g;
			GetFixedPoint(i,j,&_w,&_h);	
			rt_g = GetPointGrey(_w,_h);
			img[index++] = hist[rt_g];
		}
		index += stride-width;
	}
}

DLLAPI void C5409_GetImg(unsigned char* img,int x, int y, int width, int height, int stride)
{
	int index = 0;
	for (j=y; j<height; j++)
	{
		for (i=x; i<width; i++)
		{
			short _w=0, _h=0, rt_g;
			GetFixedPoint(i,j,&_w,&_h);	
			rt_g = GetPointGrey(_w,_h);
			img[index++] = rt_g;
		}
		index += stride-width;
	}
}

DLLAPI void C5409_GethistequalsingleImg(unsigned char* img,int x, int y, int width, int height, int stride)
{
	int index = 0;
	for (j=y; j<height; j++)
	{
		for (i=x; i<width; i++)
		{
			short _w=0, _h=0, rt_g;
			GetFixedPoint(i,j,&_w,&_h);	
			rt_g = GetPointGreySingleMode(_w,_h);
			img[index++] = hist[rt_g];
		}
		index += stride-width;
	}
}

DLLAPI void C5409_GetsingleImg(unsigned char* img,int x, int y, int width, int height, int stride)
{
	int index = 0;
	for (j=y; j<height; j++)
	{
		for (i=x; i<width; i++)
		{
			short _w=0, _h=0, rt_g;
			GetFixedPoint(i,j,&_w,&_h);	
			rt_g = GetPointGreySingleMode(_w,_h);
			img[index++] = rt_g;
		}
		index += stride-width;
	}
}
