using System.Formats.Asn1;
using Microsoft.EntityFrameworkCore;
using ZimLabs.Settings.Sqlite.Data;
using ZimLabs.Settings.Sqlite.Models;

namespace ZimLabs.Settings.Sqlite;

/// <summary>
/// Provides the functions for the interaction with the settings database (SQLite)
/// </summary>
public sealed class SettingsManager
{
    /// <summary>
    /// The name of the database
    /// </summary>
    private readonly string _databaseName;

    /// <summary>
    /// Creates a new instance of the <see cref="SettingsManager"/>
    /// </summary>
    /// <param name="databaseName">The name of the database (optional, The extension "<b>.db</b>" will be automatically added if it's missing)</param>
    public SettingsManager(string databaseName = "Settings.db")
    {
        if (!databaseName.EndsWith(".db", StringComparison.OrdinalIgnoreCase))
            databaseName += ".db";

        _databaseName = databaseName;

        using var context = new SettingsContext(databaseName);
        context.Database.EnsureCreated();
    }

    #region Load functions
    /// <summary>
    /// Loads all available settings entries
    /// </summary>
    /// <returns>The list with the settings</returns>
    public async Task<List<SettingsEntry>> LoadAsync()
    {
        await using var context = new SettingsContext(_databaseName);
        return await context.Settings.AsNoTracking().Select(s => (SettingsEntry) s).ToListAsync();
    }

    /// <summary>
    /// Loads a single settings entry according to the specified key
    /// </summary>
    /// <param name="key">The key of the entry</param>
    /// <returns>The settings entry</returns>
    public async Task<SettingsEntry?> LoadEntryAsync(int key)
    {
        await using var context = new SettingsContext(_databaseName);
        var result = await context.Settings.AsNoTracking().FirstOrDefaultAsync(f => f.Key == key);

        return result == null ? null : (SettingsEntry) result;
    }

    /// <summary>
    /// Loads the value of a single settings entry according to the specified key
    /// </summary>
    /// <param name="key">The key of the entry</param>
    /// <returns>The value. <b>null</b> will be returned when the settings entry doesn't exist</returns>
    public async Task<string?> LoadValueAsync(int key)
    {
        var entry = await LoadEntryAsync(key);
        return entry?.Value;
    }

    /// <summary>
    /// Loads the value of a single settings entry according to the specified key and converts the value into the desired type
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="key">The key of the entry</param>
    /// <returns>The value. The default value of <b>T</b> will be returned when the settings entry doesn't exist</returns>
    public async Task<T?> LoadValueAsync<T>(int key)
    {
        var entry = await LoadEntryAsync(key);

        if (entry == null)
            return default;

        return (T) Convert.ChangeType(entry.Value, typeof(T));
    }

    #endregion

    #region Add functions
    /// <summary>
    /// Adds a new settings entry
    /// </summary>
    /// <param name="key">The key of the entry</param>
    /// <param name="value">The value of the entry</param>
    /// <param name="description">The description of the entry</param>
    /// <returns>The created entry</returns>
    /// <exception cref="DbUpdateException">Will be thrown when for example the desired key already exists</exception>
    public async Task<SettingsEntry> AddEntryAsync(int key, string value, string description)
    {
        return await AddEntryAsync(new SettingsEntry
        {
            Key = key,
            Value = value,
            Description = description
        });
    }
    /// <summary>
    /// Adds a new settings entry
    /// </summary>
    /// <param name="key">The key of the entry</param>
    /// <param name="value">The value of the entry</param>
    /// <param name="description">The description of the entry</param>
    /// <returns>The created entry</returns>
    /// <exception cref="DbUpdateException">Will be thrown when for example the desired key already exists</exception>
    public async Task<SettingsEntry> AddEntryAsync(int key, object value, string description)
    {
        return await AddEntryAsync(new SettingsEntry
        {
            Key = key,
            Value = value.ToString() ?? string.Empty,
            Description = description
        });
    }

    /// <summary>
    /// Adds a new settings entry
    /// </summary>
    /// <param name="entry">The entry which should be added</param>
    /// <returns>The created entry</returns>
    /// <exception cref="DbUpdateException">Will be thrown when for example the desired key already exists</exception>
    public async Task<SettingsEntry> AddEntryAsync(SettingsEntry entry)
    {
        await using var context = new SettingsContext(_databaseName);

        // Remove the id if any was provided
        entry.Id = 0;

        var insertEntry = (SettingsDbModel) entry;
        await context.Settings.AddAsync(insertEntry);

        await context.SaveChangesAsync();

        return (SettingsEntry) insertEntry;
    }
    #endregion

    #region Update functions
    /// <summary>
    /// Updates the specified entry
    /// </summary>
    /// <param name="key">The key of the entry</param>
    /// <param name="value">The value of the entry</param>
    /// <param name="description">The description of the entry (optional, <b>Note:</b> If you don't provide a value, the original value will be kept)</param>
    /// <returns>The awaitable task</returns>
    public async Task UpdateEntryAsync(int key, string value, string description = "")
    {
        await UpdateEntryAsync(new SettingsEntry
        {
            Key = key,
            Value = value,
            Description = description
        });
    }

    /// <summary>
    /// Updates the specified entry
    /// </summary>
    /// <param name="entry">The entry which should be updated</param>
    /// <returns>The awaitable task</returns>
    public async Task UpdateEntryAsync(SettingsEntry entry)
    {
        await using var context = new SettingsContext(_databaseName);

        var dbEntry = await context.Settings.FirstOrDefaultAsync(f => f.Key == entry.Key);
        if (dbEntry == null) // No entry available, return
            return;

        dbEntry.Value = entry.Value;
        if (!string.IsNullOrEmpty(dbEntry.Description))
            dbEntry.Description = dbEntry.Description;

        await context.SaveChangesAsync();
    }
    #endregion

    #region Delete functions

    /// <summary>
    /// Deletes the specified entry
    /// </summary>
    /// <param name="key">The key of the entry</param>
    /// <returns>The awaitable task</returns>
    public async Task DeleteEntryAsync(int key)
    {
        await DeleteEntryAsync(new SettingsEntry
        {
            Key = key
        });
    }

    /// <summary>
    /// Deletes the specified entry
    /// </summary>
    /// <param name="entry">The entry which should be deleted</param>
    /// <returns>The awaitable task</returns>
    public async Task DeleteEntryAsync(SettingsEntry entry)
    {
        await using var context = new SettingsContext(_databaseName);

        var dbEntry = await context.Settings.FirstOrDefaultAsync(f => f.Key == entry.Key);
        if (dbEntry == null) 
            return;

        context.Settings.Remove(dbEntry);

        await context.SaveChangesAsync();
    }
    #endregion
}