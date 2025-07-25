// SPDX-FileCopyrightText: 2024 Ed <96445749+TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 SaffronFennec <firefoxwolf2020@protonmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Traits;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.Lobby.UI.Roles;

[GenerateTypedNameReferences]
public sealed partial class TraitPreferenceSelector : Control
{
    public event Action<bool>? PreferenceChanged;

    private readonly CheckBox _checkbox;
    private readonly Label _costLabel;

    public int Cost
    {
        get => _cost;
        set
        {
            _cost = value;
            UpdateCostLabel();
        }
    }
    private int _cost;
    
        public bool Preference
    {
        get => _checkbox.Pressed;
        set => _checkbox.Pressed = value;
    }

    public TraitPreferenceSelector(string name, int cost, string? description = null)
    {
        RobustXamlLoader.Load(this);
        _checkbox = Checkbox;
        _costLabel = CostLabel;
        
        _checkbox.Text = name;
        Cost = cost;

        if (description != null)
            _checkbox.ToolTip = description;

        _checkbox.OnToggled += OnCheckBoxToggled;
        UpdateCostLabel();
    }

    private void UpdateCostLabel()
    {
        if (_cost == 0)
        {
            _costLabel.Text = "0";
            _costLabel.Modulate = Color.FromHex("#C8C8C8");
        }
        else
        {
            var sign = _cost >= 0 ? "+" : "";
            _costLabel.Text = $"{sign}{_cost}";
            _costLabel.Modulate = _cost >= 0 ? Color.FromHex("#FF4040") : Color.FromHex("#40FF40");
        }
    }

    private void OnCheckBoxToggled(BaseButton.ButtonToggledEventArgs args)
    {
        PreferenceChanged?.Invoke(args.Pressed);
    }
}
