using UnityEngine;

public class scr_money : MonoBehaviour
{
    public void UpdateMoneyLmao()
    {
        GameObject.Find("obj_var").GetComponent<scr_var>().UpdateMoneyText();
    }
}
