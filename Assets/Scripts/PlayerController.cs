using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float SecondsTillAttached = 1f;
    public float SecondsTillStuck = 3f;
    public GameObject Player;

    private Vector2 currentMousePos;
    private Vector2 currentMouseDelta;

    private Vector2 mouseStartedClick; // For swipe direction
    private Vector2 mouseFinishedClick;

    private Vector2 mouseDelta; // For swipe magnitute

    // Process Player Input Click
    public void OnMainInput(InputAction.CallbackContext context)
    {
        // Input Just Held (Left Mouse Button Held)
        if (context.started)
        {
            // Get Mouse Started Click Position
            mouseStartedClick = currentMousePos;
        }

        // Input Just stopped (Left Mouse Button Released)
        if (context.canceled)
        {
            // Get Mouse Released Position
            mouseFinishedClick = currentMousePos;

            // Throw Player
            ThrowPlayer();
        }
    }
    // Update Mouse Positiion
    public void UpdateMousePos(InputAction.CallbackContext context)
    {
        currentMousePos = context.ReadValue<Vector2>();
    }

    // Update Mouse Move Delta
    public void UpdateMoveDelta(InputAction.CallbackContext context)
    {
        currentMouseDelta = context.ReadValue<Vector2>();  
    }

    // Throw player
    private void ThrowPlayer()
    {
        Vector3 direction = (mouseFinishedClick - mouseStartedClick).normalized;
        float magnitude = currentMouseDelta.magnitude;

        Player.GetComponent<Rigidbody>().AddForceAtPosition(direction*magnitude, Player.transform.position);
    }
}
