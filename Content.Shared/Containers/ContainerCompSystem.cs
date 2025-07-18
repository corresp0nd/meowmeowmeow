// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Shared.Containers;

/// <summary>
/// Applies / removes an entity prototype from a child entity when it's inserted into a container.
/// </summary>
public sealed class ContainerCompSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ContainerCompComponent, EntInsertedIntoContainerMessage>(OnConInsert);
        SubscribeLocalEvent<ContainerCompComponent, EntRemovedFromContainerMessage>(OnConRemove);
    }

    private void OnConRemove(Entity<ContainerCompComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        if (args.Container.ID != ent.Comp.Container || _timing.ApplyingState)
            return;

        if (_proto.TryIndex(ent.Comp.Proto, out var entProto))
        {
            EntityManager.RemoveComponents(args.Entity, entProto.Components);
        }
    }

    private void OnConInsert(Entity<ContainerCompComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != ent.Comp.Container || _timing.ApplyingState)
            return;

        if (_proto.TryIndex(ent.Comp.Proto, out var entProto))
        {
            EntityManager.AddComponents(args.Entity, entProto.Components);
        }
    }
}
