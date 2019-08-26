
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;

[RequireComponent(typeof(Button))]
public class CargaImagen : MonoBehaviour, IPointerDownHandler
{
    public RawImage output;

    //
    // Standalone platforms & editor
    //
    public void OnPointerDown(PointerEventData eventData) { }

   
    public void OnClick()
    {
        var extensions = new[] {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            };
        var paths = LoadImage.OpenFilePanel("Title", "", extensions, false);
        if (paths.Length > 0)
        {
            StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
        }
    }

    private IEnumerator OutputRoutine(string url)
    {
        var loader = new WWW(url);
        yield return loader;
        output.texture = loader.texture;
    }
}