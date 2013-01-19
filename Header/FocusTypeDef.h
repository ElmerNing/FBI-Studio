/*
 *	File :	FocusTypeDef.h
 *  Created : [11:53 18/7/2011]
 *  Author : Nicky@ FocusBanker
 *  Email : Nicky918@qq.com
 *  Purpose : type redefine in DM642 plat
 *  All rights reserved .
 */
#ifndef _FOCUS_TYPE_DEF_H_
#define _FOCUS_TYPE_DEF_H_

#define WITHIN(x,a,b)   ((x)>=(a) && (x)<(b))
#define WITHIN2(x,a,b)   ((x)>=(a) && (x)<=(b))

#ifndef max
#define  max(a,b) (a)>=(b)?(a):(b)
#endif

#ifndef min
#define  min(a,b) (a)<=(b)?(a):(b)
#endif

#ifndef NULL
#define  NULL                         0
#endif

#ifdef CHIP_DM642
typedef Uint8                         UINT8;
typedef Uint16                        UINT16;
typedef Uint32                        UINT32;
typedef Uint8  *                      PUINT8;
typedef Uint32 *                      PUINT32;
#else
typedef unsigned char                         UINT8;
typedef unsigned short                        UINT16;
typedef unsigned int                        UINT32;
typedef UINT8  *                      PUINT8;
typedef UINT32 *                      PUINT32;
#endif

#define RAM32(x) (*(Uint32*)(x))

#endif

