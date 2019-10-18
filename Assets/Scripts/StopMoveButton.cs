using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StopMoveButton : MonoBehaviour
{
    public Animator animationBtn;
    public JsonData jsonData;

    void Update()
    {             
        if ( this.name.Equals("ButtonGift") && File.Exists(Application.dataPath + "/Gamedata.json"))
        {   
            jsonData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
            
            animationBtn.enabled = (("0").Equals(""+jsonData[4]));
        }
        if (this.name.Equals("ButtonLetter") && File.Exists(Application.dataPath + "/Gamedata.json"))
        {
            jsonData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
            
            animationBtn.enabled = (("0").Equals("" + jsonData[5]));
        }
        if (this.name.Equals("ButtonNews") && File.Exists(Application.dataPath + "/Gamedata.json"))
        {
            jsonData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Gamedata.json"));
            
            animationBtn.enabled = (("0").Equals("" + jsonData[6]));
        }
    }

    public void clickBtn()
    {
        animationBtn.enabled = false;
    }
}
