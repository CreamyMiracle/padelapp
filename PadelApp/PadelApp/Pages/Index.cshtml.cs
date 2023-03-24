using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PadelApp.Controller;
using PadelApp.Model;
using PadelApp.Model.Dto;
using PaldeApp;
using System.Collections.Generic;
using System.Linq;

public class IndexModel : PageModel
{
    private readonly IGameAPI _gameAPI;
    public IndexModel(IGameAPI gameAPI)
    {
        _gameAPI = gameAPI;
    }

    [BindProperty]
    public string OpenGameName { get; set; }

    public List<TableRow> Rows { get; set; } = new List<TableRow>();

    public GameDto? Game { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await GetGame();
        return Page();
    }

    public async Task<IActionResult> OnGetOpenGame(string gameId)
    {
        await GetGame(gameId);
        if (Game == null)
        {
            await NewGame(gameId);
        }

        return Page();
    }

    public async Task<IActionResult> OnGetAddRow(string name)
    {
        await GetGame();
        if (Game == null)
        {
            await NewGame(DateTime.UtcNow.ToString("dd-MM-yyyy"));
        }

        Game?.PlayerPoints.TryAdd(name, 0);

        Game = (await _gameAPI.UpdateGame(Game)).Content;

        UpdateRows();

        return Page();
    }

    public async Task<IActionResult> OnGetDeleteRow(string name)
    {
        await GetGame();

        Game?.PlayerPoints.Remove(name);

        Game = (await _gameAPI.UpdateGame(Game)).Content;

        UpdateRows();
        return Page();
    }

    public async Task<IActionResult> OnGetUpdateRow(string name, int points)
    {
        await GetGame();

        var row = Rows.FirstOrDefault(r => r.Name == name);
        if (row != null)
        {
            row.Points = points;
        }

        await UpdateGame();

        return Page();
    }

    public async Task<IActionResult> OnGetIncrementPoints(string name)
    {
        await GetGame();

        var row = Rows.FirstOrDefault(r => r.Name == name);
        if (row != null)
        {
            row.Points++;
        }

        await UpdateGame();

        return Page();
    }

    public async Task<IActionResult> OnGetDecrementPoints(string name)
    {
        await GetGame();

        var row = Rows.FirstOrDefault(r => r.Name == name);
        if (row != null)
        {
            row.Points--;
        }

        await UpdateGame();

        return Page();
    }

    private async Task NewGame(string gameId)
    {
        Game = new GameDto() { Id = gameId };
        Game = (await _gameAPI.UpdateGame(Game)).Content;
    }

    private async Task UpdateGame()
    {
        Game?.PlayerPoints.Clear();
        foreach (TableRow item in Rows)
        {
            Game?.PlayerPoints.TryAdd(item.Name, item.Points);
        }
        Game = (await _gameAPI.UpdateGame(Game)).Content;
    }

    private async Task GetGame()
    {
        Game = (await _gameAPI.GetGame()).Content;
        UpdateRows();
    }

    private async Task GetGame(string gameId)
    {
        Game = (await _gameAPI.GetGame(gameId)).Content;
        UpdateRows();
    }

    private void UpdateRows()
    {
        Rows.Clear();
        if (Game != null)
        {
            foreach (KeyValuePair<string, int> pair in Game?.PlayerPoints)
            {
                Rows.Add(new TableRow { Name = pair.Key, Points = pair.Value });
            }
        }
    }
}

public class TableRow
{
    public string Name { get; set; }
    public int Points { get; set; }
}