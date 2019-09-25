using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Colegio : MonoBehaviour
{
    public GameObject panelIF;
    public Image imagenLider;
    public TextMeshProUGUI infoLider;

    void Start()
    {
        infoLider.text = "Kevin va a sus clases...";
        StartCoroutine(quitImage());
    }

    IEnumerator quitImage()
    {
        yield return new WaitForSeconds(4.5f);
        infoLider.text = "";
        yield return new WaitForSeconds(4.5f);
        imagenLider.sprite = null;
        infoLider.text = "Tras una larga jornada...";
        yield return new WaitForSeconds(5);
        SceneManager.LoadSceneAsync("Calle");
    }
}
