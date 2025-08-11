using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class Clicker_Player : MonoBehaviour
{
    [SerializeField] public InputActionReference map_Click_Action;
    [SerializeField] public InputActionReference map_Cursor_Action;
    [SerializeField] private float move_Speed;
    public Camera cam;
    private Vector3 goal_Position;
    private Vector3 goal_Rotation;
    private bool moving_To_Goal = false;
    private bool panning = false;


    public UnityEvent reached_Goal_Pos = new UnityEvent();

    void Start()
    {
        map_Click_Action.action.performed += Click_Recieved;

        cam = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Camera>();

        goal_Position = transform.position;
        goal_Rotation = transform.forward;

    }


    void Update()
    {
        //If we're moving to a set location, do that
        if ((Vector3.Distance(goal_Position, transform.position) > 0.1f) || (Vector3.Distance(goal_Rotation, transform.forward) > 0.1f))
        {
            moving_To_Goal = true;
            transform.position = Vector3.MoveTowards(transform.position, goal_Position, move_Speed * Time.deltaTime);
            transform.forward = Vector3.RotateTowards(transform.forward, goal_Rotation, move_Speed * Time.deltaTime, move_Speed * Time.deltaTime);

            if ((Vector3.Distance(goal_Position, transform.position) < 0.1f) && (Vector3.Distance(goal_Rotation, transform.forward) < 0.1f))
            {
                reached_Goal_Pos.Invoke();
            }
        }
        else { moving_To_Goal = false; }

        //If we're using a pointer to pan
        if ((!moving_To_Goal) && panning)
        {
            if (map_Click_Action.action.IsPressed() == false) { panning = false; }

            Vector2 pan_Input = map_Cursor_Action.action.ReadValue<Vector2>();

            Vector3 panned_Position = new Vector3(transform.position.x - pan_Input.x, transform.position.y, transform.position.z - pan_Input.y);

            transform.position = Vector3.MoveTowards(transform.position, panned_Position, move_Speed * Time.deltaTime);

            goal_Position = transform.position;
        }
    }

    private void Click_Recieved(InputAction.CallbackContext context)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            if (clickable == null) { panning = true; return; }

            clickable.On_Click(this);
            ///If we're already there, call the reached goal event now
            if (Vector3.Distance(goal_Position, transform.position) < 0.1f) { reached_Goal_Pos.Invoke(); }
        }
        else { panning = true; }


    }


    public void Set_Goal_Position(Vector3 new_Position)
    {
        goal_Position = new_Position;
    }
    
    public void Set_Goal_Rotation(Vector3 new_Rotation)
    {
        goal_Rotation = new_Rotation;
    }
}
