using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZimLabs.Settings.Sqlite.Models;

/// <summary>
/// Represents an entry of the settings database
/// </summary>
[Table("Settings")]
[Index(nameof(Key), IsUnique = true)]
internal sealed class SettingsDbModel
{
    /// <summary>
    /// Gets or sets the id of the entry
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the key of the entry
    /// </summary>
    public int Key { get; set; }

    /// <summary>
    /// Gets or sets the value of the entry
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the entry
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Converts a <see cref="SettingsDbModel"/> into a <see cref="SettingsEntry"/>
    /// </summary>
    /// <param name="model">The <see cref="SettingsDbModel"/> entry</param>
    public static explicit operator SettingsEntry(SettingsDbModel model)
    {
        return new SettingsEntry
        {
            Id = model.Id,
            Key = model.Key,
            Value = model.Value,
            Description = model.Description
        };
    }

    /// <summary>
    /// Converts a <see cref="SettingsDbModel"/> into a <see cref="SettingsEntry"/>
    /// </summary>
    /// <param name="entry">The <see cref="SettingsEntry"/> object</param>
    public static explicit operator SettingsDbModel(SettingsEntry entry)
    {
        return new SettingsDbModel
        {
            Id = entry.Id,
            Key = entry.Key,
            Value = entry.Value,
            Description = entry.Description
        };
    }
}