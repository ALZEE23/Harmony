using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnbaseCamera : MonoBehaviour
{
    public GameObject ui;
    public Transform enemy;
    public GameObject player;
    public Animator animator;


    void Start(){
        
    }
    void OnEnable()
    {
        // Enable input actions if needed
        if (Player.playerInputActions != null)
        {
            Player.playerInputActions.Player.Enable();
        }
    }

    void OnDisable()
    {
        // Disable input actions if needed
        if (Player.playerInputActions != null)
        {
            Player.playerInputActions.Player.Disable();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Transform playerTransform = other.transform;

            // Mengubah posisi player
            Vector3 newPosition = playerTransform.position;
            newPosition.x = enemy.position.x - 6;
            newPosition.z = enemy.position.z; // Menggeser 6 unit ke kiri
            playerTransform.position = newPosition;
            if (Player.playerInputActions != null)
            {
                Player.playerInputActions.Player.Disable(); // Disable player input when in trigger
            }
            animator.Play("Turnbase");
            // Debug.Log("kena");
            ui.SetActive(true);

            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Player.playerInputActions != null)
            {
                Player.playerInputActions.Player.Enable(); // Enable player input when out of trigger
            }
            animator.Play("World");
            Debug.Log("keluar");
        }
    }
}
