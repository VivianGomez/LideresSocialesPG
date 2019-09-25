using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip dormir, abrirAlgo, cocinar, 
    comer, campanaColegio, vocesColegio, hablarGente, patearBalon;
    static AudioSource audioSource; 
    void Start()
    {
        dormir = Resources.Load<AudioClip>("audios/dormir");
        abrirAlgo = Resources.Load<AudioClip>("audios/abrir");
        cocinar = Resources.Load<AudioClip>("audios/cocinando");
        comer = Resources.Load<AudioClip>("audios/comerCorto");
        campanaColegio = Resources.Load<AudioClip>("audios/campColegio");
        vocesColegio = Resources.Load<AudioClip>("audios/ninosEscuela");
        hablarGente = Resources.Load<AudioClip>("audios/genteHabla");
        patearBalon = Resources.Load<AudioClip>("audios/patearBalon");


        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip){
        switch(clip){
        case "dormir":
            audioSource.PlayOneShot(dormir);
            break;
        case "abrirAlgo":
            audioSource.PlayOneShot(abrirAlgo);
            break;   
        case "cocinar":
            audioSource.PlayOneShot(cocinar);
            break;
        case "comer":
            audioSource.PlayOneShot(comer);
            break;
        case "campanaColegio":
            audioSource.PlayOneShot(campanaColegio);
            break;
        case "vocesColegio":
            audioSource.PlayOneShot(vocesColegio);
            break;
        case "hablarGente":
            audioSource.PlayOneShot(hablarGente);
            break;
        case "patearBalon":
            audioSource.PlayOneShot(patearBalon);
            break; 
        }
    }
}
