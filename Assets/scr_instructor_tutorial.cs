using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

public class scr_instructor_tutorial : MonoBehaviour
{
    [SerializeField] GameObject objcam;

    [Header("Camera Position Presets")]
    [SerializeField] GameObject npcdialog;
    [SerializeField] GameObject stationcook;
    [SerializeField] GameObject stationmix;
    [SerializeField] GameObject stationcut;
    [SerializeField] GameObject stationcutsalt;
    [SerializeField] GameObject stationplate;
    [SerializeField] GameObject stationserve;
    [SerializeField] GameObject stationclosing;

    [Header("Station References")]
    [SerializeField] GameObject cook;
    [SerializeField] GameObject mix;
    [SerializeField] GameObject cut;
    [SerializeField] GameObject plate;
    [SerializeField] GameObject serve;
    [SerializeField] GameObject closing;


    [Header("Cutting Object References")]
    [SerializeField] GameObject testcut;
    [SerializeField] GameObject carrot;
    [SerializeField] GameObject tomato;
    [SerializeField] GameObject eggplant;
    [SerializeField] GameObject onion;
    [SerializeField] GameObject potato;
    [SerializeField] GameObject meat;
    [SerializeField] GameObject butter;

    [Header("tutorial Object References")]
    [SerializeField] public GameObject tut_cut;
    [SerializeField] public GameObject tut_salt;
    [SerializeField] public GameObject tut_mix;


    [Header("Others")]
    [SerializeField] Animator txttitle;
    public GameObject targetObject;
    public scr_tutorial_controller controller;
    public float moveSpeed = 5f;    // Speed at which the camera moves towards the target
    public float rotationSpeed = 5f; // Speed at which the camera rotates towards the target
    private int crntstep = 0;
    public int crntdish = 0;

    public void StationClosing()
    {
        cook.GetComponent<scr_cook>().enabled = false;
        mix.GetComponent<scr_mixer>().enabled = false;
        cut.GetComponent<scr_station_cut>().enabled = false;
        plate.GetComponent<scr_station_plate>().enabled = false;
        serve.GetComponent<scr_station_serve>().enabled = false;
        GameObject.Find("obj_controller").GetComponent<scr_controller>().crnt_dialog_index = 999;
        GameObject.Find("obj_controller").GetComponent<scr_controller>().FinishedDialog();
        GameObject.Find("obj_controller").GetComponent<scr_controller>().enabled = false;

        targetObject = stationclosing;

        closing.GetComponent<scr_station_closing>().StartScrape();
    }

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
    public void PlayWhoosh()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(12);
    }
    public void StartCook()
    {
        if(controller.dishlist.Count > 0)
        {
            //Get Last Dish In list
            string[] last = controller.dishlist.Last().Split("_");
            string dish = last[0];
            int count = System.Convert.ToInt32(last[1]);
            crntstep = 0;
            crntdish = System.Convert.ToInt32(dish);
            CookBook(crntdish, crntstep, null);
            
            if ((count - 1) > 0)
            {
                Debug.Log((count - 1) + " amount of " + dish + "left");
                controller.dishlist.RemoveAt(controller.dishlist.Count() - 1);
                controller.dishlist.Add(dish + "_" + (count - 1));
            }
            else if ((count - 1) <= 0)
            {
                controller.dishlist.RemoveAt(controller.dishlist.Count - 1);
                Debug.Log("LAST OF CURRENT DISH WOOHOO");
            }
        }
        else
        {
            StationServe();
            Debug.Log("NO more dishes in line");
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
                        List<GameObject> list = new List<GameObject>( new GameObject[] {carrot, meat});
                        List<bool> list2 = new List<bool>(new bool[] { false, true });
                        StationCut(list, list2);
                        break;
                    case 1:
                        StationCook(ingr, 20, 10, false);
                        
                        break;
                    case 2:
                        StationPlate(dish);
                        break;
                    case 3:
                        Debug.Log("DISH IS DONE");
                        serve.GetComponent<scr_station_serve>().AddDish(dish);
                        StartCook();
                        break;
                }
                break;
            case 2:
                //Soup
                switch (step)
                {
                    case 0:
                        List<GameObject> list = new List<GameObject>(new GameObject[] { onion, carrot, tomato, potato });
                        List<bool> list2 = new List<bool>(new bool[] { false, false, true, false });
                        StationCut(list, list2);
                        break;
                    case 1:
                        StationMix(ingr, 10);
                        break;
                    case 2:
                        StationCook(ingr, 20, 20, true);
                        break;
                    case 3:
                        StationPlate(dish);
                        break;
                    case 4:
                        Debug.Log("DISH IS DONE");
                        serve.GetComponent<scr_station_serve>().AddDish(dish);
                        StartCook();
                        break;
                }
                break;
            case 3:
                //Steak
                switch (step)
                {
                    case 0:
                        List<GameObject> list = new List<GameObject>(new GameObject[] { meat, butter});
                        List<bool> list2 = new List<bool>(new bool[] { true, false});
                        StationCut(list, list2);
                        break;
                    case 1:
                        StationCook(ingr, 20, 20, false);
                        break;
                    case 2:
                        StationPlate(dish);
                        break;
                    case 3:
                        Debug.Log("DISH IS DONE");
                        serve.GetComponent<scr_station_serve>().AddDish(dish);
                        StartCook();
                        break;
                }
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
        cut.GetComponent<scr_station_cut>().ToCut.Clear();
        cut.GetComponent<scr_station_cut>().ToSalt.Clear();
        cut.GetComponent<scr_station_cut>().ToCut = cutted;
        cut.GetComponent<scr_station_cut>().ToSalt = salt;
        cut.GetComponent<scr_station_cut>().StartCut();

        PlayWhoosh();
        targetObject = stationcut;
        GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler>().ChangePhase(1);
        tut_cut.SetActive(true);
    }

    private void StationMix(List<GameObject> cutted, int goal)
    {
        mix.GetComponent<scr_mixer>().enabled = true;
        mix.GetComponent<scr_mixer>().Reinit(goal,cutted);
        PlayWhoosh();
        targetObject = stationmix;
        GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler>().ChangePhase(3);
    }

    private void StationCook(List<GameObject> cutted, int target, int length, bool pot)
    {
        GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler>().ChangePhase(4);
        cook.GetComponent<scr_cook>().enabled = true;
        cook.GetComponent<scr_cook>().ReInit(cutted);
        cook.GetComponent<scr_cook>().targettime = length;
        cook.GetComponent<scr_cook>().targetvalue = target;
        cook.GetComponent<scr_cook>().pot(pot);
        PlayWhoosh();
        targetObject = stationcook;
    }

    private void StationPlate(int dish)
    {
        GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler>().ChangePhase(4);
        plate.GetComponent<scr_station_plate>().ReInit(dish);
        PlayWhoosh();
        targetObject = stationplate;
    }

    private void StationServe()
    {
        GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler>().ChangePhase(4);
        serve.GetComponent<scr_station_serve>().readytoserve = true;
        targetObject = stationserve;

    }
}

