using System.Collections;
using System.IO;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Net;

public class MenuPausa : MonoBehaviour
{
    public JsonData instrucciones;
    public GameObject fondo;
    public GameObject menuPausa;

    public GameObject menuVolumen;

    public GameObject menuSalir;

    public GameObject panelInstrucciones;
    public Image instruccionActual;

    public Slider sliderAmbient;
    public Slider sliderDialogue;

    private bool enable;

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

    // Update is called once per frame
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
        WWW requestImage = new WWW(""+instrucciones[0]["imagen"]);
        StartCoroutine(LoadImageInstruction(requestImage));
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
    }

    public void changeVolumeAmbient()
    {
        Debug.Log("VolAmbient"+ sliderAmbient.value);
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
