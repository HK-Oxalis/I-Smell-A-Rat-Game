using UnityEngine;

public class Location : MonoBehaviour, IClickable
{
    public void On_Click(Clicker_Player player)
    {
        Debug.Log("Location clicked");
        player.Set_Goal_Position(new Vector3(transform.position.x, 0, transform.position.z));
    }
}
