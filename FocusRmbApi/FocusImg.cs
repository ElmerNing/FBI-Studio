using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Focus
{
    public class FocusImg
    {
        private byte[] m_imgData = null;
        public byte[] ImgData
        {
            get { return m_imgData; }
            set
            {
                if (value==null || value.Length != Stride*Height)
                {
                    throw new Exception("对FocusImg中的m_imgData赋值出错");
                }
                else
                    m_imgData = value;
            }
        }

        private int m_height;
        public int Height
        {
            get { return m_height; }
        }
        private int m_width;
        public int Width
        {
            get { return m_width; }
        }
        private int m_stride;
        public int Stride
        {
            get { return m_stride; }
        }

        public FocusImg(int width, int height)
        {
            if (width%4!=0)
            {
                m_stride = (width+4)&0x7FFFFFFC;
            }
            else
                m_stride = width;

            m_width = width;
            m_height = height;
            m_imgData = new byte[m_height*m_stride];
        }

        public FocusImg(FocusImg img)
        {
            m_height = img.Height;
            m_width = img.Width;
            m_stride = img.m_stride;
            m_imgData = new byte[m_height*m_stride];
            Array.Copy(img.ImgData, m_imgData, Height*m_stride);
        }

        public short this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < m_width && y >= 0 && y < m_height)
                {
                    return m_imgData[y * m_stride + x];
                }
                else
                {
                    return 256;
                }
            }
            set
            {
                if (x >= 0 && x < m_width && y >= 0 && y < m_height)
                {
                    m_imgData[y * m_stride + x] = (byte)value;
                }
            }
        }

        public short GetPixel(int x, int y)
        {
            if (x>=0 && x<m_width && y>=0 && y<m_height)
            {
                return m_imgData[y*m_stride + x];
            }
            else
            {
                return 256;
                //throw new Exception("FocusImg访问越界");
            }
        }

        public void SetPixel(int x, int y, byte g)
        {
            if (x >= 0 && x < m_width && y >= 0 && y < m_height)
            {
                m_imgData[y * m_stride + x] = g;
            }
            else
            {
                return;
                //throw new Exception("FocusImg访问越界");
            }
        }

        public FocusImg GetImgByRect(Rectangle rect, RotateType rotateType = RotateType.FlipNone)
        {
            Rectangle srcRect = new Rectangle(0, 0, m_width, m_height);
            if (srcRect.Contains(rect))
            {
                FocusImg dstImg = new FocusImg(rect.Width, rect.Height);
                int index = 0;
                for (int j=rect.Top; j<rect.Bottom; j++)
                {
                    for (int i=rect.Left; i<rect.Right; i++)
                    {
                        dstImg.ImgData[index++] = m_imgData[j * m_stride + i];
                        //this[,]
                    }
                    index += dstImg.m_stride-dstImg.m_width;
                }
                return dstImg;
            }
            else
            {
                throw new Exception("FocusImg访问越界");
            }
        }

        public Bitmap ConvertTo8bppBitmap(int scale = 1)
        {
            return ConvertTo8bppBitmap(scale, scale);
        }
        
        public Bitmap ConvertTo8bppBitmap(int scaleX, int scaleY)
        {
            if (scaleX <= 1 && scaleY <= 1)
            {
	            Bitmap dstBitmap = new Bitmap(m_width, m_height, PixelFormat.Format8bppIndexed);
	            ColorPalette cp = dstBitmap.Palette;
	            for (int i=0; i<256; i++)
	                cp.Entries[i] = Color.FromArgb(i, i, i);
	            dstBitmap.Palette = cp;
	            BitmapData dstBitData = dstBitmap.LockBits(new Rectangle(0, 0, m_width, m_height), ImageLockMode.WriteOnly, dstBitmap.PixelFormat);
	            System.Runtime.InteropServices.Marshal.Copy(m_imgData, 0, dstBitData.Scan0, m_height*m_stride);
	            dstBitmap.UnlockBits(dstBitData);
	            return dstBitmap;
            }
            else
            {
                return this.ZoomIn(scaleX,scaleY).ConvertTo8bppBitmap();
            }
        }

        public Bitmap ConvertTo32bppBitmap(int scale = 1)
        {
            return ConvertTo32bppBitmap(scale, scale);
        }

        public Bitmap ConvertTo32bppBitmap(int scaleX , int scaleY)
        {
            if (scaleX <= 1&&scaleY<=1)
            {
                Bitmap dstBitmap = new Bitmap(m_width, m_height, PixelFormat.Format32bppArgb);

                BitmapData dstBitData = dstBitmap.LockBits(new Rectangle(0, 0, m_width, m_height), ImageLockMode.WriteOnly, dstBitmap.PixelFormat);
                int[] data = new int[m_width * m_height];
                for (int j = 0; j < m_height; j++)
                {
                    for (int i = 0; i < m_width; i++)
                    {
                        byte g = m_imgData[j * m_stride + i];//GetPixel(i, j);
                        data[j * m_width + i] = (g << 8) + (255 << 24) + (g << 16) + g;

                    }
                }

                System.Runtime.InteropServices.Marshal.Copy(data, 0, dstBitData.Scan0, m_height * m_width);
                dstBitmap.UnlockBits(dstBitData);
                return dstBitmap;
            }
            else
            {
                return this.ZoomIn(scaleX,scaleY).ConvertTo32bppBitmap();
            }
        }

        public FocusImg ZoomIn(int scaleX,int scaleY)
        {
            if (scaleX <= 1&&scaleY<=1)
            {
                return this;
            }
            int width = m_width * scaleX;
            int height = m_height * scaleY;
            FocusImg dstimg = new FocusImg(width, height);
            int stride = dstimg.m_stride;
            for (int j=0; j<m_height; j++ )
            {
                for (int i=0; i<m_width; i++ )
                {
                    byte gray = m_imgData[j*m_stride+i];
                    int index = j*scaleY*stride + i*scaleX;
                    for (int y=0; y<scaleY; y++ )
                    {
                        for (int x=0; x<scaleX; x++ )
                        {                        
                            dstimg.ImgData[index + y*stride +x] = gray;
                        }
                    }
                }
            }
            return dstimg;
        }

/*        public FocusImg Rotate180()
        {  
            FocusImg rotateImg = new FocusImg(m_width,m_height);
            int index1 = 0;
            int index2 = m_height * m_stride - 1;
            for (int y = 0; y < m_height; y++ )
            {
                for (int x = 0; x < m_width; x++ )
                {
                    rotateImg.ImgData[index2--] = m_imgData[index1++]; 
                }
                index1 += (m_stride - m_width);
                index2 -= (m_stride - m_width);
            }
            return rotateImg;
        }
 */
        public enum RotateType
        {
            FlipNone ,FlipX, FlipY, FlipXY,
        }

        public FocusImg Flip(RotateType type)
        {
            FocusImg rotateImg = new FocusImg(m_width, m_height);
            for (int y = 0; y < m_height; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    switch (type)
                    {
                    case RotateType.FlipX:
                        rotateImg[x, y] = this[m_width-x-1, y];
                    	break;
                    case RotateType.FlipY:
                        rotateImg[x, y] = this[x, m_height-y-1];                        
                    	break;
                    case RotateType.FlipXY:
                        rotateImg[x, y] = this[m_width-x-1, m_height-y-1];
                    	break;
                    default:
                        break;
                        
                    }
                }
            }
            return rotateImg;
        }

        public void AverageFilter()
        {
            FocusImg copy = new FocusImg(this);
            for (int j = 1; j < Height - 1; j++) 
            {
                for (int i = 1; i < Width - 1; i++)
                {
                        int g = 0;
                        g += copy.m_imgData[j * copy.m_stride + i];
                        g += copy.m_imgData[(j - 1) * copy.m_stride + i];
                        g += copy.m_imgData[(j + 1) * copy.m_stride + i];
                        g += copy.m_imgData[j * copy.m_stride + i + 1];
                        g += copy.m_imgData[j * copy.m_stride + i - 1];
                        g = (int)(g*0.2);
                        m_imgData[j * m_stride + i] = (byte)g;
                }
            }
        }

        public void Equalization(int[] hist)
        {
            for (int j = 0; j < Height; j++) 
            {
                for (int i = 0; i < Width; i++)
                {
                    byte g = m_imgData[j * m_stride + i];
                    g = (byte)hist[g];
                    m_imgData[j * m_stride + i] = g;
                }
            }
        }

        public void Equalization()
        {
            const int HIST_SIZE = 256;
            int[] histequ = new int[HIST_SIZE];
            for (int j = 0; j < m_height; j++)
            {
                for (int i = 0; i < m_width; i++)
                {
                    histequ[this[i, j]]++;
                }
            }

            float SIZEofPIXEL = (HIST_SIZE - 1) * 1.0f / (m_width * m_height);
            for (int i = 1; i < HIST_SIZE; ++i)
            {
                histequ[i] += histequ[i - 1];
                histequ[i - 1] = (short)(histequ[i - 1] * SIZEofPIXEL);
            }
            histequ[HIST_SIZE - 1] = (short)(histequ[HIST_SIZE - 1] * SIZEofPIXEL);
            Equalization(histequ);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="format">null的时候保存为自定义的Focus格式</param>
        /// <returns></returns>
        public bool Save(string savePath, ImageFormat format = null)
        {
            try
            {
                if (format != null)
                {
                    this.ConvertTo32bppBitmap().Save(savePath, format);
                    return true;
                }

                //format不为null保存为自定义格式
                string folderName = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
                BinaryWriter binWriter = new BinaryWriter(File.Open(savePath, FileMode.Create));
                binWriter.Write(Width);
                binWriter.Write(Height);
                binWriter.Write(ImgData);
                binWriter.Flush();
                binWriter.Close();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
