// SPDX-FileCopyrightText: 2024 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 marc-pelletier <113944176+marc-pelletier@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Charges.Components;
using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.Examine;
using Content.Shared.FixedPoint;
using Robust.Shared.Timing;

namespace Content.Server.Charges.Systems;

public sealed class ChargesSystem : SharedChargesSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<LimitedChargesComponent, AutoRechargeComponent>();
        while (query.MoveNext(out var uid, out var charges, out var recharge))
        {
            if (charges.Charges == charges.MaxCharges || _timing.CurTime < recharge.NextChargeTime)
                continue;

            AddCharges(uid, 1, charges);
            recharge.NextChargeTime = _timing.CurTime + recharge.RechargeDuration;
        }
    }

    protected override void OnExamine(EntityUid uid, LimitedChargesComponent comp, ExaminedEvent args)
    {
        base.OnExamine(uid, comp, args);

        // only show the recharging info if it's not full
        if (!args.IsInDetailsRange || comp.Charges == comp.MaxCharges || !TryComp<AutoRechargeComponent>(uid, out var recharge))
            return;

        var timeRemaining = Math.Round((recharge.NextChargeTime - _timing.CurTime).TotalSeconds);
        args.PushMarkup(Loc.GetString("limited-charges-recharging", ("seconds", timeRemaining)));
    }

    public override void AddCharges(EntityUid uid, FixedPoint2 change, LimitedChargesComponent? comp = null)
    {
        if (!Query.Resolve(uid, ref comp, false))
            return;

        var startRecharge = comp.Charges == comp.MaxCharges;
        base.AddCharges(uid, change, comp);

        // if a charge was just used from full, start the recharge timer
        // TODO: probably make this an event instead of having le server system that just does this
        if (change < 0 && startRecharge && TryComp<AutoRechargeComponent>(uid, out var recharge))
            recharge.NextChargeTime = _timing.CurTime + recharge.RechargeDuration;
    }
}
