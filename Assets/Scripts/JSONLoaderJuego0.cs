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

public class JSONLoaderJuego0 : MonoBehaviour
{

    public SpriteRenderer backgroundSprite;
    private string jsonString;
    private JsonData jData;
    private JsonData gameData;
    private string textoCartaDia;

    private JsonData infoDias;

    private TimeDayFunction timeDayFunction;

    public string textoRegaloCarta="";
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

    public Button urlNews;


    //****************************************************************************************

    public string textoNoticiaDia;
    private string urlNoticiaDia;

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

    public TextMeshProUGUI porcentajeAlimento1;
    public TextMeshProUGUI porcentajeAlimento2;
    public TextMeshProUGUI porcentajeAlimento3;
    public TextMeshProUGUI porcentajeAlimento4;
    public TextMeshProUGUI porcentajeAlimento5;
    public TextMeshProUGUI porcentajeAlimento6;
    public TextMeshProUGUI porcentajeAlimento7;
    public TextMeshProUGUI porcentajeAlimento8;

    GameObject p;

    //Dialogo
     public JsonData dialogosDia;
    public JsonData dialogosNoche;

    public string dialogNino = "";
    public TextMeshProUGUI DialogoNino;

    public const string url ="https://lideresocialespg.firebaseio.com/juegos.json";

    private SoundManager soundManager;
    private AudioScript audioScript;
    void Awake()
    {
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        audioScript = GameObject.FindObjectOfType<AudioScript>();
        timeDayFunction = GameObject.FindObjectOfType<TimeDayFunction>();
    }

    void Update(){
         StartCoroutine(LoadInfoDia((timeDayFunction.dia)-1, timeDayFunction.hora));
    }

    void Start()
    {
        p= GameObject.Find("Personaje");
        if(p!=null) p.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("StateMachineTransitions", typeof(RuntimeAnimatorController)));
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

        if(porcentajeAlimento1!= null)
        {
            porcentajeAlimento1.text = "+ 1% de energía";
            porcentajeAlimento2.text = "+ 5% de energía";
            porcentajeAlimento3.text = "+ 2% de energía";
            porcentajeAlimento4.text = "+ 4% de energía";
            porcentajeAlimento5.text = "+ 3% de energía";
            porcentajeAlimento6.text = "+ 1% de energía";
            porcentajeAlimento7.text = "+ 5% de energía";
            porcentajeAlimento8.text = "+ 5% de energía";
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
       
        p.GetComponent<Movimiento>().permiteMoverse = false;
        loadingSpriteStart.SetActive(true);
        StartCoroutine(OnResponse(request));
    }

    IEnumerator loading(){
       yield return new WaitForSeconds(2); 
        loadingSpriteStart.SetActive(false);
        p.GetComponent<Movimiento>().permiteMoverse = true;
    }


    private IEnumerator OnResponse(WWW req)
    {
        yield return req;
        if (req.error == null)
        {
           jData = JsonMapper.ToObject(req.text);

           WWW reqSprite = new WWW(""+jData[1]["personajePrincipal"]["imagenPersonaje"]);

           audioScript.sonidosAmbiente = jData[1]["sonidosAmbiente"];
           audioScript.empieza();
            
           LoadScene(jData[1]["escenas"]);
           GameObject.FindObjectOfType<MenuPausa>().instrucciones = jData[1]["instrucciones"];

           infoDias = jData[1]["infoDias"];

           gameData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
           LoadInfoAlimentos(gameData[10]);
           
        }
        else
        {
            Debug.LogError("No se puedieron cargar los datos del juego"); 
        }
    }


    private void LoadScene(JsonData escenas)
    {
            var actual = escenas[0];
            var interaccionActual = escenas[0];
            var spriteActual = escenas[0];
        for (int i = 0; i < escenas.Count; i++)
        {
               actual = escenas[i];
               if((""+actual["nombreEscena"]).Equals(""+SceneManager.GetActiveScene().name))
               {
                    WWW reqBackground = new WWW(""+actual["imagenFondo"]);
                    StartCoroutine(LoadBackground(reqBackground, ""+actual["nombreEscena"]));
                    crearTPLlegada(
                    ""+actual["tpLlegada"]["nombre"], 
                    actual["tpLlegada"]["coordenadas"],
                    ""+actual["tpLlegada"]["Tag"]
                    );
                
                    for (int j = 0; j < actual["interacciones"].Count; j++)
                    {
                        interaccionActual = actual["interacciones"][j];
                        if(actual["interacciones"][j]!=null)
                        {
                        createCollidersInteractions(""+interaccionActual["colliderActivador"], interaccionActual["coordenadasCollider"], interaccionActual["offsetCollider"], interaccionActual["tamCollider"], ""+interaccionActual["isTrigger"], ""+interaccionActual["script"], ""+interaccionActual["escenaACambiar"], ""+interaccionActual["tag"], ""+interaccionActual["sonido"]);
                        }               
                    } 

                    for (int k = 0; k < actual["personajes"].Count; k++)
                    {
                        interaccionActual = actual["personajes"][k];
                        if(!(interaccionActual["imagen"].ToString().Equals("")))
                        {
                            StartCoroutine(crearPersonaje(new WWW(""+interaccionActual["imagen"]),""+interaccionActual["colliderActivador"], interaccionActual["coordenadasCollider"], interaccionActual["tamCollider"], ""+interaccionActual["tag"], ""+interaccionActual["sonido"]));
                        }
                    } 
   
               }             
            }
    }


    private IEnumerator LoadBackground(WWW wwwBG, string nombre){
        yield return wwwBG;
        if (wwwBG.error == null)
        {                
            backgroundSprite.sprite = Sprite.Create(wwwBG.texture, new Rect(0, 0, wwwBG.texture.width, wwwBG.texture.height), new Vector2(0.5f, 0.5f));
            Vector2 S = backgroundSprite.sprite.bounds.size;
            StartCoroutine(loading());

        }
        else
        {
            Debug.LogError("No se puede cargar el fondo de la escena "+nombre);
        }
    }

    private void crearTPLlegada(string nombre, JsonData posicion, string tag){
            GameObject objTP;
            objTP = new GameObject(nombre);
            float x = float.Parse(""+posicion[0]);
            float y = float.Parse(""+posicion[1]);
            float z = float.Parse(""+posicion[2]);

            objTP.transform.position = new Vector3(x,y,z);
            objTP.tag = tag;
    }

    private IEnumerator crearPersonaje(WWW spriteWWW, string nombreActivador, JsonData posicion, JsonData escala, string tagJ, string sonido) 
        {
            yield return spriteWWW;
            if (spriteWWW.error == null)
            {
                GameObject objToSpawn;
                
                if(!(sonido.Equals("")))
                {
                    StartCoroutine(soundManager.loadAudio(sonido, nombreActivador));
                }
                
                objToSpawn = new GameObject(nombreActivador);
                if(!(tagJ.Equals("")))
                {
                 objToSpawn.tag = tagJ;
                }
                float x = float.Parse(""+posicion[0]);
                float y = float.Parse(""+posicion[1]);
                float z = float.Parse(""+posicion[2]);

                float xE = float.Parse(""+escala[0]);
                float yE = float.Parse(""+escala[1]);
                objToSpawn.AddComponent<SpriteRenderer>().sprite = Sprite.Create(spriteWWW.texture, new Rect(0, 0, spriteWWW.texture.width, spriteWWW.texture.height), new Vector2(0.5f, 0.5f));
                Vector2 S = objToSpawn.GetComponent<SpriteRenderer>().sprite.bounds.size;
                objToSpawn.transform.position = new Vector3(x,y,z);
                objToSpawn.transform.localScale = new Vector3(xE,yE,1f);
                objToSpawn.AddComponent<BoxCollider2D>();
                var coll = objToSpawn.GetComponent<BoxCollider2D>();
                coll.isTrigger = false;
                objToSpawn.AddComponent<BoxCollider2D>();
                objToSpawn.GetComponent<BoxCollider2D>().size = S;
                var coll2 = objToSpawn.GetComponent<BoxCollider2D>();
                coll2.isTrigger = true;
                objToSpawn.GetComponent<BoxCollider2D>().size = S;

            }
            else
            {
                Debug.LogError("No se puede cargar la imágen del personaje");
            }

        }

    private void createCollidersInteractions(string nombreActivador, JsonData posicion, JsonData offset, JsonData tamanio, string isTrigger, string tpScript, string cambio, string tagJ, string sonido){
            GameObject objToSpawn;

            if(!(sonido.Equals("")))
            {
                StartCoroutine(soundManager.loadAudio(sonido, nombreActivador));
            }

            objToSpawn = new GameObject(nombreActivador);
            float x = float.Parse(""+posicion[0]);
            float y = float.Parse(""+posicion[1]);
            float z = float.Parse(""+posicion[2]);

            float xO = float.Parse(""+offset[0]);
            float yO = float.Parse(""+offset[1]);

            float xT = float.Parse(""+tamanio[0]);
            float yT = float.Parse(""+tamanio[1]);

            objToSpawn.transform.position = new Vector3(x,y,z);

            objToSpawn.AddComponent<BoxCollider2D>();
            var coll = objToSpawn.GetComponent<BoxCollider2D>();
            coll.isTrigger = isTrigger.Equals("1");
            coll.offset = new Vector2(xO,yO);
            coll.size = new Vector2(xT,yT);

            if(!(tagJ.Equals("")))
            {
               objToSpawn.tag = tagJ;
            }

            if(!(tpScript.Equals("")) && !(cambio.Equals(""))){
                objToSpawn.AddComponent<Teleport>().levelToLoad = cambio;                 
        }
        }

    public IEnumerator LoadInfoDia(int dia, int hora){
        if (infoDias != null) {
            yield return new WaitForSeconds(1);
            textoCartaDia = "" + infoDias[dia]["textoCarta"];
            textoNoticiaDia = "" + infoDias[dia]["textoPeriodico"];
            urlNoticiaDia = "" + infoDias[dia]["urlNoticiaDia"];
            textoRegaloCarta = "" + infoDias[dia]["textoRegaloCarta"];
            textoRegaloLiderazgo = "" + infoDias[dia]["textoRegaloLiderazgo"];
            regaloCarta = "" + infoDias[dia]["regaloCarta"]["nombre"];
            cantRegaloCarta = (int)infoDias[dia]["regaloCarta"]["cantidad"];
            regaloLiderazgo = "" + infoDias[dia]["regaloLiderazgo"]["nombre"];
            cantRegaloLiderazgo = (int)infoDias[dia]["regaloLiderazgo"]["cantidad"];
            dialogosDia = infoDias[dia]["textosDia"];
            dialogosNoche = infoDias[dia]["textosNoche"];
        } 
    }

    public IEnumerator cargarDialogosHH(int dia)
    {
        DialogoNino.text = "";
        if (infoDias!=null)
        {
            for (int i = 0; i < infoDias[dia]["textosHH"].Count; i++)
            {
                DialogoNino.text = "" + infoDias[dia]["textosHH"][i];
                soundManager.PlaySound("PuntoDiscurso");
                yield return new WaitForSeconds(5);
            }
            
        }
        Destroy(GameObject.Find("PuntoDiscurso"));
        GameObject p = GameObject.Find("Personaje");
        p.GetComponent<Movimiento>().animator.SetTrigger("camina");
        p.GetComponent<Movimiento>().permiteMoverse = true;
        
        p.GetComponent<Movimiento>().DialogoNPC.SetActive(false);
        AbrirRegaloLiderazgo();
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
        GameObject camara = GameObject.Find("Main Camera");
        GameObject fondo = GameObject.Find("Background");

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
                ""+gameData[4].ToString(),
                ""+gameData[5].ToString(),
                "1",
                fondo.GetComponent<TimeDayFunction>().inicio,
                fondo.GetComponent<TimeDayFunction>().ultimoSegundo,
                fondo.GetComponent<TimeDayFunction>().segundoActual,
                new string[] { "Arroz", "Huevos", "Agua", "Arroz con leche", "Chocolate", "Dulces", "Granos", "Pan de centeno" });

        if (!(textoNoticiaDia.Equals(""))){
            p.GetComponent<Movimiento>().permiteMoverse = false;
            if(p.GetComponent<Movimiento>().DialogoNPC!= null)
            {
                p.GetComponent<Movimiento>().DialogoNPC.SetActive(false);
            }
            

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
            urlNews.enabled = true;
        }
        else{
            if (p.GetComponent<Movimiento>().DialogoNPC != null)
            {
                p.GetComponent<Movimiento>().DialogoNPC.SetActive(false);
            }
            
            informationT.text = "Hoy no me trajeron el periódico ... ";
            StartCoroutine(ActivationRoutine());
        }
    }

    public void darInformacion(string info){
        informationT.text = info;
        StartCoroutine(ActivationRoutine());
    }

    private IEnumerator ActivationRoutine()
    {
        yield return new WaitForSeconds(3);
        informationT.text = "";
    }

    public void abrirUrlNews(){
        Application.OpenURL(urlNoticiaDia);
    }

    public void AbrirCarta()
    {
        GameObject camara = GameObject.Find("Main Camera");
        GameObject fondo = GameObject.Find("Background");
                

        if (!(textoCartaDia.Equals(""))){

            p.GetComponent<Movimiento>().permiteMoverse = false;
            if (p.GetComponent<Movimiento>().DialogoNPC != null)
            {
                p.GetComponent<Movimiento>().DialogoNPC.SetActive(false);
            }
            
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
            urlNews.enabled = false;

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
                ""+gameData[4].ToString(),
                "1",
                ""+gameData[6].ToString(),
                fondo.GetComponent<TimeDayFunction>().inicio,
                fondo.GetComponent<TimeDayFunction>().ultimoSegundo,
                fondo.GetComponent<TimeDayFunction>().segundoActual,
                new string[] { "Arroz", "Huevos", "Agua", "Arroz con leche", "Chocolate", "Dulces", "Granos", "Pan de centeno" });

        }
        else{

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
                ""+gameData[4].ToString(),
                "1",
                ""+gameData[6].ToString(),
                fondo.GetComponent<TimeDayFunction>().inicio,
                fondo.GetComponent<TimeDayFunction>().ultimoSegundo,
                fondo.GetComponent<TimeDayFunction>().segundoActual,
                new string[] { "Arroz", "Huevos", "Agua", "Arroz con leche", "Chocolate", "Dulces", "Granos", "Pan de centeno" });

            if (p.GetComponent<Movimiento>().DialogoNPC != null)
            {
                p.GetComponent<Movimiento>().DialogoNPC.SetActive(false);
            }
            
            informationT.text = "Hoy no me trajeron cartas ... ";
            StartCoroutine(ActivationRoutine());
        }
    }

    public void AbrirDespensa()
    {
        p.GetComponent<Movimiento>().permiteMoverse = false;
        panelInventario.SetActive(true);
        imageModal.enabled = false;
        newsT.text = "";
        letterT.text = "";
        giftT.text="";
        cantRegaloModal.text="";
        tituloR.text="";
        urlNews.enabled = false;
        imageRegalo.enabled = false;
        wowRegalo.enabled = false;
        modal.SetActive(true);
        buttonCloseModal.image.color = Color.white;
    }

    private int buscarAlimento(string nombre)
    {
        int rta = -1;
        for (int i = 0; i < gameData[10].Count; i++)
        {
            if(gameData[10][i].Equals(nombre)){
                rta = i;
            }
        }
        return rta;
    }

    private TextMeshProUGUI darAlimentoCantText(int indice)
    {
       TextMeshProUGUI rta = null;
       if(indice == 0){
           rta = cantidadAlimento1;
       }
       else if(indice == 1){
           rta = cantidadAlimento2;
       }
       else if(indice == 2){
           rta = cantidadAlimento3;
       }
       else if(indice == 3){
           rta = cantidadAlimento4;
       }
       else if(indice == 4){
           rta = cantidadAlimento5;
       }
       else if(indice == 5){
           rta = cantidadAlimento6;
       }
       else if(indice == 6){
           rta = cantidadAlimento7;
       }
       else{
           rta = cantidadAlimento8;
       }
       return rta;
    }
    
    public void AbrirRegalo()
    {
        gameData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
        GameObject camara = GameObject.Find("Main Camera");
        GameObject fondo = GameObject.Find("Background");              

        if ((!(textoRegaloCarta.Equals(""))) && (gameData[4].ToString().Equals("0")))
        {
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
            int indiceAl = buscarAlimento(regaloCarta);

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
               "1",
               ""+gameData[5].ToString(),
               ""+gameData[6].ToString(),
               fondo.GetComponent<TimeDayFunction>().inicio,
               fondo.GetComponent<TimeDayFunction>().ultimoSegundo,
               fondo.GetComponent<TimeDayFunction>().segundoActual,
               new string[] { "Arroz", "Huevos", "Agua", "Arroz con leche", "Chocolate", "Dulces", "Granos", "Pan de centeno" });

            darAlimentoCantText(indiceAl).text = "" + (Int32.Parse(darAlimentoCantText(indiceAl).text) + cantRegaloCarta);
        }
        else{
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
               "1",
               ""+gameData[5].ToString(),
               ""+gameData[6].ToString(),
               fondo.GetComponent<TimeDayFunction>().inicio,
               fondo.GetComponent<TimeDayFunction>().ultimoSegundo,
               fondo.GetComponent<TimeDayFunction>().segundoActual,
               new string[] { "Arroz", "Huevos", "Agua", "Arroz con leche", "Chocolate", "Dulces", "Granos", "Pan de centeno" });
            //if (gameData[4].Equals("1")){
            if (textoRegaloCarta.Equals("")){ 
                informationT.text = "Hoy no me trajeron regalos ... ";
            }
            else if(gameData[4].Equals("1"))
            {
                informationT.text = "No hay más regalos ... ";
            }

            StartCoroutine(ActivationRoutine());
        }
    }

    public void AbrirRegaloLiderazgo()
    {
        gameData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
        if(!(textoRegaloLiderazgo.Equals("")))
        {
            p.GetComponent<Movimiento>().permiteMoverse = false;
            soundManager.PlaySound("wow");           
            buttonCloseModal.image.color = Color.black;
            panelInventario.SetActive(false);
            imageRegalo.enabled = true;
            wowRegalo.enabled = true;
            imageRegalo.sprite = Resources.Load<Sprite>("comida/"+regaloLiderazgo.ToLowerInvariant());
            imageModal.enabled = true;
            imageModal.sprite = Resources.Load<Sprite>("paper");
            modal.SetActive(true);
            newsT.text="";
            letterT.text = "";
            tituloR.text="¡Recibiste un regalo";
            giftT.text=textoRegaloLiderazgo;
            cantRegaloModal.text = "X "+ cantRegaloLiderazgo;
            int indiceAl = buscarAlimento(regaloLiderazgo);
            GameObject camara = GameObject.Find("Main Camera");
            GameObject fondo = GameObject.Find("Background");  
            int tenia = Int32.Parse(darAlimentoCantText(indiceAl).text);
            darAlimentoCantText(indiceAl).text = "" + (tenia+cantRegaloLiderazgo) ;

        }
        else{
                informationT.text = "NO HAY MÁS REGALOS ... ";
        }
    }


    public void CerrarCarta()
    {

        buttonLetter.image.sprite = Resources.Load<Sprite>("letter");
        modal.SetActive(false);
        p.GetComponent<Movimiento>().permiteMoverse = true;
        p.GetComponent<Movimiento>().animator.SetTrigger("camina");
    }

    public void agregarAudioAnimacion()
    {
        GameObject p = GameObject.Find("Personaje");
        p.GetComponent<Movimiento>().animator.SetTrigger("comer");
        soundManager.PlaySound("comer");
    }

    //ESTE MÉTODO ES TEMPORAL, LA IDEA ES QUE EL ÍNDICE ENTRE POR PARÁMETRO Y CON ESE SE BUSQUE EN EL ARREGLO DE porcentajes, ES DECIR SÓLO DICHA BÚSQUEDA SE RETORNA SIN LOS IF's
    // return porcentajesEnergia[indiceAlimento];
    public int darPorcentajeEnergiaAlimento(int indiceAlimento)
    {
                
        gameData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
        
        if (indiceAlimento == 1 && int.Parse(cantidadAlimento1.text)!=0)
        {
            agregarAudioAnimacion();            
            cantidadAlimento1.text = ""+(Int32.Parse(cantidadAlimento1.text) -1);
            informationT.text = "";
            return 1;
        }
        else if(indiceAlimento == 2 && int.Parse(cantidadAlimento2.text) != 0)
        {
            agregarAudioAnimacion();
            cantidadAlimento2.text = "" + (Int32.Parse(cantidadAlimento2.text) - 1);
            informationT.text = "";
            return 5;
        }
        else if (indiceAlimento == 3 && int.Parse(cantidadAlimento3.text) != 0)
        {
            agregarAudioAnimacion();
            cantidadAlimento3.text = "" + (Int32.Parse(cantidadAlimento3.text) - 1);
            informationT.text = "";
            return 2;
        }
        else if (indiceAlimento == 4 && int.Parse(cantidadAlimento4.text) != 0)
        {
            agregarAudioAnimacion();
            cantidadAlimento4.text = "" + (Int32.Parse(cantidadAlimento4.text) - 1);
            informationT.text = "";
            return 4;
        }
        else if (indiceAlimento == 5 && int.Parse(cantidadAlimento5.text) != 0)
        {
            agregarAudioAnimacion();
            cantidadAlimento5.text = "" + (Int32.Parse(cantidadAlimento5.text) - 1);
            informationT.text = "";
            return 3;
        }
        else if (indiceAlimento == 6 && int.Parse(cantidadAlimento6.text) != 0)
        {
            agregarAudioAnimacion();
            cantidadAlimento6.text = "" + (Int32.Parse(cantidadAlimento6.text) - 1);
            informationT.text = "";
            return 1; 
        }
        else if (indiceAlimento == 7 && int.Parse(cantidadAlimento7.text) != 0)
        {
            agregarAudioAnimacion();
            cantidadAlimento7.text = "" + (Int32.Parse(cantidadAlimento7.text) - 1);
            informationT.text = "";
            return 5;
        }
        else if(indiceAlimento==8 && int.Parse(cantidadAlimento8.text) != 0)
        {
            agregarAudioAnimacion();
            cantidadAlimento8.text = "" + (Int32.Parse(cantidadAlimento8.text) - 1);
            informationT.text = "";
            return 5;
        }
        else
        {
            return 0;
        }
    }
    
}