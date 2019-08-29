using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;
using LitJson;

public class JSONLoader : MonoBehaviour
{
    private string jsonString;
    private JsonData jData;

    public GameObject jugador;
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
            jugador.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);

            Vector2 S = jugador.GetComponent<SpriteRenderer>().sprite.bounds.size;
            jugador.GetComponent<BoxCollider2D>().size = S;
            //jugador.GetComponent<BoxCollider2D>().offset = new Vector2 ((S.x / 2), 0);
        }

        private void LoadScene(JsonData escenas)
        {
            var actual = escenas[0];
            var interaccionActual = escenas[0];
            for (int i = 0; i < escenas.Count; i++)
            {
               actual = escenas[i];
               //Debug.Log(SceneManager.GetActiveScene().name);
               if((""+actual["nombreEscena"]).Equals(""+SceneManager.GetActiveScene().name))
               {
                    string pathBG = ""+actual["imagenFondo"];
                    backgroundSprite.sprite = Resources.Load<Sprite>(pathBG);

                    for (int j = 0; j < actual["interacciones"].Count; j++)
                    {
                        interaccionActual = actual["interacciones"][j];
                        createCollidersInteractions(""+interaccionActual["colliderActivador"], interaccionActual["coordenadasCollider"], interaccionActual["offsetCollider"], interaccionActual["tamCollider"]);
                    }
               }             
            }
        }

        private void createCollidersInteractions(string nombreActivador, JsonData posicion, JsonData offset, JsonData tamanio){
            GameObject objToSpawn;
            objToSpawn = new GameObject(nombreActivador);
            float x = float.Parse(""+posicion[0]);
            float y = float.Parse(""+posicion[1]);
            float z = float.Parse(""+posicion[2]);

            float xO = float.Parse(""+offset[0]);
            float yO = float.Parse(""+offset[1]);

            float xT = float.Parse(""+tamanio[0]);
            float yT = float.Parse(""+tamanio[1]);

            objToSpawn.transform.position = new Vector3(x,y,z);

            //Add Components
            objToSpawn.AddComponent<BoxCollider2D>();
            var coll = objToSpawn.GetComponent<BoxCollider2D>();
            coll.isTrigger = true;
            //define el centro del collider
            coll.offset = new Vector2(xO,yO);
            //define el tamanio del collider
            coll.size = new Vector2(xT,yT);
        }
}
