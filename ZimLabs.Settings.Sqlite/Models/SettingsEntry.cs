namespace ZimLabs.Settings.Sqlite.Models;

/// <summary>
/// Represents a settings entry
/// </summary>
public sealed class SettingsEntry
{
    /// <summary>
    /// Gets or sets the id of the entry
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the key of the entry
    /// </summary>
    public int Key { get; set; }

    /// <summary>
    /// Gets or sets the value of the entry
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the description of the entry
    /// </summary>
    public string Description { get; set; }
}