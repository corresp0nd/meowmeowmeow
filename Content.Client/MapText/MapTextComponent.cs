// SPDX-FileCopyrightText: 2024 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.MapText;
using Robust.Client.Graphics;

namespace Content.Client.MapText;

[RegisterComponent]
public sealed partial class MapTextComponent : SharedMapTextComponent
{
    /// <summary>
    /// The font that gets cached on component init or state changes
    /// </summary>
    [ViewVariables]
    public VectorFont? CachedFont;

    /// <summary>
    /// The text currently being displayed. This is either <see cref="SharedMapTextComponent.Text"/> or the
    /// localized text <see cref="SharedMapTextComponent.LocText"/> or
    /// </summary>
    public string CachedText = string.Empty;
}
