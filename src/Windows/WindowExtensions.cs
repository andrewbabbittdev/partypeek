// Licensed to the Party Peek Contributors under one or more agreements.
// The Party Peek Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Dalamud.Hosting;

namespace PartyPeek.Windows;

/// <summary>
/// Provides extension methods for configuring Party Peek windows in a Dalamud application builder.
/// </summary>
public static class WindowExtensions
{
    /// <summary>
    /// Configures the application builder to support Party Peek windows.
    /// </summary>
    /// <param name="builder">The application builder to configure. Must not be null.</param>
    /// <returns>The same <see cref="DalamudApplicationBuilder"/> instance so that additional configuration calls can be chained.</returns>
    public static DalamudApplicationBuilder ConfigurePartyPeekWindows(this DalamudApplicationBuilder builder)
    {
        return builder.AddWindows<OverlayWindow>();
    }
}
