/*
 *	File :	FocusStructDef.h
 *  Created : [15:54 18/7/2011]
 *  Author : Nicky@ FocusBanker
 *  Email : Nicky918@qq.com
 *  All rights reserved .
 */

#ifndef _FOCUS_STRUCT_DEF_H_
#define _FOCUS_STRUCT_DEF_H_

#define HIST_SIZE        256
/// <summary>
/// �����ṹ��
/// </summary>
typedef struct _FocusPoint 
{
	short X;
	short Y;
}FocusPoint;

typedef struct _FocusLine
{
	int count;
	int differ;
	double rate;
	FocusPoint midpt;		//point at the middle of the line
}FocusLine;

/// <summary>
/// ��λͼ��߽�ʱ�Ľṹ��
/// </summary>
typedef struct _HDISNODE
{
	FocusPoint top;
	FocusPoint bottom;
	int dis;
}HDISNODE;

typedef struct _VDISNODE
{
	FocusPoint left;
	FocusPoint right;
	int dis;
}VDISNODE;

typedef struct _ImageInfo {
	int id;
	int isFake;
	int is99;
	int isVer4;
	int face;
	int ver;
	int width;
	int height;
	int unMatch;
}ImageInfo;

typedef enum _ImageLine{
	Focus200,
	Focus400,
	Focus133
}ImageLine;

/*!	ͼ�񻺳���ָ���������� */
typedef struct  _IMG_BuffObj
{
	short 	sBuffNum;	/*!< Buffer Number */
	int iBuffLength[3];		/*!< Each Buffer Length */
	unsigned char*  pBuff[3];   /*!< Each Buffer */
}IMG_BuffObj,*pBuffObj;

typedef struct _FocusImage
{
	int srcHeight;
	int srcWidth;
	int Width;
	int Height;
	int Lean;
	int iBuffLength;
	unsigned char* pBuff;
	FocusPoint TopLeft;
	FocusPoint TopRight;
	FocusPoint BottomLeft;
	FocusPoint BottomRight;
	FocusPoint Center;
	int FixedCenterX;
	int FixedCenterY;
	ImageLine il;	
	int Hist[HIST_SIZE];
}FocusImage,*pFocusImage;

#endif


