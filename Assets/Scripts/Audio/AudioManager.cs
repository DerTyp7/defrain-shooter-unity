using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // This list contains all sounds which should be accessable by the audiomanager.
    // If you want to add a sound -> add a new item in the list VIA the inspector!
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);


        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>(); // The Audio source where the "Player can hear through"

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        // Enter Music here
    }


    // USE this to play a sound in other scripts: FindObjectOfType<AudioManager>().Play(name);
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // searching for sound in Array where sound.name == name
        if(s == null)
        {
            Debug.LogWarning("Sound: " + s.name + " not found!");
            return;
        }
            
        s.source.Play();
    }
}
