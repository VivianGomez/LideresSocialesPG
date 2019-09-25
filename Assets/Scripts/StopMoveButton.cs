using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMoveButton : MonoBehaviour
{
    public Animator animationBtn;
    
    void Start()
    {
        animationBtn.enabled = true;
    }

    public void clickBtn()
    {
        animationBtn.enabled = false;
    }
}
