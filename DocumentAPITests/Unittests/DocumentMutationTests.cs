using DocumentAPI.Application.Documents;
using DocumentAPI.Application.Handlers;
using DocumentAPI.Domain.Documents;
using FluentAssertions;

namespace DocumentAPI.Tests.Unittests;

public class DocumentMutationTests
{
    [Fact]
    public async Task HandleAsync_WithExistingDocument_ShouldMutateAndSaveDocument()
    {
        // Arrange
        var document = Document.Create("Original content.");
        var repository = new TestDocumentRepository(document);
        var approvalTime = new DateTimeOffset(2025, 1, 2, 3, 4, 5, TimeSpan.Zero);
        var handler = new MutateDocumentHandler(repository, new FixedTimeProvider(approvalTime));

        // Act
        var result = await handler.HandleAsync(
            new MutateDocumentCommand(document.Id),
            CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        document.Content.Should().Contain("Original content.");
        document.Content.Should().Contain($"DateTime: {approvalTime:O}");
        document.Content.Should().Contain("Random Data");
        repository.SavedDocument.Should().BeSameAs(document);
        repository.SaveCount.Should().Be(1);
    }

    [Fact]
    public async Task HandleAsync_WithUnknownDocument_ShouldReturnFalseWithoutSaving()
    {
        // Arrange
        var repository = new TestDocumentRepository();
        var handler = new MutateDocumentHandler(repository, new FixedTimeProvider(DateTimeOffset.UtcNow));

        // Act  
        var result = await handler.HandleAsync(
            new MutateDocumentCommand(Guid.NewGuid()),
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        repository.SavedDocument.Should().BeNull();
        repository.SaveCount.Should().Be(0);
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }

    private sealed class TestDocumentRepository(Document? document = null) : IDocumentRepository
    {
        private readonly Dictionary<Guid, Document> _documents = document is null
            ? []
            : new Dictionary<Guid, Document> { [document.Id] = document };

        public Document? SavedDocument { get; private set; }
        public int SaveCount { get; private set; }

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
            SavedDocument = document;
            SaveCount++;
            return Task.CompletedTask;
        }
    }
}
