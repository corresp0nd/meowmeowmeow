// SPDX-FileCopyrightText: 2024 AJCM-git <60196617+AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 BombasterDS <115770678+BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Krunklehorn <42424291+Krunklehorn@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Mindshield.Components;
using Content.Shared.Overlays;
using Content.Shared.StatusIcon;
using Content.Shared.StatusIcon.Components;
using Robust.Shared.Prototypes;

namespace Content.Client.Overlays;

public sealed class ShowMindShieldIconsSystem : EquipmentHudSystem<ShowMindShieldIconsComponent>
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MindShieldComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
        SubscribeLocalEvent<FakeMindShieldComponent, GetStatusIconsEvent>(OnGetStatusIconsEventFake);
    }
    // TODO: Probably need to get this OFF of client since this can be read by bad actors rather easily
    //  ...imagine cheating in a game about silly paper dolls
    private void OnGetStatusIconsEventFake(EntityUid uid, FakeMindShieldComponent component, ref GetStatusIconsEvent ev)
    {
        if(!IsActive)
            return;
        if (component.IsEnabled && _prototype.TryIndex(component.MindShieldStatusIcon, out var fakeStatusIconPrototype))
            ev.StatusIcons.Add(fakeStatusIconPrototype);
    }

    private void OnGetStatusIconsEvent(EntityUid uid, MindShieldComponent component, ref GetStatusIconsEvent ev)
    {
        if (!IsActive)
            return;

        var statusIcon = component.MindShieldStatusIcon; // Goobstation - check if mindshield is broken

        if (component.Broken)
            statusIcon = component.MindShieldBrokenStatusIcon; // Goobstation - check if mindshield is broken

        if (_prototype.TryIndex(statusIcon, out var iconPrototype)) // Goobstation
            ev.StatusIcons.Add(iconPrototype);
    }
}
