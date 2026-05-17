using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SudokuGame
{
    public enum Difficulty { Easy, Medium, Hard }

    public class SudokuBoard
    {
        private int[,] _grid = new int[9, 9];
        private int[,] _solution = new int[9, 9];
        private HashSet<Point> _initialCells = new HashSet<Point>();
        private Random _rng = new Random();

        public Difficulty Difficulty { get; private set; }

        // ─── Генерація ────────────────────────────────────────────────────

        public void Generate(Difficulty difficulty)
        {
            Difficulty = difficulty;
            _grid = new int[9, 9];
            _initialCells.Clear();

            Solve(_grid);
            Array.Copy(_grid, _solution, _grid.Length);

            int toRemove = difficulty == Difficulty.Easy   ? 41
                         : difficulty == Difficulty.Medium ? 51 : 62;
            RemoveCells(toRemove);

            for (int r = 0; r < 9; r++)
                for (int c = 0; c < 9; c++)
                    if (_grid[r, c] != 0)
                        _initialCells.Add(new Point(r, c));
        }

        private bool Solve(int[,] g)
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (g[r, c] == 0)
                    {
                        var nums = Enumerable.Range(1, 9).OrderBy(_ => _rng.Next()).ToList();
                        foreach (int num in nums)
                        {
                            if (IsValidPlacement(g, r, c, num))
                            {
                                g[r, c] = num;
                                if (Solve(g)) return true;
                                g[r, c] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        private void RemoveCells(int count)
        {
            var positions = Enumerable.Range(0, 81)
                .OrderBy(_ => _rng.Next())
                .Take(count)
                .ToList();

            foreach (int pos in positions)
                _grid[pos / 9, pos % 9] = 0;
        }

        // ─── Перевірка ────────────────────────────────────────────────────

        private bool IsValidPlacement(int[,] g, int row, int col, int val)
        {
            for (int i = 0; i < 9; i++)
                if (g[row, i] == val || g[i, col] == val) return false;

            int sr = (row / 3) * 3, sc = (col / 3) * 3;
            for (int r = sr; r < sr + 3; r++)
                for (int c = sc; c < sc + 3; c++)
                    if (g[r, c] == val) return false;

            return true;
        }

        public bool HasConflict(int row, int col)
        {
            int val = _grid[row, col];
            if (val == 0) return false;

            for (int i = 0; i < 9; i++)
            {
                if (i != col && _grid[row, i] == val) return true;
                if (i != row && _grid[i, col] == val) return true;
            }

            int sr = (row / 3) * 3, sc = (col / 3) * 3;
            for (int r = sr; r < sr + 3; r++)
                for (int c = sc; c < sc + 3; c++)
                    if ((r != row || c != col) && _grid[r, c] == val) return true;

            return false;
        }

        public bool IsComplete()
        {
            for (int r = 0; r < 9; r++)
                for (int c = 0; c < 9; c++)
                    if (_grid[r, c] == 0 || HasConflict(r, c)) return false;
            return true;
        }

        // ─── Доступ до даних ──────────────────────────────────────────────

        public bool IsInitialCell(int row, int col) =>
            _initialCells.Contains(new Point(row, col));

        public int GetCell(int row, int col) => _grid[row, col];

        public int[,] GetGridCopy() => (int[,])_grid.Clone();

        public bool SetCell(int row, int col, int val)
        {
            if (IsInitialCell(row, col)) return false;
            _grid[row, col] = val;
            return true;
        }

        // ─── Підказка ─────────────────────────────────────────────────────

        public Point? GetHint()
        {
            var empty = new List<Point>();
            for (int r = 0; r < 9; r++)
                for (int c = 0; c < 9; c++)
                    if (_grid[r, c] == 0) empty.Add(new Point(r, c));

            if (empty.Count == 0) return null;

            var p = empty[_rng.Next(empty.Count)];
            _grid[p.X, p.Y] = _solution[p.X, p.Y];
            return p;
        }

        // ─── Збереження / Завантаження ────────────────────────────────────

        public void SaveToFile(string path)
        {
            using var sw = new StreamWriter(path);
            sw.WriteLine((int)Difficulty);

            for (int r = 0; r < 9; r++)
                sw.WriteLine(string.Join(",", Enumerable.Range(0, 9).Select(c => _grid[r, c])));

            for (int r = 0; r < 9; r++)
                sw.WriteLine(string.Join(",", Enumerable.Range(0, 9).Select(c => _solution[r, c])));

            foreach (var p in _initialCells)
                sw.WriteLine($"{p.X};{p.Y}");
        }

        public static SudokuBoard LoadFromFile(string path)
        {
            var board = new SudokuBoard();
            var lines = File.ReadAllLines(path);

            if (lines.Length < 19)
                throw new FormatException("Некоректний формат файлу збереження.");

            board.Difficulty = (Difficulty)int.Parse(lines[0]);

            for (int r = 0; r < 9; r++)
            {
                var parts = lines[1 + r].Split(',');
                if (parts.Length != 9) throw new FormatException("Некоректний рядок сітки.");
                for (int c = 0; c < 9; c++)
                    board._grid[r, c] = int.Parse(parts[c]);
            }

            for (int r = 0; r < 9; r++)
            {
                var parts = lines[10 + r].Split(',');
                for (int c = 0; c < 9; c++)
                    board._solution[r, c] = int.Parse(parts[c]);
            }

            for (int i = 19; i < lines.Length; i++)
            {
                var coords = lines[i].Split(';');
                board._initialCells.Add(new Point(int.Parse(coords[0]), int.Parse(coords[1])));
            }

            return board;
        }
    }
}
