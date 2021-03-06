/******************************************************************************
 * Class to connect player interactions to MLAPI lobby join/start.
 * 
 * Authors: Bill, Hamza, Max, Ryan
 *****************************************************************************/

using MLAPI;
using MLAPI.Transports.PhotonRealtime;
using TMPro;
using UnityEngine;

public class GameNetworkManager : MonoBehaviour
{
    [SerializeField]
    private GameObject UIManager;

    public string playerNickName = "";

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            // Default buttons for hosting and joining, replaced by custom buttons below
            //StartButtons();
        }
        else
        {
            //StatusLabels();

            //SubmitNewPosition();

            //QuitSession();
        }

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    static void SubmitNewPosition()
    {
        if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId,
                out var networkedClient))
            {
                //var player = networkedClient.PlayerObject.GetComponent<NetworkPlayer>();
                //if (player)
                //{
                //    //player.Move();
                //}
            }
        }
    }

    static void QuitSession()
    {
        if (GUILayout.Button("Quit")) NetworkManager.Singleton.StopClient();
    }

    //-------------------------------------------------------------------//

    public void Host()
    {
        //UIManager ui = GameObject.Find("UI Manager").GetComponent<UIManager>();
        ////UNetTransport manager = GameObject.Find("Network Manager").GetComponent<UNetTransport>();
        //Debug.Log(ui.ipBoxHost.GetComponent<TMP_InputField>().text);
        //if (ui.ipBoxHost.GetComponent<TMP_InputField>().text.Length <= 0)
        //{
        //    NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = "127.0.0.1";
        //}
        //else
        //{
        //    NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = ui.ipBoxHost.GetComponent<TMP_InputField>().text;
        //}
        
        UIManager ui = GameObject.Find("UI Manager").GetComponent<UIManager>();
        PhotonRealtimeTransport manager = GameObject.Find("Network Manager").GetComponent<PhotonRealtimeTransport>();

        manager.RoomName = ui.RoomNameHost.GetComponent<TMP_InputField>().text.ToString();
        playerNickName = ui.NickNameHost.GetComponent<TMP_InputField>().text.ToString();
        NetworkManager.Singleton.StartHost();
    }

    public void Join()
    {
        //UIManager ui = GameObject.Find("UI Manager").GetComponent<UIManager>();
        ////UNetTransport manager = GameObject.Find("Network Manager").GetComponent<UNetTransport>();
        //Debug.Log(ui.ipBoxClient.GetComponent<TMP_InputField>().text);
        //if (ui.ipBoxClient.GetComponent<TMP_InputField>().text.Length <= 0)
        //{
        //    NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = "127.0.0.1";
        //}
        //else
        //{
        //    NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = ui.ipBoxClient.GetComponent<TMP_InputField>().text;
        //}
        UIManager ui = GameObject.Find("UI Manager").GetComponent<UIManager>();
        PhotonRealtimeTransport manager = GameObject.Find("Network Manager").GetComponent<PhotonRealtimeTransport>();

        manager.RoomName = ui.RoomNameClient.GetComponent<TMP_InputField>().text.ToString();
        playerNickName = ui.NickNameClient.GetComponent<TMP_InputField>().text.ToString();

        NetworkManager.Singleton.StartClient();
    }
}
