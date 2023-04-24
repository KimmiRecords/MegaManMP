using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class NetworkPlayer : NetworkBehaviour, INetworkRunnerCallbacks
{
    public static NetworkPlayer Local { get; private set; }
    //public static int playerCount;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            Debug.Log("[custom msg] spawned own player");
        }
        else
        {
            Debug.Log("[custom msg] spawned other player");

        }
        base.Spawned();
        GameManager.Instance.PlayerCount++;
        Debug.Log("[custom msg] hay " + GameManager.Instance.PlayerCount + " players");

    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
        GameManager.Instance.PlayerCount--;
        Debug.Log("[custom msg] despawned. hay " + GameManager.Instance.PlayerCount + " players");
    }

    #region
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        //throw new NotImplementedException();
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        //GameManager.Instance.playerCount--;
        //Debug.Log("[custom msg] se fue un player. ahora hay " + GameManager.Instance.playerCount + " players");
    }


    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        //throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        //GameManager.Instance.playerCount--;
        //Debug.Log("[custom msg] onshutdown. ahora hay " + GameManager.Instance.playerCount + " players");
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        //throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        //GameManager.Instance.playerCount--;
        //Debug.Log("[custom msg] ondisconnected. ahora hay " + GameManager.Instance.playerCount + " players");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        //throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
       // throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        //throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        //throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        //throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        //throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
       // throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
       // throw new NotImplementedException();
    }

    #endregion //unused callbacks
}
