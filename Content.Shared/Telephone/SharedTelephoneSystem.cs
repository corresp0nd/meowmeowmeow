// SPDX-FileCopyrightText: 2024 MilenVolf <63782763+MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;

namespace Content.Shared.Telephone;

public abstract class SharedTelephoneSystem : EntitySystem
{
    public bool IsTelephoneEngaged(Entity<TelephoneComponent> entity)
    {
        return entity.Comp.LinkedTelephones.Any();
    }

    public string GetFormattedCallerIdForEntity(string? presumedName, string? presumedJob, Color fontColor, string fontType = "Default", int fontSize = 12)
    {
        var callerId = Loc.GetString("chat-telephone-unknown-caller",
            ("color", fontColor),
            ("fontType", fontType),
            ("fontSize", fontSize));

        if (presumedName == null)
            return callerId;

        if (presumedJob != null)
            callerId = Loc.GetString("chat-telephone-caller-id-with-job",
                ("callerName", presumedName),
                ("callerJob", presumedJob),
                ("color", fontColor),
                ("fontType", fontType),
                ("fontSize", fontSize));

        else
            callerId = Loc.GetString("chat-telephone-caller-id-without-job",
                ("callerName", presumedName),
                ("color", fontColor),
                ("fontType", fontType),
                ("fontSize", fontSize));

        return callerId;
    }

    public string GetFormattedDeviceIdForEntity(string? deviceName, Color fontColor, string fontType = "Default", int fontSize = 12)
    {
        if (deviceName == null)
        {
            return Loc.GetString("chat-telephone-unknown-device",
                ("color", fontColor),
                ("fontType", fontType),
                ("fontSize", fontSize));
        }

        return Loc.GetString("chat-telephone-device-id",
            ("deviceName", deviceName),
            ("color", fontColor),
            ("fontType", fontType),
            ("fontSize", fontSize));
    }
}
