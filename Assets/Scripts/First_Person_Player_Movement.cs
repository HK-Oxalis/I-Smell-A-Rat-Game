using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float gravity = -10;
    [SerializeField] private float camera_Sensitivity = 100;
    [SerializeField] private bool invert_Cam_Y = true;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference move_Action;
    [SerializeField] private InputActionReference look_Action;

    private float fall_Velocity = 0;
    private CharacterController controller;
    private Camera cam;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        Vector2 move_Input = move_Action.action.ReadValue<Vector2>();
        move_Input.Normalize();

        Vector2 look_Input = look_Action.action.ReadValue<Vector2>();

        look_Input.Normalize();

        look_Input *= Time.deltaTime * camera_Sensitivity;

        if (invert_Cam_Y) { look_Input.y *= -1; }

        this.transform.Rotate(0, look_Input.x, 0);
        cam.transform.Rotate(look_Input.y, 0, 0);

        if (controller.isGrounded) { fall_Velocity = 0; }

        fall_Velocity += gravity * Time.deltaTime;

        
        Vector3 final_Movement = new Vector3(move_Input.x, fall_Velocity, move_Input.y);

        controller.Move(transform.TransformDirection(final_Movement) * speed * Time.deltaTime);
    }
}
