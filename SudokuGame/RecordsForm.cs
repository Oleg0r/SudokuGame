using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SudokuGame
{
    public class RecordsForm : Form
    {
        private readonly RecordManager _records;
        private DataGridView _grid;
        private ComboBox _filterCombo;

        public RecordsForm(RecordManager records)
        {
            _records = records;
            BuildUI();
            LoadRecords();
        }

        private void BuildUI()
        {
            Text            = "Таблиця рекордів";
            Size            = new Size(520, 400);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            ShowInTaskbar   = false;
            StartPosition   = FormStartPosition.CenterParent;
            BackColor       = Color.White;

            var labelFilter = new Label
            {
                Text   = "Рівень складності:",
                Bounds = new Rectangle(10, 12, 130, 24),
                Font   = new Font("Arial", 10f)
            };
            Controls.Add(labelFilter);

            _filterCombo = new ComboBox
            {
                Bounds       = new Rectangle(145, 10, 130, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = new Font("Arial", 10f)
            };
            _filterCombo.Items.AddRange(new object[] { "Легкий", "Середній", "Складний" });
            _filterCombo.SelectedIndex = 0;
            _filterCombo.SelectedIndexChanged += (s, e) => LoadRecords();
            Controls.Add(_filterCombo);

            var btnClear = new Button
            {
                Text      = "Очистити",
                Bounds    = new Rectangle(390, 8, 100, 30),
                Font      = new Font("Arial", 10f),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClear.Click += (s, e) =>
            {
                var diff = (Difficulty)_filterCombo.SelectedIndex;
                _records.ClearRecords(diff);
                LoadRecords();
            };
            Controls.Add(btnClear);

            _grid = new DataGridView
            {
                Bounds             = new Rectangle(10, 50, 480, 280),
                ReadOnly           = true,
                AllowUserToAddRows = false,
                RowHeadersVisible  = false,
                SelectionMode      = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor    = Color.White,
                BorderStyle        = BorderStyle.None,
                Font               = new Font("Arial", 10f)
            };
            _grid.Columns.Add("place",  "Місце");
            _grid.Columns.Add("name",   "Гравець");
            _grid.Columns.Add("time",   "Час");
            _grid.Columns.Add("date",   "Дата");
            _grid.Columns["place"].Width = 60;
            Controls.Add(_grid);

            var btnClose = new Button
            {
                Text      = "Закрити",
                Bounds    = new Rectangle(190, 340, 120, 34),
                Font      = new Font("Arial", 11f),
                BackColor = Color.FromArgb(31, 78, 121),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += (s, e) => Close();
            Controls.Add(btnClose);
        }

        private void LoadRecords()
        {
            _grid.Rows.Clear();
            var diff    = (Difficulty)_filterCombo.SelectedIndex;
            var entries = _records.GetTopRecords(diff).ToList();

            for (int i = 0; i < entries.Count; i++)
            {
                var e = entries[i];
                _grid.Rows.Add(
                    $"#{i + 1}",
                    e.PlayerName,
                    $"{(int)e.Time.TotalMinutes:D2}:{e.Time.Seconds:D2}",
                    e.Date.ToString("dd.MM.yyyy HH:mm")
                );
            }
        }
    }
}
