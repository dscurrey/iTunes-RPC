namespace iTunesRPC;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IDiscordAdapter _discord;
    private readonly IItunesAdapter _itunes;

    public Worker(ILogger<Worker> logger, IDiscordAdapter discord, IItunesAdapter itunes)
    {
        _logger = logger;
        _discord = discord;
        _itunes = itunes;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Started working");
        _itunes.NowPlayingUpdated += HandleNowPlaying;
        while (!stoppingToken.IsCancellationRequested)
        {
        }

        return Task.CompletedTask;
    }

    private void HandleNowPlaying(object? sender, EventArgs e)
    {
        var track = _itunes.GetNowPlaying();
        _logger.LogInformation("Setting discord presence to {TrackName}", track?.Title ?? "NULL");
        if (track != null)
            _discord.SetPresence(track);
    }
}
