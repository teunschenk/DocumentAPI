using DocumentAPI.Application.Documents;
using DocumentAPI.Domain.Documents;

namespace DocumentAPI.Application.Handlers;

public sealed class ApproveDocumentHandler(
    IDocumentRepository documentRepository,
    TimeProvider timeProvider)
{
    public async Task<bool> HandleAsync(
        ApproveDocumentCommand command,
        CancellationToken cancellationToken)
    {
        var document = await documentRepository.GetByIdAsync(command.DocumentId, cancellationToken);
        if (document is null)
        {
            return false;
        }

        document.AddContent(GetApprovalStamp());

        await documentRepository.SaveAsync(document, cancellationToken);
        return true;
    }

    private string GetApprovalStamp()
    {
        var stamp = $"DateTime: {timeProvider.GetUtcNow():O}";
        stamp += $"\n\nChecked by: {Environment.UserName}";
        stamp += $"\n\nStatus: Approved";
        return stamp;
    }
}
