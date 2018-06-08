using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.GameLogic
{   
    public class Player
    {
        private CheckersENums.ePlayerType m_Type;
        private string m_Name;
        private int m_Score;
        private CheckersENums.ePlayerColor m_Color; 
 
        // Constructor
        internal Player(CheckersENums.ePlayerType i_Type, string i_Name, CheckersENums.ePlayerColor i_Color)
        {
            this.m_Type = i_Type;
            this.m_Name = i_Name;
            this.m_Score = 0;
            this.m_Color = i_Color;       
        }

        internal CheckersENums.ePlayerColor Color
        {
            get { return m_Color; }
        }

        public int Score { get => m_Score; set => m_Score = value; }

        public string Name { get => m_Name; set => m_Name = value; }

        public CheckersENums.ePlayerType Type { get => m_Type; set => m_Type = value; }
    }
}
