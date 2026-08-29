namespace EmotionCalendarDiary.Services;

public class MediaPickerPhotoService : IPhotoService
{
    public bool IsCaptureSupported => MediaPicker.Default.IsCaptureSupported;

    public Task<string?> PickPhotoAsync() => CopyToAppDataAsync(MediaPicker.Default.PickPhotoAsync());

    public Task<string?> CapturePhotoAsync() => CopyToAppDataAsync(MediaPicker.Default.CapturePhotoAsync());

    private static async Task<string?> CopyToAppDataAsync(Task<FileResult?> pickTask)
    {
        var result = await pickTask;
        if (result is null)
            return null;

        var photosDir = Path.Combine(FileSystem.AppDataDirectory, "Photos");
        Directory.CreateDirectory(photosDir);

        var destPath = Path.Combine(photosDir, $"{Guid.NewGuid()}{Path.GetExtension(result.FileName)}");

        using var sourceStream = await result.OpenReadAsync();
        using var destStream = File.Create(destPath);
        await sourceStream.CopyToAsync(destStream);

        return destPath;
    }
}
