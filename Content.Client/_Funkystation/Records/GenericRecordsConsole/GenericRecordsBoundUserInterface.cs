// SPDX-FileCopyrightText: 2025 corresp0nd <46357632+corresp0nd@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client._Funkystation.Records.GenericRecordsConsole.UI;
using Content.Shared._Funkystation.Records.GenericRecordsConsole;
using Content.Shared.CriminalRecords;
using Content.Shared.CriminalRecords.Components;
using Content.Shared.StationRecords;
using JetBrains.Annotations;

namespace Content.Client._Funkystation.Records.GenericRecordsConsole;

[UsedImplicitly]
public sealed class GenericRecordsBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [ViewVariables] private GenericRecordsMenu? _menu;
    [ViewVariables] private RecordsSecurityFragment? _security;

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not GenericRecordsConsoleState cast)
            return;

        _menu?.UpdateState(cast);
    }

    protected override void Open()
    {
        base.Open();

        _menu = new GenericRecordsMenu();
        _menu.OnClose += Close;

        _menu.OnListingItemSelected += meta =>
        {
            SendMessage(new GenericRecordsConsoleSelectMsg(meta?.CharacterRecordKey));

            // If we are a security records console, we also need to inform the criminal records
            // system of our state.
            if (_menu.IsSecurity() && meta?.StationRecordKey != null)
            {
                SendMessage(new SelectStationRecord(meta.Value.StationRecordKey.Value));
                _security.SetSecurityStatusEnabled(true);
            }
            else
            {
                // If the user does not have criminal records for some reason, we should not be able
                // to set their wanted status
                _security.SetSecurityStatusEnabled(false);
            }
        };

        _menu.OnFiltersChanged += (ty, txt) =>
        {
            SendMessage(txt == null
                ? new GenericRecordsConsoleFilterMsg(null)
                : new GenericRecordsConsoleFilterMsg(new StationRecordsFilter(ty, txt)));
        };

        _security.OnSetSecurityStatus += (status, reason) =>
        {
            SendMessage(new CriminalRecordChangeStatus(status, reason));
        };

        _menu.OpenCentered();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _menu?.Close();
    }

}
