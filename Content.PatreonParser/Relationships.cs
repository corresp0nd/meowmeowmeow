// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using System.Text.Json.Serialization;

namespace Content.PatreonParser;

public sealed class Relationships
{
    [JsonPropertyName("currently_entitled_tiers")]
    public CurrentlyEntitledTiers CurrentlyEntitledTiers = default!;
}
