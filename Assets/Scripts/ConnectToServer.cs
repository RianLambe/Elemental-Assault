using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public GameObject menus;
    public TMP_Text hostInput;
    public TMP_Text joinInput;
    public TMP_Text roomName;



    private void Awake() {
        PhotonNetwork.OfflineMode = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        Debug.Log("Joined lobby");
        menus.GetComponent<MainMenuScript>().ChangePage(menus.GetComponent<MainMenuScript>().titlePanel);
    }

    public override void OnJoinedRoom() {
        Debug.Log("You joined a new room : " + PhotonNetwork.CurrentRoom.Name +  " (" + PhotonNetwork.CurrentRoom.PlayerCount + ")");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            menus.GetComponent<MainMenuScript>().ChangePage(menus.GetComponent<MainMenuScript>().gamePanel);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            Debug.Log("Two players have joined the game and it is ready to start!");
            FindObjectOfType<Canvas>().gameObject.GetComponent<GameControllerScript>().photonView.RPC("PlayGame", RpcTarget.MasterClient);
            menus.GetComponent<MainMenuScript>().ChangePage(menus.GetComponent<MainMenuScript>().gamePanel);
        }
    }

    public void PlayOffline() {
        if (PhotonNetwork.IsConnected) {
            //PhotonNetwork.Disconnect();
            //PhotonNetwork.OfflineMode = true;
        }
        else {
            PhotonNetwork.OfflineMode = true;
        }

    }

    public void CreateRoom() {
        PhotonNetwork.CreateRoom(hostInput.text);
        Debug.Log("Created room : " + hostInput.text);
        roomName.text = "Game name : " + hostInput.text;
    }

    public void JoinRoom() {
        PhotonNetwork.JoinRoom(joinInput.text);
        Debug.Log("Joined room : " + joinInput.text);
    }

    public void CreateLocalRoom() {
        PhotonNetwork.JoinRoom("Local");
    }
}
