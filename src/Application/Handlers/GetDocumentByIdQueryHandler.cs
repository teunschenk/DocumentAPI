using DocumentAPI.Application.Models;
using DocumentAPI.Application.Queries;
using DocumentAPI.Domain.Documents;

namespace DocumentAPI.Application.Handlers;

/// <summary>
/// Retieve the document from the in memory storage.
/// </summary>
/// <param name="documentRepository"></param>
public sealed class GetDocumentByIdQueryHandler(IDocumentRepository documentRepository)
{
    public async Task<DocumentDto?> HandleAsync(
        GetDocumentByIdQuery query,
        CancellationToken cancellationToken)
    {
        var document = await documentRepository.GetByIdAsync(query.DocumentId, cancellationToken);
        return document is null
            ? null
            : new DocumentDto(
                document.Id,
                document.Content,
                document.CreatedAtUtc,
                document.UpdatedAtUtc);
    }
}
