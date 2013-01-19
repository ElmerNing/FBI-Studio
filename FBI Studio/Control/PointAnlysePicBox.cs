using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Focus;


namespace FBI_Studio
{
    /// <summary>
    /// edit by ny @ Focus
    /// 2011 09 28
    /// 点分析图片控件</summary>
    /// <remarks>
    /// m_zoominRect 未作安全性处理  </remarks>
    class PointAnlysePicBox : PictureBox
    {
        #region 引发事件

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <returns></returns>
        public PointAnlysePicBox()
        {
            SynReFresh += new SynFreshHandle(this.Refresh);
            SynSwitchZoomMode += new SynSwitchZoomModeHandle(this.SwitchZoomMode);
        }

        /// <summary>
        /// 引发Paint事件
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            //绘制特征点
            foreach (FocusPoint point in PointTrain.PointSet.BlackFpSet)
            {
                if (m_deletePoint != null && point.Equals(m_deletePoint))
                {
                    DrawFeatruePoint(pe.Graphics, point, true, true);
                }
                else
                {
                    DrawFeatruePoint(pe.Graphics, point, true, false);
                }
            }
            foreach (FocusPoint point in PointTrain.PointSet.WhiteFpSet)
            {
                if (m_deletePoint != null && point.Equals(m_deletePoint))
                {
                    DrawFeatruePoint(pe.Graphics, point, true, true);
                }
                else
                {
                    DrawFeatruePoint(pe.Graphics, point, false, false);
                }
            }

            //绘制选择框
            if (!m_isZoomin)
            {
                Rectangle rect = new Rectangle(m_selectRect.X * m_origScaleX, m_selectRect.Y * m_origScaleY, m_selectRect.Width * m_origScaleX, m_selectRect.Height * m_origScaleY);
                pe.Graphics.DrawRectangle(new Pen(Color.Red), rect);
            }
        }

        /// <summary>
        /// 描绘特征点
        /// </summary>
        /// <param name="graphic">GDI+ 绘图图面</param>
        /// <param name="pointset">特征点集</param>
        /// <param name="isBlack">是否为黑点</param>
        /// <param name="isDeleting">是否为删除点</param>
        /// <returns></returns>
        private void DrawFeatruePoint(Graphics graphic, FocusPoint pointset, bool isBlack, bool isDeleting)
        {
            Point showPoint = GetShowPoint(pointset);
            FocusPoint origPoint = GetOrigPoint(showPoint.X, showPoint.Y);
            if (isDeleting)
            {
                graphic.DrawLine(new Pen(Color.GreenYellow), showPoint.X - 2, showPoint.Y - 2, showPoint.X + 2, showPoint.Y + 2);
                graphic.DrawLine(new Pen(Color.GreenYellow), showPoint.X - 2, showPoint.Y + 2, showPoint.X + 2, showPoint.Y - 2);
                return;
            }
            Color c = new Color();
            if (isBlack)
            {
                if (origPoint.g < pointset.g)
                    c = Color.Blue;
                else
                    c = Color.Purple;
            }
            else
            {
                if (origPoint.g > pointset.g)
                    c = Color.Red;
                else
                    c = Color.Purple;
            }
            graphic.DrawEllipse(new Pen(c), showPoint.X - 1, showPoint.Y - 1, 3, 3);
        }

        /// <summary>
        /// 引发MouseMove事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override void OnMouseMove(MouseEventArgs e)
        { 
            FocusPoint point = GetOrigPoint(e.X, e.Y);
            //设置待删除点
            if (PointTrain.PointSet.BlackFpSet.Find(point) != null || PointTrain.PointSet.WhiteFpSet.Find(point) != null)
            {
                m_deletePoint = point;
            }
            else if(m_deletePoint != null)
            {
                m_deletePoint = null;               
            }

            //设置边框
            if (m_mouseMiddleDown)
            {
                m_selectRect.Width = point.x - m_selectRect.Location.X;
                m_selectRect.Height = point.y - m_selectRect.Location.Y;
            }
            SynReFresh();
            base.OnMouseMove(e);
        }

        /// <summary>
        /// 引发MouseClick事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            FocusPoint point = GetOrigPoint(e.X, e.Y);

            if (null == PointTrain.PointSet.BlackFpSet.Find(point) && null == PointTrain.PointSet.WhiteFpSet.Find(point))
            {
                if (e.Button == MouseButtons.Left)
                {
                    point.g = 255;
                    PointTrain.PointSet.BlackFpSet.AddLast(point);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    point.g = 0;
                    PointTrain.PointSet.WhiteFpSet.AddLast(point);
                }
            }
            else
            {
                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                {
                    LinkedListNode<FocusPoint> ptnode = PointTrain.PointSet.WhiteFpSet.Find(point);
                    if (ptnode != null)
                    {
                        PointTrain.PointSet.WhiteFpSet.Remove(ptnode);
                    }
                    ptnode = PointTrain.PointSet.BlackFpSet.Find(point);
                    if (ptnode != null)
                    {
                        PointTrain.PointSet.BlackFpSet.Remove(ptnode);
                    }
                }
            }
            SynReFresh();
        }

        /// <summary>
        /// 引发MouseDown事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            FocusPoint point = GetOrigPoint(e.X, e.Y);
            if (e.Button != MouseButtons.Middle)
                return;

            if (m_isZoomin)
            {
                m_isZoomin = false;
                SynSwitchZoomMode();
                return;
            }
            else if (m_selectRect.Contains(point.x, point.y))
            {
                m_isZoomin = true;
                SynSwitchZoomMode();
                return;
            }

            m_selectRect = new Rectangle(0, 0, 0, 0);          
            m_selectRect.Location = new Point(point.x, point.y);
            m_mouseMiddleDown = true;
        }

        /// <summary>
        /// 引发MouseUp事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Middle || m_isZoomin == true)
                return;

            m_mouseMiddleDown = false;
        }

        #endregion

        //同步刷新的委托和事件
        private delegate void SynFreshHandle();
        private static event SynFreshHandle SynReFresh = null;

        //同步切换放大模式的委托和事件
        private delegate void SynSwitchZoomModeHandle();
        private static event SynSwitchZoomModeHandle SynSwitchZoomMode = null;

        //待删除点
        private static FocusPoint m_deletePoint = null;
        //是否为放大模式
        private static bool m_isZoomin = false;
        //当m_isZoomin为true时候, 指定放大区域
        public static Rectangle m_selectRect = new Rectangle(0,0,0,0);
        //放大模式下的放大倍数, 默认3
        private int m_zoominScale = 3;
        //原图模式下的放大倍数, 默认1, 目前不可改动
        private int m_origScaleX = 1;
        private int m_origScaleY = 1;
        //原图
        private FocusImg m_focusImg = null;
        //当前鼠标是否按下
        private bool m_mouseMiddleDown = false;

        /// <summary>
        /// 显示一幅图片
        /// </summary>
        /// <param name="img">待显示图片</param>
        /// <param name="scale">放大倍数, 默认1, 目前仅支持默认形式</param>
        /// <returns></returns>
        public void ShowImg(FocusImg img, int scale = 1)
        {
            m_focusImg = img;
            m_origScaleX = 1;
            m_origScaleY = 1;
            m_zoominScale = 3;
            if (scale == 1)
            {
                if (m_focusImg.Width < 200)
                {
                    m_origScaleX = 3;
                    m_origScaleY = 3;
                    m_zoominScale = 2;
                }
                else if (m_focusImg.Height<100)
                {
                    m_origScaleX = 1;
                    m_origScaleY = 2;
                    m_zoominScale = 3;
                }

            }
            SwitchZoomMode();
        }

        /// <summary>
        /// 根据一个点的显示的坐标,获取此点在原图的坐标和灰度
        /// </summary>
        /// <param name="x">显示的点的坐标</param>
        /// <param name="y">显示的点的坐标</param>
        /// <returns>原图的坐标和灰度</returns>
        public FocusPoint GetOrigPoint(int x, int y)
        {
            FocusPoint point = new FocusPoint();
            if (m_isZoomin)
            {
                x /= m_zoominScale*m_origScaleX;
                y /= m_zoominScale*m_origScaleY;
                x += m_selectRect.Location.X;
                y += m_selectRect.Location.Y;
            }   
            else
            {
                x /= m_origScaleX;
                y /= m_origScaleY;
            }
            point.x = (short)(x);
            point.y = (short)(y);
            if (x < 0 || x >= m_focusImg.Width || y < 0 || y >= m_focusImg.Height)
                point.g = 0;
            else
                point.g = m_focusImg.GetPixel(x, y);
            return point; 
        }    

        /// <summary>
        /// 根据一个点在原图的坐标,获取此点显示的坐标
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point GetShowPoint(FocusPoint point)
        {
            Point pt = new Point();
            if (m_isZoomin)
            {
                pt.X = (point.x - m_selectRect.Location.X) * m_zoominScale * m_origScaleX + m_zoominScale * m_origScaleX / 2;
                pt.Y = (point.y - m_selectRect.Location.Y) * m_zoominScale * m_origScaleY + m_zoominScale * m_origScaleY / 2;
            }
            else
            {
                pt.X = point.x * m_origScaleX + m_origScaleX / 2;
                pt.Y = point.y * m_origScaleY + m_origScaleY / 2;
            }
            return pt;
        }

        /// <summary>
        /// 所有PointAnlysePicBox同步刷新
        /// </summary>
        /// <returns></returns>
        static public void SynFresh()
        {
            if (SynReFresh != null)
            {
                SynReFresh();
            }
        }

        /// <summary>
        /// 切换放大和原图模式
        /// </summary>
        /// <returns></returns>
        private void SwitchZoomMode()
        {
            if (m_isZoomin)
                Image = m_focusImg.GetImgByRect(m_selectRect).ConvertTo32bppBitmap(m_zoominScale*m_origScaleX, m_zoominScale*m_origScaleY);
            else
                Image = m_focusImg.ConvertTo32bppBitmap(m_origScaleX, m_origScaleY);
            SynReFresh();
        }
    }
}
