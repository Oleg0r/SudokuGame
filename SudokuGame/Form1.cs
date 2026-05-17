using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SudokuGame
{
    public partial class Form1 : Form
    {
        // ─── Поля ─────────────────────────────────────────────────────────
        private SudokuBoard  _board   = new SudokuBoard();
        private GameHistory  _history = new GameHistory();
        private SudokuTimer  _timer   = new SudokuTimer();
        private RecordManager _records = new RecordManager();

        private TextBox[,] _cells;
        private bool _updating;
        private int  _hintsLeft = 3;

        // ─── Конструктор ──────────────────────────────────────────────────
        public Form1()
        {
            InitializeComponent();
            BuildGrid();
            UpdateControls();
        }

        // ─── Побудова сітки ───────────────────────────────────────────────
        private void BuildGrid()
        {
            _cells = new TextBox[9, 9];
            const int cellSize  = 52;
            const int gridLeft  = 15;
            const int gridTop   = 100;
            const int blockGap  = 4;

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    int x = gridLeft + c * cellSize + (c / 3) * blockGap;
                    int y = gridTop  + r * cellSize + (r / 3) * blockGap;

                    var tb = new TextBox
                    {
                        Bounds    = new Rectangle(x, y, cellSize - 2, cellSize - 2),
                        Font      = new Font("Arial", 18, FontStyle.Bold),
                        TextAlign = HorizontalAlignment.Center,
                        MaxLength  = 1,
                        ReadOnly   = true,
                        TabIndex   = r * 9 + c
                    };

                    int row = r, col = c;
                    tb.KeyPress    += (s, e) => OnCellKeyPress(row, col, e);
                    tb.TextChanged += (s, e) => OnCellChanged(row, col);

                    _cells[r, c] = tb;
                    Controls.Add(tb);
                }
            }

            // Малюємо жирні межі між блоками 3×3
            Paint += DrawGridBorders;
        }

        private void DrawGridBorders(object sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(31, 78, 121), 3);
            const int cellSize = 52;
            const int blockGap = 4;
            const int gl = 15, gt = 100;

            int totalW = 9 * cellSize + 2 * blockGap;
            int totalH = 9 * cellSize + 2 * blockGap;

            // Зовнішня рамка
            e.Graphics.DrawRectangle(pen, gl - 1, gt - 1, totalW, totalH);

            // Вертикальні та горизонтальні лінії між блоками
            for (int i = 1; i <= 2; i++)
            {
                int x = gl + i * 3 * cellSize + (i - 1) * blockGap + blockGap / 2;
                int y = gt + i * 3 * cellSize + (i - 1) * blockGap + blockGap / 2;
                e.Graphics.DrawLine(pen, x, gt - 1, x, gt + totalH);
                e.Graphics.DrawLine(pen, gl - 1, y, gl + totalW, y);
            }
        }

        // ─── Нова гра ─────────────────────────────────────────────────────
        private void StartNewGame()
        {
            var diff = comboBoxDifficulty.SelectedIndex == 0 ? Difficulty.Easy
                     : comboBoxDifficulty.SelectedIndex == 1 ? Difficulty.Medium
                     : Difficulty.Hard;

            _board.Generate(diff);
            _history.Clear();
            _timer.Reset();
            _timer.Start();
            _hintsLeft = 3;
            UpdateBoard();
            UpdateControls();
        }

        private void UpdateBoard()
        {
            _updating = true;
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    int val = _board.GetCell(r, c);
                    _cells[r, c].Text     = val == 0 ? "" : val.ToString();
                    _cells[r, c].ReadOnly = _board.IsInitialCell(r, c);
                }
            }
            _updating = false;
            RefreshColors();
        }

        private void RefreshColors()
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (_board.IsInitialCell(r, c))
                        _cells[r, c].BackColor = Color.FromArgb(214, 228, 240);
                    else if (_board.HasConflict(r, c))
                        _cells[r, c].BackColor = Color.FromArgb(255, 200, 200);
                    else if (_cells[r, c].Text != "")
                        _cells[r, c].BackColor = Color.FromArgb(230, 255, 230);
                    else
                        _cells[r, c].BackColor = Color.White;
                }
            }
        }

        private void UpdateControls()
        {
            labelTimer.Text  = "⏱ " + _timer.ToString();
            labelHints.Text  = $"Підказки: {_hintsLeft}";
            buttonHint.Enabled = _hintsLeft > 0;
            buttonUndo.Enabled = _history.CanUndo;
        }

        // ─── Обробники клітинок ───────────────────────────────────────────
        private void OnCellKeyPress(int row, int col, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar == '0') e.Handled = true;
        }

        private void OnCellChanged(int row, int col)
        {
            if (_updating) return;

            string txt = _cells[row, col].Text;
            int val = string.IsNullOrEmpty(txt) ? 0 : int.Parse(txt);

            _history.Push(_board.GetGridCopy(), row, col);
            _board.SetCell(row, col, val);

            RefreshColors();
            UpdateControls();

            if (_board.IsComplete())
            {
                _timer.Stop();
                var result = MessageBox.Show(
                    $"🎉 Вітаємо! Час: {_timer}\nЗаписати результат у таблицю рекордів?",
                    "Перемога!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    string name = Microsoft.VisualBasic.Interaction.InputBox(
                        "Введіть ваше ім'я:", "Таблиця рекордів", "Гравець");
                    if (!string.IsNullOrWhiteSpace(name))
                        _records.AddRecord(name, _timer.Elapsed, _board.Difficulty);
                }
            }
        }

        // ─── Підказка ─────────────────────────────────────────────────────
        private void buttonHint_Click(object sender, EventArgs e)
        {
            var p = _board.GetHint();
            if (p == null) return;

            _hintsLeft--;
            _updating = true;
            int val = _board.GetCell(p.Value.X, p.Value.Y);
            _cells[p.Value.X, p.Value.Y].Text = val.ToString();
            _updating = false;

            RefreshColors();
            UpdateControls();
        }

        // ─── Скасування ходу ──────────────────────────────────────────────
        private void buttonUndo_Click(object sender, EventArgs e)
        {
            if (!_history.CanUndo) return;
            var state = _history.Pop();

            _updating = true;
            for (int r = 0; r < 9; r++)
                for (int c = 0; c < 9; c++)
                {
                    _board.SetCell(r, c, state.Grid[r, c]);
                    if (!_board.IsInitialCell(r, c))
                        _cells[r, c].Text = state.Grid[r, c] == 0
                            ? "" : state.Grid[r, c].ToString();
                }
            _updating = false;

            RefreshColors();
            UpdateControls();
        }

        // ─── Нова гра (кнопка) ────────────────────────────────────────────
        private void buttonNewGame_Click(object sender, EventArgs e) => StartNewGame();

        // ─── Меню File ────────────────────────────────────────────────────
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog
            {
                Filter   = "Судоку файл (*.sdk)|*.sdk",
                FileName = "game.sdk"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
                _board.SaveToFile(dlg.FileName);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog { Filter = "Судоку файл (*.sdk)|*.sdk" };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                _board = SudokuBoard.LoadFromFile(dlg.FileName);
                _history.Clear();
                _timer.Reset();
                _timer.Start();
                _hintsLeft = 3;
                UpdateBoard();
                UpdateControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка відкриття файлу: " + ex.Message,
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        // ─── Меню Records ─────────────────────────────────────────────────
        private void recordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var form = new RecordsForm(_records);
            form.ShowDialog(this);
        }

        // ─── Таймер (оновлення мітки) ─────────────────────────────────────
        private void OnTimerTick()
        {
            if (InvokeRequired) Invoke((Action)OnTimerTick);
            else labelTimer.Text = "⏱ " + _timer.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _timer.Tick += OnTimerTick;
            comboBoxDifficulty.SelectedIndex = 0;
        }
    }
}
