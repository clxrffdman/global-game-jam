using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;

public class PlayerController : UnitySingleton<PlayerController>
{
    public enum DragState { Null, Left, Right };
    [Header("Tuning Variables")]

    public LayerMask groundLayer;
    public float maxSpeed = 20;
    public float maxStrength = 5;
    public float strengthMultiplier = 1;
    public float torqueStrength = 1;
    public float camForwardScalar = 0;

    [Header("Root System")]
    public float SecondsTillAttached = 1f;
    public float SecondsTillStuck = 3f;

    [Header("Dependencies")]
    public GameObject Player;
    public Rigidbody rb;

    private Vector2 currentMousePos;
    private Vector2 currentMouseDelta;

    private Vector2 mouseStartedClick; // For swipe direction
    private Vector2 mouseFinishedClick;

    private Vector2 mouseDelta; // For swipe magnitute

    private bool ClimbingWall = false;

    [Header("Current State")]

    public DragState isDragging;

    private void Start()
    {
        rb = Player.GetComponent<Rigidbody>();
    }

    private void Update()
    {

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        if (Input.GetKeyDown(KeyCode.R))
        {

        }
    }




    // Process Player Input Click
    public void OnMainInput(InputAction.CallbackContext context)
    {
        // Input Just Held (Left Mouse Button Held)
        if (context.started)
        {
            if (isDragging == DragState.Right)
            {
                return;
            }
            // Get Mouse Started Click Position
            mouseStartedClick = currentMousePos;

            isDragging = DragState.Left;
        }


        // Input Just stopped (Left Mouse Button Released)
        if (context.canceled)
        {
            if (isDragging != DragState.Left)
            {
                return;
            }
            // Get Mouse Released Position
            mouseFinishedClick = currentMousePos;
            mouseDelta = currentMouseDelta;

            // Remember if player was locked in or not
            // Done here, before we clear springs, so we can use this info in ThrowPlayer later
            ClimbingWall = false;
            RaycastHit hit;
            bool TouchingFloor = Physics.Raycast(Player.transform.position, Vector3.down, out hit, 2f, groundLayer);

            if (RootsController.Instance.rootSprings.Count > 0 && !TouchingFloor)
            {
                ClimbingWall = true;
            }
            RootsController.Instance.ClearAllSprings();
            // Throw Player
            ThrowPlayer(true);
            isDragging = DragState.Null;
        }
    }

    public void OnAltInput(InputAction.CallbackContext context)
    {
        // Input Just Held (Left Mouse Button Held)
        if (context.started)
        {
            if (isDragging == DragState.Left)
            {
                return;
            }
            // Get Mouse Started Click Position
            mouseStartedClick = currentMousePos;

            isDragging = DragState.Right;
        }

        // Input Just stopped (Left Mouse Button Released)
        if (context.canceled)
        {
            if (isDragging != DragState.Right)
            {
                return;
            }
            // Get Mouse Released Position
            mouseFinishedClick = currentMousePos;
            mouseDelta = currentMouseDelta;

            // Remember if player was locked in or not
            // Done here, before we clear springs, so we can use this info in ThrowPlayer later
            ClimbingWall = false;
            RaycastHit hit;
            bool TouchingFloor = Physics.Raycast(Player.transform.position, Vector3.down, out hit, 2f, groundLayer);

            if (RootsController.Instance.rootSprings.Count > 0 && !TouchingFloor)
            {
                ClimbingWall = true;
            }
            RootsController.Instance.ClearAllSprings();
            // Throw Player
            ThrowPlayer(false);
            isDragging = DragState.Null;
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
    private void ThrowPlayer(bool forward)
    {
        // Get Player Rigidbody
        Rigidbody RB = Player.GetComponent<Rigidbody>();

        Vector3 direction = (Vector3)((mouseFinishedClick - mouseStartedClick).normalized);

        // If player not on wall, move forward instead of up
        if (ClimbingWall == false)
        {
            direction += (Camera.main.transform.forward.normalized * camForwardScalar * (forward ? 1 : -1));
        }

        float magnitude = mouseDelta.magnitude * strengthMultiplier;

        // Cap magnitude of Movement at Max Strength
        magnitude = Mathf.Clamp(magnitude, 0, maxStrength);

        // Secret boost for climbing wall
        if (ClimbingWall == true)
        {
            magnitude += 5;
        }

            // Move player in direction at magnitude
        RB.AddForceAtPosition(direction * magnitude, Player.transform.position, ForceMode.Impulse);

        // Rotate player
        Vector3 torque = direction * magnitude * torqueStrength;
        Vector3 torqueY = Vector3.Project(torque, Vector3.right);
        RB.AddTorque(torqueY, ForceMode.Impulse);

        // Clamp player speed
        RB.velocity = Vector3.ClampMagnitude(RB.velocity, maxSpeed);
        Debug.Log("str: " + magnitude + " direction: " + direction);
        Debug.DrawLine(Player.transform.position, Player.transform.position + direction, Color.red, 10f, false);
    }
}
