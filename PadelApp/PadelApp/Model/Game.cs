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
        public Game()
        {

        }

        public Game(GameDto dto)
        {
            Id = dto.Id;
            if (dto.PlayerPoints != null) 
            { 
                this.PlayerPointsSerialized = JsonConvert.SerializeObject(dto.PlayerPoints); 
            }
        }

        [PrimaryKey]
        public string Id { get; set; } = null!;

        [Ignore]
        public Dictionary<string, int> PlayerPoints { get; set; } = new Dictionary<string, int>();
    
        public string PlayerPointsSerialized { get; set; }
    }
}
