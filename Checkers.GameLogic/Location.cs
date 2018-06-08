using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.GameLogic
{
    public struct Location
    {
        private int m_Row;
        private int m_Col;

        public Location(int i_Row, int i_Col)
        {
            m_Col = i_Col;
            m_Row = i_Row;
        }

        public int RowIndex
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public int ColIndex
        {
            get
            {
                return m_Col;
            }

            set
            {
                m_Col = value;
            }
        }

        public static bool operator ==(Location i_Loc1, Location i_Loc2)
        {
            return i_Loc1.ColIndex == i_Loc2.ColIndex && i_Loc1.RowIndex == i_Loc2.RowIndex;
        }

        public static bool operator !=(Location i_Loc1, Location i_Loc2)
        {
            return i_Loc1.ColIndex != i_Loc2.ColIndex || i_Loc1.RowIndex != i_Loc2.RowIndex;
        }

        public override bool Equals(object i_Loc)
        {
            return i_Loc is Location && this == (Location)i_Loc;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
