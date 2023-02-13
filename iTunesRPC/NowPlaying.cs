namespace iTunesRPC;

public class NowPlaying
{
    public NowPlaying(string? artist, string title, string album, string? albumArtist)
    {
        if ((artist == null || string.IsNullOrWhiteSpace(artist)) && (albumArtist == null || string.IsNullOrWhiteSpace(albumArtist)))
            throw new ArgumentException("Artists both cannot be null");

        Artist = artist ?? albumArtist ?? string.Empty;
        Title = title;
        Album = album;
        AlbumArtist = albumArtist ?? artist ?? string.Empty;
    }

    private string Artist { get; }
    public string Title { get; }
    private string Album { get; }
    private string AlbumArtist { get; }

    public string GetFormattedTitle()
    {
        return ArtistsMatch ? $"{Title}" : $"{Title} - {Artist}";
    }

    public string GetFormattedDetail()
    {
        return ArtistsMatch ? $"Album: {Album}" : $"Album: {Album} - {AlbumArtist}";
    }

    private bool ArtistsMatch => string.Equals(Artist, AlbumArtist, StringComparison.CurrentCultureIgnoreCase);
}
