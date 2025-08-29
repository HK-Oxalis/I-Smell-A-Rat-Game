using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UIElements;

public enum Player_Mode
{
    Map,
    Dialogue,
    Door
}

public class Clicker_Player : MonoBehaviour
{
    [SerializeField] public InputActionReference map_Click_Action;
    [SerializeField] public InputActionReference map_Cursor_Action;
    [SerializeField] private UIDocument table_Ui;
    [SerializeField] private float move_Speed;
    private Vector3 MAP_START = new Vector3(-8.5f, 20, -6);
    private Vector3 MAP_BOUNDS = new Vector3(-30, 20, 14.5f);
    public Camera cam;


    private Vector3 goal_Position;
    private Vector3 goal_Rotation;
    private bool moving_To_Goal = false;
    private bool panning = false;
    public UnityEvent reached_Goal_Pos = new UnityEvent();
    public UnityEvent entering_Map_Mode = new UnityEvent();
    public UnityEvent entering_Dialogue_Mode = new UnityEvent();



    private Player_Mode mode = Player_Mode.Map;

    void Start()
    {
        map_Click_Action.action.performed += Click_Recieved;

        cam = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Camera>();

        //Enter_Map_Mode();
        Enter_Door_Mode();

        cam.transform.forward = goal_Rotation;

    }


    void Update()
    {
        //If we're moving to a set location, do that
        if ((Vector3.Distance(goal_Position, transform.position) > 0.1f) || (Vector3.Distance(goal_Rotation, cam.transform.forward) > 0.1f))
        {
            moving_To_Goal = true;
            transform.position = Vector3.MoveTowards(transform.position, goal_Position, move_Speed * Time.deltaTime);
            cam.transform.forward = Vector3.RotateTowards(cam.transform.forward, goal_Rotation, move_Speed * Time.deltaTime, move_Speed * Time.deltaTime);

            if ((Vector3.Distance(goal_Position, transform.position) < 0.1f) && (Vector3.Distance(goal_Rotation, cam.transform.forward) < 0.1f))
            {
                reached_Goal_Pos.Invoke();
                reached_Goal_Pos.RemoveAllListeners();
            }
        }
        else { moving_To_Goal = false; }

        //If we're using a pointer to pan
        if ((!moving_To_Goal) && panning)
        {
            if (map_Click_Action.action.IsPressed() == false) { panning = false; }

            Vector2 pan_Input = map_Cursor_Action.action.ReadValue<Vector2>();

            Vector3 panned_Position = new Vector3(transform.position.x - pan_Input.x, transform.position.y, transform.position.z - pan_Input.y);

            if(panned_Position.x > MAP_START.x){panned_Position.x = MAP_START.x; }
            if(panned_Position.z < MAP_START.z){ panned_Position.z = MAP_START.z; }

            transform.position = Vector3.MoveTowards(transform.position, panned_Position, move_Speed * Time.deltaTime);

            goal_Position = transform.position;
        }
    }

    private void Click_Recieved(InputAction.CallbackContext context)
    {
        if (mode == Player_Mode.Map) { Handle_Map_Click(); }

    }

    private void Handle_Map_Click()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Chairs")))
        {
            Debug.Log("Ray: " + hit.collider.name);
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
        if(new_Position.y ==0){ new_Position.y = transform.position.y; }
        goal_Position = new_Position;
    }

    public void Set_Goal_Rotation(Vector3 new_Rotation)
    {
        goal_Rotation = new_Rotation;
    }

    public void Enter_Map_Mode()
    {
        entering_Map_Mode.Invoke();
        this.mode = Player_Mode.Map;

        table_Ui.enabled = false;

        goal_Position = MAP_START;
        goal_Rotation = new Vector3(0, -1, 0);
    }

    public void Enter_Dialogue_Mode()
    {
        entering_Dialogue_Mode.Invoke();
        this.mode = Player_Mode.Dialogue;

        table_Ui.enabled = true;
        table_Ui.rootVisualElement.Q("Stand_Up").RegisterCallbackOnce<ClickEvent>(Stand_Up);
    }

    public void Enter_Door_Mode()
    {
        this.mode = Player_Mode.Door;
        table_Ui.enabled = false;

        goal_Position = this.transform.position;
        goal_Rotation = this.transform.forward;
    }

    private void Stand_Up(ClickEvent evt)
    {
        Enter_Map_Mode();
    }
}
