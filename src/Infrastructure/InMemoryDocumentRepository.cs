using System.Collections.Concurrent;
using DocumentAPI.Domain.Documents;
using Microsoft.Extensions.Logging;

namespace DocumentAPI.Infrastructure.Persistence;

public sealed class InMemoryDocumentRepository : IDocumentRepository
{
    private readonly ILogger<InMemoryDocumentRepository> _logger;
    private readonly ConcurrentDictionary<Guid, Document> _documents = new();

    public InMemoryDocumentRepository(ILogger<InMemoryDocumentRepository> logger)
    {
        _logger = logger;
    }

    public Task AddAsync(Document document, CancellationToken cancellationToken)
    {
        if (document is null || string.IsNullOrWhiteSpace(document.Content))
        {
            throw new ArgumentNullException(nameof(document));
        }

        _documents[document.Id] = document;
        _logger.LogInformation("Document {DocumentId} added", document.Id);
        return Task.CompletedTask;
    }

    public Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _documents.TryGetValue(id, out var document);

        if (document is null)
        {
            return Task.FromResult<Document?>(null);
        }

        _logger.LogInformation("Document {DocumentId} retrieved", document.Id);
        return Task.FromResult(document);
    }

    public Task SaveAsync(Document document, CancellationToken cancellationToken)
    {
        _documents[document.Id] = document;
        _logger.LogInformation("Document {DocumentId} updated", document.Id);
        return Task.CompletedTask;
    }
}
