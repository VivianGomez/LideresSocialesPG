﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    Vector3 escala;
    float escalaX;
    float escalaY;
    bool permiteMoverse = true;
    public GameObject animacion;
    AnimationLoadManager animationLoadManager;
    public int diaActual;
    public GameObject objeto;
    public bool trigger;
    public GameObject imagen;


    void Start()
    {
        trigger = false;
        objeto.SetActive(false);
        imagen.SetActive(false);
        escala = transform.localScale;
        escalaX = escala.x;
        escalaY = escala.y;
    }

    void Update()
    {
        GameObject player = GameObject.Find("Personaje");

        transform.Translate(Input.GetAxis("Horizontal") * 5f * Time.deltaTime, 0f, 0f);
        transform.Translate(0f, Input.GetAxis("Vertical") * 5f * Time.deltaTime, 0f);

        if (Input.GetAxis("Horizontal") < 0)
        {
            escala.x = -escalaX;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            escala.x = escalaX;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            escala.y = escalaY;
        }
        transform.localScale = escala;
    }

    void loadAnimation()
    {
        GameObject tempObj = new GameObject();
        tempObj = Resources.Load("an", typeof(GameObject)) as GameObject;
        if (tempObj == null)
        {
            Debug.LogError("No esta encontrando el clip de la animacion ");
        }
        else
        {
            Animation anim = animacion.GetComponent<Animation>();

            Animation animation = new Animation();
            animation = tempObj.GetComponent<Animation>();
            AnimationClip animationClip = new AnimationClip();
            animationClip = animation.clip;


            if (anim != null)
            {
                if (animationClip != null)
                {
                    anim.AddClip(animationClip, "animation");
                    anim.Play("animation");
                }
                else
                {
                    print("objeto animacion esta bien y clip es nulo");
                }
            }
            else
            {
                if (animationClip != null)
                {
                    print("Objeto animacion es nulo pero clip encontrado");
                }
                else
                {
                    print("todo es nulo");
                }

            }
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject fondo = GameObject.Find("Background");
        Debug.Log("Colisión con " + col.name);

        if (col.name== "sofa" && fondo.GetComponent<TimeDayFunction>().hora>8)
        {
            imagen.SetActive(true);
            if (imagen != null)
            {
                //StartCoroutine(imagen.GetComponent<VideoStream>().playVideo());
            }
            else print("es nula");
            
            trigger = true;
            objeto.SetActive(true);
            imagen.SetActive(false);
        }

        //loadAnimation();   
        //animationLoadManager = animacion.GetComponent<AnimationLoadManager>();
        
        //Invoke("LoadAnimataionClip", 3);

    }

    void LoadAnimataionClip()
    {
        animationLoadManager.LoadAnimation("an", null);
    }

}