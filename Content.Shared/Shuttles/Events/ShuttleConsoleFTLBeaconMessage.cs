// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.Events;

/// <summary>
/// Raised on a client when it wishes to FTL to a beacon.
/// </summary>
[Serializable, NetSerializable]
public sealed class ShuttleConsoleFTLBeaconMessage : BoundUserInterfaceMessage
{
    public NetEntity Beacon;
    public Angle Angle;
}
