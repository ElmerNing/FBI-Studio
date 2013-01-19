using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBI_Studio
{
    /// <summary>
    /// edit by nicky @ Focus
    /// 2011 04 14
    /// 点定义类
    /// </summary>
    public class FocusPoint :  EqualityComparer<FocusPoint>, IEquatable<FocusPoint>
    {

        public FocusPoint()
        {
            this.x = this.y = this.g = 0;
        }

        public FocusPoint(short x,short y,short g)
        {
            this.x = x;
            this.y = y;
            this.g = g;
        }

        /// <summary>
        /// 点坐标X值
        /// </summary>
        public short x;

        /// <summary>
        /// 点坐标Y值
        /// </summary>
        public short y;

        /// <summary>
        /// 点灰度
        /// </summary>
        public short g;

        public  bool Equals(FocusPoint a)
        {
            return (a.x == this.x && a.y == this.y);
        }

        public override bool Equals(FocusPoint a, FocusPoint b)
        {
            return (a.x == b.x && a.y == b.y);
        }

        public override int GetHashCode(FocusPoint f)
        {
            int hCode = (int)x + (int)short.MaxValue + (int)y;
            return hCode;
        }

        public override string ToString()
        {
            return string.Format("X:{0},Y:{1},G:{2}", x, y, g);
        }
    }
}
