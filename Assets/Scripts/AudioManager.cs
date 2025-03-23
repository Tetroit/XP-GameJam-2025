using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    int currentlyPlaying = -1;
    public List<AudioClip> music;
    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.loop = true;
        PickTrack();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void PickTrack()
    {
        int newTrack = Random.Range(0, music.Count);
        if (currentlyPlaying != newTrack)
        {
            currentlyPlaying = newTrack;
            source.clip = music[newTrack];
            source.volume = 3f;
            source.Play();
        }
    }
}
