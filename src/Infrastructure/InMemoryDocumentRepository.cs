using System.Collections.Concurrent;
using DocumentAPI.Domain.Documents;

namespace DocumentAPI.Infrastructure.Persistence;

public sealed class InMemoryDocumentRepository : IDocumentRepository
{
    private readonly ConcurrentDictionary<Guid, Document> _documents = new();

    public Task AddAsync(Document document, CancellationToken cancellationToken)
    {
        _documents[document.Id] = document;
        return Task.CompletedTask;
    }

    public Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _documents.TryGetValue(id, out var document);
        return Task.FromResult(document);
    }

    public Task SaveAsync(Document document, CancellationToken cancellationToken)
    {
        _documents[document.Id] = document;
        return Task.CompletedTask;
    }
}
