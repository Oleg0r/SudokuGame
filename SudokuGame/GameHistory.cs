using System.Collections.Generic;
using System.Drawing;

namespace SudokuGame
{
    public class BoardState
    {
        public int[,] Grid { get; set; }
        public int Row    { get; set; }
        public int Col    { get; set; }
    }

    public class GameHistory
    {
        private readonly Stack<BoardState> _history = new Stack<BoardState>();

        public bool CanUndo => _history.Count > 0;

        public void Push(int[,] grid, int row, int col)
        {
            _history.Push(new BoardState
            {
                Grid = (int[,])grid.Clone(),
                Row  = row,
                Col  = col
            });
        }

        public BoardState Pop() => _history.Pop();

        public void Clear() => _history.Clear();
    }
}
