using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;
using LitJson;

public class JSONLoader : MonoBehaviour
{
    private string jsonString;
    private JsonData jData;

    public SpriteRenderer jugadorSprite;
    public SpriteRenderer backgroundSprite;
 
    void Start()
    {
        string path=Application.dataPath + "/Resources/data.json";
        if (File.Exists(path))
        {
           jsonString = File.ReadAllText(path);
           jData = JsonMapper.ToObject(jsonString);

           LoadSprite(""+jData["personaje"]["imagenPersonaje"]);
           LoadScene(jData["escenas"]);
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

        private void LoadSprite (string path)
        {
            jugadorSprite.sprite = Resources.Load<Sprite>(path);
        }

        private void LoadScene(JsonData escenas)
        {
            
            for (int i = 0; i < escenas.Count; i++)
            {
               var actual = escenas[i];
               //Debug.Log(SceneManager.GetActiveScene().name);
               if((""+actual["nombreEscena"]).Equals(""+SceneManager.GetActiveScene().name))
               {
                    string pathBG = ""+actual["imagenFondo"];
                    backgroundSprite.sprite = Resources.Load<Sprite>(pathBG);
               }
            }
        }
}
