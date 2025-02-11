using UnityEngine;
using EzySlice;
using System.Security.Cryptography;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;

public class scr_knife : MonoBehaviour
{
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

    public void Reinit()
    {
        slicedcount = 0;
        this.transform.parent.parent.localPosition = new Vector3(0,0,0);
        cutting = false;
        cutpar = null;
        
    }

    private void Update()
    {
        if (isSalting)
        {
            if(parsalt.GetComponent<scr_salt>().amt > 10)
            {
                FinishSalting();
            }
        }
        else
        {
            return;
        }
    }

    public void StartSalting()
    {
        parsalt.SetActive(true);
        parsalt.GetComponent<scr_salt>().amt = 0;
        isSalting = true;
    }

    public void FinishSalting()
    {
        isSalting = false;
        parsalt.SetActive(false);
        salt = false;
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

    public void Repos()
    {
        if (slicedcount < 5)
        {
            this.transform.parent.parent.gameObject.transform.position = new Vector3(this.gameObject.transform.parent.parent.position.x + slicedist / 5, this.gameObject.transform.parent.parent.position.y, this.transform.parent.parent.gameObject.transform.position.z);
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

            SetupSlicedObject(upperHull);
            SetupSlicedObject(lowerHull);

            Destroy(target); // Remove the original object
        }
    }

    void SetupSlicedObject(GameObject obj)
    {
        obj.AddComponent<MeshCollider>().convex = true;  // Add collider
        obj.AddComponent<Rigidbody>();  // Enable physics
        obj.tag = "Sliceable";  // Allow further slicing if needed
        obj.transform.parent = cutpar.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.layer = LayerMask.NameToLayer("Objects");

    }
}
