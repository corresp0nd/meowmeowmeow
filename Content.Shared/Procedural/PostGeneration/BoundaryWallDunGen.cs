// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Procedural.PostGeneration;

/// <summary>
/// Iterates room edges and places the relevant tiles and walls on any free indices.
/// </summary>
/// <remarks>
/// Dungeon data keys are:
/// - CornerWalls (Optional)
/// - FallbackTile
/// - Walls
/// </remarks>
public sealed partial class BoundaryWallDunGen : IDunGenLayer
{
    [DataField]
    public BoundaryWallFlags Flags = BoundaryWallFlags.Corridors | BoundaryWallFlags.Rooms;
}

[Flags]
public enum BoundaryWallFlags : byte
{
    Rooms = 1 << 0,
    Corridors = 1 << 1,
}
