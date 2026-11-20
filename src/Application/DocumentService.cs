using DocumentAPI.Domain.Documents;

namespace DocumentAPI.Application.Documents;

public sealed class DocumentService
{
    private readonly IDocumentRepository _documentRepository;

    public DocumentService(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<DocumentDto> UploadAsync(UploadDocumentCommand command, CancellationToken cancellationToken)
    {
        var document = Document.Create(command.Title, command.Content);
        await _documentRepository.AddAsync(document, cancellationToken);
        return Map(document);
    }

    public async Task<DocumentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.GetByIdAsync(id, cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<DocumentDto?> MutateAsync(Guid id, MutateDocumentCommand command, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.GetByIdAsync(id, cancellationToken);
        if (document is null)
        {
            return null;
        }

        document.Mutate(command.Content, command.Title);
        await _documentRepository.SaveAsync(document, cancellationToken);
        return Map(document);
    }

    private static DocumentDto Map(Document document)
    {
        return new DocumentDto(
            document.Id,
            document.Title,
            document.Content,
            document.CreatedAtUtc,
            document.UpdatedAtUtc);
    }
}
