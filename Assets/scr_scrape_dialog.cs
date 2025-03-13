using TMPro;
using UnityEngine;

public class scr_scrape_dialog : MonoBehaviour
{
    [SerializeField] scr_controller ctrl;

    [SerializeField] TextMeshProUGUI txtdialog;

    public int crnt_indx = 0;
 
    public void ChangeDialog()
    {
        int indlength = 0;

        switch (GameObject.Find("station_closing").GetComponent<scr_station_closing>().crnt_person)
        {
            case 0:
                //Pride
                txtdialog.text = ctrl.GetComponent<scr_controller>().Customer_Prideful[crnt_indx];
                indlength = ctrl.GetComponent<scr_controller>().Customer_Prideful.Length;
                break;
            case 1:
                //Pride
                txtdialog.text = ctrl.GetComponent<scr_controller>().Customer_Saving[crnt_indx];
                indlength = ctrl.GetComponent<scr_controller>().Customer_Saving.Length;
                break;
        }

        if(crnt_indx >= indlength - 1)
        {
            crnt_indx = 0;
        }
        else
        {
            crnt_indx++;
        }
        
        
    }
}
