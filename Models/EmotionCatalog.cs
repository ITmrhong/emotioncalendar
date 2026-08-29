namespace EmotionCalendarDiary.Models;

public static class EmotionCatalog
{
    public static readonly IReadOnlyList<EmotionType> All =
    [
        EmotionType.Joy,
        EmotionType.Sad,
        EmotionType.Angry,
        EmotionType.Depressed,
        EmotionType.Neutral,
        EmotionType.Excited,
        EmotionType.Anxious,
        EmotionType.Tired,
        EmotionType.Proud,
        EmotionType.Calm
    ];

    public static string Label(EmotionType type) => type switch
    {
        EmotionType.Joy => "기쁨",
        EmotionType.Sad => "슬픔",
        EmotionType.Angry => "화남",
        EmotionType.Depressed => "우울",
        EmotionType.Neutral => "보통",
        EmotionType.Excited => "설렘",
        EmotionType.Anxious => "불안",
        EmotionType.Tired => "피곤",
        EmotionType.Proud => "뿌듯",
        EmotionType.Calm => "평온",
        _ => type.ToString()
    };

    public static string Emoji(EmotionType type) => type switch
    {
        EmotionType.Joy => "😄",
        EmotionType.Sad => "😢",
        EmotionType.Angry => "😡",
        EmotionType.Depressed => "😞",
        EmotionType.Neutral => "😐",
        EmotionType.Excited => "😍",
        EmotionType.Anxious => "😰",
        EmotionType.Tired => "😴",
        EmotionType.Proud => "😊",
        EmotionType.Calm => "🙂",
        _ => "❓"
    };

    public static string? EmojiForKey(string? emotionKey)
    {
        if (string.IsNullOrEmpty(emotionKey))
            return null;

        return Enum.TryParse<EmotionType>(emotionKey, out var type) ? Emoji(type) : null;
    }
}
