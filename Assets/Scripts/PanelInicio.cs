using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PanelInicio : MonoBehaviour
{
    public GameObject panelIF;
    public Image imagenLider;
    public TextMeshProUGUI infoLider;

    void Start()
    {
        infoLider.text = "";
        StartCoroutine(quitImage());
    }

    IEnumerator quitImage()
    {
        GameObject camera = GameObject.Find("Main Camera");
        //camera.GetComponent<CargaAnimatorAnimations>().Request(2);
        camera.GetComponent<CargaAnimatorAnimations>().Request(0);
        yield return new WaitForSeconds(4.5f);
        camera.GetComponent<CargaAnimatorAnimations>().Request(1);
        imagenLider.sprite = null;
        infoLider.text = "Kevin Julian Leon";
        yield return new WaitForSeconds(6);
        SceneManager.LoadSceneAsync("Cuarto");
    }
}
