using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnbaseCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;

    void Awake(){
        animator = GetComponent<Animator>();
    }
    
    void OnTriggerStay(Collider other){
        animator.Play("Turnbase");
    }

    void OnTriggerExit(Collider other){
        animator.Play("World");
    }
}
