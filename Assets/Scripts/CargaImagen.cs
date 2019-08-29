
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using System.IO;
using UnityEngine.Networking;
using UnityEditor;

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

        string file = Path.GetFileNameWithoutExtension(url);

        Texture2D tex = new Texture2D(2, 2);
        byte[] bytes;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        bytes = www.downloadHandler.data;
        tex.LoadImage(bytes);
        tex.EncodeToPNG();

        string path = url.Replace("file:///", "").Replace("%20", " ");

        FileUtil.CopyFileOrDirectory(path, "assets/Resources/" + file + ".png");
        AssetDatabase.Refresh();
    }
}