using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using LitJson;

public class EnergyBar : MonoBehaviour
{
	public Scrollbar energyBar;
    private JsonData gameData;
    public float Energy = 100;
    private int tiempo=0;

    //private JSONLoader jsonLoader;
    
    private JSONLoaderJuego0 jsonLoader;

    private void Start()
    {

        if (!File.Exists(Application.dataPath + "/Gamedata.json"))
        {
            Energy = 100;
        }
        else
        {
            gameData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
            Energy = (float)((double.Parse(gameData[2].ToString()))); 
        }
    }

    void Awake()
    {
        jsonLoader = GameObject.FindObjectOfType<JSONLoaderJuego0>();
    }

    void Update()
    {
       print(tiempo);
       Hungry(0.0001f);
    }

    public void Hungry(float value)
	{
        GameObject fondo = GameObject.Find("Background");
        GameObject camara = GameObject.Find("Main Camera");
        int hora=fondo.GetComponent<TimeDayFunction>().hora;

        if (Energy>0)
        {
            Energy -= value;
            if(hora>22) jsonLoader.informationT.text = "Ya quiero ir a dormir...";
        }
        else
        {
            if(hora>22)
            {
                jsonLoader.informationT.text = "Tengo mucha hambre y sueño...";
            }
            else if(hora<22)
            {
                jsonLoader.informationT.text = "Tengo mucha hambre...";
            }
        }
        energyBar.size = Energy / 100f;
    }

    public void Comer(int indice)
    {
        Energy += (float)jsonLoader.darPorcentajeEnergiaAlimento(indice);
        energyBar.size = Energy / 100f;

        GameObject fondo = GameObject.Find("Background");
        GameObject camara = GameObject.Find("Main Camera");

        camara.GetComponent<JSONWriter>().reescribirJSON(
               fondo.GetComponent<TimeDayFunction>().dia,
               new int[] { int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento1.text),
                int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento2.text),
                int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento3.text),
                int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento4.text),
                int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento5.text),
                int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento6.text),
                int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento7.text),
                int.Parse(camara.GetComponent<JSONLoaderJuego0>().cantidadAlimento8.text) },
               (int)camara.GetComponent<EnergyBar>().Energy, fondo.GetComponent<TimeDayFunction>().hora,
               gameData[4].ToString(),
               gameData[5].ToString(),
               gameData[6].ToString(),
               fondo.GetComponent<TimeDayFunction>().inicio,
               fondo.GetComponent<TimeDayFunction>().ultimoSegundo,
               fondo.GetComponent<TimeDayFunction>().segundoActual,
               new string[] { "Arroz", "Huevos", "Agua", "Arroz con leche", "Chocolate", "Dulces", "Granos", "Pan de centeno" });


    }

}

