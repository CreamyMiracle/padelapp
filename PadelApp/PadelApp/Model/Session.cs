using Newtonsoft.Json;
using PadelApp.Model.Dto;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelApp.Model
{
    public class Session
    {
        [PrimaryKey]
        // Id of this game
        public string Id { get; set; } = null!;

        // player name as key, points per each set as value
        [TextBlob("GamesBlobbed")]
        public Dictionary<string, List<int>> Games { get; set; } = new Dictionary<string, List<int>>();

        public string GamesBlobbed { get; set; }

        public int LatestCombination { get; set; }
    }
}
