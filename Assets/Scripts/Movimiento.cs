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
    public bool permiteMoverse ;
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
    public TextMeshProUGUI  txtDialogo;

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
    public GameObject btnAnimacionPrefab;

    /////////////////////////////////////////////////////////
    public GameObject panelOpcionesHablar;
    public GameObject panelOpcionesComer;
    public GameObject panelOpcionesDiscurso;


    void Start()
    {

        trigger = false;
        objeto.SetActive(false);
        imagen.SetActive(false);
        escala = transform.localScale;
        escalaX = escala.x;
        escalaY = escala.y;

        llenarBotonesAnimaciones();

        if (panelOpcionesHablar != null)
        {
            panelOpcionesHablar.SetActive(false);
        }
        if (panelOpcionesComer != null)
        {
            panelOpcionesComer.SetActive(false);
        }
        if (panelOpcionesDiscurso != null)
        {
            panelOpcionesDiscurso.SetActive(false);
        }

        if (File.Exists(Application.dataPath + "/Gamedata.json"))
        {
            jsonData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
            diaActual = (int)jsonData[0];
        }
    }

    void llenarBotonesAnimaciones()
    {
        if(SceneManager.GetActiveScene().name.Equals("Cuarto"))
        {
            llenarBtnAnimacion("dormir","","-3,194", "1,037", "0,82");
        }
        else if(SceneManager.GetActiveScene().name.Equals("Sala"))
        {
            llenarBtnAnimacion("sentarse","sentarse", "-0,4686719", "0,1153362", "-2,126913");
            llenarBtnAnimacion("sentarseSofa","sentarse", "-6,01", "2,26", "-2,126913");
        }
    }

    void llenarBtnAnimacion(string nombre, string trigger, string posX, string posY, string posZ)
    {
        GameObject nuevo = null;

        if(nombre.Equals("dormir"))
        {
            nuevo = Instantiate(dormirPrefab);
            nuevo.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnClick());
        }
        else
        {
            nuevo = Instantiate(btnAnimacionPrefab);
            nuevo.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnClickBtnAnimacion(nombre, trigger));
        }

        nuevo.name = nombre;
        nuevo.transform.SetParent(canvas.transform);
        nuevo.transform.localScale = new Vector3(1, 1, 1);
        nuevo.transform.position = new Vector3(float.Parse(posX),float.Parse(posY),float.Parse(posZ));
        nuevo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = nombre;
        nuevo.SetActive(false);
        botonesAnimaciones.Add(nuevo);
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
        

    void OnTriggerStay2D(Collider2D col)
    {
        GameObject fondo = GameObject.Find("Background");
        if (col.name == "camaClick" && fondo.GetComponent<TimeDayFunction>().hora > 18 && diaActual < 4)
        {
            mostrarBotonAnimacion("dormir", true);
        }
        else if (col.name == "silla")
        {
            mostrarBotonAnimacion("sentarse", true);
        }
        else if(col.name == "sillon")
        {
            mostrarBotonAnimacion("sentarseSofa", true);
        }
        else if (col.name == "Nevera")
        {
            if (panelOpcionesComer != null)
            {
                panelOpcionesComer.SetActive(true);
            }
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
            if (panelOpcionesDiscurso != null)
            {
                panelOpcionesDiscurso.SetActive(true);
                botonCarta.SetActive(false);
                botonPeriodico.SetActive(false);
                botonRegalo.SetActive(false);
            }
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

        mostrarBotonAnimacion("dormir",false);

        if (col.tag == "npc"){
            DialogoNPC.SetActive(false);
        }
        else if(col.name =="silla")
        {
            mostrarBotonAnimacion("sentarse", false);
        }
        else if (col.name =="sillon" )
        {
            mostrarBotonAnimacion("sentarseSofa", false);
        }
        else if (panelOpcionesHablar != null && col.name =="mama")
        {
            DialogoNPC.SetActive(false);
        }
        else if ( col.name == "PuntoDiscurso")
        {
            panelOpcionesDiscurso.SetActive(false);
            botonCarta.SetActive(true);
            botonPeriodico.SetActive(true);
            botonRegalo.SetActive(true);
        }
        else if (panelOpcionesComer != null)
        {
            panelOpcionesComer.SetActive(false);
        }

    }

    public void OnClick(){

        GameObject fondo = GameObject.Find("Background");
        

        if ( fondo.GetComponent<TimeDayFunction>().hora > 6 && diaActual < 4)
        {
            mostrarBotonAnimacion("dormir",false);
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
        mostrarBotonAnimacion(nombreBtn, false);
        animator.SetTrigger(nombreTrigger);  
    }

    public void OnClickComer()
    {
        panelOpcionesComer.SetActive(false);
        GameObject camara = GameObject.Find("Main Camera");
        camara.GetComponent<JSONLoaderJuego0>().AbrirDespensa();
        //permiteMoverse = false;
    }

///////////////////////////////////////////////////////////////////////////////////////////////////
    public void mostrarBotonAnimacion(string nombre, bool mostrar)
    {
        print(nombre);
        buscarBtnAnim(nombre).SetActive(mostrar);
    }

    public GameObject buscarBtnAnim(string nombre)
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

    ///////////////////////////////////////////////////////////////////////////////////////////////////

    
    public void hablarNPC(string nombrePersonaje) 
    {
        if(jsonLoader.dialogosDia[nombrePersonaje]!=null){
            soundManager.PlaySound(nombrePersonaje);
            DialogoNPC.SetActive(true);
            GameObject fondo = GameObject.Find("Background");
            txtDialogo.text = (fondo.GetComponent<TimeDayFunction>().hora < 18) ? (""+jsonLoader.dialogosDia[nombrePersonaje]):(""+jsonLoader.dialogosNoche[nombrePersonaje]);
        }
    }


    public void hablarNino()
    {
        Destroy(GameObject.Find("Particle System"));
        panelOpcionesDiscurso.SetActive(false);
        DialogoNPC.SetActive(true);
        permiteMoverse = false;
        animator.SetTrigger("hablar");
        StartCoroutine(jsonLoader.cargarDialogosHH(diaActual-1));
    }

}