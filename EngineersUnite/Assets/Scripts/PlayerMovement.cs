﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    private CharacterSwitcher charSwitchScript;
    public Animator animator;

    private float horizontalMove = 0f;
    public float runSpeed = 30f;

    private bool jump, crouch = false;

    private Rigidbody2D m_rigidbody2D;
    private bool isfrozen = false;
    private bool hasFrozenPowerUp = true;



    void Start()
    {
        charSwitchScript = (CharacterSwitcher)FindObjectOfType(typeof(CharacterSwitcher));
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check whether this student is active.
        if (!name.Equals(this.charSwitchScript.getActiveCharacter()))
            return;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("IsJumping", true);
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            FallThroughPlatform();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            print("T pressed");

            if (hasFrozenPowerUp)
            {
                if (isfrozen)
                {
                    hasFrozenPowerUp = false;
                    m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                else
                {
                    hasFrozenPowerUp = true;
                    m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                isfrozen = !isfrozen;
            }
        }
    }

    public void FallThroughPlatform()
    {
        gameObject.layer = LayerMask.NameToLayer("Platform");
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false; crouch = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
            other.gameObject.SetActive(false);

        if (other.gameObject.CompareTag("Fire"))
        {
            Debug.Log("Entered fire, restarting level.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Platform"))
            gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
