using PadelApp.Model;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace PadelApp.Data
{
    public class GameRepository
    {
        private readonly SQLiteAsyncConnection db_con;

        public GameRepository(SQLiteAsyncConnection con)
        {
            db_con = con;
        }

        public async Task<Game?> GetGame(string gameId)
        {
            try
            {
                return await db_con.GetAsync<Game>(gameId);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<Game?> AddGame(Game game)
        {
            await db_con.InsertWithChildrenAsync(game);
            return game;
        }

        public async Task<Game?> UpdateGame(Game game)
        {
            await db_con.UpdateWithChildrenAsync(game);
            return game;
        }
    }
}
