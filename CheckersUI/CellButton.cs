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
    internal class CellButton : Button
    {
        internal class PiecePictureBox : PictureBox
        {
            internal CheckersENums.eShape m_Shape;
            internal CheckersENums.ePlayerColor m_Color;

            internal PiecePictureBox(CheckersENums.eShape i_Shape, Point i_Location)
            {
                this.ClientSize = new Size(FormGameBoard.k_CellSize, FormGameBoard.k_CellSize);
                this.Location = i_Location;
                m_Shape = i_Shape;
                switch (i_Shape)
                {
                    case CheckersENums.eShape.Black:
                        this.m_Color = CheckersENums.ePlayerColor.Black;
                        this.Image = global::CheckersUI.Properties.Resources.black_piece;
                        break;
                    case CheckersENums.eShape.BlackKing:
                        this.m_Color = CheckersENums.ePlayerColor.Black;
                        this.Image = global::CheckersUI.Properties.Resources.black_king;
                        break;
                    case CheckersENums.eShape.White:
                        this.m_Color = CheckersENums.ePlayerColor.White;
                        this.Image = global::CheckersUI.Properties.Resources.white_piece;
                        break;
                    case CheckersENums.eShape.WhiteKing:
                        this.m_Color = CheckersENums.ePlayerColor.White;
                        this.Image = global::CheckersUI.Properties.Resources.white_king;
                        break;
                }
            }

            internal CheckersENums.eShape Shape { get => m_Shape; set => m_Shape = value; }

            internal CheckersENums.ePlayerColor Color { get => m_Color; set => m_Color = value; }
        }

        internal enum eCellColor
        {
            White,
            Brown
        }

        private Location m_Coordinates;
        private PiecePictureBox piecePic;
        private eCellColor m_Color;

        internal PiecePictureBox PiecePicture { get => piecePic; set => piecePic = value; }

        public Location Coordinates { get => m_Coordinates; set => m_Coordinates = value; }

        internal eCellColor Color { get => m_Color; set => m_Color = value; }

        internal CellButton(Location i_Location, eCellColor i_Color)
        {
            m_Coordinates = i_Location;
            int leftBorder = FormGameBoard.k_Margin + (FormGameBoard.k_CellSize * i_Location.ColIndex);
            int upBorder = (FormGameBoard.k_Margin * 2) + (FormGameBoard.k_CellSize * i_Location.RowIndex);
            this.Location = new Point(leftBorder, upBorder);
            this.ClientSize = new Size(FormGameBoard.k_CellSize, FormGameBoard.k_CellSize);
            switch (i_Color)
            {
                case eCellColor.Brown:
                    this.BackgroundImage = global::CheckersUI.Properties.Resources.BrownCell;
                    m_Color = eCellColor.Brown;
                    break;
                case eCellColor.White:
                    this.Enabled = false;
                    m_Color = eCellColor.White;
                    this.BackgroundImage = global::CheckersUI.Properties.Resources.WhiteCell;
                    break;
            }
        }
    }
}