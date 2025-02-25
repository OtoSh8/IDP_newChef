using UnityEngine;

public class scr_money : MonoBehaviour
{
    public void UpdateMoney()
    {
        GameObject.Find("obj_var").GetComponent<scr_var>().UpdateMoney();
    }
}
