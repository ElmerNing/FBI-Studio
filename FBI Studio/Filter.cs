using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    public class Filter
    {
        public bool m_true;
        //public bool m_false;
        //public bool m_edtion2005;
        //public bool m_edtion1999;
        public bool[] m_feater = new bool[12];
        public Filter()
        {
            m_true = true;
            for (int i=0; i<12; i++ )
            {
                m_feater[i] = true;
            }
        }

        public bool Apply(string filepath)
        {
            string fileName = filepath.Substring(filepath.LastIndexOf('\\') + 1);
            int start = fileName.IndexOf("[");
            int end = fileName.IndexOf("]");
            if (end - start != 8)
                return true;
            string featrueStr = fileName.Substring(start+1, 7);
            int feater = Int32.Parse(featrueStr,System.Globalization.NumberStyles.HexNumber);
            feater &= 0x000ffff;
            if (((feater>>12)&0x1) == 1 && m_true == true)
            {
                return true;
            }
            for (int i=0; i<12; i++ )
            {
                if (((feater>>(11-i)) & 0x1) == 0 && m_feater[i] == true)
                    return true;
            }        
            return false;
        }
    }
}
