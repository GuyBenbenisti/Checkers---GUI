using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.GameLogic
{    
    public class Piece
    {
        private CheckersENums.eShape m_Shape;
        private CheckersENums.ePlayerColor m_Color;

        public Piece(CheckersENums.eShape i_Shape, CheckersENums.ePlayerColor i_Color)
        {
            m_Shape = i_Shape;
            m_Color = i_Color;
        }

        public CheckersENums.ePlayerColor Color
        {
            get { return m_Color; }
        }

        public CheckersENums.eShape Shape
        {
            get { return m_Shape; }

            set { m_Shape = value; }
        }

        public static bool operator ==(Piece i_Piece1, Piece i_Piece2)
        {
            bool output = false;
            if (object.ReferenceEquals(i_Piece1, i_Piece2))
            {
                output = true;
            }
            else if ((object)i_Piece1 == null || (object)i_Piece2 == null)
            {
                output = false;
            }
            else if (i_Piece1.Color == i_Piece2.Color)
            {
                output = true;
            }

            return output;
        }

        public static bool operator !=(Piece i_Piece1, Piece i_Piece2) => !(i_Piece1 == i_Piece2);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object i_Piece)
        {
            return this == (Piece)i_Piece;
        }
    }
}