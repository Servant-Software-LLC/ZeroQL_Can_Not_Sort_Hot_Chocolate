using HotChocoServer;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices.AddServices(builder.Services);


var app = builder.Build();

app.MapGraphQL();

app.Run();
