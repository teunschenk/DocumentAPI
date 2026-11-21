using DocumentAPI.Application.Commands;
using DocumentAPI.Application.Documents;
using DocumentAPI.Application.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DocumentAPI.Presentation.Endpoints;

public static class DocumentEndpoints
{
    public static IEndpointRouteBuilder MapDocumentEndpoints(this IEndpointRouteBuilder endpoints)
    {        
        endpoints.MapPost("/Document/Upload", UploadAsync)
            .DisableAntiforgery();
        endpoints.MapGet("/Document/{id:guid}", GetAsync);
        return endpoints;
    }

    private static async Task<IResult> GetAsync(
        Guid id,
        DocumentService documentService,
        CancellationToken cancellationToken)
    {
         var document = await documentService.GetByIdAsync(id, cancellationToken);
         return document is null
             ? Results.NotFound()
             : Results.File(
                 System.Text.Encoding.UTF8.GetBytes(document.Content),
                 "text/plain; charset=utf-8",
                 $"{document.Id}.txt");
    }

    private static async Task<IResult> UploadAsync(
        IFormFile file,
        DocumentService documentService,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync(cancellationToken);
            var document = await documentService.UploadAsync(
                new UploadDocumentCommand(content),
                cancellationToken);

            return Results.Created($"/Document/{document.Id}", document);
        }
        catch (Exception exception)
        {
            return Results.BadRequest(new { error = exception.Message });
        }
    }
}
