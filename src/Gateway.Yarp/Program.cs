var builder = WebApplication.CreateBuilder(args);

// Add YARP services and load config from the "ReverseProxy" section in appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Map the proxy routes
app.MapReverseProxy();

app.Run();
