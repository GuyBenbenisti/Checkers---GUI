using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.GameLogic
{
    public class CheckersENums
    {
        public enum eShape
        {
            Blank = ' ',
            White = 'X',
            Black = 'O',
            WhiteKing = 'K',
            BlackKing = 'U',
        }

        public enum ePlayerType
        {
            User,
            Bot
        }

        public enum ePlayerColor
        {
            White = 'X', // (X)
            Black = 'O', // (O)
            Blank = ' '// (' ')
        }

        public enum eGameStatus
        {
            Playing,
            Draw,
            Win,
        }
    }
}
