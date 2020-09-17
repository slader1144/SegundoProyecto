using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSate : MonoBehaviour
{
    public List<Player> Players { get; set; }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        Players = new List<Player>();
    }

    public Player GetPlayer(string playerId)
    {
        return Players.Find(p => p.Id == playerId);

    }

}

public class Player
{
    public string Id { get; set; }
    public bool Ready { get; set; }
    public string Nickname { get; set; }
    public Vector2 Axis;


    public Player(string id, int number)
    {
        this.Id = id;
        this.Nickname = "Jugador " + number;
        this.Axis = Vector2.zero;
    }
    public Player(string id, string nickname)
    {
        this.Id = id;
        this.Nickname = nickname;
        this.Axis = Vector2.zero;
    }

    public void SetAxis(float horizontal,float vertical)
    {
        this.Axis.x = horizontal;
        this.Axis.y = vertical;
    }
}
