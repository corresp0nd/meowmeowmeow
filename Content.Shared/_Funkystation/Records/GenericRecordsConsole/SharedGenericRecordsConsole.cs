// SPDX-FileCopyrightText: 2025 Lyndomen <49795619+Lyndomen@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 corresp0nd <46357632+corresp0nd@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Security;
using Content.Shared.StationRecords;
using Robust.Shared.Serialization;

namespace Content.Shared._Funkystation.Records.GenericRecordsConsole;

[Serializable, NetSerializable]
public enum GenericRecordsConsoleKey : byte
{
    Key
}

[Serializable, NetSerializable]
public enum RecordConsoleType : byte
{
    Security,
    Medical,
    Employment,
    /// <summary>
    /// Admin console has the functionality of all other types and has some additional admin related functionality
    /// </summary>
    Admin
}

[Serializable, NetSerializable]
public sealed class GenericRecordsConsoleState : BoundUserInterfaceState
{
    [Serializable, NetSerializable]
    public struct CharacterInfo
    {
        public string CharacterDisplayName;
        public uint? StationRecordKey;
    }

    /// <summary>
    /// The current type of the console: medical, security, employment, etc
    /// </summary>
    public RecordConsoleType ConsoleType { get; set; }

    /// <summary>
    /// Character selected in the console
    /// </summary>
    public uint? SelectedIndex { get; set; } = null;

    /// <summary>
    /// List of names+station record keys to display in the listing
    /// </summary>
    public Dictionary<uint, CharacterInfo>? CharacterList { get; set; }

    /// <summary>
    /// The contents of the selected record
    /// </summary>
    public FullCharacterRecords? SelectedRecord { get; set; } = null;

    public StationRecordsFilter? Filter { get; set; } = null;

    /// <summary>
    /// Security status of the selected record
    /// </summary>
    public (SecurityStatus, string?)? SelectedSecurityStatus = null;
}

[Serializable, NetSerializable]
public sealed class GenericRecordsConsoleFilterMsg : BoundUserInterfaceMessage
{
    public readonly StationRecordsFilter? Filter;

    public GenericRecordsConsoleFilterMsg(StationRecordsFilter? filter)
    {
        Filter = filter;
    }
}

[Serializable, NetSerializable]
public sealed class GenericRecordsConsoleSelectMsg : BoundUserInterfaceMessage
{
    public readonly uint? CharacterRecordKey;

    public GenericRecordsConsoleSelectMsg(uint? recordsKey)
    {
        CharacterRecordKey = recordsKey;
    }
}
