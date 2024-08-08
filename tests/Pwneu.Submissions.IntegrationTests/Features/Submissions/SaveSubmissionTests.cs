using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pwneu.Shared.Common;
using Pwneu.Submissions.Features.Submissions;

namespace Pwneu.Submissions.IntegrationTests.Features.Submissions;

[Collection(nameof(IntegrationTestCollection))]
public class SaveSubmissionTests(IntegrationTestsWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_Should_SaveSubmission()
    {
        // Arrange
        var saveSubmission = new SaveSubmission.Command(
            UserId: Guid.NewGuid().ToString(),
            ChallengeId: Guid.NewGuid(),
            Flag: F.Lorem.Word(),
            SubmittedAt: DateTime.UtcNow,
            IsCorrect: false);

        // Act
        var saveSubmissionResult = await Sender.Send(saveSubmission);
        var submissionId = await DbContext
            .Submissions
            .Where(s => s.Id == saveSubmissionResult.Value)
            .Select(s => s.Id)
            .FirstOrDefaultAsync();

        // Assert
        saveSubmissionResult.Should().BeOfType<Result<Guid>>();
        saveSubmissionResult.IsSuccess.Should().BeTrue();
        saveSubmissionResult.Value.Should().Be(submissionId);
    }
}