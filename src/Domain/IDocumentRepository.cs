namespace DocumentAPI.Domain.Documents;

public interface IDocumentRepository
{
    Task AddAsync(Document document, CancellationToken cancellationToken);

    Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task SaveAsync(Document document, CancellationToken cancellationToken);
}
