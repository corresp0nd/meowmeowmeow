// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Client.Animations;
using Robust.Shared.Audio;

namespace Content.Client.Trigger;

[RegisterComponent]
[Access(typeof(TimerTriggerVisualizerSystem))]
public sealed partial class TimerTriggerVisualsComponent : Component
{
    /// <summary>
    /// The key used to index the priming animation.
    /// </summary>
    [ViewVariables]
    public const string AnimationKey = "priming_animation";

    /// <summary>
    /// The RSI state used while the device has not been primed.
    /// </summary>
    [DataField("unprimedSprite")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string UnprimedSprite = "icon";

    /// <summary>
    /// The RSI state used when the device is primed.
    /// Not VVWrite-able because it's only used at component init to construct the priming animation.
    /// </summary>
    [DataField("primingSprite")]
    public string PrimingSprite = "primed";

    /// <summary>
    /// The sound played when the device is primed.
    /// Not VVWrite-able because it's only used at component init to construct the priming animation.
    /// </summary>
    [DataField("primingSound")]
    public SoundSpecifier? PrimingSound;

    /// <summary>
    /// The actual priming animation.
    /// Constructed at component init from the sprite and sound.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public Animation PrimingAnimation = default!;
}
