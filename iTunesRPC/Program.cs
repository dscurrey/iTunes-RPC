using iTunesRPC;

var discordConfig = new DiscordConfig
{
    ClientId = "1069019045447348234"
};

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        services =>
        {
            services.AddSingleton(discordConfig);
            services.AddSingleton<IDiscordAdapter, DiscordAdapter>();
            services.AddSingleton<IItunesAdapter, ItunesAdapter>();
            services.AddHostedService<Worker>();
        })
    .Build();

host.Run();
