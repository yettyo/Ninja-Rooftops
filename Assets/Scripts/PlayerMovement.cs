using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Forever;
using Lean.Touch;
using Unity.VisualScripting;
using System;

public class PlayerMovement : MonoBehaviour
{
    LaneRunner runner;
    Rigidbody rb;
    public float jumpForce = 10f;
    public float slideDuration = 1f;
    public float slideHeight = 0.5f;
    public float originalHeight = 2f;

    private bool isSliding = false;
    private bool isGrounded = true;
    void Start()
    {
        runner = GetComponent<LaneRunner>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() 
    {
        LeanTouch.OnFingerSwipe += HandleSwipe;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerSwipe -= HandleSwipe;
    }

    private void HandleSwipe(LeanFinger finger)
    {
        Vector2 swipeDirection =  finger.SwipeScreenDelta; 

        if(Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y)) 
        {
            if(swipeDirection.x > 0)
            {
                runner.lane++;
            }
            else if(swipeDirection.x <0)
            {
                runner.lane--;
            }
        }
        else
        {
            if(swipeDirection.y > 0)
            {
                Jump();
            }
            else if(swipeDirection.y < 0)
            {
                Slide();
            }
        }
    }

    private void Jump()
    {
        if(isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; //we are on air with this one!
            Debug.Log("Jump!");
        }  
    }

    private void Slide()
    {
        if(!isSliding)
        {
            isSliding = true;
            Debug.Log("Slide!");
            StartCoroutine(DoSlide());
        }
    }

    private IEnumerator DoSlide()
    {
        // Temporarily reduce the player's height (simulate crouching or sliding)
        Vector3 originalScale = transform.localScale;
        transform.localScale = new Vector3(originalScale.x, slideHeight, originalScale.z);

        // Wait for the slide duration
        yield return new WaitForSeconds(slideDuration);

        // Reset the player's height
        transform.localScale = originalScale;

        isSliding = false; // End sliding
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the player touches the ground again, mark as grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            runner.follow = false;
        }
     }
}
