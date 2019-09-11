using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeDayFunction : MonoBehaviour
{
    private float inicio;
    private int hora;
    private double ultimoSegundo;
    public TextMeshPro texto;
    public SpriteRenderer fondoEscena;
    float red;
    float green;
    float blue;

    // Start is called before the first frame update
    void Start()
    {
        red = 0.8f;
        green = 0.8f;
        blue = 0.8f;
        inicio = Time.time;
        hora = 6;
        ultimoSegundo = 0.0;
    }

    // Update is called once per frame
    void Update()
    {
        if(hora!= 24)
        {
            float t = Time.time - inicio;
            string minutos = ((int)t / 60).ToString();
            string segundos = (t % 60).ToString("f1");
            //print(minutos + ":"+segundos);

            double segundoActual = double.Parse(segundos);

            if (segundoActual != 0.0 && segundoActual % 12.50 == 0)
            {

                double valorAbsoluto = Math.Abs(ultimoSegundo - segundoActual);

                if (valorAbsoluto != 0)
                {
                    hora = hora + 1;
                    print(segundoActual);
                    texto.text="Día 1 - Hora: " + hora + ":00";
                    print("cambia la hora, hora " + hora + ":00");
                    if (hora > 17)
                    {
                        Color color = new Color(red, green, blue, 1)
                        {
                            r = red,
                            g = green,
                            b = blue,
                            a = 1
                        };

                        fondoEscena.color = color;

                        red = red - 0.1f;
                        green = green - 0.1f;
                        blue = blue - 0.1f;
                    }
                }

                ultimoSegundo = segundoActual;
            }
        }
        
    }
}
