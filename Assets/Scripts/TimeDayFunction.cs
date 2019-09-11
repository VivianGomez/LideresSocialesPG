using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeDayFunction : MonoBehaviour
{
    public float inicio;
    public int hora;
    public int dia;
    public double ultimoSegundo;
    public TextMeshPro texto;
    public SpriteRenderer fondoEscena;
    public float red;
    public float green;
    public float blue;

    // Start is called before the first frame update
    void Start()
    {
        dia = 1;
        red = 1.0f;
        green = 1.0f;
        blue = 1.0f;
        inicio = Time.time;
        hora = 6;
        ultimoSegundo = 0.0;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Personaje");
        
            if (hora != 24)
            {
                float t = Time.time - inicio;
                string minutos = ((int)t / 60).ToString();
                string segundos = (t % 60).ToString("f1");
            //print(minutos + ":"+segundos);            

            Color color = new Color(red, green, blue, 1)
            {
                r = red,
                g = green,
                b = blue,
                a = 1
            };
            fondoEscena.color = color;

            double segundoActual = double.Parse(segundos);

                if (segundoActual != 0.0 && segundoActual % 12.50 == 0)
                {

                    double valorAbsoluto = Math.Abs(ultimoSegundo - segundoActual);

                    if (valorAbsoluto != 0)
                    {
                        hora = hora + 1;
                        //print(segundoActual);
                        //texto.text="Día " + dia + " - Hora: " + hora + ":00";
                        //print("cambia la hora, hora " + hora + ":00");
                        if (hora > 17)
                        {                           

                            fondoEscena.color = color;

                            red = red - 0.1f;
                            green = green - 0.1f;
                            blue = blue - 0.1f;
                        }
                        else if (hora == 17)
                    {

                        red = 0.8f;
                        green = 0.8f;
                        blue = 0.8f;
                    }
                    }

                    ultimoSegundo = segundoActual;
                }
                texto.text = "Día " + dia + " - Hora: " + hora + ":00";
            }
          

        
        
    }
}
