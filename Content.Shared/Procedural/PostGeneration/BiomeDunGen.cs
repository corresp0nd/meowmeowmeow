// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Maps;
using Content.Shared.Parallax.Biomes;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.PostGeneration;

/// <summary>
/// Generates a biome on top of valid tiles, then removes the biome when done.
/// Only works if no existing biome is present.
/// </summary>
public sealed partial class BiomeDunGen : IDunGenLayer
{
    [DataField(required: true)]
    public ProtoId<BiomeTemplatePrototype> BiomeTemplate;

    /// <summary>
    /// creates a biome only on the specified tiles
    /// </summary>
    [DataField]
    public HashSet<ProtoId<ContentTileDefinition>>? TileMask;
}
