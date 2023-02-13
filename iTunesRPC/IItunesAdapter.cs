namespace iTunesRPC;

public interface IItunesAdapter
{
    public event EventHandler NowPlayingUpdated;
    public NowPlaying? GetNowPlaying();
}
