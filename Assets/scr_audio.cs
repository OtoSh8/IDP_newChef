using System.Collections.Generic;
using UnityEngine;

public class scr_audio : MonoBehaviour
{
    [Header("Audio References")]
    [Tooltip("heres a tooltip lmao")]
    public List<AudioClip> audiolist = new List<AudioClip>();
    // Audio List ->
    // 0 = CUT
    // 1 = MIX
    // 2 = SALT
    // 3 = PLATE

    public void PlaySoundID(int id)
    {
        AudioSource newcomp = this.gameObject.AddComponent<AudioSource>();
        newcomp.loop = false;
        newcomp.clip = audiolist[id];
        newcomp.Play();
    }

    public void PlaySoundID(int id, float vol)
    {
        AudioSource newcomp = this.gameObject.AddComponent<AudioSource>();
        newcomp.volume = vol;
        newcomp.loop = false;
        newcomp.clip = audiolist[id];
        newcomp.Play();
    }

    public void PlaySoundIDNonForce(int id)
    {
        if (!this.GetComponents<AudioSource>()[0].isPlaying)
        {
            this.GetComponents<AudioSource>()[0].loop = false;
            this.GetComponents<AudioSource>()[0].clip = audiolist[id];
            this.GetComponents<AudioSource>()[0].Play();
        }
        
    }

    public void PlaySoundIDLoop(int id)
    {
        AudioSource newcomp = this.gameObject.AddComponent<AudioSource>();
        newcomp.loop=true;
        newcomp.clip = audiolist[id];
        newcomp.Play();
    }


    public void StopAllLoop()
    {
        for (int i = 1; i < this.GetComponents<AudioSource>().Length; i++)
        {
            if (this.GetComponents<AudioSource>()[i].loop == true)
            {
                Destroy(this.GetComponents<AudioSource>()[i]);
            }
        }
    }

    private void Update()
    {
        for(int i = 1; i < this.GetComponents<AudioSource>().Length; i++)
        {
            if (this.GetComponents<AudioSource>()[i].isPlaying == false)
            {
                Destroy(this.GetComponents<AudioSource>()[i]);
            }
        }

    }


}
