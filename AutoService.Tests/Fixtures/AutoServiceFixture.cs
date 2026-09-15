using AutoService.Domain.Data;

namespace AutoService.Tests.Fixtures;

/// <summary>
/// Provides a shared seeded AutoService context for unit tests.
/// </summary>
public class AutoServiceFixture
{
    /// <summary>
    /// Gets the in-memory context containing seeded AutoService data.
    /// </summary>
    public AutoServiceContext Context { get; }

    /// <summary>
    /// Initializes the test fixture and seeds the AutoService data.
    /// </summary>
    public AutoServiceFixture(){ Context = DataSeeder.Seed(); }
}
