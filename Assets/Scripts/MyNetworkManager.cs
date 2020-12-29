using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public List<string> players = new List<string>();

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("I connected");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();
        player.gameObject.name = $"Player {conn.connectionId}";
        player.SetDisplayName($"Player {numPlayers}");
        player.SetDisplayColor(Random.ColorHSV());
        players.Add($"Player {conn.connectionId}");
        Debug.Log($"Player {conn.connectionId} has been added to list");

    }

    

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        List<string> playersToRemove = new List<string>();
        foreach (NetworkConnectionToClient client in NetworkServer.connections.Values)
        {
            // Debug.Log(client.address);
            //  Debug.Log(client.connectionId);
            // Debug.Log(client.identity.ToString());
            if (players.Contains($"Player {client.connectionId}"))
            {
                playersToRemove.Add($"Player {client.connectionId}");
            }
        }
        foreach (string removePlayer in playersToRemove)
        {
            players.Remove(removePlayer);
            Debug.Log($"{removePlayer} has been removed from list");
        }

    }

}
