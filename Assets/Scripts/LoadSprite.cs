
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using UnityEditorInternal;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class LoadSprite : MonoBehaviour, IPointerDownHandler
{
    public RawImage output;
    RawImage image;
    string file;
    public TextAsset imageAsset;

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

    public void createSaveAnim(string nombre)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(nombre);// load all sprites in "assets/Resources/nombre" folder
        print(sprites.Length);
        print(nombre);
        AnimationClip animClip = new AnimationClip();
        animClip.frameRate = 25;   // FPS
        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_" + nombre;
        ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[sprites.Length];
        for (int i = 0; i < (sprites.Length); i++)
        {
            spriteKeyFrames[i] = new ObjectReferenceKeyframe();
            spriteKeyFrames[i].time = i;
            spriteKeyFrames[i].value = sprites[i];
        }

        AnimationClipSettings animClipSett = new AnimationClipSettings();
        animClipSett.loopTime = true;

        AnimationUtility.SetAnimationClipSettings(animClip, animClipSett);

        AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);

        AssetDatabase.CreateAsset(animClip, "assets/Resources/" + nombre + ".anim");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private IEnumerator OutputRoutine(string url)
    {
        
        var loader = new WWW(url);
        yield return loader;
        output.texture            = loader.texture;

        string file= Path.GetFileNameWithoutExtension(url);
        
        Texture2D tex = new Texture2D(2, 2);
        byte[] bytes;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        bytes = www.downloadHandler.data;
        tex.LoadImage(bytes);
        tex.EncodeToPNG();

        string path = url.Replace("file:///", "").Replace("%20"," ");

        FileUtil.CopyFileOrDirectory(path, "assets/Resources/" + file + ".png");
        AssetDatabase.Refresh();

        StartCoroutine(ProcesarTextura("assets/Resources/" + file + ".png", 380, 750));
        createSaveAnim(file);

    }    
    
    IEnumerator ProcesarTextura(string path, int SliceWidth, int SliceHeight)
    {
        print("archivo -- "+Path.GetFileNameWithoutExtension(path));
        print("ruta -- " + path);
        Texture2D texture = Resources.Load<Texture2D>(Path.GetFileNameWithoutExtension(path));
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.mipmapEnabled = true;
        importer.filterMode = FilterMode.Point;
        importer.spritePivot = Vector2.zero;
        importer.textureCompression = TextureImporterCompression.Uncompressed;

        var textureSettings = new TextureImporterSettings();

        importer.ReadTextureSettings(textureSettings);
        textureSettings.spriteMeshType = SpriteMeshType.Tight;
        textureSettings.spriteExtrude = 0;

        importer.SetTextureSettings(textureSettings);

        List<SpriteMetaData> newData = new List<SpriteMetaData>();

        
        for (int i = 0; i < texture.width; i += SliceWidth)
        {
            for (int j = 0; j < texture.height; j += SliceHeight)
            {
                SpriteMetaData smd = new SpriteMetaData();
                smd.pivot = Vector2.zero;
                smd.alignment = (int)SpriteAlignment.Center;
                smd.name = (j) / SliceHeight + ", " + i / SliceWidth;
                smd.rect = new Rect(i, j, SliceWidth, SliceHeight);

                newData.Add(smd);
            }
        }



        importer.spritesheet = newData.ToArray();


        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        AssetDatabase.Refresh();

        yield return newData.ToArray();

    }   


}