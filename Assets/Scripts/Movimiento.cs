﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public bool camina;
    public bool trigger;
    public GameObject imagen;
    public Animator animator;
    public JsonData jsonData;


    void Start()
    {
        trigger = false;
        objeto.SetActive(false);
        imagen.SetActive(false);
        escala = transform.localScale;
        escalaX = escala.x;
        escalaY = escala.y;
        if (File.Exists(Application.dataPath + "/Gamedata.json"))
        {
            jsonData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
            diaActual = (int)jsonData[0];
        }
    }

    void Update()
    {
        if (File.Exists(Application.dataPath + "/Gamedata.json"))
        {
            jsonData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
            diaActual = (int)jsonData[0];
        }

        GameObject player = GameObject.Find("Personaje");

        transform.Translate(Input.GetAxis("Horizontal") * 3f * Time.deltaTime, 0f, 0f);
        transform.Translate(0f, Input.GetAxis("Vertical") * 3f * Time.deltaTime, 0f);

        if (Input.GetAxis("Horizontal") < 0)
        {
            camina = true;
            animator.SetTrigger("camina");
            escala.x = -escalaX;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            camina = true;
            animator.SetTrigger("camina");
            escala.x = escalaX;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            camina = true;
            animator.SetTrigger("camina");
            escala.y = escalaY;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            camina = true;
            animator.SetTrigger("camina");
            escala.y = escalaY;
        }
        transform.localScale = escala;

        camina = false;
        animator.SetTrigger("dejaCaminar"); 
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

        //print("El dia actual era....." + diaActual);
        if (col.name== "cama" && fondo.GetComponent<TimeDayFunction>().hora>6 && diaActual<4)
        {
            //imagen.SetActive(true);
            // if (imagen != null)
            //{
            //  StartCoroutine(imagen.GetComponent<VideoStream>().playVideo());
            //}
            //else print("es nula");

            animator.SetTrigger("duerme");
                        
        }

        //loadAnimation();   
        //animationLoadManager = animacion.GetComponent<AnimationLoadManager>();
        
        //Invoke("LoadAnimataionClip", 3);

    }

    public void permitirAnimacion()
    {
        trigger = true;
        objeto.SetActive(true);
                
    }

    void LoadAnimataionClip()
    {
        animationLoadManager.LoadAnimation("an", null);
    }

}