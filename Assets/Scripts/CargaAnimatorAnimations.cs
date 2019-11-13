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
        if (!File.Exists("Assets/Resources/StateMachineTransitions.controller")) UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Resources/StateMachineTransitions.controller");
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
            StartCoroutine(OutputRoutine(spritesheet, int.Parse("" + actual["coordenadaX"]), int.Parse("" + actual["coordenadaY"]), ("" + actual["loop"]=="0")?false:true));

            
        }
        CreateController(); 
    }

    public IEnumerator createSaveAnim(string nombre, bool loop)
    {
        print("esta cargando todo");
        Sprite[] sprites = Resources.LoadAll<Sprite>(nombre);// load all sprites in "assets/Resources/nombre" folder
        print(sprites.Length);
        print(nombre);
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
            print("tiempo "+spriteKeyFrames[i].time + " k "+ k);
            spriteKeyFrames[i].value = sprites[i];
        }

        AnimationClipSettings animClipSett = new AnimationClipSettings();
        animClipSett.loopTime = loop;

        AnimationUtility.SetAnimationClipSettings(animClip, animClipSett);

        AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);

        AssetDatabase.CreateAsset(animClip, "assets/Resources/" + nombre +".anim");
        AssetDatabase.SaveAssets(); 
        AssetDatabase.Refresh();

       // CreateController();
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

    static void CreateController()
    {
        // Se crea el controlador
        UnityEditor.Animations.AnimatorController controller = Resources.LoadAsync("StateMachineTransitions").asset as UnityEditor.Animations.AnimatorController;

        // Se crean los parametros
        print("se agregan parametris");
        controller.AddParameter("quedaQuieto", AnimatorControllerParameterType.Trigger);
        
        controller.AddParameter("caminar", AnimatorControllerParameterType.Trigger);

        controller.AddParameter("dejaCaminar", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("dormir", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("despierta", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("sentarse", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("quedarseSentado", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("comer", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("hablar", AnimatorControllerParameterType.Trigger);
    

        // Añadir los estados
        
        var rootStateMachine = controller.layers[0].stateMachine;
        var stateMachineQuedaQuieto = rootStateMachine.AddState("quedaQuieto");

        
        var stateMachineCamina = rootStateMachine.AddState("caminar");
        stateMachineCamina.motion = Resources.LoadAsync("caminar").asset as AnimationClip;

        
        var stateMachineDuerme = rootStateMachine.AddState("dormir");
        stateMachineDuerme.motion = Resources.LoadAsync("dormir").asset as AnimationClip;

        var stateMachineDespierta = rootStateMachine.AddState("despierta");

        var stateMachineSientaSilla = rootStateMachine.AddState("sentarse");
        stateMachineSientaSilla.motion = Resources.LoadAsync("sentarse").asset as AnimationClip;

        var stateMachineComer = rootStateMachine.AddState("comer");

        var stateMachineHablar = rootStateMachine.AddState("hablar");
        stateMachineComer.motion = Resources.LoadAsync("comer").asset as AnimationClip;
        stateMachineHablar.motion = Resources.LoadAsync("hablar").asset as AnimationClip;
    


        // Añadir todas las transiciones

        var stateMachineTransitionA = rootStateMachine.AddAnyStateTransition(stateMachineQuedaQuieto);
        stateMachineTransitionA.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "quedaQuieto");

        
        var stateMachineTransitionI = stateMachineQuedaQuieto.AddTransition(stateMachineCamina);
        stateMachineTransitionI.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "caminar");

        var stateMachineTransitionJ = stateMachineCamina.AddTransition(stateMachineQuedaQuieto);
        stateMachineTransitionJ.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "dejaCaminar");

        var stateMachineTransitionB = stateMachineQuedaQuieto.AddTransition(stateMachineDuerme);
        stateMachineTransitionB.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "dormir");

        var stateMachineTransitionC = stateMachineDuerme.AddTransition(stateMachineQuedaQuieto);
        stateMachineTransitionC.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "despierta");

        var stateMachineTransitionD = stateMachineQuedaQuieto.AddTransition(stateMachineSientaSilla);
        stateMachineTransitionD.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "sentarse");

        var stateMachineTransitionH = stateMachineSientaSilla.AddTransition(stateMachineQuedaQuieto);
        stateMachineTransitionH.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "caminar");

        var stateMachineTransitionF = stateMachineQuedaQuieto.AddTransition(stateMachineComer);
        stateMachineTransitionF.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "comer");

        var stateMachineTransitionK = stateMachineHablar.AddTransition(stateMachineQuedaQuieto);
        stateMachineTransitionK.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "caminar");

        var stateMachineTransitionG = stateMachineQuedaQuieto.AddTransition(stateMachineHablar);
        stateMachineTransitionG.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "hablar");

        var stateMachineTransitionL = stateMachineComer.AddTransition(stateMachineQuedaQuieto);
        stateMachineTransitionL.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "caminar");
    
    }

}
