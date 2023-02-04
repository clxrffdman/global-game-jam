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
    public float Swipe;

    private Rigidbody Rigidbody;

    private bool rootedIn = true;
    private bool falling = false;
    private bool LeftClicked = false;
    private float oldMag = -1;

    private Vector2 clickedPosition;

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

    public void OnMouseInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
        }

        if (context.canceled)
        {
            Debug.Log(mouseDelta);
            Vector3 Force = new Vector3(mouseDelta.x, mouseDelta.y, 0);
            Rigidbody.AddForceAtPosition(Force, Player.transform.position);
        }
    }
}
