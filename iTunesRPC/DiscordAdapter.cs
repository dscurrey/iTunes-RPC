using System.Text;
using DiscordRPC;
using DiscordRPC.Logging;
using LogLevel = DiscordRPC.Logging.LogLevel;

namespace iTunesRPC;

public class DiscordAdapter : IDiscordAdapter
{
    private readonly ILogger<DiscordAdapter> _logger;
    private readonly DiscordConfig _config;
    private DiscordRpcClient? _client;

    public DiscordAdapter(ILogger<DiscordAdapter> logger, DiscordConfig config)
    {
        _logger = logger;
        _config = config;
        Initialise();
    }

    private void Initialise()
    {
        _client = new DiscordRpcClient(_config.ClientId);

        _client.Logger = new ConsoleLogger(LogLevel.Info);
        _client.OnReady += (_, e) =>
        {
            _logger.LogInformation("Received ready: {Username}", e.User.Username);
        };

        _client.OnPresenceUpdate += (_, e) =>
        {
            _logger.LogInformation("Received update: {Presence}", e.Presence.State);
        };

        _client.Initialize();
        
        _client.SetPresence(new RichPresence
        {
            Details = "RPC INIT",
            State = "INITIALISED",
            Assets = new Assets
            {
                LargeImageKey = "logo"
            }
        });
    }

    public void SetPresence(NowPlaying track)
    {
        if (_client == null)
            throw new InvalidOperationException();
        
        var presence = new RichPresence
        {
            Details = TruncateString(track.GetFormattedTitle()),
            State = TruncateString(track.GetFormattedDetail()),
            Assets = new Assets
            {
                LargeImageKey = "itunes_logo"
            }
        };
        
        _client.SetPresence(presence);
    }
    
    private static string TruncateString(string input)
    {
        var length = Encoding.Unicode.GetByteCount(input);
        if (length < 128) return input;

        input = input[..64];
        while (Encoding.Unicode.GetByteCount(input) > 127)
        {
            input = input[..^1];
        }

        return $"{input}...";
    }
}
