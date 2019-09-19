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
        GameObject camara = GameObject.Find("Main Camera");
        GameObject fondo = GameObject.Find("Background");
        camara.GetComponent<JSONWriter>().reescribirJSON(fondo.GetComponent<TimeDayFunction>().dia, new int[] { int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento1.text), int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento2.text), int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento3.text), int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento4.text), int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento5.text), int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento6.text), int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento7.text), int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento8.text) }, (int)camara.GetComponent<EnergyBar>().Energy, fondo.GetComponent<TimeDayFunction>().hora, false, fondo.GetComponent<TimeDayFunction>().inicio, fondo.GetComponent<TimeDayFunction>().ultimoSegundo, fondo.GetComponent<TimeDayFunction>().segundoActual, new string[] { "Arroz", "Huevos", "Agua", "Arroz con leche", "Chocolate", "Dulces", "Granos", "Pan de centeno" });


        if (cTrigger.tag != "Player") { return; }

        SceneManager.LoadSceneAsync(levelToLoad);
        //ls.LoadImageCanvas();
    }
}