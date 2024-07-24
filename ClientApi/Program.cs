using Refit;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.MapGet("/test", async (IWebHostEnvironment webHostEnvironment) =>
    {
        var authApi = RestService.For<IAuthApiClient>("https://localhost:7242");
        var signUpDetails = new SignUpDetails
        {
            EmailAddress = "example@example.com",
            LinkedInLink = "https://linkedin.com/in/example"
        };

        var filePath = "Resume v3.pdf";
        var fileAbsolutePath = Path.Combine(webHostEnvironment.WebRootPath, filePath);

        if (!File.Exists(fileAbsolutePath))
            return Results.NotFound("File not found.");

        await using var fileStream = new FileStream(fileAbsolutePath, FileMode.Open, FileAccess.Read);
        var fileAttachment = new StreamPart(fileStream, "cv.pdf");

        var response = await authApi.SignUp(signUpDetails, fileAttachment);

        return response.IsSuccessStatusCode
            ? Results.Ok("Sign-up successful!")
            : Results.Problem($"Sign-up failed: {response.Error.Content}");
    })
    .WithName("TestApi")
    .WithOpenApi();

app.Run();

public interface IAuthApiClient
{
    [Multipart]
    [Post("/api/auth/sign-up")]
    Task<ApiResponse<string>> SignUp([AliasAs("signUpDetails")] SignUpDetails signUpDetails, [AliasAs("cvAttachment")] StreamPart cvAttachment);
}