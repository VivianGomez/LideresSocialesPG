using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private void Start()
    {
        if(!File.Exists("Assets/StateMachineTransitions.controller")) CreateController();        
    }

    static void CreateController()
    {
        // Se crea el controlador
        var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Resources/StateMachineTransitions.controller");

        // Se crean los parametros
        controller.AddParameter("quedaQuieto", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("camina", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("dejaCaminar", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("duerme", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("despierta", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("sientaSilla", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("quedarseSentado", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("comer", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("hablar", AnimatorControllerParameterType.Trigger);

        // Añadir los estados
        var rootStateMachine = controller.layers[0].stateMachine;
        var stateMachineQuedaQuieto = rootStateMachine.AddState("quedaQuieto");
        var stateMachineCamina = rootStateMachine.AddState("camina");
        stateMachineCamina.motion = Resources.Load("caminar") as AnimationClip;

        var stateMachineDuerme = rootStateMachine.AddState("duerme");
        stateMachineDuerme.motion = Resources.LoadAsync("dormir").asset as AnimationClip;

        var stateMachineDespierta = rootStateMachine.AddState("despierta");

        var stateMachineSientaSilla = rootStateMachine.AddState("sientaSilla");
        stateMachineSientaSilla.motion = Resources.LoadAsync("sentarse").asset as AnimationClip;

        var stateMachineComer = rootStateMachine.AddState("comer");
        
        var stateMachineHablar = rootStateMachine.AddState("hablar");
        stateMachineComer.motion = Resources.LoadAsync("comer").asset as AnimationClip;
        stateMachineHablar.motion = Resources.LoadAsync("hablar").asset as AnimationClip;


        // Añadir todas las transiciones

        var stateMachineTransitionA = rootStateMachine.AddAnyStateTransition(stateMachineQuedaQuieto);
        stateMachineTransitionA.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "quedaQuieto");

        var stateMachineTransitionI = stateMachineQuedaQuieto.AddTransition(stateMachineCamina);
        stateMachineTransitionI.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "camina");

        var stateMachineTransitionJ = stateMachineCamina.AddTransition(stateMachineQuedaQuieto);
        stateMachineTransitionJ.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "dejaCaminar");
        
        var stateMachineTransitionB = stateMachineQuedaQuieto.AddTransition(stateMachineDuerme);
        stateMachineTransitionB.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "duerme");

        var stateMachineTransitionC = stateMachineDuerme.AddTransition(stateMachineQuedaQuieto);
        stateMachineTransitionC.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "despierta");

        var stateMachineTransitionD = stateMachineQuedaQuieto.AddTransition(stateMachineSientaSilla);
        stateMachineTransitionD.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "sientaSilla");

        var stateMachineTransitionH = stateMachineSientaSilla.AddTransition(stateMachineQuedaQuieto);
        stateMachineTransitionH.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "camina");

        var stateMachineTransitionF = stateMachineQuedaQuieto.AddTransition(stateMachineComer);
        stateMachineTransitionF.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "comer");

        var stateMachineTransitionK = stateMachineHablar.AddTransition(stateMachineQuedaQuieto);
        stateMachineTransitionK.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "camina");

        var stateMachineTransitionG = stateMachineQuedaQuieto.AddTransition(stateMachineHablar );
        stateMachineTransitionG.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "hablar");

        var stateMachineTransitionL = stateMachineComer.AddTransition(stateMachineQuedaQuieto);
        stateMachineTransitionL.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "camina");
    }
}
