using LitJson;
using TMPro;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movimiento : MonoBehaviour
{
    public DayChanger changer;
    public Animator transicionNoche;

    Vector3 escala;
    float escalaX;
    float escalaY;
    public bool permiteMoverse;
    public GameObject animacion;
    AnimationLoadManager animationLoadManager;
    public int diaActual;
    public GameObject objeto;
    public bool camina;
    public bool trigger;
    public GameObject imagen;
    public Animator animator;
    public JsonData jsonData;

    public GameObject DialogoNPC;
    public TextMeshProUGUI txtDialogo;

    public GameObject botonRegalo;
    public GameObject botonCarta;
    public GameObject botonPeriodico;

    public GameObject personaje;
    public AnimationClip animacionClip;
    private Animation animacionObject;

    public List<GameObject> botonesAnimaciones;

    private JSONLoaderJuego0 jsonLoader;

    private SoundManager soundManager;
    void Awake()
    {
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        jsonLoader = GameObject.FindObjectOfType<JSONLoaderJuego0>();
    }

    /////////////////////////////////////////////////////
    public GameObject canvas;
    public GameObject dormirPrefab;
    public GameObject comerPrefab;
    public GameObject btnAnimacionPrefab;
    public GameObject discursoPrefab;
    /////////////////////////////////////////////////////////


    void Start()
    {

        trigger = false;
        objeto.SetActive(false);
        imagen.SetActive(false);
        escala = transform.localScale;
        escalaX = escala.x;
        escalaY = escala.y;

        llenarBotonesAnimaciones();
        if (File.Exists(Application.dataPath + "/Gamedata.json"))
        {
            jsonData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
            diaActual = (int)jsonData[0];
        }
    }

    void Update()
    {
        if(permiteMoverse)
        {
            if (File.Exists(Application.dataPath + "/Gamedata.json"))
            {
                jsonData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
                diaActual = (int)jsonData[0];
                
            }

            GameObject player = GameObject.Find("Personaje");
            GameObject camara = GameObject.Find("Main Camera");

            if (camara.GetComponent<EnergyBar>().Energy < 1)
            {
                transform.Translate(Input.GetAxis("Horizontal") * 1.0f * Time.deltaTime, 0f, 0f);
                transform.Translate(0f, Input.GetAxis("Vertical") * 1.0f * Time.deltaTime, 0f);
            }
            else
            {
                transform.Translate(Input.GetAxis("Horizontal") * 3.2f * Time.deltaTime, 0f, 0f);
                transform.Translate(0f, Input.GetAxis("Vertical") * 3.2f * Time.deltaTime, 0f);
            }

            if (Input.GetAxis("Horizontal") < 0) 
            {
                camina = true;
                animator.SetTrigger("caminar");
                escala.x = -escalaX;
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                camina = true;
                animator.SetTrigger("caminar"); 
                escala.x = escalaX;
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                camina = true;
                animator.SetTrigger("caminar");
                escala.y = escalaY;
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                camina = true;
                animator.SetTrigger("caminar");
                escala.y = escalaY;
            }
            transform.localScale = escala;

            camina = false;
            animator.SetTrigger("dejaCaminar");
        }
       
    }


    void llenarBotonesAnimaciones()
    {
        if (SceneManager.GetActiveScene().name.Equals("Cuarto"))
        {
            LlenarBtnAnimacion("dormir", "dormir", "dormir", "0,05", "0,07", "1,27");
        }
        else if (SceneManager.GetActiveScene().name.Equals("Sala"))
        {
            LlenarBtnAnimacion("sentarse", "sentarse", "sentarse", "-0,4686719", "0,1153362", "-2,126913");
            LlenarBtnAnimacion("sentarseSofa", "sentarse", "sentarse", "-6,01", "2,26", "-2,126913");
        }
        else if (SceneManager.GetActiveScene().name.Equals("Cocina"))
        {
            LlenarBtnAnimacion("comer", "comer", "abrir despensa", "0,007", "-0,013", "0,45");
        }
        else if (SceneManager.GetActiveScene().name.Equals("Coorporacion"))
        {
            LlenarBtnAnimacion("puntoDiscurso", "puntoDiscurso", "DAR DISCURSO \n(1min)", "0,13", "-2,95", "0,45");
        }
    }

    void LlenarBtnAnimacion(string nombre, string trigger, string texto, string posX, string posY, string posZ)
    {
        GameObject nuevo = dormirPrefab;

        if (nombre.Equals("dormir"))
        {
            nuevo = Instantiate(dormirPrefab);
            nuevo.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnClick());
        }
        else if (nombre.Equals("comer"))
        {
            nuevo = Instantiate(comerPrefab);
            nuevo.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnClickComer());
        }
        else if (nombre.Equals("puntoDiscurso"))
        {
            nuevo = Instantiate(comerPrefab);
            nuevo.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnClickDiscurso());
        }
        else
        {
            nuevo = Instantiate(btnAnimacionPrefab);
            nuevo.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnClickBtnAnimacion(nombre, trigger));
        }

        nuevo.name = nombre;
        nuevo.transform.SetParent(canvas.transform);
        nuevo.transform.localScale = new Vector3(1, 1, 1);
        nuevo.transform.position = new Vector3(float.Parse(posX), float.Parse(posY), float.Parse(posZ));
        nuevo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = texto;
        nuevo.SetActive(false);
        botonesAnimaciones.Add(nuevo);
    }


    void OnTriggerStay2D(Collider2D col)
    {
        GameObject fondo = GameObject.Find("Background");
        if (col.name == "camaClick" && fondo.GetComponent<TimeDayFunction>().hora > 18 && diaActual < 4)
        {
            MostrarBotonAnimacion("dormir", true);
        }
        else if (col.name == "silla")
        {
            MostrarBotonAnimacion("sentarse", true);
        }
        else if(col.name == "sillon")
        {
            MostrarBotonAnimacion("sentarseSofa", true);
        }
        else if (col.name == "Nevera")
        {
            MostrarBotonAnimacion("comer", true);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject fondo = GameObject.Find("Background");
        if (col.name == "cama" && fondo.GetComponent<TimeDayFunction>().hora < 19)
        {
            jsonLoader.darInformacion("NO TENGO GANAS DE DORMIR");
        }
        else if(col.name== "PuntoDiscurso")
        {       
            MostrarBotonAnimacion("puntoDiscurso", true);
            botonCarta.SetActive(false);
            botonPeriodico.SetActive(false);
            botonRegalo.SetActive(false);
        }
        else if (col.tag == "npc"){
            hablarNPC(col.name);
        }
        if (col.tag == "interactivo")
        {
            soundManager.PlaySound(col.name);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        GameObject fondo = GameObject.Find("Background");
        if(col.name == "dormir")
        {
            MostrarBotonAnimacion("cama", false);
        }
        if (col.tag == "npc"){
            DialogoNPC.SetActive(false);
        }
        else if(col.name =="silla")
        {
            MostrarBotonAnimacion("sentarse", false);
        }
        else if (col.name =="sillon" )
        {
            MostrarBotonAnimacion("sentarseSofa", false);
        }
        else if ( col.name == "PuntoDiscurso")
        {
            MostrarBotonAnimacion("puntoDiscurso", false);
            botonCarta.SetActive(true);
            botonPeriodico.SetActive(true);
            botonRegalo.SetActive(true);
        }
        else if (col.name == "Nevera")
        {
            MostrarBotonAnimacion("comer", false);
        }

    }

    /////////////////////  FUNCIONES ONCLICK PARA ANIMACIONES  /////////////////////////////
    
    public void OnClick(){

        GameObject fondo = GameObject.Find("Background");
        

        if ( fondo.GetComponent<TimeDayFunction>().hora > 6 && diaActual < 4)
        {
            MostrarBotonAnimacion("dormir",false);
            trigger = true;
            objeto.SetActive(true);
            animator.SetTrigger("dormir");
            soundManager.PlaySound("cama");
            permiteMoverse = false;
            StartCoroutine(changer.onFadeComplete(transicionNoche));
        } 
        
    }

    public void OnClickBtnAnimacion(string nombreBtn, string nombreTrigger)
    {
        MostrarBotonAnimacion(nombreBtn, false);
        animator.SetTrigger(nombreTrigger);  
    }

    public void OnClickComer()
    {
        MostrarBotonAnimacion("comer", false);
        GameObject camara = GameObject.Find("Main Camera");
        camara.GetComponent<JSONLoaderJuego0>().AbrirDespensa();
        //permiteMoverse = false;
    }

    public void OnClickDiscurso()
    {
        Destroy(GameObject.Find("Particle System"));
        MostrarBotonAnimacion("puntoDiscurso", false);
        DialogoNPC.SetActive(true);
        permiteMoverse = false;
        animator.SetTrigger("hablar");
        StartCoroutine(jsonLoader.cargarDialogosHH(diaActual - 1));
    }

    //////////////////////////////////////////////////////////////////////////////////////////

    public void MostrarBotonAnimacion(string nombre, bool mostrar)
    {
        print(nombre);
        BuscarBtnAnim(nombre).SetActive(mostrar);
    }

    public GameObject BuscarBtnAnim(string nombre)
    {
        GameObject res = null;
        foreach (var item in botonesAnimaciones) 
        {
             if (item.name.Equals(nombre))
             {
                 res = item;
             }
        }
        return res;
    }

    
    public void hablarNPC(string nombrePersonaje) 
    {
        if(jsonLoader.dialogosDia[nombrePersonaje]!=null){
            soundManager.PlaySound(nombrePersonaje);
            DialogoNPC.SetActive(true);
            GameObject fondo = GameObject.Find("Background");
            txtDialogo.text = (fondo.GetComponent<TimeDayFunction>().hora < 18) ? (""+jsonLoader.dialogosDia[nombrePersonaje]):(""+jsonLoader.dialogosNoche[nombrePersonaje]);
        }
    }
}