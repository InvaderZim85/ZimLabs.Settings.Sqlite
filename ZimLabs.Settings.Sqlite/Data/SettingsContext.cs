using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ZimLabs.Settings.Sqlite.Models;

namespace ZimLabs.Settings.Sqlite.Data;

/// <summary>
/// Provides the functions for the interaction with the settings tables
/// </summary>
internal class SettingsContext : DbContext
{
    /// <summary>
    /// Gets or sets the name of the database
    /// </summary>
    private readonly string _databaseName;

    /// <summary>
    /// Gets or sets the settings table
    /// </summary>
    public DbSet<SettingsDbModel> Settings => Set<SettingsDbModel>();

    /// <summary>
    /// Creates a new instance of the <see cref="SettingsContext"/>
    /// </summary>
    /// <param name="databaseName">The name of the database</param>
    public SettingsContext(string databaseName)
    {
        _databaseName = databaseName;
    }

    /// <summary>
    /// Occurs when the context is configured
    /// </summary>
    /// <param name="optionsBuilder">The options builder</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        var dbPath = Path.Combine(path, _databaseName);
        optionsBuilder.UseSqlite($"Data Source = {dbPath}");
    }
}