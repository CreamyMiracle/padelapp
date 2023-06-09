﻿using AutoMapper;
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
        private readonly GameRepository _gameRepository;
        private readonly IMapper _mapper;

        public GamesController(ILogger<GamesController> logger, GameRepository gameRepository, IMapper mapper)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<SessionDto>> GetGame()
        {
            string? gameId = ControllerContext.HttpContext.Request.Cookies["currentGameId"];

            Session? game = await _gameRepository.GetGame(gameId);

            if (game == null) { return NotFound(string.Format("Game with ID '{0}' not found", gameId)); }
            
            return _mapper.Map<SessionDto>(game);
        }

        [HttpGet("{gameId}")]
        public async Task<ActionResult<SessionDto>> GetGame(string gameId)
        {
            ControllerContext.HttpContext.Response.Cookies.Append("currentGameId", gameId);

            Session? game = await _gameRepository.GetGame(gameId);

            if (game == null) { return NotFound(string.Format("Game with ID '{0}' not found", gameId)); }

            return _mapper.Map<SessionDto>(game);
        }

        [HttpPut]
        public async Task<ActionResult<SessionDto>> UpdateGame(SessionDto dto)
        {
            Session? upsertedGame = null;
            if (await _gameRepository.GetGame(dto.Id) != null)
            {
                upsertedGame = await _gameRepository.UpdateGame(_mapper.Map<Session>(dto));
            }
            else
            {
                upsertedGame = await _gameRepository.AddGame(_mapper.Map<Session>(dto));
            }

            if (upsertedGame == null) { return StatusCode(500, string.Format("Something went wrong")); }

            ControllerContext.HttpContext.Response.Cookies.Append("currentGameId", upsertedGame.Id);
            return _mapper.Map<SessionDto>(upsertedGame);
        }
    }
}