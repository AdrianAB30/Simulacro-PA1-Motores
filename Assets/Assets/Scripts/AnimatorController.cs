using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;

    private void OnEnable() {
        
    }

    private void OnDisable() {
        
    }

    /// <summary>
    /// Metodo <c>SetVelocity</c> define el valor de velocidad del animator controller para determinar qué animación ejecutaría.
    /// El valor de la velocidad que se le va a pasar al animator.
    ///  Si es menor a 0.1, Idle.
    ///  Si es mayor a 0.1 pero menor a 1.5, Walk.
    ///  Si es mayor a 2.5, Run.
    /// </summary>
    /// <param name="velocityCharacter">El valor de velocidad que se pasa al animator.</param>
    
    public void SetVelocity(float velocityCharacter){
        myAnimator.SetFloat("Velocity", velocityCharacter);
    }

    public void SetHurt(){
        myAnimator.SetTrigger("GoHurt");
    }

    public void SetAttack(){
        myAnimator.SetTrigger("GoAttack");
    }

    public void SetDie(){
        myAnimator.SetTrigger("GoDie");
    }
}
