var _builder = WebApplication.CreateBuilder(args);

ConfigurationManager c_configuration = _builder.Configuration;
var _appSettings = c_configuration.Get<AppSettings>(options => options.BindNonPublicProperties = true)!;


try
{
    _builder.Services.AddSingleton<IAppSettings>(_appSettings)
        .AddSingleton<Rabbit>();


    var _app = _builder.Build();

    var _listener = _app.Services.GetService<Rabbit>()!;

    _app.Lifetime.ApplicationStarted.Register(async () =>
    {
        await _listener.Register();
    });

    _app.Lifetime.ApplicationStopping.Register(() =>
    {
        //_listener.Deregister();
    });

    _app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

