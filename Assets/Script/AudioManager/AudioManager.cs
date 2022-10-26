using System;
using UnityEngine.Audio;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [Header("Liste de Son")]
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //Joue un son au démarage 
    private void Start()
    {
        //Play("Theme");
    }
    
    //Joue un son depuis le début : FindObjectOfType<AudioManager>().Play("NomDuSon");
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Le Son : " + name + " n'existe pas... Oublier de le mettre ou mal écrit");
            return;
        }
            
        s.source.Play();
    }
    
    //Arrête un son : FindObjectOfType<AudioManager>().Stop("NomDuSon");
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Le Son : " + name + " n'existe pas... Oublier de le mettre ou mal écrit");
            return;
        }
            
        s.source.Stop();
    }
    
    //Arrête un son : FindObjectOfType<AudioManager>().RandomPitch("NomDuSon");
    public void RandomPitch(string name)
    {
        float alea = Random.Range(0.6f, 1.5f);
        
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Le Son : " + name + " n'existe pas... Oublier de le mettre ou mal écrit");
            return;
        }

        s.source.pitch = alea;
        s.source.Play();
    }
    
    //Met en pause un son : FindObjectOfType<AudioManager>().Pause("NomDuSon");
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Le Son : " + name + " n'existe pas... Oublier de le mettre ou mal écrit");
            return;
        }
            
        s.source.Pause();
    }
    
    //Reprend un son en pause : FindObjectOfType<AudioManager>().UnPause("NomDuSon");
    public void UnPause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Le Son : " + name + " n'existe pas... Oublier de le mettre ou mal écrit");
            return;
        }
            
        s.source.UnPause();
    }
}
