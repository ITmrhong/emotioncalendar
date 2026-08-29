using EmotionCalendarDiary.Models;
using SQLite;

namespace EmotionCalendarDiary.Services;

public class SqliteDiaryRepository : IDiaryRepository
{
    private readonly SQLiteAsyncConnection _connection;
    private bool _initialized;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public SqliteDiaryRepository()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "emotioncalendar.db3");
        _connection = new SQLiteAsyncConnection(dbPath);
    }

    public async Task InitializeAsync()
    {
        if (_initialized)
            return;

        await _initLock.WaitAsync();
        try
        {
            if (_initialized)
                return;

            await _connection.CreateTableAsync<DiaryEntry>();
            _initialized = true;
        }
        finally
        {
            _initLock.Release();
        }
    }

    public async Task<IReadOnlyList<DiaryEntry>> GetForMonthAsync(int year, int month)
    {
        await InitializeAsync();

        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        return await _connection.Table<DiaryEntry>()
            .Where(e => e.Date >= start && e.Date < end)
            .ToListAsync();
    }

    public async Task<DiaryEntry?> GetByDateAsync(DateTime date)
    {
        await InitializeAsync();

        var day = date.Date;
        return await _connection.Table<DiaryEntry>()
            .Where(e => e.Date == day)
            .FirstOrDefaultAsync();
    }

    public async Task SaveAsync(DiaryEntry entry)
    {
        await InitializeAsync();

        entry.Date = entry.Date.Date;
        entry.UpdatedAt = DateTime.Now;

        if (entry.Id == 0)
        {
            entry.CreatedAt = DateTime.Now;
            await _connection.InsertAsync(entry);
        }
        else
        {
            await _connection.UpdateAsync(entry);
        }
    }

    public async Task DeleteAsync(DiaryEntry entry)
    {
        await InitializeAsync();
        await _connection.DeleteAsync(entry);
    }
}
