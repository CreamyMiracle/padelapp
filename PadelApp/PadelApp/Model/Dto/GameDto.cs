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
        public GameDto()
        {

        }

        public GameDto(Game game)
        {
            if (game == null)
            {
                return;
            }

            Id = game.Id;
            if (game.PlayerPointsSerialized != null) 
            { 
                this.PlayerPoints = JsonConvert.DeserializeObject<Dictionary<string, int>>(game.PlayerPointsSerialized);
            }
        }

        public string Id { get; set; } = null!;

        public Dictionary<string, int> PlayerPoints { get; set; } = new Dictionary<string, int>();
    }
}
