using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class CargaAnimatorAnimations : MonoBehaviour
{
    private JsonData jData;
    public const string url = "https://lideresocialespg.firebaseio.com/juegos.json";

    // Start is called before the first frame update
    void Start()
    {
        Request();
    }

    public void Request()
    {
        WWW request = new WWW(url);

        StartCoroutine(OnResponse(request));
    }
     
    private IEnumerator OnResponse(WWW req)
    {
        yield return req;
        if (req.error == null)
        {
            jData = JsonMapper.ToObject(req.text);

            WWW reqSprite = new WWW("" + jData[1]["personaje"]["imagenPersonaje"]);

            LoadAnimations(jData[1]["animaciones"]);
        }
        else
        {
            Debug.LogError("No se pudieron cargar los datos del juego");
        }
    }

    private void LoadAnimations(JsonData animaciones)
    {
        var actual = animaciones[0];
        for (int i = 0; i < animaciones.Count; i++)
        {
            print("animaciones " + animaciones.Count);
            actual = animaciones[i];
            WWW spritesheet = new WWW("" + actual["nombreImagen"]);
            StartCoroutine(OutputRoutine(spritesheet, int.Parse("" + actual["coordenadaX"]), int.Parse("" + actual["coordenadaY"]), bool.Parse(""+actual["loop"])));
        }
    }

    public IEnumerator createSaveAnim(string nombre, bool loop)
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
        animClipSett.loopTime = loop;

        AnimationUtility.SetAnimationClipSettings(animClip, animClipSett);

        AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);

        AssetDatabase.CreateAsset(animClip, "assets/Resources/" + "nombre.anim");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        yield return sprites;
    }

    private IEnumerator OutputRoutine(WWW url, int width, int height, bool loop)
    {
        string file = Path.GetFileNameWithoutExtension(url.url);
        //print("archivo se llamaria:::" + file);

        Texture2D tex = new Texture2D(2, 2);
        byte[] bytes;
        UnityWebRequest www = UnityWebRequest.Get(url.url);
        yield return www.SendWebRequest();
        bytes = www.downloadHandler.data;
        tex.LoadImage(bytes);
        tex.EncodeToPNG();

        WebClient client = new WebClient();
        if (!File.Exists("assets/Resources/" + file + ".png"))
        {
            client.DownloadFile(url.url, @"assets/Resources/" + file + ".png");
            AssetDatabase.Refresh();

            StartCoroutine(ProcesarTextura("assets/Resources/" + file + ".png", width, height));
            StartCoroutine(createSaveAnim(file, loop));


        }

        yield return true;

    }

    IEnumerator ProcesarTextura(string path, int SliceWidth, int SliceHeight)
    {
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
