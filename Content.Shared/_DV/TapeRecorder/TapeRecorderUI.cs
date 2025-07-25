// SPDX-FileCopyrightText: 2025 IronDragoon <8961391+IronDragoon@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared._DV.TapeRecorder;

[Serializable, NetSerializable]
public enum TapeRecorderVisuals : byte
{
    Mode,
    TapeInserted
}

[Serializable, NetSerializable]
public enum TapeRecorderMode : byte
{
    Stopped,
    Recording,
    Playing,
    Rewinding
}

[Serializable, NetSerializable]
public enum TapeRecorderUIKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class ChangeModeTapeRecorderMessage(TapeRecorderMode mode) : BoundUserInterfaceMessage
{
    public TapeRecorderMode Mode = mode;
}

[Serializable, NetSerializable]
public sealed class PrintTapeRecorderMessage : BoundUserInterfaceMessage;

[Serializable, NetSerializable]
public sealed class TapeRecorderState : BoundUserInterfaceState
{
    // TODO: check the itemslot on client instead of putting easy casette stuff in the state
    public bool HasCasette;
    public bool HasData;
    public float CurrentTime;
    public float MaxTime;
    public string CassetteName;
    public TimeSpan PrintCooldown;

    public TapeRecorderState(
        bool hasCasette,
        bool hasData,
        float currentTime,
        float maxTime,
        string cassetteName,
        TimeSpan printCooldown)
    {
        HasCasette = hasCasette;
        HasData = hasData;
        CurrentTime = currentTime;
        MaxTime = maxTime;
        CassetteName = cassetteName;
        PrintCooldown = printCooldown;
    }
}
