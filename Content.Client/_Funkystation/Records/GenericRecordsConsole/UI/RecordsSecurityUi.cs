using Content.Client.UserInterface.Fragments;
using Content.Shared._Funkystation.Records.GenericRecordsConsole;
using Content.Shared.CriminalRecords;
using Robust.Client.UserInterface;

namespace Content.Client._Funkystation.Records.GenericRecordsConsole.UI;

public sealed partial class RecordsSecurityUi : UIFragment
{
    private RecordsSecurityFragment? _fragment;

    public override Control GetUIFragmentRoot()
    {
        return _fragment!;
    }

    public override void Setup(BoundUserInterface userInterface, EntityUid? fragmentOwner)
    {
        if (fragmentOwner == null)
            return;

        _fragment = new RecordsSecurityFragment();

        // this isnt working rn
        _fragment.OnSetSecurityStatus += (status, reason) =>
        {
            userInterface.SendMessage(new CriminalRecordChangeStatus(status, reason));
        };
    }

    public override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is not GenericRecordsConsoleState consoleState)
            return;

        _fragment?.UpdateState(consoleState);
    }
}
