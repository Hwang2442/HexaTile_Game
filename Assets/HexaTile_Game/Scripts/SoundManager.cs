using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Instance

    private static SoundManager instance;

    public static SoundManager Instance
    {
        get 
        {
            if (instance == null)
                instance = FindObjectOfType<SoundManager>();

            return instance; 
        }
    }

    #endregion

    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] private AudioSource source;

    public void Play(string name, float volume = 1, bool loop = true, AudioSource source = null)
    {
        var clip = sounds.FirstOrDefault(s => s.name == name);
        if (clip != null)
        {
            AudioSource target = source == null ? this.source : source;
            target.clip = clip;
            target.loop = loop;
            target.volume = volume;
            target.Play();
        }
    }

    public void PlayOneShot(string name, AudioSource source = null)
    {
        var clip = sounds.FirstOrDefault(s => s.name == name);
        if (clip != null)
        {
            AudioSource target = source == null ? this.source : source;
            target.PlayOneShot(clip);
        }
    }
}
