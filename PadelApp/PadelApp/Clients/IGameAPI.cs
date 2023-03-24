using PadelApp.Model.Dto;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaldeApp
{
    public interface IGameAPI
    {
        [Get("/")]
        Task<ApiResponse<GameDto>> GetGame();

        [Get("/{gameId}")]
        Task<ApiResponse<GameDto>> GetGame(string gameId);

        [Put("/")]
        Task<ApiResponse<GameDto>> UpdateGame(GameDto game);
    }
}