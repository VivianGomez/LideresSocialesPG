﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;


public class PanelInicio : MonoBehaviour
{
    public GameObject panelIF;
    public Image imagenLider;
    public TextMeshProUGUI  infoLider;
    
    void Start()
    {
        infoLider.text = "";
        StartCoroutine(cargarSonidoInicio());
    }

    IEnumerator cargarSonidoInicio()
    {
        GameObject camera= GameObject.Find("Main Camera");
        camera.GetComponent<CargaAnimatorAnimations>().Request(3);
        imagenLider.sprite = Resources.Load<Sprite>("recursosImgs/inicio");
        yield return null;
    }

    public IEnumerator quitImage()
    {
        GameObject camera= GameObject.Find("Main Camera");
        imagenLider.sprite = Resources.Load<Sprite>("recursosImgs/KevinInicio");
        camera.GetComponent<CargaAnimatorAnimations>().Request(2);
        //camera.GetComponent<CargaAnimatorAnimations>().Request(0);
        yield return new WaitForSeconds(6f);
        //camera.GetComponent<CargaAnimatorAnimations>().Request(1);
        imagenLider.sprite = null;
        infoLider.text = "Kevin Julian Leon";
        yield return new WaitForSeconds(10f);   
        infoLider.text = "";
        imagenLider.sprite = Resources.Load<Sprite>("recursosImgs/PCControl");
        yield return new WaitForSeconds(10f);   
        imagenLider.sprite = Resources.Load<Sprite>("recursosImgs/Interacciones");
        if (File.Exists("assets/Resources/audios/ambiente.wav") && SceneManager.GetActiveScene().name.Equals("Inicio"))
        {
            cambioEscenaPI();
        }

    }

    //Se llama cuando todos los recursos ya se han cargado
    public static void cambioEscenaPI()
    {
        SceneManager.LoadSceneAsync("Cuarto");
    }
}
