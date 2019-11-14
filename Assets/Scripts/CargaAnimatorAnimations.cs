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
        if (!File.Exists("Assets/Resources/StateMachineTransitions.controller"))
        {
            UnityEditor.Animations.AnimatorController controller=UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Resources/StateMachineTransitions.controller");
            controller.AddParameter("quedaQuieto", AnimatorControllerParameterType.Trigger);
        }
        
        //Request();
    }

    public void Request(int i)
    {
        WWW request = new WWW(url);

        if (i == 0) StartCoroutine(OnResponse(request,i));
        else if (i == 1) StartCoroutine(OnResponse(request,i));
    }
     
    private IEnumerator OnResponse(WWW req, int i)
    {
        yield return req;
        if (req.error == null)
        {
            jData = JsonMapper.ToObject(req.text);
            if (i == 0) LoadAnimations(jData[1]["animaciones"]);
            else if (i == 1) StartCoroutine(CreateController(jData[1]["animaciones"]));
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
            print("sigue mostrando animaciones");
            actual = animaciones[i];
            WWW spritesheet = new WWW("" + actual["nombreImagen"]);
            StartCoroutine(OutputRoutine(spritesheet, int.Parse("" + actual["coordenadaX"]), int.Parse("" + actual["coordenadaY"]), ("" + actual["loop"]=="0")?false:true));
            
        }

        print("termino");
        //StartCoroutine(CreateController(animaciones));
    }

    public IEnumerator createSaveAnim(string nombre, bool loop)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(nombre);// load all sprites in "assets/Resources/nombre" folder
       
        AnimationClip animClip = new AnimationClip();
        animClip.frameRate = 12;   // FPS
        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";
        ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[sprites.Length];
        float j = 0.0f;
        string k="";
        for (int i = 0; i < (sprites.Length); i++)
        {
            j += 0.01f;
            k=j.ToString("f2");
            spriteKeyFrames[i] = new ObjectReferenceKeyframe();
            spriteKeyFrames[i].time = (float)i/animClip.frameRate;
            spriteKeyFrames[i].value = sprites[i];
        }

        AnimationClipSettings animClipSett = new AnimationClipSettings();
        animClipSett.loopTime = loop;

        AnimationUtility.SetAnimationClipSettings(animClip, animClipSett);

        AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);

        AssetDatabase.CreateAsset(animClip, "assets/Resources/" + nombre +".anim");
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
            client.DownloadFile(url.url, "assets/Resources/" + file + ".png");
            AssetDatabase.Refresh();

            StartCoroutine(ProcesarTextura("assets/Resources/" + file + ".png", width, height));
            StartCoroutine(createSaveAnim(file, loop));


        }
        else
        {
            if(!File.Exists("assets/Resources/" + file + ".anim"))
            {
                StartCoroutine(createSaveAnim(file, loop));
            }
            
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

    public static IEnumerator CreateController(JsonData animaciones)
    {
        // Se crea el controlador
        UnityEditor.Animations.AnimatorController controller = Resources.LoadAsync("StateMachineTransitions").asset as UnityEditor.Animations.AnimatorController;

        // Se crean los parametros
        print("se agregan parametro ");

        var actual = animaciones[0];

        // Añadir los estados        
        var rootStateMachine = controller.layers[0].stateMachine;
        var stateMachineQuedaQuieto = rootStateMachine.AddState("quedaQuieto");

        var stateMachineTransitionA = rootStateMachine.AddAnyStateTransition(stateMachineQuedaQuieto);
        stateMachineTransitionA.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "quedaQuieto");

        for (int i = 0; i < animaciones.Count; i++)
        {
            actual = animaciones[i];
            WWW spritesheet = new WWW("" + actual["nombreImagen"]);
            string parametro = Path.GetFileNameWithoutExtension(spritesheet.url);
            
            controller.AddParameter(parametro, AnimatorControllerParameterType.Trigger);
            controller.AddParameter("" + actual["desactivador"], AnimatorControllerParameterType.Trigger);

            var stateMachine = rootStateMachine.AddState(parametro);
            
            print(parametro+" "+File.Exists("assets/Resources/" + parametro + ".anim"));
            stateMachine.motion = Resources.LoadAsync(parametro).asset as AnimationClip;

            var stateMachineTransitionI = stateMachineQuedaQuieto.AddTransition(stateMachine);
            stateMachineTransitionI.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, parametro);

            var stateMachineTransitionJ = stateMachine.AddTransition(stateMachineQuedaQuieto);
            stateMachineTransitionJ.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "" + actual["desactivador"]);
        }

        yield return null;
    
    }

}
