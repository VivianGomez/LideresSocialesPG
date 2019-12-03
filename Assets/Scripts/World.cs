using UnityEngine;
using UnityEngine.SceneManagement;

public class World : SceneController
{
    public Transform player;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if (prevScene == "Sala" && currentScene=="Cocina" || prevScene == "Cocina" && currentScene == "Sala")
        {
            if (player != null)
            {
                if (GameObject.FindGameObjectWithTag("Cocina") != null) player.position = GameObject.FindGameObjectWithTag("Cocina").transform.position;
            }
        } 
        else if (prevScene == "Sala" && currentScene == "Cuarto" || prevScene == "Cuarto" && currentScene == "Sala")
        {
            if (player != null)
            {
                if (GameObject.FindGameObjectWithTag("Cuarto") != null) player.position = GameObject.FindGameObjectWithTag("Cuarto").transform.position;
            }
        }
        else if (prevScene == "Sala" && currentScene == "Calle" || prevScene == "Calle" && currentScene == "Sala")
        {
            if (player != null)
            {
                if (GameObject.FindGameObjectWithTag("Calle") != null) player.position = GameObject.FindGameObjectWithTag("Calle").transform.position;
            }
        }
        else if (prevScene == "Calle" && currentScene == "Coorporacion" || prevScene == "Coorporacion" && currentScene == "Calle")
        {
            if (player != null)
            {
                if (GameObject.FindGameObjectWithTag("HH") != null) player.position = GameObject.FindGameObjectWithTag("HH").transform.position;
            }
        }
        else if( prevScene == "Colegio" && currentScene == "Calle")
        {
            player.position = GameObject.FindGameObjectWithTag("Colegio").transform.position;
        }
    }

}