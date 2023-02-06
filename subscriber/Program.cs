var _builder = WebApplication.CreateBuilder(args);

ConfigurationManager c_configuration = _builder.Configuration;
var _appSettings = c_configuration.Get<AppSettings>(options => options.BindNonPublicProperties = true)!;

// _builder.Logging.ClearProviders();
// var _logger = new LoggerConfiguration()
//     .ReadFrom.Configuration(_builder.Configuration)
//     .CreateLogger();

try
{
    // _logger.Information("About to start the application");

    // _builder.Host.UseSerilog((ctx, lc) => lc
    //     .MinimumLevel.Information()
    //     .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    //     .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    //     .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    //     .Filter.ByExcluding(Matching.WithProperty("RequestPath", "/healthz"))
    //     .ReadFrom.Configuration(_builder.Configuration));

    _builder.Services.AddSingleton<IAppSettings>(_appSettings)
        .AddSingleton<Rabbit>();


    var _app = _builder.Build();

    var _listener = _app.Services.GetService<Rabbit>()!;

    _app.Lifetime.ApplicationStarted.Register(() =>
    {
        _listener.Register();
    });

    _app.Lifetime.ApplicationStopping.Register(() =>
    {
        _listener.Deregister();
    });

    _app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
//    Log.Error(ex.ToString());
}

