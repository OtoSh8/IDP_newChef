using UnityEngine;

public class scr_npc : MonoBehaviour
{
    void OnExit()
    {
        GameObject.Find("obj_controller").GetComponent<scr_controller>().GetCust();
    }
}
