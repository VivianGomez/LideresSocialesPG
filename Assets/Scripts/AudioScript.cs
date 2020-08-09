using LitJson;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEditor;

public class AudioScript : MonoBehaviour
{
    public float volumen = 0.2f;
    public AudioClip MusicClip, MusicFinalOut, MusicFinalIns;
    public AudioSource MusicSource;

    public JsonData jsonData;
    public GameObject rain;

    public JsonData sonidosAmbiente;


    public int diaActual;


    void Update()
    {
        MusicSource.volume = volumen;
    }

    void Start()
    {
        if (rain != null)
        {
            rain.SetActive(false);
        }

        if (File.Exists(Application.dataPath + "/Gamedata.json"))
        {
            jsonData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
            diaActual = (int)jsonData[0];
        }
        
        if(!(SceneManager.GetActiveScene().name.Equals("Inicio")))
        {
            verificarAudioDia(diaActual);  
        }
	}

    public void empieza(JsonData sonidos){
        sonidosAmbiente = sonidos;
        volumen = 0.2f;
        verificarAudioDia(diaActual);
    }

    public void verificarAudioDiaLocal(int dia)
    {
        if (dia != 4)
        {
            MusicSource.clip = MusicClip;
            MusicSource.Play();
        }
        else
        {
            if (SceneManager.GetActiveScene().name.Equals("Calle"))
            {
                MusicSource.clip = MusicFinalOut;
                oscuroYLluvia();
            }
            else if (SceneManager.GetActiveScene().name.Equals("Cuarto") || SceneManager.GetActiveScene().name.Equals("Cocina") || SceneManager.GetActiveScene().name.Equals("Sala"))
            {
                MusicSource.clip = MusicFinalIns;
            }
            MusicSource.volume = volumen;
            MusicSource.Play();
        }
    }

    public void verificarAudioDia(int dia)
    {

        if(sonidosAmbiente!=null){

        char[] spearator = {','}; 
        string[] escenasSonidoAct = new string[] {"Calle", "Sala", "Robert"};
        string[] diasSonidoAct = new string[] {"Calle", "Sala", "Robert"};
        bool termina = false;

        int s = sonidosAmbiente.Count;

        for (int i = 0; i < s && !termina; i++)
        {

           escenasSonidoAct= (sonidosAmbiente[i]["escenas"]).ToString().Split(spearator);
           diasSonidoAct = (sonidosAmbiente[i]["dias"]).ToString().Split(spearator);
            
            if((System.Array.IndexOf (escenasSonidoAct, ""+SceneManager.GetActiveScene().name) != -1) && (System.Array.IndexOf (diasSonidoAct, ""+dia) != -1))
            {
                string nombreAudio = ""+sonidosAmbiente[i]["nombre"];

                print(nombreAudio);

                if (!File.Exists("assets/Resources/audios/" + nombreAudio + ".wav"))
                {
                    Debug.Log ("*****************************Cargando audio...*********************************");
                    StartCoroutine(loadAudio(""+sonidosAmbiente[i]["sonido"],nombreAudio));
                }
                else{

                    print("EXISTE");
                   // AssetDatabase.Refresh();
                    MusicSource.clip = Resources.Load<AudioClip>("audios/"+nombreAudio);
                    if(!(nombreAudio.Equals("ambiente"))) volumen = 1f;
                    MusicSource.Play();
                }

                termina = true;
                if (sonidosAmbiente[i]["dias"].Equals(1)) { oscuroYLluvia(); }
            }
        }   
      }      
    }

    void oscuroYLluvia()
    {
        rain.SetActive(true);
        GameObject bg = GameObject.Find("Background");
        bg.GetComponent<TimeDayFunction>().oscurecer();
    }

    public IEnumerator loadAudio(string path, string nombre){
        WWW request = new WWW(path);

        if (request.error == null)
        {
            yield return request;
            MusicSource.clip = request.GetAudioClip(false, false, AudioType.WAV);
            if(!(SceneManager.GetActiveScene().name.Equals("Inicio")))
            {
                MusicSource.Play();
            }
            if(request.progress == 1)
            {
                SavWav.Save(nombre, MusicSource.clip);
                if(SceneManager.GetActiveScene().name.Equals("Inicio")) PanelInicio.cambioEscenaPI();
            }
        }
        else
        {
            print("No se puede cargar el audio");
        }

    }

}