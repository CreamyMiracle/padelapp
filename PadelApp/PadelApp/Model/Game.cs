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
    public class Game
    {
        [PrimaryKey]
        // Id of this game
        public string Id { get; set; } = null!;

        // player name as key, points per each set as value
        [TextBlob("SetsBlobbed")]
        public Dictionary<string, List<int>> Sets { get; set; } = new Dictionary<string, List<int>>();

        public string SetsBlobbed { get; set; }
    }
}
