// SPDX-FileCopyrightText: 2024 Hannah Giovanna Dawson <karakkaraz@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using System.Text.RegularExpressions;

namespace Content.Shared.Chat.V2.Moderation;

public sealed class RegexCensor(Regex censorInstruction) : IChatCensor
{
    private readonly Regex _censorInstruction = censorInstruction;

    public bool Censor(string input, out string output, char replaceWith = '*')
    {
        output = _censorInstruction.Replace(input, replaceWith.ToString());

        return !string.Equals(input, output);
    }
}
