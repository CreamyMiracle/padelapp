using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PadelApp.Data;
using PadelApp.Model;
using PadelApp.Model.Dto;

namespace PadelApp.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private GameRepository _gameRepository;

        public GamesController(ILogger<GamesController> logger, GameRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }

        [HttpGet]
        public async Task<ActionResult<GameDto>> GetGame()
        {
            string? gameId = ControllerContext.HttpContext.Request.Cookies["currentGameId"];

            Game? game = await _gameRepository.GetGame(gameId);

            if (game == null) { return NotFound(string.Format("Game with ID '{0}' not found", gameId)); }
            
            return new GameDto(game);
        }

        [HttpGet("{gameId}")]
        public async Task<ActionResult<GameDto>> GetGame(string gameId)
        {
            Game? game = await _gameRepository.GetGame(gameId);

            if (game == null) { return NotFound(string.Format("Game with ID '{0}' not found", gameId)); }

            ControllerContext.HttpContext.Response.Cookies.Append("currentGameId", gameId);
            return new GameDto(game);
        }

        [HttpPut]
        public async Task<ActionResult<GameDto>> UpdateGame(GameDto dto)
        {
            Game? upsertedGame = null;
            if (await _gameRepository.GetGame(dto.Id) != null)
            {
                upsertedGame = await _gameRepository.UpdateGame(new Game(dto));
            }
            else
            {
                upsertedGame = await _gameRepository.AddGame(new Game(dto));
            }

            if (upsertedGame == null) { return StatusCode(500, string.Format("Something went wrong")); }

            ControllerContext.HttpContext.Response.Cookies.Append("currentGameId", upsertedGame.Id);
            return new GameDto(upsertedGame);
        }
    }
}