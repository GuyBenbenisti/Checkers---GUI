using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Checkers.GameLogic.CheckersENums;

namespace Checkers.GameLogic
{
    public class Game
    {
        public event EventHandler EndOfRound;

        public event EventHandler SwitchTurn;

        public event EventHandler InvalidMove;

        public event EventHandler RoyalMove;

        public event EventHandler UpdateBoard;

        private int m_BoardSize;
        private bool m_ChangeTurn;
        private Player m_PlayerOne;
        private Player m_PlayerTwo;
        private Player m_CurrentPlayerTurn; // Pointer, which player turn it is.
        private Player m_NextPlayerTurn;
        private Board m_Board;
        private eGameStatus m_GameStatus;

        public eGameStatus GameStatus { get => m_GameStatus; set => m_GameStatus = value; }

        public Player PlayerOne { get => m_PlayerOne; set => m_PlayerOne = value; }

        public Player PlayerTwo { get => m_PlayerTwo; set => m_PlayerTwo = value; }

        public bool ChangeTurn { get => m_ChangeTurn; set => m_ChangeTurn = value; }

        // Constructor for game against user
        public Game(string i_PlayerOneName, string i_PlayerTwoName, int i_BoardSize)
        {
            m_ChangeTurn = true;
            m_BoardSize = i_BoardSize;
            m_PlayerOne = new Player(ePlayerType.User, i_PlayerOneName, ePlayerColor.White); // Create first player
            m_PlayerTwo = new Player(ePlayerType.User, i_PlayerTwoName, ePlayerColor.Black);
            m_GameStatus = eGameStatus.Playing;
            m_Board = new Board(i_BoardSize);
            m_CurrentPlayerTurn = m_PlayerOne;
            m_NextPlayerTurn = m_PlayerTwo;
        }

        // Constructor for Game against computer
        public Game(string i_PlayerOneName, int i_BoardSize)
        {
            m_ChangeTurn = true;
            m_BoardSize = i_BoardSize;
            m_PlayerOne = new Player(ePlayerType.User, i_PlayerOneName, ePlayerColor.White); // Create first player
            m_PlayerTwo = new Player(ePlayerType.Bot, "Bot", ePlayerColor.Black);
            m_GameStatus = eGameStatus.Playing;
            m_Board = new Board(i_BoardSize);
            m_CurrentPlayerTurn = m_PlayerOne;
            m_NextPlayerTurn = m_PlayerTwo;
        }

        public void InitiateNewRound()
        {
            this.m_Board = new Board(m_BoardSize);
            m_ChangeTurn = true;
            this.m_CurrentPlayerTurn = m_PlayerOne;
            this.m_NextPlayerTurn = m_PlayerTwo;
        }

        public void PlayTurn(Move i_Move)
        {
            bool isValidMove;
            Turn currentTurn = new Turn(m_Board, m_CurrentPlayerTurn);
            updateTurnEvents(currentTurn);
            isValidMove = currentTurn.isValidMove(i_Move);
            if (!isValidMove)
            {
                InvalidMove.Invoke(this, EventArgs.Empty);
            }
            else
            {
                currentTurn.PlayTurn();
                m_Board = currentTurn.Board;
                if (!checkForEndRound(currentTurn))
                {
                    if (currentTurn.SwitchTurn)
                    {
                        ChangeTurn = false;
                        switch (m_NextPlayerTurn.Type)
                        {
                            case CheckersENums.ePlayerType.Bot:
                                currentTurn.CurrentPlayer = m_NextPlayerTurn;
                                currentTurn.PlayBotTurn();
                                SwitchTurn(m_CurrentPlayerTurn.Color, EventArgs.Empty);
                                break;
                            case CheckersENums.ePlayerType.User:
                                ChangeTurn = true;
                                SwitchTurn(m_NextPlayerTurn.Color, EventArgs.Empty);
                                switchPlayers();                                
                                break;
                        }
                    }
                }
                else
                {
                    m_ChangeTurn = false;

                    SwitchTurn(m_CurrentPlayerTurn.Color, EventArgs.Empty);
                }

                if (checkForEndRound(currentTurn))
                {
                    Player roundWinner = updateScore();
                    EndOfRound.Invoke(roundWinner, EventArgs.Empty);
                }
            }
        }

        internal bool checkForEndRound(Turn i_CurrentTurn)
        {
            bool endRound = false;
            if (i_CurrentTurn.CheckForNoMoreMoves(m_CurrentPlayerTurn) && i_CurrentTurn.CheckForNoMoreMoves(m_NextPlayerTurn))
            {
                m_GameStatus = eGameStatus.Draw;
                endRound = true;
            }
            else if (i_CurrentTurn.CheckForNoMoreMoves(m_CurrentPlayerTurn) || i_CurrentTurn.CheckForNoMoreMoves(m_NextPlayerTurn))
            {
                m_GameStatus = eGameStatus.Win;
                endRound = true;
            }

            return endRound;
        }

        private void switchPlayers()
        {
            Player temp = m_CurrentPlayerTurn;
            m_CurrentPlayerTurn = m_NextPlayerTurn;
            m_NextPlayerTurn = temp;
        }

        private Player updateScore()
        {
            int currentPlayerOneScore = 0;
            int currentPlayerTwoScore = 0;
            foreach (Board.BoardSquare currentSquare in m_Board.Matrix)
            {
                if (currentSquare.GetShape == eShape.White)
                {
                    m_PlayerOne.Score++;
                    currentPlayerOneScore++;
                }

                if (currentSquare.GetShape == eShape.WhiteKing)
                {
                    m_PlayerOne.Score += 4;
                    currentPlayerOneScore++;
                }

                if (currentSquare.GetShape == eShape.Black)
                {
                    m_PlayerTwo.Score++;
                    currentPlayerTwoScore++;
                }

                if (currentSquare.GetShape == eShape.BlackKing)
                {
                    m_PlayerTwo.Score += 4;
                    currentPlayerTwoScore++;
                }
            }

            return currentPlayerOneScore > currentPlayerTwoScore ? m_PlayerOne : m_PlayerTwo;
        }

        private void updateTurnEvents(Turn i_CurrentTurn)
        {
            i_CurrentTurn.UpdateBoard += this.UpdateBoard;
            i_CurrentTurn.RoyalMove += this.RoyalMove;
        }
    }
}
