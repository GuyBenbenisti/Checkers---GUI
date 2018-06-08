using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.GameLogic
{
    public class Move
    {
        private Location m_OriginCoordinates;
        private Location m_DestCoordinates;
        private Board.BoardSquare m_Origin;
        private Board.BoardSquare m_Dest;
        private bool m_IsEatMove;

        internal Move(Board.BoardSquare i_Origin, Board.BoardSquare i_Dest, bool i_IsEatMove)
        {
            m_DestCoordinates = i_Dest.Coordinates;
            m_OriginCoordinates = i_Origin.Coordinates;
            m_IsEatMove = i_IsEatMove;
            m_Origin = i_Origin;
            m_Dest = i_Dest;
        }

        public Move(Location i_Origin, Location i_Dest)
        {
            m_DestCoordinates = i_Dest;
            m_OriginCoordinates = i_Origin;            
        }

        internal Board.BoardSquare Origin
        {
            get { return m_Origin; }

            set { m_Origin = value; }
        }

        internal Board.BoardSquare Destination
        {
            get { return m_Dest; }

            set { m_Dest = value; }
        }

        public bool IsEatMove
        {
            get { return m_IsEatMove; }
            set { m_IsEatMove = value; }
        }

        public Location OriginCoordinates { get => m_OriginCoordinates; set => m_OriginCoordinates = value; }

        public Location DestCoordinates { get => m_DestCoordinates; set => m_DestCoordinates = value; }
    }
}