// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2024 yglop <95057024+yglop@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Eris <erisfiregamer1@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Timing;
using Robust.Server.GameObjects;
using Content.Shared.Changeling;
using Content.Shared.Mind;
using Content.Server.Body.Systems;
using Content.Shared.Store.Components;

namespace Content.Server.Changeling;

public sealed partial class ChangelingEggSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly BodySystem _bodySystem = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;


    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_timing.IsFirstTimePredicted)
            return;

        foreach (var comp in EntityManager.EntityQuery<ChangelingEggComponent>())
        {
            var uid = comp.Owner;

            if (_timing.CurTime < comp.UpdateTimer)
                continue;

            comp.UpdateTimer = _timing.CurTime + TimeSpan.FromSeconds(comp.UpdateCooldown);

            Cycle(uid, comp);
        }
    }
    public void Cycle(EntityUid uid, ChangelingEggComponent comp)
    {
        if (comp.active == false)
        {
            comp.active = true;
            return;
        }

        var newUid = Spawn("MobMonkey", Transform(uid).Coordinates);

        var mind = EnsureComp<MindComponent>(newUid);
        _mind.TransferTo(comp.lingMind, newUid);

        var ling = EnsureComp<ChangelingComponent>(newUid);
        ling = comp.lingComp;

        EntityManager.AddComponent(newUid, comp.lingStore);

        _bodySystem.GibBody((EntityUid) uid);
    }
}
