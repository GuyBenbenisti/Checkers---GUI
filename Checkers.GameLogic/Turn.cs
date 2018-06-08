using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Checkers.GameLogic.CheckersENums;

namespace Checkers.GameLogic
{
    public class Turn
    {
        public event EventHandler RoyalMove;

        public event EventHandler UpdateBoard;

        private bool m_JustAte;
        private bool m_RoyalMove;
        private bool m_SwitchTurn;
        private Move m_CurrentMove;
        private List<Move> m_LegalMoves;
        private Board m_Board;
        private Player m_CurPlayer;

        internal static bool CheckBoundaries(int i_ColIndex, int i_RowIndex, int i_BoardSize)
        {
            return (i_ColIndex < i_BoardSize && i_ColIndex >= 0) && (i_RowIndex < i_BoardSize && i_RowIndex >= 0);
        }

        internal Turn(Board i_Board, Player i_CurTurn)
        {
            m_JustAte = false;
            m_CurPlayer = i_CurTurn;
            m_Board = i_Board;
            m_LegalMoves = legalMoves(i_Board, i_CurTurn);
        }

        internal bool JustAte
        {
            get { return m_JustAte; }

            set { m_JustAte = value; }
        }

        internal bool SwitchTurn
        {
            get { return m_SwitchTurn; }

            set { m_SwitchTurn = value; }
        }

        internal bool isValidMove(Move i_UserInput)
        {
            bool output = false;
            foreach (Move legalMove in m_LegalMoves)
            {
                if (i_UserInput.OriginCoordinates == legalMove.OriginCoordinates && i_UserInput.DestCoordinates == legalMove.DestCoordinates)
                {
                    m_CurrentMove = legalMove;
                    output = true;
                    break;
                }
            }

            return output;
        }

        internal void PlayBotTurn()
        {
            List<Move> botMoves = legalMoves(m_Board, m_CurPlayer);
            Random random = new Random();
            int randomBotMove = random.Next(0, botMoves.Count);
            m_CurrentMove = botMoves.ElementAt(randomBotMove);
            PlayTurn();
            if (!m_SwitchTurn)
            {
                PlayBotTurn();
            }
        }

        private List<Move> updateEdibleMoves(List<Move> i_LegalMoves)
        {
            List<Move> onlyEatMoves = new List<Move>();
            bool foundEatingMove = false;
            foreach (Move move in i_LegalMoves)
            {
                if (move.IsEatMove)
                {
                    foundEatingMove = true;
                    onlyEatMoves.Add(move);
                    continue;
                }
            }

            List<Move> output;
            if (m_JustAte)
            {
                output = onlyEatMoves;
            }
            else
            {
                output = foundEatingMove ? onlyEatMoves : i_LegalMoves;
            }

            return output;
        }

        internal Board Board { get => m_Board; }

        public Player CurrentPlayer { get => m_CurPlayer; set => m_CurPlayer = value; }

        internal void PlayTurn()
        {
            int originRowIndex = m_CurrentMove.Origin.Coordinates.RowIndex;
            int originColIndex = m_CurrentMove.Origin.Coordinates.ColIndex;
            int destRowIndex = m_CurrentMove.Destination.Coordinates.RowIndex;
            int destColIndex = m_CurrentMove.Destination.Coordinates.ColIndex;
            Board.BoardSquare origin = m_Board.getBoardSquare(originRowIndex, originColIndex);
            Board.BoardSquare destination = m_Board.getBoardSquare(destColIndex, destRowIndex);
            m_Board.setBoardSquare(originRowIndex, originColIndex, eShape.Blank, ePlayerColor.Blank);

            if (m_CurPlayer.Color == ePlayerColor.White && destRowIndex == 0)
            {
                m_Board.setBoardSquare(destRowIndex, destColIndex, eShape.WhiteKing, m_CurPlayer.Color);
                m_RoyalMove = true;
            }
            else if (m_CurPlayer.Color == ePlayerColor.Black && destRowIndex == m_Board.Size - 1)
            {
                m_Board.setBoardSquare(destRowIndex, destColIndex, eShape.BlackKing, m_CurPlayer.Color);
                m_RoyalMove = true;
            }
            else
            {
                m_Board.setBoardSquare(destRowIndex, destColIndex, origin.GetShape, m_CurPlayer.Color);
            }

            if (m_CurrentMove.IsEatMove)
            {
                int toEatRowIndex = (originRowIndex > destRowIndex) ? (originRowIndex - 1) : (originRowIndex + 1);
                int toEatColIndex = (originColIndex > destColIndex) ? (originColIndex - 1) : (originColIndex + 1);
                m_Board.setBoardSquare(toEatRowIndex, toEatColIndex, eShape.Blank, ePlayerColor.Blank);
                m_JustAte = true;
                CheckForExtraEat(m_Board.getBoardSquare(destRowIndex, destColIndex));
            }
            else
            {
                this.SwitchTurn = true;
            }

            UpdateBoard.Invoke(m_CurrentMove, EventArgs.Empty);

            if (m_RoyalMove)
            {
                RoyalMove.Invoke(m_CurrentMove, EventArgs.Empty);
                m_RoyalMove = false;
            }
        }

        internal bool IsWithdrawLegal()
        {
            int whitePlayerScore = 0;
            int blackPlayerScore = 0;
            ePlayerColor currentPlayerColor = m_CurPlayer.Color;
            bool legalWithdraw = false;
            foreach (Board.BoardSquare square in m_Board.Matrix)
            {
                if (square.GetShape != eShape.Blank)
                {
                    switch (square.GetShape)
                    {
                        case eShape.Black:
                            blackPlayerScore++;
                            break;
                        case eShape.BlackKing:
                            blackPlayerScore += 4;
                            break;
                        case eShape.White:
                            whitePlayerScore++;
                            break;
                        case eShape.WhiteKing:
                            whitePlayerScore += 4;
                            break;
                    }
                }
            }

            switch (currentPlayerColor)
            {
                case ePlayerColor.White:
                    legalWithdraw = whitePlayerScore <= blackPlayerScore;
                    break;
                case ePlayerColor.Black:
                    legalWithdraw = whitePlayerScore >= blackPlayerScore;
                    break;
            }

            return legalWithdraw;
        }

        internal bool CheckForNoMoreMoves(Player i_CurPlayer)
        {
            List<Move> moves = legalMoves(m_Board, i_CurPlayer);
            return moves.Count == 0;
        }

        private void CheckForExtraEat(Board.BoardSquare i_Origin)
        {
            List<Move> legalMoves = squareLegalMove(i_Origin);
            legalMoves = updateEdibleMoves(legalMoves);
            if (legalMoves.Count > 0)
            {
                m_SwitchTurn = false;
                m_LegalMoves = legalMoves;
            }
            else
            {
                m_SwitchTurn = true;
                m_JustAte = false;
            }
        }

        public Move getBoardSquaresFromMove(Move i_UserInput)
        {
            Board.BoardSquare originSquare = m_Board.getBoardSquare(i_UserInput.OriginCoordinates.ColIndex, i_UserInput.OriginCoordinates.RowIndex);
            Board.BoardSquare destSquare = m_Board.getBoardSquare(i_UserInput.DestCoordinates.ColIndex, i_UserInput.DestCoordinates.RowIndex);
            return new Move(originSquare, destSquare, false);
        }

        private List<Move> legalMoves(Board i_CurBoard, Player i_CurTurn)
        {
            List<Move> legalMoves = new List<Move>();
            int size = i_CurBoard.Size;
            foreach (Board.BoardSquare square in i_CurBoard.Matrix)
            {
                if (square.BoardSquareChecker != null)
                {
                    if (square.BoardSquareChecker.Color == i_CurTurn.Color)
                    {
                        legalMoves.AddRange(squareLegalMove(square));
                    }
                }
            }

            return updateEdibleMoves(legalMoves);
        }

        // TODO: $G$ DSN-003 (-10) the code should be divided to methods
        // Divide to methods by directions or something more general
        private List<Move> squareLegalMove(Board.BoardSquare i_BoardSquare)
        {
            List<Move> output = new List<Move>();
            Location currentLocation = i_BoardSquare.Coordinates;
            int colIndex = currentLocation.ColIndex;
            int rowIndex = currentLocation.RowIndex;
            switch (i_BoardSquare.GetShape)
            {
                case eShape.White:
                    // Checks available right upwards move
                    output.AddRange(rightUpward(colIndex, rowIndex, eShape.White));

                    // Checks available left upwards move
                    output.AddRange(leftUpward(colIndex, rowIndex, eShape.White));

                    break;
                case eShape.WhiteKing:
                    // Checks available right upwards move             
                    output.AddRange(rightUpward(colIndex, rowIndex, eShape.WhiteKing));

                    // Checks available left upwards move
                    output.AddRange(leftUpward(colIndex, rowIndex, eShape.WhiteKing));

                    // Checks available right downwards move        
                    output.AddRange(rightDownward(colIndex, rowIndex, eShape.WhiteKing));

                    // Checks available left downwards move              
                    output.AddRange(leftDownward(colIndex, rowIndex, eShape.WhiteKing));
                    break;
                case eShape.Black:
                    // Checks available right downwards move                 
                    output.AddRange(rightDownward(colIndex, rowIndex, eShape.Black));

                    // Checks available left downwards move                    
                    output.AddRange(leftDownward(colIndex, rowIndex, eShape.Black));
                    break;
                case eShape.BlackKing:
                    // Checks available right upwards move                    
                    output.AddRange(rightUpward(colIndex, rowIndex, eShape.BlackKing));

                    // Checks available left upwards move                   
                    output.AddRange(leftUpward(colIndex, rowIndex, eShape.BlackKing));

                    // Checks available right downwards move
                    output.AddRange(rightDownward(colIndex, rowIndex, eShape.BlackKing));

                    // Checks available left downwards move
                    output.AddRange(leftDownward(colIndex, rowIndex, eShape.BlackKing));
                    break;
            }

            return output;
        }

        private List<Move> rightUpward(int i_ColIndex, int i_RowIndex, eShape i_Shape)
        {
            List<Move> output = new List<Move>();
            if (CheckBoundaries(i_RowIndex - 1, i_ColIndex + 1, m_Board.Size))
            {
                if (m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex + 1).GetShape == eShape.Blank)
                {
                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex + 1), false));
                }
                else
                {
                    switch (i_Shape)
                    {
                        case eShape.White:
                        case eShape.WhiteKing:
                            if ((m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex + 1).GetShape == eShape.Black) || (m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex + 1).GetShape == eShape.BlackKing))
                            {
                                if (CheckBoundaries(i_RowIndex - 2, i_ColIndex + 2, m_Board.Size) && m_Board.getBoardSquare(i_RowIndex - 2, i_ColIndex + 2).GetShape == eShape.Blank)
                                {
                                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex - 2, i_ColIndex + 2), true));
                                }
                            }

                            break;
                        case eShape.Black:
                        case eShape.BlackKing:
                            if ((m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex + 1).GetShape == eShape.White) || (m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex + 1).GetShape == eShape.WhiteKing))
                            {
                                if (CheckBoundaries(i_RowIndex - 2, i_ColIndex + 2, m_Board.Size) && m_Board.getBoardSquare(i_RowIndex - 2, i_ColIndex + 2).GetShape == eShape.Blank)
                                {
                                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex - 2, i_ColIndex + 2), true));
                                }
                            }

                            break;
                    }
                }
            }

            return output;
        }

        private List<Move> leftUpward(int i_ColIndex, int i_RowIndex, eShape i_Shape)
        {
            List<Move> output = new List<Move>();
            if (CheckBoundaries(i_RowIndex - 1, i_ColIndex - 1, m_Board.Size))
            {
                if (m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex - 1).GetShape == eShape.Blank)
                {
                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex - 1), false));
                }
                else
                {
                    switch (i_Shape)
                    {
                        case eShape.White:
                        case eShape.WhiteKing:
                            if ((m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex - 1).GetShape == eShape.Black) || (m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex - 1).GetShape == eShape.BlackKing))
                            {
                                if (CheckBoundaries(i_RowIndex - 2, i_ColIndex - 2, m_Board.Size) && m_Board.getBoardSquare(i_RowIndex - 2, i_ColIndex - 2).GetShape == eShape.Blank)
                                {
                                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex - 2, i_ColIndex - 2), true));
                                }
                            }

                            break;
                        case eShape.Black:
                        case eShape.BlackKing:
                            if ((m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex - 1).GetShape == eShape.White) || (m_Board.getBoardSquare(i_RowIndex - 1, i_ColIndex - 1).GetShape == eShape.WhiteKing))
                            {
                                if (CheckBoundaries(i_RowIndex - 2, i_ColIndex - 2, m_Board.Size) && m_Board.getBoardSquare(i_RowIndex - 2, i_ColIndex - 2).GetShape == eShape.Blank)
                                {
                                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex - 2, i_ColIndex - 2), true));
                                }
                            }

                            break;
                    }
                }
            }

            return output;
        }

        private List<Move> leftDownward(int i_ColIndex, int i_RowIndex, eShape i_Shape)
        {
            List<Move> output = new List<Move>();
            if (CheckBoundaries(i_RowIndex + 1, i_ColIndex - 1, m_Board.Size))
            {
                if (m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex - 1).GetShape == eShape.Blank)
                {
                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex - 1), false));
                }
                else
                {
                    switch (i_Shape)
                    {
                        case eShape.White:
                        case eShape.WhiteKing:
                            if ((m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex - 1).GetShape == eShape.Black) || (m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex - 1).GetShape == eShape.BlackKing))
                            {
                                if (CheckBoundaries(i_RowIndex + 2, i_ColIndex - 2, m_Board.Size) && m_Board.getBoardSquare(i_RowIndex + 2, i_ColIndex - 2).GetShape == eShape.Blank)
                                {
                                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex + 2, i_ColIndex - 2), true));
                                }
                            }

                            break;
                        case eShape.Black:
                        case eShape.BlackKing:
                            if ((m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex - 1).GetShape == eShape.White) || (m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex - 1).GetShape == eShape.WhiteKing))
                            {
                                if (CheckBoundaries(i_RowIndex + 2, i_ColIndex - 2, m_Board.Size) && m_Board.getBoardSquare(i_RowIndex + 2, i_ColIndex - 2).GetShape == eShape.Blank)
                                {
                                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex + 2, i_ColIndex - 2), true));
                                }
                            }

                            break;
                    }
                }
            }

            return output;
        }

        private List<Move> rightDownward(int i_ColIndex, int i_RowIndex, eShape i_Shape)
        {
            List<Move> output = new List<Move>();
            if (CheckBoundaries(i_RowIndex + 1, i_ColIndex + 1, m_Board.Size))
            {
                if (m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex + 1).GetShape == eShape.Blank)
                {
                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex + 1), false));
                }
                else
                {
                    switch (i_Shape)
                    {
                        case eShape.White:
                        case eShape.WhiteKing:
                            if ((m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex + 1).GetShape == eShape.Black) || (m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex + 1).GetShape == eShape.BlackKing))
                            {
                                if (CheckBoundaries(i_RowIndex + 2, i_ColIndex + 2, m_Board.Size) && m_Board.getBoardSquare(i_RowIndex + 2, i_ColIndex + 2).GetShape == eShape.Blank)
                                {
                                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex + 2, i_ColIndex + 2), true));
                                }
                            }

                            break;
                        case eShape.Black:
                        case eShape.BlackKing:
                            if ((m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex + 1).GetShape == eShape.White) || (m_Board.getBoardSquare(i_RowIndex + 1, i_ColIndex + 1).GetShape == eShape.WhiteKing))
                            {
                                if (CheckBoundaries(i_RowIndex + 2, i_ColIndex + 2, m_Board.Size) && m_Board.getBoardSquare(i_RowIndex + 2, i_ColIndex + 2).GetShape == eShape.Blank)
                                {
                                    output.Add(new Move(m_Board.getBoardSquare(i_RowIndex, i_ColIndex), m_Board.getBoardSquare(i_RowIndex + 2, i_ColIndex + 2), true));
                                }
                            }

                            break;
                    }
                }
            }

            return output;
        }
    }
}