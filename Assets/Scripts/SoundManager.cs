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
            print("No se puede cargar el audio"+ nombre);
        }
        
    }

    public IEnumerator loadAndPlay(string path){
        WWW request = new WWW(path);
        
        if (request.error == null)
        {
            print(path);
            yield return request;
            AudioClip myClip = request.GetAudioClip(false, false, AudioType.WAV); 
            audioSource.volume = 0.5f;
            audioSource.clip = myClip;
            audioSource.Play();
            GameObject camera= GameObject.Find("Main Camera");
            StartCoroutine(camera.GetComponent<PanelInicio>().quitImage());
        }
        else{
            print("No se puede cargar el audio");
        }
        
    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        campanaColegio = Resources.Load<AudioClip>("audios/campColegio");
        clips.Add(campanaColegio);
        vocesColegio = Resources.Load<AudioClip>("audios/ninosEscuela");
        clips.Add(vocesColegio);
        abrirAlgo = Resources.Load<AudioClip>("audios/abrir");
        clips.Add(abrirAlgo);

        comer = Resources.Load<AudioClip>("audios/comerCorto");
        comer.name = "comer";
        clips.Add(comer);

        patearBalon = Resources.Load<AudioClip>("audios/patearBalon");
        patearBalon.name = "balon";
        clips.Add(patearBalon);

        wow = Resources.Load<AudioClip>("audios/wow");
        clips.Add(wow);

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
