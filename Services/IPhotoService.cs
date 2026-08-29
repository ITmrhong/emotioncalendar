namespace EmotionCalendarDiary.Services;

public interface IPhotoService
{
    bool IsCaptureSupported { get; }
    Task<string?> PickPhotoAsync();
    Task<string?> CapturePhotoAsync();
}
