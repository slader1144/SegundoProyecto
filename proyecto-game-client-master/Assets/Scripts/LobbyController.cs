using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public int MIN_PLAYERS = 2;
    public int MAX_PLAYERS = 2;

    Network networkController;
    GameSate GAME_STATE;

    // Start is called before the first frame update
    void Start()
    {
        GAME_STATE = GameObject.Find("GameState").GetComponent<GameSate>();


        networkController = GameObject.Find("SocketIO").GetComponent<Network>();
        networkController.CreateRoom();

        networkController.onRoomCreated += SetRoomCode;

        networkController.onPlayerEnter += OnPlayerEnter;
        networkController.onPlayerReady += OnPlayerReady;

    }

   
    void SetRoomCode(string roomCode)
    {
        InputField txtRoomCode = GameObject.Find("txtRoomCode").GetComponent<InputField>();
        txtRoomCode.text = roomCode;
    }


    void OnPlayerEnter(string playerId)
    {
        if (GAME_STATE.Players.Count <= this.MAX_PLAYERS)
        {
            Player player = new Player(playerId, GAME_STATE.Players.Count+1);
            GAME_STATE.Players.Add(player);
            GameObject.Find("SlotPlayer" + GAME_STATE.Players.Count).GetComponentInChildren<Text>().text = player.Nickname+" Conectado";
        }

    }

    void OnPlayerReady(string playerId)
    {
        var player = GAME_STATE.Players.Find(p => p.Id == playerId);
        player.Ready = true;
        int index = GAME_STATE.Players.IndexOf(player)+1;
        GameObject.Find("SlotPlayer" + index).GetComponentInChildren<Text>().text = player.Nickname+ " Listo";

        if (!GAME_STATE.Players.Exists(p => !p.Ready) && GAME_STATE.Players.Count == MAX_PLAYERS)
            this.gameObject.SendMessage("LoadScene", 2, SendMessageOptions.RequireReceiver);               

    }
}


