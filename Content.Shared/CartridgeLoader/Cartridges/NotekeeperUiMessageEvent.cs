// SPDX-FileCopyrightText: 2022 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 2023 MishaUnity <81403616+MishaUnity@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.Cartridges;

[Serializable, NetSerializable]
public sealed class NotekeeperUiMessageEvent : CartridgeMessageEvent
{
    public readonly NotekeeperUiAction Action;
    public readonly string Note;

    public NotekeeperUiMessageEvent(NotekeeperUiAction action, string note)
    {
        Action = action;
        Note = note;
    }
}

[Serializable, NetSerializable]
public enum NotekeeperUiAction
{
    Add,
    Remove
}
