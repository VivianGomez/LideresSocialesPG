using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static AudioClip dormir, abrirAlgo, cocinar, 
    comer, campanaColegio, vocesColegio, hablarGente, patearBalon, hablaPlayer, wow;
    static AudioSource audioSource; 

    public  List<AudioClip> clips;

    public IEnumerator loadAudio(string path, string nombre){
        WWW request = new WWW(path);
        
        if (request.error == null)
        {
            yield return request;
            AudioClip myClip = request.GetAudioClip(false, false, AudioType.WAV); 
            myClip.name = nombre;    
            clips.Add(myClip);
        }
        else{
            print("No se puede cargar el audio");
        }
        
    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        abrirAlgo = Resources.Load<AudioClip>("audios/abrir");
        cocinar = Resources.Load<AudioClip>("audios/cocinando");
        comer = Resources.Load<AudioClip>("audios/comerCorto");
        campanaColegio = Resources.Load<AudioClip>("audios/campColegio");
        vocesColegio = Resources.Load<AudioClip>("audios/ninosEscuela");
        hablarGente = Resources.Load<AudioClip>("audios/genteHabla");
        patearBalon = Resources.Load<AudioClip>("audios/patearBalon");
        wow = Resources.Load<AudioClip>("audios/wow");

        audioSource = GetComponent<AudioSource>();
    }


    public AudioClip buscarClip(string nameClip)
    {
        AudioClip res = null;
        foreach (var item in clips) 
        {
             if (item.name.Equals(nameClip))
             {
                 res = item;
             }
        }
        return res;
    }

    public void PlaySound(string clip){
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(buscarClip(clip));
    }
}
