using UnityEngine;

public class scr_dishlist : MonoBehaviour
{
    public void PlayPaperIn()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(8);
    }

    public void PlayPaperOut()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(9);
    }
}
