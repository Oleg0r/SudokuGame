namespace SudokuGame
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip1           = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem    = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem    = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem    = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem    = new System.Windows.Forms.ToolStripMenuItem();
            this.recordsMenu              = new System.Windows.Forms.ToolStripMenuItem();
            this.recordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            this.panelTop             = new System.Windows.Forms.Panel();
            this.comboBoxDifficulty   = new System.Windows.Forms.ComboBox();
            this.buttonNewGame        = new System.Windows.Forms.Button();
            this.buttonHint           = new System.Windows.Forms.Button();
            this.buttonUndo           = new System.Windows.Forms.Button();
            this.labelTimer           = new System.Windows.Forms.Label();
            this.labelHints           = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // menuStrip1
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.fileToolStripMenuItem, this.recordsMenu });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Size = new System.Drawing.Size(560, 24);

            // File menu
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.saveToolStripMenuItem,
                this.openToolStripMenuItem,
                new System.Windows.Forms.ToolStripSeparator(),
                this.exitToolStripMenuItem });
            this.fileToolStripMenuItem.Text = "Файл";

            this.saveToolStripMenuItem.Text = "Зберегти гру";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);

            this.openToolStripMenuItem.Text = "Відкрити гру";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);

            this.exitToolStripMenuItem.Text = "Вихід";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);

            // Records menu
            this.recordsMenu.DropDownItems.Add(this.recordsToolStripMenuItem);
            this.recordsMenu.Text = "Рекорди";

            this.recordsToolStripMenuItem.Text = "Таблиця рекордів";
            this.recordsToolStripMenuItem.Click += new System.EventHandler(this.recordsToolStripMenuItem_Click);

            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.panelTop.Bounds    = new System.Drawing.Rectangle(0, 24, 560, 70);

            // comboBoxDifficulty
            this.comboBoxDifficulty.Items.AddRange(new object[] { "Легкий", "Середній", "Складний" });
            this.comboBoxDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDifficulty.Bounds        = new System.Drawing.Rectangle(10, 22, 110, 26);
            this.comboBoxDifficulty.Font          = new System.Drawing.Font("Arial", 10f);
            this.panelTop.Controls.Add(this.comboBoxDifficulty);

            // buttonNewGame
            this.buttonNewGame.Text      = "▶ Нова гра";
            this.buttonNewGame.Bounds    = new System.Drawing.Rectangle(130, 18, 110, 32);
            this.buttonNewGame.Font      = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
            this.buttonNewGame.BackColor = System.Drawing.Color.FromArgb(46, 134, 171);
            this.buttonNewGame.ForeColor = System.Drawing.Color.White;
            this.buttonNewGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNewGame.Click    += new System.EventHandler(this.buttonNewGame_Click);
            this.panelTop.Controls.Add(this.buttonNewGame);

            // buttonHint
            this.buttonHint.Text      = "💡 Підказка";
            this.buttonHint.Bounds    = new System.Drawing.Rectangle(250, 18, 110, 32);
            this.buttonHint.Font      = new System.Drawing.Font("Arial", 10f);
            this.buttonHint.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.buttonHint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHint.Click    += new System.EventHandler(this.buttonHint_Click);
            this.panelTop.Controls.Add(this.buttonHint);

            // buttonUndo
            this.buttonUndo.Text      = "↩ Скасувати";
            this.buttonUndo.Bounds    = new System.Drawing.Rectangle(370, 18, 110, 32);
            this.buttonUndo.Font      = new System.Drawing.Font("Arial", 10f);
            this.buttonUndo.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.buttonUndo.ForeColor = System.Drawing.Color.White;
            this.buttonUndo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUndo.Click    += new System.EventHandler(this.buttonUndo_Click);
            this.panelTop.Controls.Add(this.buttonUndo);

            // labelTimer
            this.labelTimer.Text      = "⏱ 00:00";
            this.labelTimer.Bounds    = new System.Drawing.Rectangle(10, 0, 120, 20);
            this.labelTimer.Font      = new System.Drawing.Font("Arial", 11f, System.Drawing.FontStyle.Bold);
            this.labelTimer.ForeColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.labelTimer);

            // labelHints
            this.labelHints.Text      = "Підказки: 3";
            this.labelHints.Bounds    = new System.Drawing.Rectangle(490, 22, 60, 32);
            this.labelHints.Font      = new System.Drawing.Font("Arial", 9f);
            this.labelHints.ForeColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.labelHints);

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize    = new System.Drawing.Size(560, 620);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip   = this.menuStrip1;
            this.MaximizeBox     = false;
            this.Text            = "Судоку";
            this.BackColor       = System.Drawing.Color.White;

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.MenuStrip        menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recordsMenu;
        private System.Windows.Forms.ToolStripMenuItem recordsToolStripMenuItem;
        private System.Windows.Forms.Panel            panelTop;
        private System.Windows.Forms.ComboBox         comboBoxDifficulty;
        private System.Windows.Forms.Button           buttonNewGame;
        private System.Windows.Forms.Button           buttonHint;
        private System.Windows.Forms.Button           buttonUndo;
        private System.Windows.Forms.Label            labelTimer;
        private System.Windows.Forms.Label            labelHints;
    }
}
