using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string levelToLoad;
    //public LoadSceneTransition ls;


    public void OnTriggerEnter2D(Collider2D cTrigger)
    {

        if (cTrigger.tag != "Player") { return; }

        SceneManager.LoadSceneAsync(levelToLoad);
        //ls.LoadImageCanvas();
    }
}