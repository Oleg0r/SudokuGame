using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SudokuGame
{
    public class RecordEntry
    {
        public string     PlayerName { get; set; }
        public TimeSpan   Time       { get; set; }
        public DateTime   Date       { get; set; }
        public Difficulty Difficulty { get; set; }
    }

    public class RecordManager
    {
        private readonly string _path;
        private List<RecordEntry> _records;

        public RecordManager(string path = "records.json")
        {
            _path = path;
            Load();
        }

        public void AddRecord(string playerName, TimeSpan time, Difficulty difficulty)
        {
            _records.Add(new RecordEntry
            {
                PlayerName = playerName,
                Time       = time,
                Date       = DateTime.Now,
                Difficulty = difficulty
            });
            Save();
        }

        public IEnumerable<RecordEntry> GetTopRecords(Difficulty difficulty, int count = 10) =>
            _records
                .Where(r => r.Difficulty == difficulty)
                .OrderBy(r => r.Time)
                .Take(count);

        public void ClearRecords(Difficulty difficulty)
        {
            _records.RemoveAll(r => r.Difficulty == difficulty);
            Save();
        }

        private void Load()
        {
            if (!File.Exists(_path)) { _records = new List<RecordEntry>(); return; }
            try
            {
                var json = File.ReadAllText(_path);
                _records = JsonSerializer.Deserialize<List<RecordEntry>>(json)
                           ?? new List<RecordEntry>();
            }
            catch { _records = new List<RecordEntry>(); }
        }

        private void Save()
        {
            var json = JsonSerializer.Serialize(_records, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_path, json);
        }
    }
}
