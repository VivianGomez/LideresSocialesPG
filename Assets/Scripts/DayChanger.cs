﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayChanger : MonoBehaviour
{
    public Animator animator;

    private void Update()
    {
        GameObject p = GameObject.Find("Personaje");
        if (p.GetComponent<Movimiento>().trigger)
        {
            //print("entra a transicion");
            animator.SetTrigger("fade");
        }
        else
        {
            animator.SetTrigger("returnn");
        }
    }
       

    public void onFadeComplete()
    {
        GameObject fondo = GameObject.Find("Background");
        GameObject camara = GameObject.Find("Main Camera");
        

        fondo.GetComponent<TimeDayFunction>().dia = fondo.GetComponent<TimeDayFunction>().dia + 1;
        fondo.GetComponent<TimeDayFunction>().red = 1;
        fondo.GetComponent<TimeDayFunction>().green = 1;
        fondo.GetComponent<TimeDayFunction>().blue = 1;
        fondo.GetComponent<TimeDayFunction>().inicio = Time.time;
        fondo.GetComponent<TimeDayFunction>().hora = 6;
        fondo.GetComponent<TimeDayFunction>().ultimoSegundo = 0.0;
        print("se aumento el dia");

        camara.GetComponent<JSONWriter>().reescribirJSON(fondo.GetComponent<TimeDayFunction>().dia, new int[] { 0, 0, 0, 0 }, (int)camara.GetComponent<EnergyBar>().Energy);

        
        

    }

    public void pasarEstado()
    {
        GameObject p = GameObject.Find("Personaje");
        p.GetComponent<Movimiento>().trigger = false;
    }
}