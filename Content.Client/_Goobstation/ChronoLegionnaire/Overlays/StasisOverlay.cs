// SPDX-FileCopyrightText: 2024 BombasterDS <115770678+BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._Goobstation.ChronoLegionnaire.Components;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;

namespace Content.Client._Goobstation.ChronoLegionnaire;

public sealed class StasisOverlay : Overlay
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override bool RequestScreenTexture => true;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    private readonly ShaderInstance _coloredScreenBorder;

    public StasisOverlay()
    {
        IoCManager.InjectDependencies(this);
        _coloredScreenBorder = _prototypeManager.Index<ShaderPrototype>("WideColoredScreenBorder").InstanceUnique();
    }

    protected override bool BeforeDraw(in OverlayDrawArgs args)
    {
        if (_entityManager.HasComponent<InsideStasisComponent>(_playerManager.LocalSession?.AttachedEntity))
            return true;

        return false;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (ScreenTexture == null)
            return;

        _coloredScreenBorder?.SetParameter("SCREEN_TEXTURE", ScreenTexture);
        _coloredScreenBorder?.SetParameter("borderColor", Color.CornflowerBlue);
        _coloredScreenBorder?.SetParameter("borderSize", 55.0f);

        var handle = args.WorldHandle;
        var viewport = args.WorldBounds;

        handle.UseShader(_coloredScreenBorder);
        handle.DrawRect(viewport, Color.White);
        handle.UseShader(null);
    }
}
