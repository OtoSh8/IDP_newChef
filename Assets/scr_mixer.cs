using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_mixer : MonoBehaviour
{
    [SerializeField] GameObject objmixer;
    [SerializeField] Transform spawn;

    private bool isMoving = false;
    private float timecounter;

    public float Width;
    public float Length;
    public float Speed;

    private Vector3 startingpos;

    public int amt = 0;
    public int goal = 0;

    [SerializeField] scr_instructor instr;

    List<GameObject> ingr = new List<GameObject> ();
    public void Reinit(int x, List<GameObject> cutted)
    {
        amt = 0;
        goal = x;
        if(cutted != null)
        {
            foreach(GameObject obj in cutted)
            {
                obj.transform.parent = this.transform.parent;
                obj.transform.localPosition = spawn.localPosition;
                obj.SetActive(true);
                ingr.Add(obj);
            }
        }
    }

    private void Start()
    {
        startingpos = objmixer.transform.position;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isMoving == false)
        {
            isMoving = true;
            amt++;
            StartCoroutine(GetSignal());
        }

        if (isMoving)
        {
            timecounter += Time.deltaTime * Speed;
        }
         

        objmixer.transform.position = new Vector3(Mathf.Sin(timecounter)* Width, 0f , Mathf.Cos(timecounter)*Length) + startingpos;
    }

    IEnumerator GetSignal()
    {

        yield return new WaitForSeconds(0.5f);
        isMoving = false;
        if(amt >= goal)
        {
            Finished();
        }
        yield return null;
    }

    public void Finished()
    {
        instr.FinishStep(ingr);
        this.GetComponent<scr_mixer>().enabled = false;
    }
}
