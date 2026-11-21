using DocumentAPI.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentAPI.Tests.Unittests
{
    public class InMemoryDocumentRepositoryTests
    {
        [Fact]
        public async Task AddAsync_WithDocument_ShouldSucceed()
        {
            // Arrange
            var logger = NullLogger<InMemoryDocumentRepository>.Instance;
            var repository = new InMemoryDocumentRepository(logger);
            var document = DocumentAPI.Domain.Documents.Document.Create("This is a test document.");

            // Act
            await repository.AddAsync(document, CancellationToken.None);
            var retrievedDocument = await repository.GetByIdAsync(document.Id, CancellationToken.None);

            // Assert
            retrievedDocument.Should().BeEquivalentTo(document);
        }

        [Fact]
        public async Task AddAsync_WithoutDocument_ShouldFail()
        {
            // Arrange
            var logger = NullLogger<InMemoryDocumentRepository>.Instance;
            var repository = new InMemoryDocumentRepository(logger);

            // Act & Assert            
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await repository.AddAsync(null!, CancellationToken.None);
            });
        }

        [Fact]
        public async Task GetByIdAsync_WithUnknownId_ShouldReturnNull()
        {
            var logger = NullLogger<InMemoryDocumentRepository>.Instance;
            var repository = new InMemoryDocumentRepository(logger);

            var document = await repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

            document.Should().BeNull();
        }
    }
}
