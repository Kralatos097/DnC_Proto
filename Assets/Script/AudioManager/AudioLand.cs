using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLand : MonoBehaviour
{
    public void PlaySound()
    {
        FindObjectOfType<AudioManager>().Play("Test");
    }
    
    public void StopSound()
    {
        FindObjectOfType<AudioManager>().Stop("Test");
    }
    
    public void PauseSound()
    {
        FindObjectOfType<AudioManager>().Pause("Test");
    }
    
    public void UnPauseSound()
    {
        FindObjectOfType<AudioManager>().UnPause("Test");
    }
}
