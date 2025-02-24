using UnityEngine;

public class scr_station_serve : MonoBehaviour
{
    [SerializeField] GameObject par_toserve;
    private int totalmoney = 0;
    private int crntamt = 0;
    public bool readytoserve = false;


    [SerializeField] Material fr;
    [SerializeField] Material soup;

    public void AddDish(int no)
    {
        par_toserve.transform.GetChild(crntamt).gameObject.SetActive(true);
        switch (no)
        {
            case 1:
                par_toserve.transform.GetChild(crntamt).transform.GetChild(0).GetComponent<MeshRenderer>().material = fr;
                totalmoney += 20;
                break;
            case 2:
                par_toserve.transform.GetChild(crntamt).transform.GetChild(0).GetComponent<MeshRenderer>().material = soup;
                totalmoney += 12;
                break;
        }
        crntamt++;

    }
}
