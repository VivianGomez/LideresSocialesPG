using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Dialogos : MonoBehaviour
{

    //public string npcName;

    //Oraciones a poner en el cuadro de dialogo, tienen un tamaño entre 5 a 10 caracteres
    //La cantidad de oraciones se configuran en el inspector

    [TextArea(5, 10)]
    public string[] sentences;
    public int currentSentence = 0;
    public bool endSentence = false;

    public string GetMsg()
    {
        return sentences[currentSentence];
    }

    public void BtnContinue()
    {

        if (currentSentence + 1 < sentences.Length)
        {
            print("actual " + currentSentence);
            print("If " + endSentence);
        }
        else
        {
            endSentence = true;
            print("Else " + endSentence);
        }
        currentSentence++;
    }

    public void BtnClose()
    {
        endSentence = false;
        currentSentence = 0;
    }
}
