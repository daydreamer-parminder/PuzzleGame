using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [HideInInspector]
    public AudioSource source;

    public void Awake() 
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(string name)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
            return;
        }
        source.Stop();
        source.clip = sound.clip;
        source.Play();
    }

    public void Stop(string name)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
            return;
        }
        source.Stop();
    }
}
