// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Roles;

/// <summary>
/// This is most likely not the component you are looking for, almost nothing should be using this.
/// Consider using GhostRoleComponent or AntagSelectionComponent instead.
///
/// The specified mind role will be added to the mob on spawn.
///
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class StartingMindRoleComponent : Component
{
    /// <summary>
    ///     The ID of the mind role to add
    /// </summary>
    [DataField(required: true)]
    public EntProtoId MindRole;

    /// <summary>
    ///     Add the mind role silently
    /// </summary>
    [DataField]
    public bool Silent = true;
}
