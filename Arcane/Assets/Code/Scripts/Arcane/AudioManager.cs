using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public int audioPoolSize = 4;
    public AudioSource[] audioPool;

    public GameEvent winEvent;

    private int audioIndex = 0;

    private static AudioManager self;

    private void OnEnable()
    {
        winEvent.AddListener(OnPlayerWin);
    }


    private void OnDisable()
    {
        winEvent.RemoveListener(OnPlayerWin);
    }

    private void OnPlayerWin(object data)
    {
        StopAll();
    }

    private void StopAll()
    {
        for (int i = 0; i < audioPoolSize; i++)
        {
            audioPool[i].Stop();
        }
    }

    private void Awake()
    {
        audioPool = new AudioSource[audioPoolSize];
        for (int i = 0; i < audioPoolSize; i++)
        {
            var audio = new GameObject(string.Format("AudioSource[{0}]", i));
            audioPool[i] = audio.AddComponent<AudioSource>();
        }
        self = this;
    }

    public static void PlayFromSourceInLocation(AudioClip clip,AudioSource source,Transform location)
    {
        var pool = self.audioPool[self.audioIndex++];
        pool.clip = clip;
        pool.volume = source.volume;
        pool.pitch = source.pitch;
        pool.spatialBlend = source.spatialBlend;
        pool.transform.position = location.position;
        pool.Play();
        self.audioIndex = self.audioIndex % self.audioPool.Length;
    }



}
