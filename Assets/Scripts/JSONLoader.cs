﻿using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;
using LitJson;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Net;
using System;

public class JSONLoader : MonoBehaviour
{
    private string jsonString;
    private JsonData jData;

    public GameObject jugador;
    public SpriteRenderer backgroundSprite;
    public GameObject loadingSpriteStart;
    public GameObject animacion;
    

    //public const string url ="https://firebasestorage.googleapis.com/v0/b/lideresocialespg.appspot.com/o/juego0.json?alt=media&token=3d8deac2-9fd0-4a22-98a3-3bc7629f809b";
    public const string url ="https://lideresocialespg.firebaseio.com/juegos.json";
    void Start()
    {
        loadingSpriteStart.SetActive(false);
        Request();
    }

    public void Request()
    {
        WWW request = new WWW(url);
        loadingSpriteStart.SetActive(true);
        StartCoroutine(OnResponse(request));
    }

    private IEnumerator OnResponse(WWW req)
    {
        yield return req;
        //string path=Application.dataPath + "/Resources/data.json";
        if (req.error == null)
        {
           //jsonString = File.ReadAllText(path);
           //print(req.text);
           jData = JsonMapper.ToObject(req.text);

           WWW reqSprite = new WWW(""+jData[0]["personaje"]["imagenPersonaje"]);
           StartCoroutine(LoadSprite(reqSprite));

           LoadScene(jData[0]["escenas"]);
           
        }
        else
        {
            Debug.LogError("No se puedieron cargar los datos del juego");
        }
    }

        private IEnumerator  LoadSprite (WWW spriteWWW)
        {
            yield return spriteWWW;
            if (spriteWWW.error == null)
            {
                jugador.GetComponent<SpriteRenderer>().sprite = Sprite.Create(spriteWWW.texture, new Rect(0, 0, spriteWWW.texture.width, spriteWWW.texture.height), new Vector2(0.5f, 0.5f));

                Vector2 S = jugador.GetComponent<SpriteRenderer>().sprite.bounds.size;
                jugador.GetComponent<BoxCollider2D>().size = S;
                //jugador.GetComponent<BoxCollider2D>().offset = new Vector2 ((S.x / 2), 0);
            }
            else
            {
                Debug.LogError("No se puede cargar la imágen del personaje");
            }

        }

        private void LoadScene(JsonData escenas)
        {
            var actual = escenas[0];
            var interaccionActual = escenas[0];
            var spriteActual = escenas[0];
        for (int i = 0; i < escenas.Count; i++)
            {
               actual = escenas[i];
               if((""+actual["nombreEscena"]).Equals(""+SceneManager.GetActiveScene().name))
               {
                    WWW reqBackground = new WWW(""+actual["imagenFondo"]);
                    StartCoroutine(LoadBackGround(reqBackground, ""+actual["nombreEscena"]));

                    for (int j = 0; j < actual["interacciones"].Count; j++)
                    {
                        interaccionActual = actual["interacciones"][j];
                        createCollidersInteractions(""+interaccionActual["colliderActivador"], interaccionActual["coordenadasCollider"], interaccionActual["offsetCollider"], interaccionActual["tamCollider"]);

                        for (int k=0;k<interaccionActual["sprites"].Count;k++)
                         {
                        spriteActual = interaccionActual["sprites"][k];
                        WWW spritesheet = new WWW("" + spriteActual["nombreImagen"]);
                        StartCoroutine(OutputRoutine(spritesheet,int.Parse("" + spriteActual["coordenadaX"]), int.Parse("" + spriteActual["coordenadaY"])));

                         }
                    
                }
                    
               }             
            }
        }

        private IEnumerator LoadBackGround(WWW wwwBG, string nombre){
            yield return wwwBG;
            if (wwwBG.error == null)
            {
                backgroundSprite.sprite = Sprite.Create(wwwBG.texture, new Rect(0, 0, wwwBG.texture.width, wwwBG.texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                Debug.LogError("No se puede cargar el fondo de la escena "+nombre);
            }
        }

        private void createCollidersInteractions(string nombreActivador, JsonData posicion, JsonData offset, JsonData tamanio){
            GameObject objToSpawn;
            objToSpawn = new GameObject(nombreActivador);
            float x = float.Parse(""+posicion[0]);
            float y = float.Parse(""+posicion[1]);
            float z = float.Parse(""+posicion[2]);

            float xO = float.Parse(""+offset[0]);
            float yO = float.Parse(""+offset[1]);

            float xT = float.Parse(""+tamanio[0]);
            float yT = float.Parse(""+tamanio[1]);

            objToSpawn.transform.position = new Vector3(x,y,z);

            //Add Components
            objToSpawn.AddComponent<BoxCollider2D>();
            var coll = objToSpawn.GetComponent<BoxCollider2D>();
            coll.isTrigger = true;
            //define el centro del collider
            coll.offset = new Vector2(xO,yO);
            //define el tamanio del collider
            coll.size = new Vector2(xT,yT);
        }

    public IEnumerator createSaveAnim(string nombre)
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

        AssetDatabase.CreateAsset(animClip, "assets/Resources/an.anim");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        yield return sprites;
    }

    private IEnumerator OutputRoutine(WWW url, int width, int height)
    {
        string file = Path.GetFileNameWithoutExtension(url.url);
        print("archivo se llamaria:::" + file);

        Texture2D tex = new Texture2D(2, 2);
        byte[] bytes;
        UnityWebRequest www = UnityWebRequest.Get(url.url);
        yield return www.SendWebRequest();
        bytes = www.downloadHandler.data;
        tex.LoadImage(bytes);
        tex.EncodeToPNG();

        WebClient client = new WebClient();
        if(!File.Exists("assets/Resources/" + file + ".png"))
        {
            client.DownloadFile(url.url, @"assets/Resources/" + file + ".png");
            AssetDatabase.Refresh();

            StartCoroutine(ProcesarTextura("assets/Resources/" + file + ".png", width, height));
            StartCoroutine(createSaveAnim(file));
            

        }
        loadingSpriteStart.SetActive(false);

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
