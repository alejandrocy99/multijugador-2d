using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConectionManager : MonoBehaviourPunCallbacks
{

    public TextMeshProUGUI conexion;
    public Button connectButton;
    public TextMeshProUGUI playernum;
    private void Start()
    {
        
        connectButton.gameObject.SetActive(false);
        // Start the connection
        PhotonNetwork.ConnectUsingSettings();
        conexion.text = "Conectando...";
    }
    override
    public void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnected)
        {
            conexion.text = "Conectado";
            connectButton.gameObject.SetActive(true);
        }
        else
        {
            conexion.text = "No se pudo conectar al servidor";
        }
    
    }

    public void ButtonConnect(){
        RoomOptions options = new RoomOptions() {MaxPlayers = 4};
        PhotonNetwork.JoinOrCreateRoom("room1", options, TypedLobby.Default);
        
    }
    override
    public void OnJoinedRoom()
    {
        Debug.Log("Joined room" + PhotonNetwork.CurrentRoom.Name);
        conexion.text = "Conectado a la sala " + PhotonNetwork.CurrentRoom.Name;
        Debug.Log("Players in room: " + PhotonNetwork.CurrentRoom.PlayerCount);
        playernum.text = "Jugadores en la sala: " + PhotonNetwork.CurrentRoom.PlayerCount;
    }
}
