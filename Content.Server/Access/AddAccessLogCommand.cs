// SPDX-FileCopyrightText: 2023 AJCM-git <60196617+AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 MilenVolf <63782763+MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Shared.Access.Components;
using Content.Shared.Administration;
using Robust.Shared.Toolshed;
using Robust.Shared.Toolshed.Syntax;

namespace Content.Server.Access;

[ToolshedCommand, AdminCommand(AdminFlags.Mapping)]
public sealed class AddAccessLogCommand : ToolshedCommand
{
    [CommandImplementation]
    public void AddAccessLog(IInvocationContext ctx, EntityUid input, float seconds, string accessor)
    {
        var accessReader = EnsureComp<AccessReaderComponent>(input);

        var accessLogCount = accessReader.AccessLog.Count;
        if (accessLogCount >= accessReader.AccessLogLimit)
            ctx.WriteLine($"WARNING: Surpassing the limit of the log by {accessLogCount - accessReader.AccessLogLimit+1} entries!");

        var accessTime = TimeSpan.FromSeconds(seconds);
        accessReader.AccessLog.Enqueue(new AccessRecord(accessTime, accessor));
        ctx.WriteLine($"Successfully added access log to {input} with this information inside:\n " +
                      $"Time of access: {accessTime}\n " +
                      $"Accessed by: {accessor}");
    }

    [CommandImplementation]
    public void AddAccessLogPiped(IInvocationContext ctx, [PipedArgument] EntityUid input, float seconds, string accessor)
    {
        AddAccessLog(ctx, input, seconds, accessor);
    }
}
