using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

public class scr_instructor : MonoBehaviour
{
    [SerializeField] GameObject objcam;

    [Header("Camera Position Presets")]
    [SerializeField] GameObject npcdialog;
    [SerializeField] GameObject stationcook;
    [SerializeField] GameObject stationmix;
    [SerializeField] GameObject stationcut;
    [SerializeField] GameObject stationcutsalt;

    [Header("Station References")]
    [SerializeField] GameObject cook;
    [SerializeField] GameObject mix;
    [SerializeField] GameObject cut;

    [Header("Cutting Object References")]
    [SerializeField] GameObject testcut;
    [SerializeField] GameObject carrot;

    [Header("Others")]
    [SerializeField] Animator txttitle;
    public GameObject targetObject;
    public scr_controller controller;
    public float moveSpeed = 5f;    // Speed at which the camera moves towards the target
    public float rotationSpeed = 5f; // Speed at which the camera rotates towards the target
    private int crntstep = 0;
    private int crntdish = 0;

    private void Start()
    {
        targetObject = npcdialog;
    }

    private void Update()
    {
        //Cam Control Here =>
        objcam.transform.position = Vector3.Lerp(objcam.transform.position, targetObject.transform.position, moveSpeed * Time.deltaTime);
        objcam.transform.rotation = Quaternion.Slerp(objcam.transform.rotation, targetObject.transform.rotation, rotationSpeed * Time.deltaTime);
    }

    public void StartCook()
    {
        //Get Last Dish In list
        string[] last = controller.dishlist.Last().Split("_");
        string dish = last[0];
        int count = System.Convert.ToInt32(last[1]);
        crntstep = 0;
        crntdish = System.Convert.ToInt32(dish);
        CookBook(crntdish, crntstep,null);
        controller.dishlist.RemoveAt(controller.dishlist.Count()-1);
        if((count-1) > 0)
        {
            controller.dishlist.Add(dish + "_" + count);
        }
    }

    private void CookBook(int dish, int step, List<GameObject> ingr)
    {
        switch (dish)
        {
            case 1:
                //Fried Rice
                switch (step)
                {
                    case 0:
                        List<GameObject> list = new List<GameObject>( new GameObject[] {carrot, carrot});
                        List<bool> list2 = new List<bool>(new bool[] { false, true });
                        StationCut(list, list2);
                        break;
                    case 1:
                        StationMix(ingr,10);
                        break;
                    case 2:
                        StationCook(ingr, 20, 10);
                        
                        break;
                    case 3:
                        Debug.Log("DISH IS DONE");
                        break;
                }
                break;
            case 2:
                //Soup

                break;
        }
    }

    public void FinishStep(List<GameObject> ingr)
    {
        crntstep++;
        CookBook(crntdish,crntstep,ingr);
    }

    public void DishFinished()
    {
        //DIsh Is FInished!
    }

    private void StationCut(List<GameObject> cutted,List<bool> salt)
    {
        //Gameobject Array, if needed to be salted, same index on boolean array will be true!!!
        cut.GetComponent<scr_station_cut>().enabled = true;
        cut.GetComponent<scr_station_cut>().ToCut = cutted;
        cut.GetComponent<scr_station_cut>().ToSalt = salt;
        cut.GetComponent<scr_station_cut>().StartCut();

        //Move Camera
        targetObject = stationcut;
    }

    private void StationMix(List<GameObject> cutted, int goal)
    {
        mix.GetComponent<scr_mixer>().enabled = true;
        mix.GetComponent<scr_mixer>().Reinit(goal,cutted);

        targetObject = stationmix;
    }

    private void StationCook(List<GameObject> cutted, int target, int length)
    {
        cook.GetComponent<scr_cook>().enabled = true;
        cook.GetComponent<scr_cook>().ReInit(cutted);
        cook.GetComponent<scr_cook>().targettime = length;
        cook.GetComponent<scr_cook>().targetvalue = target;

        targetObject = stationcook;
    }
}

