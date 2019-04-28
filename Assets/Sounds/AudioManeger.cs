using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManeger : MonoBehaviour
{
    public static AudioManeger main;
    public List<AudioClip> ShootSounds = new List<AudioClip>();
    public List<AudioClip> StepSound = new List<AudioClip>();
    public List<AudioClip> ReloadSound = new List<AudioClip>();
    public List<AudioClip> WallHitSound = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    public void Play(AudioClip a, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(a, pos);
    }

    public void Play(List<AudioClip> a, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(a[UnityEngine.Random.Range(0, a.Count)] , pos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
