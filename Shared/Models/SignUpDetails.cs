namespace Shared.Models;

public sealed record SignUpDetails
{
    public string EmailAddress { get; set; } = default!;

    public string LinkedInLink { get; set; } = default!;

    public Stream CvAttachment { get; set; } = default!;
}