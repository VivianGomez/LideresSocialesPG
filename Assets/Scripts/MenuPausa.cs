﻿using System.Collections;
using UnityEngine;
using LitJson;
using UnityEngine.UI;


public class MenuPausa : MonoBehaviour
{
    public JsonData instrucciones;
    public GameObject fondo;
    public GameObject menuPausa;

    public GameObject menuVolumen;

    public GameObject menuSalir;

    public GameObject panelInstrucciones;
    public Image instruccionActual;
    
    public GameObject btnNext;


    public Slider sliderAmbient;
    public Slider sliderDialogue;
    private bool enable;

    private int indiceInstruccion = 0;

    private SoundManager soundManager;
    private AudioScript audioScript;

    void Awake()
    {
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        audioScript = GameObject.FindObjectOfType<AudioScript>();
    }


    public void switchViewMenu()
    {

        if (enable)
        {
            menuPausa.SetActive(false);
            enable = false;
            fondo.SetActive(false);

        }
        else
        {
            menuPausa.SetActive(true);
            enable = true;
            fondo.SetActive(true);
        }
    }


    void Start()
    {
        enable = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switchViewMenu();
        }
    }

    public void ClickGuardar()
    {
        Debug.Log("Guardar");
    }

     public void ClickMostrarInstrucciones()
    {
        panelInstrucciones.SetActive(true);
        cambiarInstruccion();
    }

    public void cambiarInstruccion(){
        if(indiceInstruccion<instrucciones.Count){
           WWW requestImage = new WWW(""+instrucciones[indiceInstruccion]["imagen"]);
           StartCoroutine(LoadImageInstruction(requestImage));
           indiceInstruccion++;
        }
        else
        {
           btnNext.SetActive(false);
        }
    }

     private IEnumerator LoadImageInstruction(WWW wwwBG){
        yield return wwwBG;
        if (wwwBG.error == null)
        {   
            instruccionActual.sprite = Sprite.Create(wwwBG.texture, new Rect(0, 0, wwwBG.texture.width, wwwBG.texture.height), new Vector2(0.5f, 0.5f));
            Vector2 S = instruccionActual.sprite.bounds.size;
        }
        else
        {
            Debug.LogError("No se puede cargar la imagen");
        }
    }

    public void cerrarInstrucciones()
    {
        panelInstrucciones.SetActive(false);
        btnNext.SetActive(true);
        indiceInstruccion = 0;
    }

    public void changeVolumeAmbient()
    {
        audioScript.volumen = (sliderAmbient.value/10);
    }

    public void changeVolumeDialogues()
    {
        Debug.Log("VolDialogue" + sliderDialogue.value);
    }

    public void ClickVolumen()
    {
        menuVolumen.SetActive(true);
        menuPausa.SetActive(false);
    }

    public void ClickVolverMenuPausa()
    {
        menuPausa.SetActive(true);
        menuVolumen.SetActive(false);
    }

    public void ClickSalir()
    {
        menuSalir.SetActive(true);        
    }

    public void ClickVolverMenuPausaSalir()
    {
        menuSalir.SetActive(false);
    }

    public void ClickConfirmaSalir()
    {
        Application.Quit();
    }
}
