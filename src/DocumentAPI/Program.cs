using DocumentAPI.Application.Documents;
using DocumentAPI.Application.Handlers;
using DocumentAPI.Domain.Documents;
using DocumentAPI.Infrastructure.Persistence;
using DocumentAPI.Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IDocumentRepository, InMemoryDocumentRepository>();
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);
builder.Services.AddScoped<MutateDocumentHandler>();
builder.Services.AddScoped<ValidateDocumentHandler>();
builder.Services.AddScoped<GetDocumentByIdQueryHandler>();
builder.Services.AddScoped<DocumentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "Document API v1"));
}

app.UseHttpsRedirection();
app.MapDocumentEndpoints();

app.Run();
