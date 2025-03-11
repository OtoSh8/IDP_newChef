using UnityEngine;
using EzySlice;
using System.Security.Cryptography;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;

public class scr_knife : MonoBehaviour
{
    public GameObject campos;
    public GameObject camposstation;

    public Material crossSectionMaterial = null; // Material for the sliced section
    public GameObject detected;

    public GameObject slicespawn;
    public scr_station_cut mainguy;


    public int slicedcount = 0;
    private float slicedist;
    public bool cutting = false;

    public bool salt;
    private int saltgoal;
    private GameObject cutpar;
    public List<GameObject> cutpars = new List<GameObject>();

    [SerializeField] GameObject parsalt;
    private bool isSalting = false;
    [SerializeField] public GameObject ui_icon_cut;
    [SerializeField] public GameObject ui_icon_salt;

    public void Reinit()
    {
        slicedcount = 0;
        /*this.transform.parent.parent.localPosition = new Vector3(0,0,0);*/
        cutting = false;
        cutpar = null;
        
    }

    private void Update()
    {
        if (isSalting)
        {
            ui_icon_cut.SetActive(false);
            ui_icon_salt.SetActive(true);
            if (parsalt.GetComponent<scr_salt>().amt > 10)
            {
                FinishSalting();
            }
        }
        else
        {
            ui_icon_salt.SetActive(false);
            ui_icon_cut.SetActive(true);
            return;
        }
    }

    public void StartSalting()
    {
        if (GameObject.Find("obj_arduino_handler") != null)
        {
            GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler>().ChangePhase(2);
        }


        parsalt.SetActive(true);
        parsalt.GetComponent<scr_salt>().amt = 0;
        isSalting = true;

        if(GameObject.Find("obj_instructor").GetComponent<scr_instructor>() != null)
        {
            GameObject.Find("obj_instructor").GetComponent<scr_instructor>().targetObject = campos;
        }
        else
        {
            GameObject.Find("obj_instructor").GetComponent<scr_instructor_tutorial>().targetObject = campos;
            GameObject.Find("obj_instructor").GetComponent<scr_instructor_tutorial>().tut_salt.SetActive(true);
        }
        

        this.transform.parent.GetComponent<MeshRenderer>().enabled = false;
    }

    public void FinishSalting()
    {
        GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler>().ChangePhase(1);

        isSalting = false;
        parsalt.SetActive(false);
        salt = false;
        if (GameObject.Find("obj_instructor").GetComponent<scr_instructor>() != null)
        {
            GameObject.Find("obj_instructor").GetComponent<scr_instructor>().targetObject = camposstation;
        }
        else
        {
            GameObject.Find("obj_instructor").GetComponent<scr_instructor_tutorial>().targetObject = camposstation;
        }

        
        this.transform.parent.GetComponent<MeshRenderer>().enabled = true;
        Repos();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Sliceable") && cutting == false) // Ensure the object is tagged properly
        {
            if(slicedcount == 0) 
            { 
                slicedist = other.GetComponent<MeshRenderer>().bounds.size.x;
                
                cutpar = new GameObject();
                cutpar.transform.position = slicespawn.transform.position;
            }
            detected = other.gameObject;
            CutIt();
            cutting = true;
        }
    }

    public void CutIt()
    {
        if (slicedcount < 5)
        {
            SliceObject(detected);
            slicedcount++;
        }
    }
    public void PosStart( float x)
    {
        this.transform.parent.parent.gameObject.transform.position = new Vector3(slicespawn.transform.position.x - (x/2) + (x/6), this.gameObject.transform.parent.parent.position.y, this.transform.parent.parent.gameObject.transform.position.z);
        Debug.Log("STARTPOS");
    }

    public void Repos()
    {
        if (slicedcount < 5)
        {
            this.transform.parent.parent.gameObject.transform.position = new Vector3(this.gameObject.transform.parent.parent.position.x + slicedist / 6, this.gameObject.transform.parent.parent.position.y, this.transform.parent.parent.gameObject.transform.position.z);
            cutting = false;
        }
        else if(salt == true)
        {
            StartSalting();
        }
        else if (salt == false)
        {
            cutpar.SetActive(false);
                cutpars.Add(cutpar);
                Debug.Log(cutpars.Count);
            ui_icon_cut.SetActive(false);
            mainguy.StartCut();
        }
    }

    void SliceObject(GameObject target)
    {
        if (target == null) return;

        // Perform slicing using the plane's position and normal
        SlicedHull slicedHull = target.Slice(transform.position, transform.up, crossSectionMaterial);
        if (slicedHull != null)
        {
            GameObject upperHull = slicedHull.CreateUpperHull(target, crossSectionMaterial);
            GameObject lowerHull = slicedHull.CreateLowerHull(target, crossSectionMaterial);

            SetupSlicedObject(upperHull,true);
            SetupSlicedObject(lowerHull,false);

            Destroy(target); // Remove the original object
        }
    }

    void SetupSlicedObject(GameObject obj,bool upper)
    {
        obj.AddComponent<BoxCollider>();  // Add collider
        var rgd = obj.AddComponent<Rigidbody>();  // Enable physics
        rgd.mass = 100;
        rgd.linearDamping = 10;
        rgd.angularDamping = 10;
        /*rgd.freezeRotation = true;*/
        obj.tag = "Sliceable";  // Allow further slicing if needed
        obj.transform.parent = cutpar.transform;
        obj.transform.localPosition = Vector3.zero;
        

        if (upper)
        {
            obj.layer = LayerMask.NameToLayer("Objects");
        }
        else
        {
            obj.layer = LayerMask.NameToLayer("Default");
        }
    }
}
