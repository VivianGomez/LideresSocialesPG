using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateAnimationSprite : MonoBehaviour
{
    public string nombre;

    public void createSaveAnim()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(nombre); // load all sprites in "assets/Resources/nombre" folder
        AnimationClip animClip = new AnimationClip();
        animClip.frameRate = 25;   // FPS
        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_"+nombre;
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

        AssetDatabase.CreateAsset(animClip, "assets/"+nombre+".anim");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
