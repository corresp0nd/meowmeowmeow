// SPDX-FileCopyrightText: 2024 BombasterDS <115770678+BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;

namespace Content.Server.Mail.Components
{
    /// <summary>
    /// This is for the mail teleporter.
    /// Random mail will be teleported to this every few minutes.
    /// </summary>
    [RegisterComponent]
    public sealed partial class MailTeleporterComponent : Component
    {

        // Not starting accumulator at 0 so mail carriers have some deliveries to make shortly after roundstart.
        [DataField("accumulator")]
        public float Accumulator = 285f;

        [DataField("teleportInterval")]
        public TimeSpan TeleportInterval = TimeSpan.FromMinutes(7.5);

        /// <summary>
        /// The sound that's played when new mail arrives.
        /// </summary>
        [DataField("teleportSound")]
        public SoundSpecifier TeleportSound = new SoundPathSpecifier("/Audio/Effects/teleport_arrival.ogg");

        /// <summary>
        /// The MailDeliveryPoolPrototype that's used to select what mail this
        /// teleporter can deliver.
        /// </summary>
        [DataField]
        public string MailPool = "BaseMailDeliveryPool"; // Goobstation / Frontier / DeltaV: Mail rework

        /// <summary>
        /// How many mail candidates do we need per actual delivery sent when
        /// the mail goes out? The number of candidates is divided by this number
        /// to determine how many deliveries will be teleported in.
        /// It does not determine unique recipients. That is random.
        /// </summary>
        [DataField("candidatesPerDelivery")]
        public int CandidatesPerDelivery = 8;

        [DataField("minimumDeliveriesPerTeleport")]
        public int MinimumDeliveriesPerTeleport = 1;

        /// <summary>
        /// Do not teleport any more mail in, if there are at least this many
        /// undelivered parcels.
        /// </summary>
        /// <remarks>
        /// Currently this works by checking how many MailComponent entities
        /// are sitting on the teleporter's tile.
        ///
        /// It should be noted that if the number of actual deliveries to be
        /// made based on the number of candidates divided by candidates per
        /// delivery exceeds this number, the teleporter will spawn more mail
        /// than this number.
        ///
        /// This is just a simple check to see if anyone's been picking up the
        /// mail lately to prevent entity bloat for the sake of performance.
        /// </remarks>
        [DataField("maximumUndeliveredParcels")]
        public int MaximumUndeliveredParcels = 5;

        /// <summary>
        /// Any item that breaks or is destroyed in less than this amount of
        /// damage is one of the types of items considered fragile.
        /// </summary>
        [DataField("fragileDamageThreshold")]
        public int FragileDamageThreshold = 10;

        /// <summary>
        /// What's the bonus for delivering a fragile package intact?
        /// </summary>
        [DataField("fragileBonus")]
        public int FragileBonus = 100;

        /// <summary>
        /// What's the malus for failing to deliver a fragile package?
        /// </summary>
        [DataField("fragileMalus")]
        public int FragileMalus = -100;

        /// <summary>
        /// What's the chance for any one delivery to be marked as priority mail?
        /// </summary>
        [DataField("priorityChance")]
        public float PriorityChance = 0.1f;

        /// <summary>
        /// How long until a priority delivery is considered as having failed
        /// if not delivered?
        /// </summary>
        [DataField("priorityDuration")]
        public TimeSpan priorityDuration = TimeSpan.FromMinutes(5);

        /// <summary>
        /// What's the bonus for delivering a priority package on time?
        /// </summary>
        [DataField("priorityBonus")]
        public int PriorityBonus = 250;

        /// <summary>
        /// What's the malus for failing to deliver a priority package?
        /// </summary>
        [DataField]
        public int PriorityMalus = -250;

        // Frontier: Large mail
        /// <summary>
        /// What's the bonus for delivering a large package intact?
        /// </summary>
        [DataField]
        public int LargeBonus = 500; //Goob; 5000 to 500

        /// <summary>
        /// What's the malus for failing to deliver a large package?
        /// </summary>
        [DataField]
        public int LargeMalus = -250; //DeltaV
        // End Frontier: Large mail
    }
}
