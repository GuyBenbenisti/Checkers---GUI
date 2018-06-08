using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace CheckersUI
{
    public class FormGameSettings : Form
    {
        private int m_BoardSize;

        // Board size buttons
        private RadioButton radioButtonSix;
        private RadioButton radioButtonEight;
        private RadioButton radioButtonTen;

        // Players name labels
        private Label labelPlayers;
        private Label labelPlayerOne;
        private Label labelBoardSize;

        private CheckBox checkBoxPlayerTwo;

        private TextBox textBoxPlayerOne;
        private TextBox textBoxPlayerTwo;
        private Button buttonDone;
        
        public CheckBox CheckBoxPlayerTwo { get => checkBoxPlayerTwo; }
        
        public TextBox TextBoxPlayerOne { get => textBoxPlayerOne; }

        public TextBox TextBoxPlayerTwo { get => textBoxPlayerTwo; }

        public int BoardSize { get => m_BoardSize; set => m_BoardSize = value; }

        public FormGameSettings()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Text = "Game Settings";
            this.ClientSize = new Size(227, 181);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.InitializeComponent();
        }                

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGameSettings));
            this.labelBoardSize = new System.Windows.Forms.Label();
            this.radioButtonSix = new System.Windows.Forms.RadioButton();
            this.radioButtonEight = new System.Windows.Forms.RadioButton();
            this.radioButtonTen = new System.Windows.Forms.RadioButton();
            this.labelPlayers = new System.Windows.Forms.Label();
            this.labelPlayerOne = new System.Windows.Forms.Label();
            this.checkBoxPlayerTwo = new System.Windows.Forms.CheckBox();
            this.textBoxPlayerOne = new System.Windows.Forms.TextBox();
            this.textBoxPlayerTwo = new System.Windows.Forms.TextBox();
            this.buttonDone = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // labelBoardSize        
            this.labelBoardSize.BackColor = System.Drawing.Color.Transparent;
            this.labelBoardSize.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.labelBoardSize.Location = new System.Drawing.Point(22, 9);
            this.labelBoardSize.Name = "labelBoardSize";
            this.labelBoardSize.Size = new System.Drawing.Size(82, 16);
            this.labelBoardSize.TabIndex = 9;
            this.labelBoardSize.Text = "Board Size:";

            // radioButtonSix
            this.radioButtonSix.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonSix.Checked = true;
            this.radioButtonSix.Location = new System.Drawing.Point(44, 32);
            this.radioButtonSix.Name = "radioButtonSix";
            this.radioButtonSix.Size = new System.Drawing.Size(48, 17);
            this.radioButtonSix.TabIndex = 8;
            this.radioButtonSix.TabStop = true;
            this.radioButtonSix.Text = "6 x 6";
            this.radioButtonSix.UseVisualStyleBackColor = false;

            // radioButtonEight
            this.radioButtonEight.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonEight.Location = new System.Drawing.Point(107, 32);
            this.radioButtonEight.Name = "radioButtonEight";
            this.radioButtonEight.Size = new System.Drawing.Size(48, 17);
            this.radioButtonEight.TabIndex = 7;
            this.radioButtonEight.Text = "8 x 8";
            this.radioButtonEight.UseVisualStyleBackColor = false;

            // radioButtonTen
            this.radioButtonTen.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonTen.ForeColor = System.Drawing.SystemColors.WindowText;
            this.radioButtonTen.Location = new System.Drawing.Point(170, 32);
            this.radioButtonTen.Name = "radioButtonTen";
            this.radioButtonTen.Size = new System.Drawing.Size(60, 17);
            this.radioButtonTen.TabIndex = 6;
            this.radioButtonTen.Text = "10 x 10";
            this.radioButtonTen.UseVisualStyleBackColor = false;

            // labelPlayers
            this.labelPlayers.AutoSize = true;
            this.labelPlayers.BackColor = System.Drawing.Color.Transparent;
            this.labelPlayers.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.labelPlayers.Location = new System.Drawing.Point(23, 55);
            this.labelPlayers.Name = "labelPlayers";
            this.labelPlayers.Size = new System.Drawing.Size(59, 16);
            this.labelPlayers.TabIndex = 5;
            this.labelPlayers.Text = "Players:";

            // labelPlayerOne
            this.labelPlayerOne.BackColor = System.Drawing.Color.Transparent;
            this.labelPlayerOne.Location = new System.Drawing.Point(37, 85);
            this.labelPlayerOne.Name = "labelPlayerOne";
            this.labelPlayerOne.Size = new System.Drawing.Size(48, 13);
            this.labelPlayerOne.TabIndex = 4;
            this.labelPlayerOne.Text = "Player 1:";

            // checkBoxPlayerTwo
            this.checkBoxPlayerTwo.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxPlayerTwo.Location = new System.Drawing.Point(18, 109);
            this.checkBoxPlayerTwo.Name = "checkBoxPlayerTwo";
            this.checkBoxPlayerTwo.Size = new System.Drawing.Size(67, 17);
            this.checkBoxPlayerTwo.TabIndex = 3;
            this.checkBoxPlayerTwo.Text = "Player 2:";
            this.checkBoxPlayerTwo.UseVisualStyleBackColor = false;
            this.checkBoxPlayerTwo.CheckedChanged += new System.EventHandler(this.checkboxPlayerTwo_CheckedChanged);

            // textBoxPlayerOne
            this.textBoxPlayerOne.Location = new System.Drawing.Point(101, 83);
            this.textBoxPlayerOne.Name = "textBoxPlayerOne";
            this.textBoxPlayerOne.Size = new System.Drawing.Size(100, 20);
            this.textBoxPlayerOne.TabIndex = 2;

            // textBoxPlayerTwo
            this.textBoxPlayerTwo.Enabled = false;
            this.textBoxPlayerTwo.Location = new System.Drawing.Point(100, 109);
            this.textBoxPlayerTwo.Name = "textBoxPlayerTwo";
            this.textBoxPlayerTwo.Size = new System.Drawing.Size(100, 20);
            this.textBoxPlayerTwo.TabIndex = 1;
            this.textBoxPlayerTwo.Text = "[Computer]";

            // buttonDone
            this.buttonDone.BackColor = System.Drawing.Color.Transparent;
            this.buttonDone.Location = new System.Drawing.Point(156, 146);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 0;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = false;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);  

            // FormGameSettings
            this.AcceptButton = this.buttonDone;
            this.BackgroundImage = global::CheckersUI.Properties.Resources.Background;
            this.ClientSize = new System.Drawing.Size(256, 180);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.textBoxPlayerTwo);
            this.Controls.Add(this.textBoxPlayerOne);
            this.Controls.Add(this.checkBoxPlayerTwo);
            this.Controls.Add(this.labelPlayerOne);
            this.Controls.Add(this.labelPlayers);
            this.Controls.Add(this.radioButtonTen);
            this.Controls.Add(this.radioButtonEight);
            this.Controls.Add(this.radioButtonSix);
            this.Controls.Add(this.labelBoardSize);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGameSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            if (radioButtonSix.Checked)
            {
                this.BoardSize = 6;
            }
            else if (radioButtonEight.Checked)
            {
                this.BoardSize = 8;
            }
            else if (radioButtonTen.Checked)
            {
                this.BoardSize = 10;
            }
        }

        private void checkboxPlayerTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.textBoxPlayerTwo.Enabled)
            {
                this.textBoxPlayerTwo.Enabled = false;
                this.textBoxPlayerTwo.Text = "[Computer]";
            }
            else
            {
                this.textBoxPlayerTwo.Enabled = true;
            }
        }      
    }
}
