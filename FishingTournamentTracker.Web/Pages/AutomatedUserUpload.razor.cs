using Microsoft.AspNetCore.Components.Forms;

namespace FishingTournamentTracker.Web.Pages;

public partial class AutomatedUserUpload
{
    private string? CurrentFileUpload { get; set; }

    private bool UploadFileIsDisabled
    {
        get
        {
            return string.IsNullOrWhiteSpace(CurrentFileUpload);
        }
    }

    private async Task LoadFiles(InputFileChangeEventArgs args)
    {
        CurrentFileUpload = await ConvertFileToBase64(args.File);
    }

    private async Task UploadFile()
    {
        await userService.AutomatedUserUpload(CurrentFileUpload!);
        navigationManager.NavigateTo("/users");
    }

    private static async Task<string> ConvertFileToBase64(IBrowserFile file)
    {
        using var fileInput = file.OpenReadStream();
        var buffer = new byte[file.Size];
        await fileInput.ReadAsync(buffer.AsMemory(0, (int)file.Size));
        return Convert.ToBase64String(buffer);
    }
}
