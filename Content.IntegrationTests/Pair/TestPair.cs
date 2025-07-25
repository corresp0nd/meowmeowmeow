// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2024 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

#nullable enable
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Content.Server.GameTicking;
using Content.Shared.Players;
using Robust.Shared.Configuration;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.UnitTesting;

namespace Content.IntegrationTests.Pair;

/// <summary>
/// This object wraps a pooled server+client pair.
/// </summary>
public sealed partial class TestPair
{
    public readonly int Id;
    private bool _initialized;
    private TextWriter _testOut = default!;
    public readonly Stopwatch Watch = new();
    public readonly List<string> TestHistory = new();
    public PoolSettings Settings = default!;
    public TestMapData? TestMap;
    private List<NetUserId> _modifiedProfiles = new();

    private int _nextServerSeed;
    private int _nextClientSeed;

    public int ServerSeed;
    public int ClientSeed;

    public RobustIntegrationTest.ServerIntegrationInstance Server { get; private set; } = default!;
    public RobustIntegrationTest.ClientIntegrationInstance Client { get;  private set; } = default!;

    public void Deconstruct(
        out RobustIntegrationTest.ServerIntegrationInstance server,
        out RobustIntegrationTest.ClientIntegrationInstance client)
    {
        server = Server;
        client = Client;
    }

    public ICommonSession? Player => Server.PlayerMan.SessionsDict.GetValueOrDefault(Client.User!.Value);

    public ContentPlayerData? PlayerData => Player?.Data.ContentData();

    public PoolTestLogHandler ServerLogHandler { get;  private set; } = default!;
    public PoolTestLogHandler ClientLogHandler { get;  private set; } = default!;

    public TestPair(int id)
    {
        Id = id;
    }

    public async Task Initialize(PoolSettings settings, TextWriter testOut, List<string> testPrototypes)
    {
        if (_initialized)
            throw new InvalidOperationException("Already initialized");

        _initialized = true;
        Settings = settings;
        (Client, ClientLogHandler) = await PoolManager.GenerateClient(settings, testOut);
        (Server, ServerLogHandler) = await PoolManager.GenerateServer(settings, testOut);
        ActivateContext(testOut);

        Client.CfgMan.OnCVarValueChanged += OnClientCvarChanged;
        Server.CfgMan.OnCVarValueChanged += OnServerCvarChanged;

        if (!settings.NoLoadTestPrototypes)
            await LoadPrototypes(testPrototypes!);

        if (!settings.UseDummyTicker)
        {
            var gameTicker = Server.ResolveDependency<IEntityManager>().System<GameTicker>();
            await Server.WaitPost(() => gameTicker.RestartRound());
        }

        // Always initially connect clients to generate an initial random set of preferences/profiles.
        // This is to try and prevent issues where if the first test that connects the client is consistently some test
        // that uses a fixed seed, it would effectively prevent it from beingrandomized.

        Client.SetConnectTarget(Server);
        await Client.WaitIdleAsync();
        var netMgr = Client.ResolveDependency<IClientNetManager>();
        await Client.WaitPost(() => netMgr.ClientConnect(null!, 0, null!));
        await ReallyBeIdle(10);
        await Client.WaitRunTicks(1);

        if (!settings.ShouldBeConnected)
        {
            await Client.WaitPost(() => netMgr.ClientDisconnect("Initial disconnect"));
            await ReallyBeIdle(10);
        }

        var cRand = Client.ResolveDependency<IRobustRandom>();
        var sRand = Server.ResolveDependency<IRobustRandom>();
        _nextClientSeed = cRand.Next();
        _nextServerSeed = sRand.Next();
    }

    public void Kill()
    {
        State = PairState.Dead;
        ServerLogHandler.ShuttingDown = true;
        ClientLogHandler.ShuttingDown = true;
        Server.Dispose();
        Client.Dispose();
    }

    private void ClearContext()
    {
        _testOut = default!;
        ServerLogHandler.ClearContext();
        ClientLogHandler.ClearContext();
    }

    public void ActivateContext(TextWriter testOut)
    {
        _testOut = testOut;
        ServerLogHandler.ActivateContext(testOut);
        ClientLogHandler.ActivateContext(testOut);
    }

    public void Use()
    {
        if (State != PairState.Ready)
            throw new InvalidOperationException($"Pair is not ready to use. State: {State}");
        State = PairState.InUse;
    }

    public enum PairState : byte
    {
        Ready = 0,
        InUse = 1,
        CleanDisposed = 2,
        Dead = 3,
    }

    public void SetupSeed()
    {
        var sRand = Server.ResolveDependency<IRobustRandom>();
        if (Settings.ServerSeed is { } severSeed)
        {
            ServerSeed = severSeed;
            sRand.SetSeed(ServerSeed);
        }
        else
        {
            ServerSeed = _nextServerSeed;
            sRand.SetSeed(ServerSeed);
            _nextServerSeed = sRand.Next();
        }

        var cRand = Client.ResolveDependency<IRobustRandom>();
        if (Settings.ClientSeed is { } clientSeed)
        {
            ClientSeed = clientSeed;
            cRand.SetSeed(ClientSeed);
        }
        else
        {
            ClientSeed = _nextClientSeed;
            cRand.SetSeed(ClientSeed);
            _nextClientSeed = cRand.Next();
        }
    }
}
