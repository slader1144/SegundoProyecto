using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;

public delegate void MessageDelegate(string mensagge);
public delegate void InputDelegate(string playerId,PlayerInputData input);

public class Network : MonoBehaviour
{
    public SocketIOController socket;

    public event MessageDelegate onConnectedToServer;
    public event MessageDelegate onRoomCreated;
    public event MessageDelegate onPlayerEnter;
    public event MessageDelegate onPlayerReady;
    public event InputDelegate onPlayerInput;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);   
    }

    public void ConnectedToServer()
    {
        
        socket.On("onConnection", OnConnection);
        socket.On("roomCreated", RoomCreated);
        socket.On("playerEnter", PlayerEnter);
        socket.On("playerReady", PlayerReady);
        socket.On("playerInput", PlayerInput);
        socket.Connect();
        
    }



    void OnConnection(SocketIOEvent evt)
    {

        JsonData data = JsonUtility.FromJson<JsonData>(evt.data);
        string message = data.message;
        Debug.Log(message);
        onConnectedToServer(message);        
      
    }

    public void CreateRoom()
    {
        Debug.Log("Create Room");

        RoomOptions roomOptions = new RoomOptions(2, 2, "Mi sala");
        socket.Emit("createRoom", JsonUtility.ToJson(roomOptions));
    }
    public void GameReady()
    {
        Debug.Log("Game Ready");
        socket.Emit("gameReady");
    }


    void RoomCreated (SocketIOEvent evt)
    {
        JsonData data = JsonUtility.FromJson<JsonData>(evt.data);
        string roomCode = data.room;
        Debug.Log("Room Created: "+ roomCode);

        

        onRoomCreated(roomCode);

    }
    void PlayerEnter(SocketIOEvent evt)
    {
        JsonData data = JsonUtility.FromJson<JsonData>(evt.data);
        string playerId = data.playerId;
        onPlayerEnter(playerId);

    }
    void PlayerReady(SocketIOEvent evt)
    {
        JsonData data = JsonUtility.FromJson<JsonData>(evt.data);
  
        string playerId =data.playerId;
        onPlayerReady(playerId);
    }

    void PlayerInput(SocketIOEvent evt)
    {
        Debug.Log("data:"+evt.data);
        PlayerInputData inputData = JsonUtility.FromJson<PlayerInputData>(evt.data);

        string playerId = inputData.playerId;
    

        onPlayerInput(playerId, inputData);
    }


}

class JsonData
{
    public string message;
    public string room;
    public string playerId;
}

public class PlayerInputData
{
    public string playerId;
    public string command;
    public float axisHorizontal;
    public float axisVertical;


    override
    
        public string ToString()
    {
        if (command == "MOVE")
        {

            return "Vertical:" + axisVertical + " Horizontal: " + axisHorizontal;
        
        } else if (command=="UP") {
            
            return "Vertical:" + axisVertical + " Horizontal: " + axisHorizontal;
        }
        else if (command == "DOWN")
        {
            
            return "Vertical:" + axisVertical + " Horizontal: " + axisHorizontal;
        }
        return command;
    }

}

class RoomOptions
{
    public int MinPlayers;
    public int MaxPlayers;
    public string Name;

    public RoomOptions(int min, int max, string name)
    {
        MinPlayers = min;
        MaxPlayers = max;
        Name = name;
    }
}
