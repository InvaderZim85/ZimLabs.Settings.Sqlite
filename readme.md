# ZimLabs.Settings.Sqlite

**Content**

- [General](#general)
- [Example](#example

## General

This library provides functions to manage settings using a SQLite database.

The settings are stored as a "Key" - "Value" pair. 

## Example

Here a small example

```csharp
// Using
using ZimLabs.Settings.Sqlite;

var settingsManager = new SettingsManager("NameOfMyDatabase");

// Add an entry
var entry = await settingsManager.AddEntryAsync(1, "TheValue", "Some description");

// Add another entry
var secondEntry = await settingsManager.AddEntryAsync(new SettingsEntry
{
    Key = 1,
    Value = "10",
    Description = "Some description"
});

// Update an entry
secondEntry.Value = "100";
await settingsManager.UpdateEntryAsync(secondEntry);

// Update the first entry
await settingsManager.UpdateEntryAsync(1, "NewValue");

// Delete an entry
await settingsManager.DeleteEntryAsync(1);

// Load all entries
var entries = await settingsManager.LoadAsync();

// Load the entry with the key "2"
var tmpEntry = await settingsManager.LoadEntryAsync(2);

// Load the value of the second entry
var tmpValue = await settingsManager.LoadValueAsync(2);

// Load the value of the second entry and convert it into an int
var tmpValueInt = await settingsManager.LoadValueAsync<int>(2);
```

For more examples see the Demo project.