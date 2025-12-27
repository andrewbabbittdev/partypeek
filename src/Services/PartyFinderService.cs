// Licensed to the Party Peek Contributors under one or more agreements.
// The Party Peek Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System.Collections.Concurrent;

namespace PartyPeek.Services;

/// <summary>
/// 
/// </summary>
public class PartyFinderService : IDisposable
{
    /// <summary>
    /// Gets a value indicating whether the party finder details is currently visible to the user.
    /// </summary>
    public bool Visible { get; private set; }

    /// <summary>
    /// Occurs when the visibility of the party finder details changes.
    /// </summary>
    public event EventHandler? VisibilityChanged;

    /// <summary>
    /// The X position of the party finder details window.
    /// </summary>
    public short X { get; private set; }

    /// <summary>
    /// The Y position of the party finder details window.
    /// </summary>
    public short Y { get; private set; }

    /// <summary>
    /// The height of the party finder details window.
    /// </summary>
    public float Height { get; private set; }

    /// <summary>
    /// The width of the party finder details window.
    /// </summary>
    public float Width { get; private set; }

    /// <summary>
    /// The current party members listed in the party finder details.
    /// </summary>
    public ConcurrentBag<string> CurrentParty { get; init; } = [];

    private readonly IAddonLifecycle _addonLifecycle;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="addonLifecycle"></param>
    /// <param name="gameGui"></param>
    public PartyFinderService(IAddonLifecycle addonLifecycle, IGameGui gameGui)
    {
        _addonLifecycle = addonLifecycle;

        var addon = gameGui.GetAddonByName("LookingForGroupDetail");

        Visible = addon.IsVisible;
        X = addon.X;
        Y = addon.Y;
        Height = addon.ScaledHeight;
        Width = addon.ScaledWidth;

        _addonLifecycle.RegisterListener(AddonEvent.PostOpen, "LookingForGroupDetail", OnPartyFinderOpened);
        _addonLifecycle.RegisterListener(AddonEvent.PreClose, "LookingForGroupDetail", OnPartyFinderClosed);
        _addonLifecycle.RegisterListener(AddonEvent.PostDraw, "LookingForGroupDetail", OnPartyFinderDrawn);
        _addonLifecycle.RegisterListener(AddonEvent.PreRefresh, "LookingForGroupDetail", OnPartyFinderPreRefresh);
        _addonLifecycle.RegisterListener(AddonEvent.PostRefresh, "LookingForGroupDetail", OnPartyFinderPostRefresh);
        _addonLifecycle.RegisterListener(AddonEvent.PreFinalize, "LookingForGroupDetail", OnPartyFinderPreFinalize);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _addonLifecycle.UnregisterListener(AddonEvent.PostOpen, "LookingForGroupDetail", OnPartyFinderOpened);
        _addonLifecycle.UnregisterListener(AddonEvent.PreClose, "LookingForGroupDetail", OnPartyFinderClosed);
        _addonLifecycle.UnregisterListener(AddonEvent.PostDraw, "LookingForGroupDetail", OnPartyFinderDrawn);
        _addonLifecycle.UnregisterListener(AddonEvent.PreRefresh, "LookingForGroupDetail", OnPartyFinderPreRefresh);
        _addonLifecycle.UnregisterListener(AddonEvent.PostRefresh, "LookingForGroupDetail", OnPartyFinderPostRefresh);
        _addonLifecycle.UnregisterListener(AddonEvent.PreFinalize, "LookingForGroupDetail", OnPartyFinderPreFinalize);

        GC.SuppressFinalize(this);
    }

    private void OnPartyFinderOpened(AddonEvent addonEvent, AddonArgs addonArgs)
    {
        Visible = true;
        VisibilityChanged?.Invoke(this, new());
        CurrentParty.Clear();
    }

    private void OnPartyFinderClosed(AddonEvent addonEvent, AddonArgs addonArgs)
    {
        Visible = false;
        VisibilityChanged?.Invoke(this, new());
        CurrentParty.Clear();
    }

    private void OnPartyFinderDrawn(AddonEvent addonEvent, AddonArgs addonArgs)
    {
        X = addonArgs.Addon.X;
        Y = addonArgs.Addon.Y;
        Height = addonArgs.Addon.ScaledHeight;
        Width = addonArgs.Addon.ScaledWidth;
    }

    private unsafe void OnPartyFinderPreRefresh(AddonEvent addonEvent, AddonArgs addonArgs)
    {
        CurrentParty.Clear();
    }

    private unsafe void OnPartyFinderPostRefresh(AddonEvent addonEvent, AddonArgs addonArgs)
    {
        var agent = AgentLookingForGroup.Instance();
        var nameCache = NameCache.Instance();

        if (agent is null || nameCache is null || agent->LastViewedListing.TotalSlots == 0)
        {
            return;
        }

        for (var i = 0; i < agent->LastViewedListing.TotalSlots; i++)
        {
            if (agent->LastViewedListing.MemberContentIds[i] == 0)
            {
                continue;
            }

            CurrentParty.Add(nameCache->GetNameByContentId(agent->LastViewedListing.MemberContentIds[i]).ToString());
        }
    }

    private void OnPartyFinderPreFinalize(AddonEvent addonEvent, AddonArgs addonArgs)
    {
        CurrentParty.Clear();
    }
}
