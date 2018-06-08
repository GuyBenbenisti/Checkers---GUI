using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Checkers.GameLogic;

namespace CheckersUI
{
    public class FormGameBoard : Form
    {
        internal const int k_CellSize = 52;
        internal const int k_Margin = 25;
        internal ComponentResourceManager m_Resources = new ComponentResourceManager(typeof(FormGameSettings));

        private int m_Size;
        private Label labelPlayerOneName;
        private Label labelPlayerTwoName;
        private string m_PlayerOneName;
        private string m_PlayerTwoName;
        private bool m_BotGame;
        private FormGameSettings m_GameSettings;
        private CellButton[,] boardButtons;
        private Game m_Game;
        private CellButton cellButtonLastClicked;

        public FormGameBoard(FormGameSettings i_GameSettings)
        {
            m_GameSettings = i_GameSettings;
            m_BotGame = !m_GameSettings.CheckBoxPlayerTwo.Checked;
            this.Text = "BeSt DamKa EVERRRR";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            boardButtons = new CellButton[m_GameSettings.BoardSize, m_GameSettings.BoardSize];
            m_Size = (k_CellSize * m_GameSettings.BoardSize) + (k_Margin * 2);
            this.Icon = (Icon)m_Resources.GetObject("$this.Icon");
            this.ClientSize = new Size(m_Size, m_Size + k_Margin);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.initializeComponent();

            this.initiateBoard();
            if (m_BotGame)
            {
                this.m_Game = new Game(m_PlayerOneName, m_GameSettings.BoardSize);
            }
            else
            {
                this.m_Game = new Game(m_PlayerOneName, m_PlayerTwoName, m_GameSettings.BoardSize);
            }

            this.initiatePlayers();

            this.registerEvents();
        }

        private void registerEvents()
        {
            m_Game.SwitchTurn += new EventHandler(turnChanged);
            m_Game.UpdateBoard += new EventHandler(boardUpdate);
            m_Game.RoyalMove += new EventHandler(royalUpdate);
            m_Game.InvalidMove += new EventHandler(invalidMove);
            m_Game.EndOfRound += new EventHandler(endRoundMessage);
        }

        private void invalidMove(object sender, EventArgs e)
        {
            MessageBox.Show("Invalid Move!" + Environment.NewLine + "Please choose another move", "best Damka ever", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void initializeComponent()
        {
            // Player One Name            
            labelPlayerOneName = new Label();
            labelPlayerOneName.AutoSize = true;
            m_PlayerOneName = m_GameSettings.TextBoxPlayerOne.Text;
            labelPlayerOneName.Text = m_GameSettings.TextBoxPlayerOne.Text + ": 0";
            labelPlayerOneName.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            labelPlayerOneName.Location = new Point(k_Margin * 3, k_Margin);
            this.Controls.Add(labelPlayerOneName);

            // Player Two Name            
            labelPlayerTwoName = new Label();
            labelPlayerTwoName.AutoSize = true;
            m_PlayerTwoName = m_GameSettings.TextBoxPlayerTwo.Text;
            labelPlayerTwoName.Text = m_GameSettings.TextBoxPlayerTwo.Text + ": 0";
            labelPlayerTwoName.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            labelPlayerTwoName.Location = new Point(m_Size - (labelPlayerOneName.Left + labelPlayerOneName.Width), labelPlayerOneName.Location.Y);
            this.Controls.Add(labelPlayerTwoName);
        }

        private void initiateBoard()
        {
            int boardSize = m_GameSettings.BoardSize;
            bool cellColor;
            CellButton.eCellColor color;
            CellButton curCell;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    cellColor = (i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0);
                    color = cellColor ? CellButton.eCellColor.White : CellButton.eCellColor.Brown;
                    Location cellLocation = new Location(i, j);
                    curCell = new CellButton(cellLocation, color);
                    curCell.Click += new EventHandler(cellButtonOnClick);
                    boardButtons[i, j] = curCell;
                    this.Controls.Add(curCell);
                }
            }
        }

        private void initiatePlayers()
        {
            int boardSize = m_GameSettings.BoardSize;
            int numberOfCheckersLines = (boardSize / 2) - 1;
            CellButton curCellBlack, curCellWhite;
            for (int i = 0; i < numberOfCheckersLines; i++)
            {
                for (int j = 0; j < m_GameSettings.BoardSize; j++)
                {
                    boardButtons[i, j].PiecePicture = null;
                    boardButtons[i, j].Image = null;
                    if ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0))
                    {
                        curCellBlack = boardButtons[i, j];
                        curCellWhite = boardButtons[boardSize - i - 1, boardSize - j - 1];
                        curCellWhite.PiecePicture = new CellButton.PiecePictureBox(CheckersENums.eShape.White, curCellWhite.Location);
                        curCellBlack.PiecePicture = new CellButton.PiecePictureBox(CheckersENums.eShape.Black, curCellBlack.Location);
                        curCellWhite.Image = curCellWhite.PiecePicture.Image;
                        curCellBlack.Image = curCellBlack.PiecePicture.Image;
                    }
                }
            }

            turnChanged(CheckersENums.ePlayerColor.White, EventArgs.Empty);
        }

        private void turnChanged(object sender, EventArgs e)
        {
            if (sender is CheckersENums.ePlayerColor curColorToEnable)
            {
                foreach (CellButton curButton in boardButtons)
                {
                    if (curButton.PiecePicture != null)
                    {
                        if (curButton.PiecePicture.Color == curColorToEnable)
                        {
                            curButton.Enabled = true;
                        }
                        else
                        {
                            curButton.Enabled = false;
                        }
                    }
                    else
                    {
                        if (curButton.Color == CellButton.eCellColor.Brown)
                        {
                            curButton.Enabled = true;
                        }
                    }
                }

                if (m_Game.ChangeTurn)
                {
                    changeLabelsColors();
                }
            }
        }

        private void changeLabelsColors()
        {
            if (labelPlayerOneName.BackColor == Color.Lime)
            {
                labelPlayerOneName.BackColor = Color.Transparent;
                labelPlayerTwoName.BackColor = Color.Lime;
            }
            else
            {
                labelPlayerOneName.BackColor = Color.Lime;
                labelPlayerTwoName.BackColor = Color.Transparent;
            }
        }

        private void cellButtonOnClick(object sender, EventArgs e)
        {
            if (cellButtonLastClicked != null)
            {
                if (cellButtonLastClicked == sender)
                {
                    cellButtonLastClicked = null;
                    labelPlayerOneName.Focus();
                }
                else
                {
                    Move curMove = new Move(cellButtonLastClicked.Coordinates, (sender as CellButton).Coordinates);
                    m_Game.PlayTurn(curMove);
                    cellButtonLastClicked = null;
                }
            }
            else
            {
                cellButtonLastClicked = sender as CellButton;
            }
        }

        private void boardUpdate(object sender, EventArgs e)
        {
            if (sender is Move)
            {
                Move curMove = sender as Move;
                Location origin = curMove.OriginCoordinates;
                Location dest = curMove.DestCoordinates;
                CellButton originCell = boardButtons[origin.RowIndex, origin.ColIndex];
                if (curMove.IsEatMove)
                {
                    int toEatRowIndex = origin.RowIndex > dest.RowIndex ? origin.RowIndex - 1 : origin.RowIndex + 1;
                    int toEatColIndex = origin.ColIndex > dest.ColIndex ? origin.ColIndex - 1 : origin.ColIndex + 1;
                    boardButtons[toEatRowIndex, toEatColIndex].PiecePicture = null;
                    boardButtons[toEatRowIndex, toEatColIndex].Image = null;
                }

                CellButton destCell = boardButtons[curMove.DestCoordinates.RowIndex, curMove.DestCoordinates.ColIndex];
                destCell.PiecePicture = new CellButton.PiecePictureBox(originCell.PiecePicture.Shape, destCell.Location);
                destCell.Image = destCell.PiecePicture.Image;
                originCell.PiecePicture = null;
                originCell.Image = null;
            }
        }

        private void royalUpdate(object sender, EventArgs e)
        {
            if (sender is Move)
            {
                Move curMove = sender as Move;
                Location dest = curMove.DestCoordinates;
                CellButton destCell = boardButtons[curMove.DestCoordinates.RowIndex, curMove.DestCoordinates.ColIndex];
                switch (destCell.PiecePicture.Shape)
                {
                    case CheckersENums.eShape.Black:
                        destCell.PiecePicture.Shape = CheckersENums.eShape.BlackKing;
                        destCell.PiecePicture.Image = global::CheckersUI.Properties.Resources.black_king;
                        break;
                    case CheckersENums.eShape.White:
                        destCell.PiecePicture.Shape = CheckersENums.eShape.WhiteKing;
                        destCell.PiecePicture.Image = global::CheckersUI.Properties.Resources.white_king;
                        break;
                }

                destCell.Image = destCell.PiecePicture.Image;
            }
        }

        private void endRoundMessage(object sender, EventArgs e)
        {
            DialogResult anotherRound = DialogResult.None;
            switch (m_Game.GameStatus)
            {
                case CheckersENums.eGameStatus.Draw:
                    string tieMessage = "Tie !!" + Environment.NewLine + "Another Round?";
                    anotherRound = MessageBox.Show(tieMessage, "BeSt DamKa EVERRRR", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    break;
                case CheckersENums.eGameStatus.Win:
                    string winMessage = (sender as Player).Name + " Won !!" + Environment.NewLine + "Another Round?";
                    anotherRound = MessageBox.Show(winMessage, "BeSt DamKa EVERRRR", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    break;
            }

            if (anotherRound == DialogResult.No)
            {
                this.Close();
                Application.Exit();
            }

            if (anotherRound == DialogResult.Yes)
            {
                this.startAnotherRound();
            }
        }

        private void startAnotherRound()
        {
            int PlayerOneScore = m_Game.PlayerOne.Score;
            int PlayerTwoScore = m_Game.PlayerTwo.Score;
            this.Controls.Clear();
            this.OnLoad(EventArgs.Empty);
            this.m_Game.PlayerOne.Score = PlayerOneScore;
            this.m_Game.PlayerTwo.Score = PlayerTwoScore;
            this.labelPlayerOneName.Text = m_Game.PlayerOne.Name + ": " + m_Game.PlayerOne.Score;
            this.labelPlayerTwoName.Text = m_Game.PlayerTwo.Name + ": " + m_Game.PlayerTwo.Score;
        }
    }
}
