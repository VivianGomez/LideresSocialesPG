﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PanelInicioFin : MonoBehaviour
{
    public GameObject panelIF;
    public Image imagenLider;
    public TextMeshProUGUI  infoLider;

    void Start()
    {
        infoLider.text = "";
        StartCoroutine(quitImage());
    }

     IEnumerator quitImage()
    { 
        yield return new WaitForSeconds(3);
        imagenLider.sprite = null;
        infoLider.text = "Kevin Julian León";
        yield return new WaitForSeconds(4);
        SceneManager.LoadSceneAsync("Cuarto");
    }
}
