﻿using UnityEngine;

public class World : SceneController
{
    public Transform player;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if (prevScene == "Sala" && currentScene=="Cocina")
        {
            player.position = GameObject.FindGameObjectWithTag("Cocina").transform.position;
        } 
        else if(prevScene == "Cocina" && currentScene == "Sala")
        {
            player.position = GameObject.FindGameObjectWithTag("Cocina").transform.position;
        } 
    }

}