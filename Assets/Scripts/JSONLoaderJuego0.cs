﻿using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;
using LitJson;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Net;
using UnityEngine.UI;
using TMPro;
using System;

public class JSONLoaderJuego0 : MonoBehaviour
{
    private string jsonString;
    private JsonData jData;
    private JsonData gameData;
    private string textoCartaDia;

    private JsonData infoDias;

    private TimeDayFunction timeDayFunction;

    private string textoRegaloCarta;
    private string textoRegaloLiderazgo;
    private string regaloCarta;
    public int cantRegaloCarta;

    private string regaloLiderazgo;
    public int cantRegaloLiderazgo;

    public Image imageRegalo;
    public Image wowRegalo;
    public TextMeshProUGUI giftT;
    public TextMeshProUGUI tituloR;
    public TextMeshProUGUI cantRegaloModal;


    //****************************************************************************************

    private string textoNoticiaDia;

    public GameObject jugador;
    public GameObject loadingSpriteStart;
    public GameObject animacion;

    public GameObject modal;
    public Image imageModal;

    public TextMeshProUGUI  letterT;
    public TextMeshProUGUI  newsT;
    public TextMeshProUGUI  informationT;
    public GameObject panelInventario;

    public Button buttonLetter;
    public Button buttonCloseModal;

    public TextMeshProUGUI buttonAlimento1;
    public TextMeshProUGUI buttonAlimento2;
    public TextMeshProUGUI buttonAlimento3;
    public TextMeshProUGUI buttonAlimento4;
    public TextMeshProUGUI buttonAlimento5;
    public TextMeshProUGUI buttonAlimento6;
    public TextMeshProUGUI buttonAlimento7;
    public TextMeshProUGUI buttonAlimento8;

    public TextMeshProUGUI cantidadAlimento1;
    public TextMeshProUGUI cantidadAlimento2;
    public TextMeshProUGUI cantidadAlimento3;
    public TextMeshProUGUI cantidadAlimento4;
    public TextMeshProUGUI cantidadAlimento5;
    public TextMeshProUGUI cantidadAlimento6;
    public TextMeshProUGUI cantidadAlimento7;
    public TextMeshProUGUI cantidadAlimento8;


    //public const string url ="https://firebasestorage.googleapis.com/v0/b/lideresocialespg.appspot.com/o/juego0.json?alt=media&token=3d8deac2-9fd0-4a22-98a3-3bc7629f809b";
    public const string url ="https://lideresocialespg.firebaseio.com/juegos.json";

    void Awake()
    {
        timeDayFunction = GameObject.FindObjectOfType<TimeDayFunction>();
    }

    void Update(){
         StartCoroutine(LoadInfoDia((timeDayFunction.dia)-1));
    }

    void Start()
    {
        if (!File.Exists(Application.dataPath + "/Gamedata.json"))
        {
            cantidadAlimento1.text = (1).ToString();
            cantidadAlimento2.text = (3).ToString();
            cantidadAlimento3.text = (5).ToString();
            cantidadAlimento4.text = (0).ToString();
            cantidadAlimento5.text = (0).ToString();
            cantidadAlimento6.text = (0).ToString();
            cantidadAlimento7.text = (0).ToString();
            cantidadAlimento8.text = (0).ToString();
        }
        else
        {
            gameData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
            cantidadAlimento1.text = (gameData[1][0]).ToString();
            cantidadAlimento2.text = (gameData[1][1]).ToString();
            cantidadAlimento3.text = (gameData[1][2]).ToString();
            cantidadAlimento4.text = (gameData[1][3]).ToString();
            cantidadAlimento5.text = (gameData[1][4]).ToString();
            cantidadAlimento6.text = (gameData[1][5]).ToString();
            cantidadAlimento7.text = (gameData[1][6]).ToString();
            cantidadAlimento8.text = (gameData[1][7]).ToString();
        }

        informationT.text = "";
        loadingSpriteStart.SetActive(false);
        modal.SetActive(false);
        panelInventario.SetActive(false);
        Request();
    }

    public void Request()
    {
        WWW request = new WWW(url);
        loadingSpriteStart.SetActive(true);
        StartCoroutine(OnResponse(request));
    }

    private IEnumerator OnResponse(WWW req)
    {
        yield return req;
        //string path=Application.dataPath + "/Resources/data.json";
        if (req.error == null)
        {
           //jsonString = File.ReadAllText(path);
           //print(req.text);
           jData = JsonMapper.ToObject(req.text);

           WWW reqSprite = new WWW(""+jData[1]["personaje"]["imagenPersonaje"]);

           infoDias = jData[1]["infoDias"];

           gameData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
           LoadInfoAlimentos(gameData[8]);
           loadingSpriteStart.SetActive(false);

        }
        else
        {
            Debug.LogError("No se puedieron cargar los datos del juego");
        }
    }


    public IEnumerator LoadInfoDia(int dia){
        if(!loadingSpriteStart.active)   {
            yield return new WaitForSeconds(1);   
            textoCartaDia = ""+infoDias[dia]["textoCarta"];
            textoNoticiaDia = ""+infoDias[dia]["textoPeriodico"];
            textoRegaloCarta = ""+infoDias[dia]["textoRegaloCarta"];
            textoRegaloLiderazgo = ""+infoDias[dia]["textoRegaloLiderazgo"];
            regaloCarta = ""+infoDias[dia]["regaloCarta"]["nombre"];
            cantRegaloCarta = (int)infoDias[dia]["regaloCarta"]["cantidad"];
            regaloLiderazgo = ""+infoDias[dia]["regaloLiderazgo"]["nombre"];
            cantRegaloLiderazgo = (int)infoDias[dia]["regaloLiderazgo"]["cantidad"];
        } 
    }

    void LoadInfoAlimentos(JsonData alimentos)
    {
        buttonAlimento1.text= "" + alimentos[0];
        buttonAlimento2.text = "" + alimentos[1];
        buttonAlimento3.text = "" + alimentos[2];
        buttonAlimento4.text = "" + alimentos[3];
        buttonAlimento5.text = "" + alimentos[4];
        buttonAlimento6.text = "" + alimentos[5];
        buttonAlimento7.text = "" + alimentos[6];
        buttonAlimento8.text = "" + alimentos[7];
    }

    public void AbrirPeriodico()
    {
        if(!(textoNoticiaDia.Equals(""))){
            buttonCloseModal.image.color = Color.black;
            panelInventario.SetActive(false);
            imageModal.enabled = true;
            imageModal.sprite = Resources.Load<Sprite>("periodico");
            modal.SetActive(true);
            letterT.text="";
            giftT.text="";
            cantRegaloModal.text="";
            tituloR.text="";
            imageRegalo.enabled = false;
            wowRegalo.enabled = false;
            newsT.text = textoNoticiaDia;
        }
        else{
            informationT.text = "Hoy no me trajeron el periódico ... ";
            StartCoroutine(ActivationRoutine());
        }
    }

    private IEnumerator ActivationRoutine()
    {
        yield return new WaitForSeconds(3);
        informationT.text = "";
    }

    public void AbrirCarta()
    {
        if(!(textoCartaDia.Equals(""))){
            buttonCloseModal.image.color = Color.black;
            panelInventario.SetActive(false);
            imageRegalo.enabled = false;
            wowRegalo.enabled = false;
            imageModal.enabled = true;
            buttonLetter.image.sprite = Resources.Load<Sprite>("sobreAbierto");
            imageModal.sprite = Resources.Load<Sprite>("paper");
            modal.SetActive(true);
            newsT.text="";
            giftT.text="";
            cantRegaloModal.text="";
            tituloR.text="";
            letterT.text = textoCartaDia;
        }
        else{
            informationT.text = "Hoy no me trajeron cartas ... ";
            StartCoroutine(ActivationRoutine());
        }
    }

    public void AbrirDespensa()
    {
        panelInventario.SetActive(true);
        imageModal.enabled = false;
        newsT.text = "";
        letterT.text = "";
        giftT.text="";
        cantRegaloModal.text="";
        tituloR.text="";
        imageRegalo.enabled = false;
        wowRegalo.enabled = false;
        modal.SetActive(true);
        buttonCloseModal.image.color = Color.white;
    }

    
    public void AbrirRegalo()
    {
        if(!(textoRegaloCarta.Equals(""))){
            buttonCloseModal.image.color = Color.black;
            panelInventario.SetActive(false);
            imageRegalo.enabled = true;
            wowRegalo.enabled = true;
            imageRegalo.sprite = Resources.Load<Sprite>("comida/"+regaloCarta.ToLowerInvariant());
            imageModal.enabled = true;
            imageModal.sprite = Resources.Load<Sprite>("paper");
            modal.SetActive(true);
            newsT.text="";
            letterT.text = "";
            tituloR.text="¡Recibiste un regalo";
            giftT.text=textoRegaloCarta;
            cantRegaloModal.text = "X "+ cantRegaloCarta;
        }
        else{
            informationT.text = "Hoy no me trajeron regalos ... ";
            StartCoroutine(ActivationRoutine());
        }
    }

    public void CerrarCarta()
    {
        buttonLetter.image.sprite = Resources.Load<Sprite>("letter");
        modal.SetActive(false);
    }


    //ESTE MÉTODO ES TEMPORAL, LA IDEA ES QUE EL ÍNDICE ENTRE POR PARÁMETRO Y CON ESE SE BUSQUE EN EL ARREGLO DE porcentajes, ES DECIR SÓLO DICHA BÚSQUEDA SE RETORNA SIN LOS IF's
    // return porcentajesEnergia[indiceAlimento];
    public int darPorcentajeEnergiaAlimento(int indiceAlimento)
    {
        gameData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
        if (indiceAlimento == 1 && int.Parse(cantidadAlimento1.text)!=0)
        {
            cantidadAlimento1.text = ""+(Int32.Parse(cantidadAlimento1.text) -1);
            return 1;
        }
        else if(indiceAlimento == 2 && int.Parse(cantidadAlimento2.text) != 0)
        {
            cantidadAlimento2.text = "" + (Int32.Parse(cantidadAlimento2.text) - 1);
            return 5;
        }
        else if (indiceAlimento == 3 && int.Parse(cantidadAlimento3.text) != 0)
        {
            cantidadAlimento3.text = "" + (Int32.Parse(cantidadAlimento3.text) - 1);
            return 2;
        }
        else if (indiceAlimento == 4 && int.Parse(cantidadAlimento4.text) != 0)
        {
            cantidadAlimento4.text = "" + (Int32.Parse(cantidadAlimento4.text) - 1);
            return 4;
        }
        else if (indiceAlimento == 5 && int.Parse(cantidadAlimento5.text) != 0)
        {
            cantidadAlimento5.text = "" + (Int32.Parse(cantidadAlimento5.text) - 1);
            return 3;
        }
        else if (indiceAlimento == 6 && int.Parse(cantidadAlimento6.text) != 0)
        {
            cantidadAlimento6.text = "" + (Int32.Parse(cantidadAlimento6.text) - 1);
            return 1; 
        }
        else if (indiceAlimento == 7 && int.Parse(cantidadAlimento7.text) != 0)
        {
            cantidadAlimento7.text = "" + (Int32.Parse(cantidadAlimento7.text) - 1);
            return 5;
        }
        else if(indiceAlimento==8 && int.Parse(cantidadAlimento8.text) != 0)
        {
            cantidadAlimento8.text = "" + (Int32.Parse(cantidadAlimento8.text) - 1);
            return 5;
        }
        else
        {
            return 0;
        }
    }


}
