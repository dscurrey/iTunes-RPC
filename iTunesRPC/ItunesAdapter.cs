using System.Runtime.InteropServices;
using iTunesLib;

namespace iTunesRPC;

internal class ItunesAdapter : IItunesAdapter, IDisposable
{
    private readonly iTunesApp _app;
    private readonly ILogger<ItunesAdapter> _logger;
    private NowPlaying? _nowPlaying;
        
    public event EventHandler? NowPlayingUpdated;

    public ItunesAdapter(ILogger<ItunesAdapter> logger)
    {
        _logger = logger;
        _app = new iTunesApp();

        _app.OnPlayerPlayEvent += OnPlayerStart;
        _app.OnQuittingEvent += OnUserQuitting;

    }

    public NowPlaying? GetNowPlaying()
    {
        return _nowPlaying;
    }

    private void OnPlayerStart(object track)
    {
        _logger.LogDebug("iTunes started playing");
        var theTrack = track as IITFileOrCDTrack;
        SetNowPlaying(theTrack);
    }

    private void SetNowPlaying(IITFileOrCDTrack? theTrack)
    {
        if (theTrack == null)
        {
            _nowPlaying = null;
        }
        else
        {
            var track = new NowPlaying(theTrack.Artist, theTrack.Name, theTrack.Album, theTrack.AlbumArtist);
            _nowPlaying = track;
        }

        NowPlayingUpdated?.Invoke(this, EventArgs.Empty);
    }

    private void OnUserQuitting()
    {
        _logger.LogDebug("Quitting");
        Dispose();
    }

    private void ReleaseUnmanagedResources()
    {
#pragma warning disable CA1416
        Marshal.ReleaseComObject(_app);
#pragma warning restore CA1416
    }

    public void Dispose()
    {
        _app.OnPlayerPlayEvent -= OnPlayerStart;
            
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~ItunesAdapter()
    {
        ReleaseUnmanagedResources();
    }
}
