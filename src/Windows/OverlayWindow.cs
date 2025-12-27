// Licensed to the Party Peek Contributors under one or more agreements.
// The Party Peek Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using PartyPeek.Services;
using System.Numerics;

namespace PartyPeek.Windows;

/// <summary>
/// Represents a window that displays party finder information.
/// </summary>
public class OverlayWindow : Window, IDisposable
{
    private readonly PartyFinderService _partyFinder;

    /// <summary>
    /// Initializes a new instance of the OverlayWindow.
    /// </summary>
    public OverlayWindow(PartyFinderService partyFinder) : base("Party Peek")
    {
        _partyFinder = partyFinder;
        _partyFinder.VisibilityChanged += OnPartyFinderVisibilityChanged;

        Flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.AlwaysAutoResize;
        IsOpen = _partyFinder.Visible;
    }

    /// <inheritdoc/>
    public override void PreDraw()
    {
        Position = new Vector2(_partyFinder.X + _partyFinder.Width + 5, _partyFinder.Y + 5);
    }

    /// <inheritdoc/>
    public override unsafe void Draw()
    {
        foreach (var user in _partyFinder.CurrentParty)
        {
            ImGui.Text(user);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _partyFinder.VisibilityChanged -= OnPartyFinderVisibilityChanged;

        GC.SuppressFinalize(this);
    }

    private void OnPartyFinderVisibilityChanged(object? sender, EventArgs e)
    {
        IsOpen = _partyFinder.Visible;
    }
}
