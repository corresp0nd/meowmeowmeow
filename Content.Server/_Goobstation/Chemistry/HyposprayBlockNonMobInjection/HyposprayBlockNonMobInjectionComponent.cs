// SPDX-FileCopyrightText: 2024 Aviu00 <93730715+Aviu00@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 John Space <bigdumb421@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._Goobstation.Chemistry.HyposprayBlockNonMobInjection;

/// <summary>
/// For some reason if you set HyposprayComponent onlyAffectsMobs to true it would be able to draw from containers
/// even if injectOnly is also true. I don't want to modify HypospraySystem, so I made this component.
/// </summary>
[RegisterComponent]
public sealed partial class HyposprayBlockNonMobInjectionComponent : Component
{
}
