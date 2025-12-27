// Licensed to the Party Peek Contributors under one or more agreements.
// The Party Peek Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Dalamud.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace PartyPeek.Services;

/// <summary>
/// Provides extension methods for configuring Party Peek services in a Dalamud application builder.
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// Configures the application builder to support Party Peek services.
    /// </summary>
    /// <param name="builder">The application builder to configure. Must not be null.</param>
    /// <returns>The same <see cref="DalamudApplicationBuilder"/> instance so that additional configuration calls can be chained.</returns>
    public static DalamudApplicationBuilder ConfigurePartyPeekServices(this DalamudApplicationBuilder builder)
    {
        builder.Services.AddSingleton<PartyFinderService>();

        return builder;
    }
}
