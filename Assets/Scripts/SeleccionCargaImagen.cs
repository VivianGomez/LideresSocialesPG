using System.Collections;
using UnityEngine;
using SFB;

public class SeleccionCargaImagen : MonoBehaviour
{
    private string _path;

    
    public void OnClick()
    {
        var extensions = new[] {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            };
        WriteResult(LoadImage.OpenFilePanel("Open File", "", extensions, true));
    }

    public void WriteResult(string[] paths)
    {
        if (paths.Length == 0)
        {
            return;
        }

        _path = "";
        foreach (var p in paths)
        {
            _path += p + "\n";
        }
    }

    public void WriteResult(string path)
    {
        _path = path;
    }
}