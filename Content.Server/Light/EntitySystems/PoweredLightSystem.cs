// SPDX-FileCopyrightText: 2021 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2021 Kara D <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2022 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jack Fox <35575261+DubiousDoggo@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Júlio César Ueti <52474532+Mirino97@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Matz05 <Matz05@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 ShadowCommander <10494922+ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 themias <89101928+themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 AJCM-git <60196617+AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 2023 Kevin Zheng <kevinz5000@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2023 Slava0135 <40753025+Slava0135@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 LordCarve <27449516+LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2024 Plykiya <58439124+Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 2025 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.DeviceLinking.Systems;
using Content.Server.DeviceNetwork;
using Content.Server.DeviceNetwork.Systems;
using Content.Server.Emp;
using Content.Server.Ghost;
using Content.Server.Light.Components;
using Content.Server.Power.Components;
using Content.Shared.Audio;
using Content.Shared.Damage;
using Content.Shared.DeviceLinking.Events;
using Content.Shared.DoAfter;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Light;
using Content.Shared.Light.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Containers;
using Robust.Shared.Timing;
using Robust.Shared.Audio.Systems;
using Content.Shared.Damage.Systems;
using Content.Shared.Damage.Components;
using Content.Shared.Power;

namespace Content.Server.Light.EntitySystems
{
    /// <summary>
    ///     System for the PoweredLightComponents
    /// </summary>
    public sealed class PoweredLightSystem : EntitySystem
    {
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        [Dependency] private readonly SharedAmbientSoundSystem _ambientSystem = default!;
        [Dependency] private readonly LightBulbSystem _bulbSystem = default!;
        [Dependency] private readonly SharedHandsSystem _handsSystem = default!;
        [Dependency] private readonly DeviceLinkSystem _signalSystem = default!;
        [Dependency] private readonly SharedContainerSystem _containerSystem = default!;
        [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
        [Dependency] private readonly SharedAudioSystem _audio = default!;
        [Dependency] private readonly PointLightSystem _pointLight = default!;
        [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
        [Dependency] private readonly DamageOnInteractSystem _damageOnInteractSystem = default!;

        private static readonly TimeSpan ThunkDelay = TimeSpan.FromSeconds(2);
        public const string LightBulbContainer = "light_bulb";

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<PoweredLightComponent, ComponentInit>(OnInit);
            SubscribeLocalEvent<PoweredLightComponent, MapInitEvent>(OnMapInit);
            SubscribeLocalEvent<PoweredLightComponent, InteractUsingEvent>(OnInteractUsing);
            SubscribeLocalEvent<PoweredLightComponent, InteractHandEvent>(OnInteractHand);

            SubscribeLocalEvent<PoweredLightComponent, GhostBooEvent>(OnGhostBoo);
            SubscribeLocalEvent<PoweredLightComponent, DamageChangedEvent>(HandleLightDamaged);

            SubscribeLocalEvent<PoweredLightComponent, SignalReceivedEvent>(OnSignalReceived);
            SubscribeLocalEvent<PoweredLightComponent, DeviceNetworkPacketEvent>(OnPacketReceived);

            SubscribeLocalEvent<PoweredLightComponent, PowerChangedEvent>(OnPowerChanged);

            SubscribeLocalEvent<PoweredLightComponent, PoweredLightDoAfterEvent>(OnDoAfter);
            SubscribeLocalEvent<PoweredLightComponent, EmpPulseEvent>(OnEmpPulse);
        }

        private void OnInit(EntityUid uid, PoweredLightComponent light, ComponentInit args)
        {
            light.LightBulbContainer = _containerSystem.EnsureContainer<ContainerSlot>(uid, LightBulbContainer);
            _signalSystem.EnsureSinkPorts(uid, light.OnPort, light.OffPort, light.TogglePort);
        }

        private void OnMapInit(EntityUid uid, PoweredLightComponent light, MapInitEvent args)
        {
            // TODO: Use ContainerFill dog
            if (light.HasLampOnSpawn != null)
            {
                var entity = EntityManager.SpawnEntity(light.HasLampOnSpawn, EntityManager.GetComponent<TransformComponent>(uid).Coordinates);
                _containerSystem.Insert(entity, light.LightBulbContainer);
            }
            // need this to update visualizers
            UpdateLight(uid, light);
        }

        private void OnInteractUsing(EntityUid uid, PoweredLightComponent component, InteractUsingEvent args)
        {
            if (args.Handled)
                return;

            args.Handled = InsertBulb(uid, args.Used, component);
        }

        private void OnInteractHand(EntityUid uid, PoweredLightComponent light, InteractHandEvent args)
        {
            if (args.Handled)
                return;

            // check if light has bulb to eject
            var bulbUid = GetBulb(uid, light);
            if (bulbUid == null)
                return;

            var userUid = args.User;
            //removing a broken/burned bulb, so allow instant removal
            if(TryComp<LightBulbComponent>(bulbUid.Value, out var bulb) && bulb.State != LightBulbState.Normal)
            {
                args.Handled = EjectBulb(uid, userUid, light) != null;
                return;
            }

            // removing a working bulb, so require a delay
            _doAfterSystem.TryStartDoAfter(new DoAfterArgs(EntityManager, userUid, light.EjectBulbDelay, new PoweredLightDoAfterEvent(), uid, target: uid)
            {
                BreakOnMove = true,
                BreakOnDamage = true,
            });

            args.Handled = true;
        }

        #region Bulb Logic API
        /// <summary>
        ///     Inserts the bulb if possible.
        /// </summary>
        /// <returns>True if it could insert it, false if it couldn't.</returns>
        public bool InsertBulb(EntityUid uid, EntityUid bulbUid, PoweredLightComponent? light = null)
        {
            if (!Resolve(uid, ref light))
                return false;

            // check if light already has bulb
            if (GetBulb(uid, light) != null)
                return false;

            // check if bulb fits
            if (!EntityManager.TryGetComponent(bulbUid, out LightBulbComponent? lightBulb))
                return false;
            if (lightBulb.Type != light.BulbType)
                return false;

            // try to insert bulb in container
            if (!_containerSystem.Insert(bulbUid, light.LightBulbContainer))
                return false;

            UpdateLight(uid, light);
            return true;
        }

        /// <summary>
        ///     Ejects the bulb to a mob's hand if possible.
        /// </summary>
        /// <returns>Bulb uid if it was successfully ejected, null otherwise</returns>
        public EntityUid? EjectBulb(EntityUid uid, EntityUid? userUid = null, PoweredLightComponent? light = null)
        {
            if (!Resolve(uid, ref light))
                return null;

            // check if light has bulb
            if (GetBulb(uid, light) is not { Valid: true } bulb)
                return null;

            // try to remove bulb from container
            if (!_containerSystem.Remove(bulb, light.LightBulbContainer))
                return null;

            // try to place bulb in hands
            _handsSystem.PickupOrDrop(userUid, bulb);

            UpdateLight(uid, light);
            return bulb;
        }

        /// <summary>
        ///     Replaces the spawned prototype of a pre-mapinit powered light with a different variant.
        /// </summary>
        public bool ReplaceSpawnedPrototype(Entity<PoweredLightComponent> light, string bulb)
        {
            if (light.Comp.LightBulbContainer.ContainedEntity != null)
                return false;

            if (LifeStage(light.Owner) >= EntityLifeStage.MapInitialized)
                return false;

            light.Comp.HasLampOnSpawn = bulb;
            return true;
        }

        /// <summary>
        ///     Try to replace current bulb with a new one
        ///     If succeed old bulb just drops on floor
        /// </summary>
        public bool ReplaceBulb(EntityUid uid, EntityUid bulb, PoweredLightComponent? light = null)
        {
            EjectBulb(uid, null, light);
            return InsertBulb(uid, bulb, light);
        }

        /// <summary>
        ///     Try to get light bulb inserted in powered light
        /// </summary>
        /// <returns>Bulb uid if it exist, null otherwise</returns>
        public EntityUid? GetBulb(EntityUid uid, PoweredLightComponent? light = null)
        {
            if (!Resolve(uid, ref light))
                return null;

            return light.LightBulbContainer.ContainedEntity;
        }

        /// <summary>
        ///     Try to break bulb inside light fixture
        /// </summary>
        public bool TryDestroyBulb(EntityUid uid, PoweredLightComponent? light = null)
        {
            if (!Resolve(uid, ref light, false))
                return false;

            // if we aren't mapinited,
            // just null the spawned bulb
            if (LifeStage(uid) < EntityLifeStage.MapInitialized)
            {
                light.HasLampOnSpawn = null;
                return true;
            }

            // check bulb state
            var bulbUid = GetBulb(uid, light);
            if (bulbUid == null || !EntityManager.TryGetComponent(bulbUid.Value, out LightBulbComponent? lightBulb))
                return false;
            if (lightBulb.State == LightBulbState.Broken)
                return false;

            // break it
            _bulbSystem.SetState(bulbUid.Value, LightBulbState.Broken, lightBulb);
            _bulbSystem.PlayBreakSound(bulbUid.Value, lightBulb);
            UpdateLight(uid, light);
            return true;
        }
        #endregion

        private void UpdateLight(EntityUid uid,
            PoweredLightComponent? light = null,
            ApcPowerReceiverComponent? powerReceiver = null,
            AppearanceComponent? appearance = null)
        {
            if (!Resolve(uid, ref light, ref powerReceiver, false))
                return;

            // Optional component.
            Resolve(uid, ref appearance, false);

            // check if light has bulb
            var bulbUid = GetBulb(uid, light);
            if (bulbUid == null || !EntityManager.TryGetComponent(bulbUid.Value, out LightBulbComponent? lightBulb))
            {
                SetLight(uid, false, light: light);
                powerReceiver.Load = 0;
                _appearance.SetData(uid, PoweredLightVisuals.BulbState, PoweredLightState.Empty, appearance);
                return;
            }

            switch (lightBulb.State)
            {
                case LightBulbState.Normal:
                    if (powerReceiver.Powered && light.On)
                    {
                        SetLight(uid, true, lightBulb.Color, light, lightBulb.LightRadius, lightBulb.LightEnergy, lightBulb.LightSoftness);
                        _appearance.SetData(uid, PoweredLightVisuals.BulbState, PoweredLightState.On, appearance);
                        var time = _gameTiming.CurTime;
                        if (time > light.LastThunk + ThunkDelay)
                        {
                            light.LastThunk = time;
                            _audio.PlayPvs(light.TurnOnSound, uid, light.TurnOnSound.Params.AddVolume(-10f));
                        }
                    }
                    else
                    {
                        SetLight(uid, false, light: light);
                        _appearance.SetData(uid, PoweredLightVisuals.BulbState, PoweredLightState.Off, appearance);
                    }
                    break;
                case LightBulbState.Broken:
                    SetLight(uid, false, light: light);
                    _appearance.SetData(uid, PoweredLightVisuals.BulbState, PoweredLightState.Broken, appearance);
                    break;
                case LightBulbState.Burned:
                    SetLight(uid, false, light: light);
                    _appearance.SetData(uid, PoweredLightVisuals.BulbState, PoweredLightState.Burned, appearance);
                    break;
            }

            powerReceiver.Load = (light.On && lightBulb.State == LightBulbState.Normal) ? lightBulb.PowerUse : 0;
        }

        /// <summary>
        ///     Destroy the light bulb if the light took any damage.
        /// </summary>
        public void HandleLightDamaged(EntityUid uid, PoweredLightComponent component, DamageChangedEvent args)
        {
            // Was it being repaired, or did it take damage?
            if (args.DamageIncreased)
            {
                // Eventually, this logic should all be done by this (or some other) system, not a component.
                TryDestroyBulb(uid, component);
            }
        }

        private void OnGhostBoo(EntityUid uid, PoweredLightComponent light, GhostBooEvent args)
        {
            if (light.IgnoreGhostsBoo)
                return;

            // check cooldown first to prevent abuse
            var time = _gameTiming.CurTime;
            if (light.LastGhostBlink != null)
            {
                if (time <= light.LastGhostBlink + light.GhostBlinkingCooldown)
                    return;
            }

            light.LastGhostBlink = time;

            ToggleBlinkingLight(uid, light, true);
            uid.SpawnTimer(light.GhostBlinkingTime, () =>
            {
                ToggleBlinkingLight(uid, light, false);
            });

            args.Handled = true;
        }

        private void OnPowerChanged(EntityUid uid, PoweredLightComponent component, ref PowerChangedEvent args)
        {
            // TODO: Power moment
            var metadata = MetaData(uid);

            if (metadata.EntityPaused || TerminatingOrDeleted(uid, metadata))
                return;

            UpdateLight(uid, component);
        }

        public void ToggleBlinkingLight(EntityUid uid, PoweredLightComponent light, bool isNowBlinking)
        {
            if (light.IsBlinking == isNowBlinking)
                return;

            light.IsBlinking = isNowBlinking;

            if (!EntityManager.TryGetComponent(uid, out AppearanceComponent? appearance))
                return;

            _appearance.SetData(uid, PoweredLightVisuals.Blinking, isNowBlinking, appearance);
        }

        private void OnSignalReceived(EntityUid uid, PoweredLightComponent component, ref SignalReceivedEvent args)
        {
            if (args.Port == component.OffPort)
                SetState(uid, false, component);
            else if (args.Port == component.OnPort)
                SetState(uid, true, component);
            else if (args.Port == component.TogglePort)
                ToggleLight(uid, component);
        }

        /// <summary>
        /// Turns the light on or of when receiving a <see cref="DeviceNetworkConstants.CmdSetState"/> command.
        /// The light is turned on or of according to the <see cref="DeviceNetworkConstants.StateEnabled"/> value
        /// </summary>
        private void OnPacketReceived(EntityUid uid, PoweredLightComponent component, DeviceNetworkPacketEvent args)
        {
            if (!args.Data.TryGetValue(DeviceNetworkConstants.Command, out string? command) || command != DeviceNetworkConstants.CmdSetState) return;
            if (!args.Data.TryGetValue(DeviceNetworkConstants.StateEnabled, out bool enabled)) return;

            SetState(uid, enabled, component);
        }

        private void SetLight(EntityUid uid, bool value, Color? color = null, PoweredLightComponent? light = null, float? radius = null, float? energy = null, float? softness = null)
        {
            if (!Resolve(uid, ref light))
                return;

            light.CurrentLit = value;
            _ambientSystem.SetAmbience(uid, value);

            if (EntityManager.TryGetComponent(uid, out PointLightComponent? pointLight))
            {
                _pointLight.SetEnabled(uid, value, pointLight);

                if (color != null)
                    _pointLight.SetColor(uid, color.Value, pointLight);
                if (radius != null)
                    _pointLight.SetRadius(uid, (float) radius, pointLight);
                if (energy != null)
                    _pointLight.SetEnergy(uid, (float) energy, pointLight);
                if (softness != null)
                    _pointLight.SetSoftness(uid, (float) softness, pointLight);
            }

            // light bulbs burn your hands!
            if (TryComp<DamageOnInteractComponent>(uid, out var damageOnInteractComp))
                _damageOnInteractSystem.SetIsDamageActiveTo((uid, damageOnInteractComp), value);
        }

        public void ToggleLight(EntityUid uid, PoweredLightComponent? light = null)
        {
            if (!Resolve(uid, ref light))
                return;

            light.On = !light.On;
            UpdateLight(uid, light);
        }

        public void SetState(EntityUid uid, bool state, PoweredLightComponent? light = null)
        {
            if (!Resolve(uid, ref light))
                return;

            light.On = state;
            UpdateLight(uid, light);
        }

        private void OnDoAfter(EntityUid uid, PoweredLightComponent component, DoAfterEvent args)
        {
            if (args.Handled || args.Cancelled || args.Args.Target == null)
                return;

            EjectBulb(args.Args.Target.Value, args.Args.User, component);

            args.Handled = true;
        }

        private void OnEmpPulse(EntityUid uid, PoweredLightComponent component, ref EmpPulseEvent args)
        {
            if (TryDestroyBulb(uid, component))
                args.Affected = true;
        }
    }
}
