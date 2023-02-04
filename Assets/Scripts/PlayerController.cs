using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class PlayerController : MonoBehaviour
{
    private InputActions _inputController;
    public GameObject Player;

    public Vector2 mouseDelta;
    public Vector2 mousePosition;
    public float Swipe;

    private Rigidbody Rigidbody;

    private bool rootedIn = true;
    private bool falling = false;
    private bool LeftClicked = false;
    private float oldMag = -1;

    private Vector2 clickedPosition;

    private Vector2 startingPos;
    private Vector2 endingPos;

    [Header("Tuning Variables")]
    
    public float maxSpeed = 20;
    public float maxStrength = 5;
    public float strengthMultiplier = 1;

    private void Awake()
    {
        _inputController = new InputActions();
        _inputController.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = Player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseDelta = _inputController.Player.Mouse.ReadValue<Vector2>();

    }

    public void OnMouseDelta(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();

        
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            startingPos = mousePosition;

        }

        if (context.canceled)
        {
            endingPos = mousePosition;

            float strength = mouseDelta.magnitude * strengthMultiplier;

            strength = Mathf.Clamp(strength, 0, maxStrength);

            Vector3 direction = (endingPos - startingPos).normalized;

            Vector3 Force = direction * strength;

            Vector3 torque = Force * 0.5f;

            Rigidbody.AddForceAtPosition(Force, Player.transform.position, ForceMode.Impulse);
            //Rigidbody.AddTorque(Force, ForceMode.Impulse);

            Vector3 torqueY = Vector3.Project(torque, Vector3.forward);
            Rigidbody.AddTorque(torqueY, ForceMode.Impulse);

            Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, maxSpeed);
            Debug.Log("str: " + strength + " direction: " + direction);

        }
    }
}
