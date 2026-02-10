var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<app.Config.IPathResolver, app.Config.PathResolver>();
builder.Services.AddSingleton<app.Config.IVtprojLocator>(_ => new app.Config.VtprojLocator(args));
builder.Services.AddSingleton<app.Config.IConfigLoader, app.Config.ConfigLoader>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
