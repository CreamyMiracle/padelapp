using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public string NewItemName { get; set; }

    [BindProperty]
    public string NewGameName { get; set; }

    [BindProperty]
    public string OpenGameName { get; set; }

    public List<TableRow> Rows { get; set; } = new List<TableRow>();

    public GameDto? Game { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await GetGame();
        return Page();
    }

    public async Task<IActionResult> OnPostOpenGame()
    {
        await GetGame(OpenGameName);
        return Page();
    }

    public async Task<IActionResult> OnPostNewGame()
    {
        await NewGame(NewGameName);
        await GetGame();
        return Page();
    }

    public async Task<IActionResult> OnPostAddRow()
    {   
        await GetGame();
        if (Game == null)
        {
            await NewGame(DateTime.UtcNow.ToString("dd-MM-yyyy"));
        }

        Game?.PlayerPoints.TryAdd(NewItemName, 0);

        var res = await _gameAPI.UpdateGame(Game);
        Game = res.Content;

        await GetGame();

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
        var res = await _gameAPI.UpdateGame(Game);
    }

    private async Task UpdateGame()
    {
        Game?.PlayerPoints.Clear();
        foreach (TableRow item in Rows)
        {
            Game?.PlayerPoints.TryAdd(item.Name, item.Points);
        }
        var res = await _gameAPI.UpdateGame(Game);
    }

    private async Task GetGame()
    {
        var res = await _gameAPI.GetGame();
        Game = res.Content;

        Rows.Clear();

        if (Game != null)
        {
            foreach (KeyValuePair<string, int> pair in Game?.PlayerPoints)
            {
                Rows.Add(new TableRow { Name = pair.Key, Points = pair.Value });
            }
        }
    }

    private async Task GetGame(string gameId)
    {
        var res = await _gameAPI.GetGame(gameId);
        Game = res.Content;

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