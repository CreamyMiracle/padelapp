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
        Task<ApiResponse<SessionDto>> GetGame();

        [Get("/{gameId}")]
        Task<ApiResponse<SessionDto>> GetGame(string gameId);

        [Put("/")]
        Task<ApiResponse<SessionDto>> UpdateGame(SessionDto game);
    }
}