using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CheckersUI
{
    public class Program
    {
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            FormGameSettings formGameSettings = new FormGameSettings();
            formGameSettings.ShowDialog();
            if (formGameSettings.DialogResult == DialogResult.OK)
            {
                FormGameBoard gameBoard = new FormGameBoard(formGameSettings);
                gameBoard.ShowDialog();
            }
        }
    }
}
