// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mr. 27 <45323883+Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;

namespace Content.Server.Zombies;

/// <summary>
/// This is used for a zombie that cannot be cured by any methods. Gives a succumb to zombie infection action.
/// </summary>
[RegisterComponent]
public sealed partial class IncurableZombieComponent : Component
{
    [DataField]
    public EntProtoId ZombifySelfActionPrototype = "ActionTurnUndead";

    [DataField]
    public EntityUid? Action;
}
