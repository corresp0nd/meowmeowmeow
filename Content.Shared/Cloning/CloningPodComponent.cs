// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 2025 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.DeviceLinking;
using Content.Shared.Materials;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Cloning;

[RegisterComponent]
public sealed partial class CloningPodComponent : Component
{
    [DataField]
    public ProtoId<SinkPortPrototype> PodPort = "CloningPodReceiver";

    [ViewVariables]
    public ContainerSlot BodyContainer = default!;

    /// <summary>
    /// How long the cloning has been going on for.
    /// </summary>
    [ViewVariables]
    public float CloningProgress = 0;

    [ViewVariables]
    public int UsedBiomass = 70;

    [ViewVariables]
    public bool FailedClone = false;

    /// <summary>
    /// The material that is used to clone entities.
    /// </summary>
    [DataField]
    public ProtoId<MaterialPrototype> RequiredMaterial = "Biomass";

    /// <summary>
    /// The current amount of time it takes to clone a body.
    /// </summary>
    [DataField]
    public float CloningTime = 30f;

    /// <summary>
    /// The mob to spawn on emag.
    /// </summary>
    [DataField]
    public EntProtoId MobSpawnId = "MobAbomination";

    /// <summary>
    /// The sound played when a mob is spawned from an emagged cloning pod.
    /// </summary>
    [DataField]
    public SoundSpecifier ScreamSound = new SoundCollectionSpecifier("ZombieScreams")
    {
        Params = AudioParams.Default.WithVolume(4),
    };

    [ViewVariables(VVAccess.ReadWrite)]
    public CloningPodStatus Status;

    [ViewVariables]
    public EntityUid? ConnectedConsole;
}

[Serializable, NetSerializable]
public enum CloningPodVisuals : byte
{
    Status
}

[Serializable, NetSerializable]
public enum CloningPodStatus : byte
{
    Idle,
    Cloning,
    Gore,
    NoMind
}
