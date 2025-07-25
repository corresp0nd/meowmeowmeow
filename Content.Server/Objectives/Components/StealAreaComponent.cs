// SPDX-FileCopyrightText: 2024 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2024 Ed <96445749+TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Objectives.Systems;
using Content.Server.Thief.Systems;

namespace Content.Server.Objectives.Components;

/// <summary>
/// An abstract component that allows other systems to count adjacent objects as "stolen" when controlling other systems
/// </summary>
[RegisterComponent, Access(typeof(StealConditionSystem), typeof(ThiefBeaconSystem))]
public sealed partial class StealAreaComponent : Component
{
    [DataField]
    public bool Enabled = true;

    [DataField]
    public float Range = 1f;

    /// <summary>
    /// all the minds that will be credited with stealing from this area.
    /// </summary>
    [DataField]
    public HashSet<EntityUid> Owners = new();
}
