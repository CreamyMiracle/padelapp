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

        public async Task<Session?> GetGame(string gameId)
        {
            try
            {
                return await db_con.GetWithChildrenAsync<Session>(gameId);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<Session?> AddGame(Session game)
        {
            await db_con.InsertOrReplaceWithChildrenAsync(game);
            return game;
        }

        public async Task<Session?> UpdateGame(Session game)
        {
            await db_con.UpdateWithChildrenAsync(game);
            return game;
        }
    }
}
