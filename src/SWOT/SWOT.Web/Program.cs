var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Content("S.W.O.T bootstrap shell", "text/plain"));

app.Run();
