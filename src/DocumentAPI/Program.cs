using DocumentAPI.Application.Documents;
using DocumentAPI.Domain.Documents;
using DocumentAPI.Infrastructure.Persistence;
using DocumentAPI.Presentation.Documents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IDocumentRepository, InMemoryDocumentRepository>();
builder.Services.AddScoped<DocumentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapDocumentEndpoints();

app.Run();
