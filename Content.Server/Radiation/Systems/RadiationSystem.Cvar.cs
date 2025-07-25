// SPDX-FileCopyrightText: 2022 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.CCVar;

namespace Content.Server.Radiation.Systems;

// cvar updates
public partial class RadiationSystem
{
    public float MinIntensity { get; private set; }
    public float GridcastUpdateRate { get; private set; }
    public bool GridcastSimplifiedSameGrid { get; private set; }
    public float GridcastMaxDistance { get; private set; }

    private void SubscribeCvars()
    {
        Subs.CVar(_cfg, CCVars.RadiationMinIntensity, radiationMinIntensity => MinIntensity = radiationMinIntensity, true);
        Subs.CVar(_cfg, CCVars.RadiationGridcastUpdateRate, updateRate => GridcastUpdateRate = updateRate, true);
        Subs.CVar(_cfg, CCVars.RadiationGridcastSimplifiedSameGrid, simplifiedSameGrid => GridcastSimplifiedSameGrid = simplifiedSameGrid, true);
        Subs.CVar(_cfg, CCVars.RadiationGridcastMaxDistance, maxDistance => GridcastMaxDistance = maxDistance, true);
    }
}
