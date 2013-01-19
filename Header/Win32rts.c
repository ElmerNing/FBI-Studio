#include <math.h>

int _min2(int a, int b)
{
	short sa1,sa2,sb1,sb2;
	sa1 = a>>16;
	sb1 = b>>16;
	sa2 = a&0x0000FFFF;
	sb2 = b&0x0000FFFF;
	sa1 = sa1<sb1 ? sa1 : sb1;
	sa2 = sa2<sb2 ? sa2 : sb2; 
	return (((unsigned short)sa1)<<16) + (unsigned short)sa2;
}

int _max2(int a, int b)
{
	short sa1,sa2,sb1,sb2;
	sa1 = a>>16;
	sb1 = b>>16;
	sa2 = a&0x0000FFFF;
	sb2 = b&0x0000FFFF;
	sa1 = sa1>sb1 ? sa1 : sb1;
	sa2 = sa2>sb2 ? sa2 : sb2; 
	return (((unsigned short)sa1)<<16) + (unsigned short)sa2;
}

int _abs(int x)
{
	return abs(x);
}

int _subabs4(int src1, int src2)
{
	int sub = src1 - src2;
	sub = abs(sub);
	return sub;
}

unsigned int _bitr(unsigned int n)
{
	n =((n >> 1)& 0x55555555)|((n << 1)& 0xaaaaaaaa);  
	n =((n >> 2)& 0x33333333)|((n << 2)& 0xcccccccc);  
	n =((n >> 4)& 0x0f0f0f0f)|((n << 4)& 0xf0f0f0f0);  
	n =((n >> 8)& 0x00ff00ff)|((n << 8)& 0xff00ff00);  
	n =((n >> 16)& 0x0000ffff)|((n << 16)& 0xffff0000);   
	return n;  
}

int _add2( int src1, int src2 )
{
	short sa1,sa2,sb1,sb2;
	sa1 = src1>>16;
	sb1 = src2>>16;
	sa2 = src1&0x0000FFFF;
	sb2 = src2&0x0000FFFF;
	sa1 += sb1;
	sa2 += sb2;
	return (((unsigned short)sa1)<<16) + (unsigned short)sa2;
}

int _sub2( int src1, int src2 )
{
	short sa1,sa2,sb1,sb2;
	sa1 = src1>>16;
	sb1 = src2>>16;
	sa2 = src1&0x0000FFFF;
	sb2 = src2&0x0000FFFF;
	sa1 -= sb1;
	sa2 -= sb2;
	return (((unsigned short)sa1)<<16) + (unsigned short)sa2;
}

int _dotp2( int src1, int src2 )
{
	short sa1,sa2,sb1,sb2;
	sa1 = src1>>16;
	sb1 = src2>>16;
	sa2 = src1&0x0000FFFF;
	sb2 = src2&0x0000FFFF;
	return sa1*sb1 + sa2*sb2;
}

unsigned int _lmbd(unsigned int src1, unsigned int src2)
{
	int tail = src1 & 0x00000001;
	int i;
	for (i=0; i<32;)
	{
		if (tail == ((src2>>(31-i)) & 0x00000001))
		{
			break;
		}
		i++;
	}
	return i;
}

unsigned int _mpyu(unsigned int src1, unsigned int src2)
{
	src1 &= 0xFFFF;
	src2 &= 0xFFFF;
	return src1*src2;
}


void DSP_radix2(int n, short* xy, short* w)
{
	short n1,n2,ie,ia,i,j,k,l;                                      
	short xt,yt,c,s;                                                

	n2 = n;                                                         
	ie = 1;                                                         
	for (k=n; k > 1; k = (k >> 1) )                                 
	{                                                               
		n1 = n2;                                                    
		n2 = n2>>1;                                                 
		ia = 0;                                                     
		for (j=0; j < n2; j++)                                      
		{                                                           
			c = w[2*ia];                                            
			s = w[2*ia+1];                                          
			ia = ia + ie;                                           
			for (i=j; i < n; i += n1)                               
			{                                                       
				l = i + n2;                                         
				xt		  = xy[2*l] - xy[2*i];                        
				xy[2*i]	  = xy[2*i] + xy[2*l];                        
				yt        = xy[2*l+1] - xy[2*i+1];                    
				xy[2*i+1] = xy[2*i+1] + xy[2*l+1];                  
				xy[2*l]   = (c*xt + s*yt)>>15;                      
				xy[2*l+1] = (c*yt - s*xt)>>15;                      
			}                                                       
		}                                                           
		ie = ie<<1;                                                 
	}                                                                                                                                 
}

typedef enum _CACHEMODE{CACHE0KB,CACHE32KB,CACHE64KB,CACHE128KB,CACHE256KB}CACHEMODE; 
void KERNEl_enalbleCache(CACHEMODE cacheMode)
{

}





