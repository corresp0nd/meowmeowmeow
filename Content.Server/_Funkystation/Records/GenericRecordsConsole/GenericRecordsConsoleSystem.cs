// SPDX-FileCopyrightText: 2025 corresp0nd <46357632+corresp0nd@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Station.Systems;
using Content.Server.StationRecords;
using Content.Server.StationRecords.Systems;
using Content.Shared._Funkystation.Records;
using Content.Shared._Funkystation.Records.GenericRecordsConsole;
using Content.Shared.CriminalRecords;
using Content.Shared.Security;
using Content.Shared.StationRecords;
using Robust.Server.GameObjects;

namespace Content.Server._Funkystation.Records.GenericRecordsConsole;

public sealed class GenericRecordsConsoleSystem : EntitySystem
{
    [Dependency] private readonly CharacterRecordsSystem _characterRecords = default!;
    [Dependency] private readonly StationSystem _station = default!;
    [Dependency] private readonly StationRecordsSystem _records = default!;
    [Dependency] private readonly UserInterfaceSystem _ui = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GenericRecordsConsoleComponent, CharacterRecordsModifiedEvent>((uid, component, _) =>
            UpdateUi(uid, component));

        Subs.BuiEvents<GenericRecordsConsoleComponent>(GenericRecordsConsoleKey.Key,
            subr =>
            {
                subr.Event<BoundUIOpenedEvent>((uid, component, _) => UpdateUi(uid, component));
                subr.Event<GenericRecordsConsoleSelectMsg>(OnKeySelect);
                subr.Event<GenericRecordsConsoleFilterMsg>(OnFilterApplied);
            });
    }

    private void OnFilterApplied(Entity<GenericRecordsConsoleComponent> ent, ref GenericRecordsConsoleFilterMsg msg)
    {
        ent.Comp.Filter = msg.Filter;
        UpdateUi(ent);
    }

    private void OnKeySelect(Entity<GenericRecordsConsoleComponent> ent, ref GenericRecordsConsoleSelectMsg msg)
    {
        ent.Comp.SelectedIndex = msg.CharacterRecordKey;
        UpdateUi(ent);
    }

    private void UpdateUi(EntityUid entity, GenericRecordsConsoleComponent? console = null)
    {
        if (!Resolve(entity, ref console))
            return;

        var station = _station.GetOwningStation(entity);
        if (!HasComp<StationRecordsComponent>(station) || !HasComp<CharacterRecordsComponent>(station))
            return;

        var characterRecords = _characterRecords.QueryRecords(station.Value);
        // Get the name and station records key display from the list of records
        var names = new Dictionary<uint, GenericRecordsConsoleState.CharacterInfo>();
        foreach (var (i, r) in characterRecords)
        {
            var nameJob = $"{r.Name} ({r.JobTitle})";

            // Apply any filter the user has set
            if (console.Filter != null)
            {
                if (IsSkippedRecord(console.Filter, r, nameJob))
                    continue;
            }

            if (names.ContainsKey(i))
            {
                Log.Error(
                    $"We somehow have duplicate character record keys, NetEntity: {i}, Entity: {entity}, Character Name: {r.Name}");
            }

            names[i] = new GenericRecordsConsoleState.CharacterInfo
                { CharacterDisplayName = nameJob, StationRecordKey = r.StationRecordsKey };
        }

        var record =
            console.SelectedIndex == null || !characterRecords.TryGetValue(console.SelectedIndex!.Value, out var value)
                ? null
                : value;

        (SecurityStatus, string?)? securityStatus = null;

        // If we need the character's security status, gather it from the criminal records
        if ((console.ConsoleType == RecordConsoleType.Admin ||
             console.ConsoleType == RecordConsoleType.Security)
            && record?.StationRecordsKey != null)
        {
            var key = new StationRecordKey(record.StationRecordsKey.Value, station.Value);
            if (_records.TryGetRecord<CriminalRecord>(key, out var entry))
                securityStatus = (entry.Status, entry.Reason);
        }

        SendState(entity,
            new GenericRecordsConsoleState
            {
                CharacterList = names,
                SelectedIndex = console.SelectedIndex,
                SelectedRecord = record,
                Filter = console.Filter,
                ConsoleType = console.ConsoleType,
                SelectedSecurityStatus = securityStatus,
            });
    }

    private void SendState(EntityUid entity, GenericRecordsConsoleState state)
    {
        _ui.SetUiState(entity, GenericRecordsConsoleKey.Key, state);
    }

    /// <summary>
    /// Almost exactly the same as <see cref="StationRecordsSystem.IsSkipped"/>
    /// </summary>
    private static bool IsSkippedRecord(StationRecordsFilter filter,
        FullCharacterRecords record,
        string nameJob)
    {
        var isFilter = filter.Value.Length > 0;

        if (!isFilter)
            return false;

        var filterLowerCaseValue = filter.Value.ToLower();

        return filter.Type switch
        {
            StationRecordFilterType.Name =>
                !nameJob.Contains(filterLowerCaseValue, StringComparison.CurrentCultureIgnoreCase),
            StationRecordFilterType.Prints => record.Fingerprint != null
                && IsFilterWithSomeCodeValue(record.Fingerprint, filterLowerCaseValue),
            StationRecordFilterType.DNA => record.DNA != null
                                                && IsFilterWithSomeCodeValue(record.DNA, filterLowerCaseValue),
            _ => throw new ArgumentOutOfRangeException(nameof(filter), "Invalid Character Record filter type"),
        };
    }

    private static bool IsFilterWithSomeCodeValue(string value, string filter)
    {
        return !value.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase);
    }
}
