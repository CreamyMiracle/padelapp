using PadelApp.Model;
using SQLite;

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
            return await db_con.InsertAsync(game) == 1 ? game : null;
        }

        public async Task<Game?> UpdateGame(Game game)
        {
            return await db_con.UpdateAsync(game) == 1 ? game : null;
        }
    }
}
