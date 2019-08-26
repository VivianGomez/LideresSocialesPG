using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour
{
    public string levelToLoad;


    public void OnClick()
    {

        SceneManager.LoadSceneAsync(levelToLoad);
        //ls.LoadImageCanvas();
    }
}