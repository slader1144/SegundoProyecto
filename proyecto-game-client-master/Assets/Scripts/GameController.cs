using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Commands {
    NONE,
    UP,
    DOWN,
    MOVE
}


public class GameController : MonoBehaviour
{
    GameSate GAME_STATE;

    Network networkController;

    public GameObject PlayerPrefab;
    public Transform PlayersContainer;
    public int speed = 10;




    Dictionary<string,GameObject> PlayersGameObjects= new Dictionary<string,GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GAME_STATE = GameObject.Find("GameState").GetComponent<GameSate>();

        networkController = GameObject.Find("SocketIO").GetComponent<Network>();
        networkController.onPlayerInput+= OnPlayerInput;

        networkController.GameReady();

        PlayersGameObjects = new Dictionary<string, GameObject>();
        InstantiatePlayer();
    }


    void InstantiatePlayer()
    {
        PlayersGameObjects.Add(GAME_STATE.Players[0].Id,Instantiate(PlayerPrefab,new Vector3(-8.5f,0,0),new Quaternion() ,PlayersContainer));
        PlayersGameObjects.Add(GAME_STATE.Players[1].Id, Instantiate(PlayerPrefab, new Vector3(8.5f, 0, 0), new Quaternion(), PlayersContainer));
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = Vector2.zero;
        //Player 1
        Player player1 = GAME_STATE.Players[0];
        Transform player1Transform = PlayersGameObjects[player1.Id].transform;
        dir.y = player1.Axis.y;
        player1Transform.Translate(player1.Axis * speed*Time.deltaTime);

        //Player 2
        Player player2 = GAME_STATE.Players[1];
        Transform player2Transform = PlayersGameObjects[player2.Id].transform;
        dir.y = player2.Axis.y;
        
        player2Transform.Translate(dir * speed*Time.deltaTime);



    }

    void OnPlayerInput(string playerId, PlayerInputData input)
    {
        //GameObject playerGameObject = Players[playerId];
        proccessCommand(playerId, input);
    }

    void proccessCommand(string playerId,PlayerInputData input)
    {
        Debug.Log("command: " + input.command);

        Player player = GAME_STATE.GetPlayer(playerId);

        Commands playerCommand = Commands.NONE;

        Enum.TryParse(input.command, out playerCommand);

        switch (playerCommand)
        {
            case Commands.UP:
                Debug.Log("Player: " + player.Nickname + " Press UP");
                input.axisHorizontal = 0;
                input.axisVertical = 1;
                player.SetAxis(input.axisHorizontal, input.axisVertical);
                break;
            case Commands.DOWN:
                Debug.Log("Player: " + player.Nickname + " Press DOWN");
                input.axisHorizontal = 0;
                input.axisVertical = -1;
                player.SetAxis(input.axisHorizontal, input.axisVertical);
                break;
            case Commands.MOVE:
                Debug.Log("Player: " + player.Nickname + " Axis: " + input);
                player.SetAxis(input.axisHorizontal, input.axisVertical);
                break;
            default:
                Debug.Log("Player: " + player.Nickname + " Press NONE");
                break;
        }
    }
}
