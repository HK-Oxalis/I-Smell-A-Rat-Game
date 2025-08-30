using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class Location : MonoBehaviour, IClickable
{
    [SerializeField] Transform table_Camera_Transform; //This is an empty that represents where the camera should go after you sit down here

    private UIDocument document;
    private Transform menu_Spawn; //This is an empty that represents where the sit down menu should spawn in world space
    private Transform earshot_Indicator;

    [SerializeField] private Transform sittable_Indicator;

    private InputActionReference click_Action;
    private Clicker_Player clicker_Player;


    private void Start()
    {
        document = gameObject.transform.GetChild(0).gameObject.GetComponent<UIDocument>();
        menu_Spawn = gameObject.transform.GetChild(1);
        earshot_Indicator = gameObject.transform.GetChild(2);

        float uniform_Scale = 5 * Conversation_Playback.Room_Earshot_Scale;
        earshot_Indicator.localScale = new Vector3(uniform_Scale, uniform_Scale, uniform_Scale);

        sittable_Indicator.GetComponent<MeshRenderer>().enabled = true;
    }
    public void On_Click(Clicker_Player player)
    {
        player.Set_Goal_Position(new Vector3(transform.position.x, 0, transform.position.z));
        player.reached_Goal_Pos.AddListener(On_Player_Reach_Pos);

        this.click_Action = player.map_Click_Action;

        clicker_Player = player;
    }

    private void On_Player_Reach_Pos()
    {
        document.enabled = true;
        sittable_Indicator.GetComponent<MeshRenderer>().enabled = false;

        click_Action.action.performed += On_Unclick;
        clicker_Player.reached_Goal_Pos.RemoveListener(On_Player_Reach_Pos);

        VisualElement menu = document.rootVisualElement;
        menu.transform.position = clicker_Player.cam.WorldToScreenPoint(menu_Spawn.position);

        //call the sit down function when the button named that is clicked
        (menu.Q("Sit_Down") as Button).RegisterCallback<ClickEvent>(Sit_Down);

        earshot_Indicator.GetComponent<MeshRenderer>().enabled = true;
    }

    private void On_Unclick(InputAction.CallbackContext context)
    {
        VisualElement picked_El = document.runtimePanel.Pick(RuntimePanelUtils.ScreenToPanel(document.runtimePanel, Input.mousePosition));

        if (picked_El == document.rootVisualElement)
        {
            document.enabled = false;
            sittable_Indicator.GetComponent<MeshRenderer>().enabled = true;

            earshot_Indicator.GetComponent<MeshRenderer>().enabled = false;
            click_Action.action.performed -= On_Unclick;
        }
        
    }

    private void Sit_Down(ClickEvent evt)
    {
        Debug.Log("Sitting down");
        document.enabled = false;
        click_Action.action.performed -= On_Unclick;

        earshot_Indicator.GetComponent<MeshRenderer>().enabled = false;


        clicker_Player.Set_Goal_Position(table_Camera_Transform.position);
        clicker_Player.Set_Goal_Rotation(table_Camera_Transform.forward);

        clicker_Player.reached_Goal_Pos.AddListener(clicker_Player.Enter_Dialogue_Mode);
        clicker_Player.entering_Map_Mode.AddListener(() => sittable_Indicator.GetComponent<MeshRenderer>().enabled = true);
    }
}
