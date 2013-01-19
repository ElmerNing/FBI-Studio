#define CORNERRECOVERY
#define MISMATCHPOINT 	9
#define CASH_TYPE       9
#define HIST_SIZE		256
#define LineCountFor3D  5
#define TRUE            1
#define FALSE           0
#define WH_RATE         0.92		//为了求SLOPE值的RATE
#define SLOPE_RATE      0.96		//为了求WH值的RATE
#define WITHIN(x,a,b)   ( ( (a)<=(x) ) && ( (x)<(b) ) )
#define MAX_HEIGHT		150
#define MAX_WIDTH		220
#ifdef CORNERRECOVERY
//*************************链码数组********************************//
unsigned short leftCode[27]=
{
	/*	0		1		2		3		4		5		6		7		8		*/ 
	27,		25,		24,		23,		22,		21,		21,		20,		20,

	/*	9		10		11		12		13		14		15		16		17		*/
	20,		20,		20,		20,		19,		19,		19,		19,		19,		

	/*	18		19		20		21		22		23		24		25		26		*/
	19,		19,		19,		19,		19,		19,		19,		19,		19,
};

unsigned short  rightCode[27]=
{
	/*	0		1		2		3		4		5		6		7		8		*/ 
	27,		29,		30,		31,		32,		33,		33,		34,		34,

	/*	9		10		11		12		13		14		15		16		17		*/
	34,		34,		34,		34,		35,		35,		35,		35,		35,		

	/*	18		35		34		33		22		23		24		25		26		*/
	35,		35,		35,		35,		35,		35,		35,		35,		35,
};
short leftLinkCode[MAX_HEIGHT];
short rightLinkCode[MAX_HEIGHT];
#define LINE_COUNT      5
#define PosOffset       4
typedef struct _LINE
{
	short startPos;
	short endPos;
	short value;
	unsigned short len;
}ALINE;
typedef struct _FLAG
{
	short TOPLEFT;
	short TOPRIGHT;
	short BOTTOMLEFT;
	short BOTTOMRIGHT;
}FLAG;
FLAG flag;
#endif

void LoadFileBuffer(unsigned short * buffer);
void Set();
short Recognize(void);
void histEqual(void);
float Sqrt(float a);
void getWH(void );
float GetSlope1(short* point, short iStart,short iEnd);
short ImageCheck(const unsigned short Point[],const unsigned short Threshold[],
	const unsigned short PointSize,short isWhite,short Count,short MaxReturn);
short GetLinkCode3( short del );
short AnalyzeLinkCode( short link[],short linkCount,ALINE * l1,ALINE * l2 );
void GetFixedPoint(short DstX,short DstY,short* SrcX,short* SrcY);
short GetPointGreySingleMode(short w,short h);
short GetPointGrey(short w,short h);
