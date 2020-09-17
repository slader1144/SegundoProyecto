using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InicioController : MonoBehaviour
{

    Network networkController;


    void Start()
    {
        networkController = GameObject.Find("SocketIO").GetComponent<Network>();
        networkController.onConnectedToServer += OnConnectedToServer;
        networkController.ConnectedToServer();
    }

    void OnConnectedToServer(string mensagge)
    {
        Button btnEntrar = GameObject.Find("BtnEntrar").GetComponent<Button>();
        btnEntrar.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
