// Licensed to the Party Peek Contributors under one or more agreements.
// The Party Peek Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Dalamud.Hosting;
using Dalamud.Plugin;

namespace PartyPeek;

/// <summary>
/// Represents a Dalamud plugin that manages its application lifecycle and integrates with the Dalamud plugin
/// environment.
/// </summary>
public class Plugin : IDalamudPlugin
{
    /// <summary>
    /// Gets or sets the current instance of the Dalamud application used by the plugin framework.
    /// </summary>
    public static DalamudApplication App { get; private set; } = default!;

    /// <summary>
    /// Initializes a new instance of the Plugin class using the specified Dalamud plugin interface.
    /// </summary>
    /// <param name="pluginInterface">The interface provided by Dalamud for interacting with the plugin environment. Cannot be null.</param>
    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        var builder = DalamudApplication.CreateBuilder(pluginInterface);

        App = builder.Build();

        App.Start();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        App.Stop();
        App.Dispose();

        GC.SuppressFinalize(this);
    }
}
