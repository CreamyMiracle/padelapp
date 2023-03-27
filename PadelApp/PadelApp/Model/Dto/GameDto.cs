using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelApp.Model.Dto
{
    public class GameDto
    {
        // Id of this game
        public string Id { get; set; } = null!;

        // player name as key, points per each set as value
        public Dictionary<string, List<int>> Sets { get; set; } = new Dictionary<string, List<int>>();
    }
}
