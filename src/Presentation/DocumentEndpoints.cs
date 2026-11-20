using DocumentAPI.Application.Documents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DocumentAPI.Presentation.Documents;

public static class DocumentEndpoints
{
    public static IEndpointRouteBuilder MapDocumentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/documents");

        group.MapPost("/", async (UploadDocumentRequest request, DocumentService documentService, CancellationToken cancellationToken) =>
        {
            try
            {
                var document = await documentService.UploadAsync(
                    new UploadDocumentCommand(request.Title, request.Content),
                    cancellationToken);

                return Results.Created($"/documents/{document.Id}", document);
            }
            catch (ArgumentException exception)
            {
                return Results.BadRequest(new { error = exception.Message });
            }
        });

        group.MapGet("/{id:guid}", async (Guid id, DocumentService documentService, CancellationToken cancellationToken) =>
        {
            var document = await documentService.GetByIdAsync(id, cancellationToken);
            return document is null ? Results.NotFound() : Results.Ok(document);
        });

        group.MapPut("/{id:guid}", async (Guid id, MutateDocumentRequest request, DocumentService documentService, CancellationToken cancellationToken) =>
        {
            try
            {
                var document = await documentService.MutateAsync(
                    id,
                    new MutateDocumentCommand(request.Content, request.Title),
                    cancellationToken);

                return document is null ? Results.NotFound() : Results.Ok(document);
            }
            catch (ArgumentException exception)
            {
                return Results.BadRequest(new { error = exception.Message });
            }
        });

        return endpoints;
    }
}
