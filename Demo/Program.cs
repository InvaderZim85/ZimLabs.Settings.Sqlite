using Demo.Common;
using Serilog;
using System.Diagnostics;
using ZimLabs.Settings.Sqlite;
using ZimLabs.Settings.Sqlite.Models;

namespace Demo;

/// <summary>
/// Provides the main logic
/// </summary>
internal static class Program
{
    /// <summary>
    /// The main entry point of the program
    /// </summary>
    /// <param name="args">The arguments</param>
    private static async Task Main(string[] args)
    {
        // Init the logger
        Helper.InitLogger(true);

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        try
        {
            var settingsManager = new SettingsManager("ZimLabs.Settings");

            // Add an entry
            Log.Information("Add an entry");
            await settingsManager.AddEntryAsync(1, "SomeValue", "Some description");

            // Load the entry
            var value = await settingsManager.LoadEntryAsync(1);
            if (value != null)
            {
                Log.Information("Value loaded: Id {id}, Key {key}, Value {value}, Description: {description}", value.Id,
                    value.Key, value.Value, value.Description);
            }

            // Add an entry with an int
            Log.Information("Add another value");
            var secondEntry = await settingsManager.AddEntryAsync(new SettingsEntry
            {
                Id = 1,
                Key = 2,
                Value = "10",
                Description = "Some description"
            });
            Log.Information("Id of the entry: {id}", secondEntry.Id);

            // Load the entry
            var intValue = await settingsManager.LoadValueAsync<int>(2);
            Log.Information("Value loaded. Value: {value}", intValue);

            // Add a third value
            await settingsManager.AddEntryAsync(3, true, "Some bool value...");
            Log.Information("Add a third value");

            // Load all values
            var values = await settingsManager.LoadAsync();

            foreach (var entry in values)
            {
                Log.Information("Id {id}, Key {key}, Value {value}, Description: {description}", entry.Id,
                    entry.Key, entry.Value, entry.Description);
            }

            // Delete the first value
            if (value != null)
            {
                Log.Information("Delete first value");
                await settingsManager.DeleteEntryAsync(value);
            }

            // Reload all values
            values = await settingsManager.LoadAsync();

            foreach (var entry in values)
            {
                Log.Information("Id {id}, Key {key}, Value {value}, Description: {description}", entry.Id,
                    entry.Key, entry.Value, entry.Description);
            }

            // Add a new entry (this will cause an error because the key is already in use)
            var tmpEntry = await settingsManager.AddEntryAsync(new SettingsEntry
            {
                Key = 2,
                Value = "10",
                Description = "Some description"
            });
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "A fatal error has occurred.");
        }
        finally
        {
            stopwatch.Stop();
            Log.Information("Duration: {duration}", stopwatch.Elapsed);
            Log.Information("Done");
        }
    }
}