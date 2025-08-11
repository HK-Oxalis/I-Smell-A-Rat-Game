using UnityEngine;
using UnityEngine.InputSystem;


public class Clicker_Player : MonoBehaviour
{
    [SerializeField] private InputActionReference click_Action;
    [SerializeField] private InputActionReference cursor_Action;
    [SerializeField] private float move_Speed;
    private Camera cam;
    private Vector3 goal_Position;
    private bool moving_To_Goal = false;
    private bool panning = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        click_Action.action.performed += Click_Recieved;

        cam = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Camera>();

        goal_Position = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(goal_Position, transform.position) > 0.1f)
        {
            moving_To_Goal = true;
            transform.position = Vector3.MoveTowards(transform.position, goal_Position, move_Speed * Time.deltaTime);
        }
        else { moving_To_Goal = false; }

        if ((!moving_To_Goal) && panning)
        {
            if (click_Action.action.WasReleasedThisFrame()) { panning = false; }

            Vector2 pan_Input = cursor_Action.action.ReadValue<Vector2>();

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
            if (clickable == null) {panning = true;  return; }

            clickable.On_Click(this);
        }
        else{ panning = true; }
        
        
    }


    public void Set_Goal_Position(Vector3 new_Position)
    {
        goal_Position = new_Position;
    }
}
