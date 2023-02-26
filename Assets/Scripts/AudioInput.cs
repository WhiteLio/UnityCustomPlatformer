using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInput : MonoBehaviour
{
    // input audio is at least this many times louder than bg noise aka threshold
    [Range(1f, 3f)]
    [SerializeField] private float backgroundRatio = 2f;

    // multiplier on output audio
    [SerializeField] private float sensitivity = 20;

    // num of seconds of memory
    [SerializeField] private float window = 10;

    // 
    [SerializeField] private AudioSource audioSource;

    public float loudestRawVolume { get; private set; }

    private string micName;
    private AudioClip micClip;
    private Queue<float> recentVolumes;
    private Queue<float> recentBackgroundVolumes;

    private readonly int SAMPLE_WINDOW = 1024;

    void Awake()
    {
        loudestRawVolume = 0.0001f;
        micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 20, AudioSettings.outputSampleRate);
        print(AudioSettings.outputSampleRate);
        audioSource.clip = micClip;
        audioSource.Play();
        recentVolumes = new Queue<float>();
        recentVolumes.Enqueue(0);
        recentBackgroundVolumes = new Queue<float>();
        recentBackgroundVolumes.Enqueue(1f);
    }

    void Update()
    {
        float currRawVolume = GetRawVolumeOfMic();

        loudestRawVolume = currRawVolume > loudestRawVolume ? currRawVolume : loudestRawVolume;

        AddToMemory(recentVolumes, currRawVolume);

        bool isBackgroundNoise = currRawVolume < GetThreshold();
        if (isBackgroundNoise)
        {
            AddToMemory(recentBackgroundVolumes, currRawVolume);
        }
    }

    private void AddToMemory(Queue<float> memory, float newValue)
    {
        memory.Enqueue(newValue);
        float amountToDequeue = memory.Count() - GetFps() * window;
        for (int i = 0; i < amountToDequeue; i++)
        {
            memory.Dequeue();
        }
    }

    public float GetThreshold()
    {
        return backgroundRatio * GetRecentAverageBackgroundVolume();
    }

    public int GetFps()
    {
        int fps = (int)(1f / Time.unscaledDeltaTime);
        fps = fps <= 0 ? 1 : fps;
        return fps;
    }

    public float GetRecentAverageRawVolume()
    { 
        return recentVolumes.Average();
    }
    
    public float GetRecentAverageBackgroundVolume()
    { 
        return recentBackgroundVolumes.Average();
    }

    public float GetRawVolumeOfMic()
    {
        return GetVolumeOfAudioClip(micClip, Microphone.GetPosition(micName));
    }

    public float GetVolumeOfMic()
    {
        float volume = GetRawVolumeOfMic();
        volume *= sensitivity;
        volume = volume < GetThreshold() ? 0 : volume;
        
        return volume;
    }

    private float GetVolumeOfAudioClip(AudioClip audioClip, int clipPosition)
    {
        int startPosition = clipPosition - SAMPLE_WINDOW;
        startPosition = startPosition < 0 ? 0 : startPosition;

        float[] data = new float[SAMPLE_WINDOW];
        audioClip.GetData(data, startPosition);

        print(PitchDetector.EstimateFundamentalFrequency(data));

        float volumeSum = 0;
        foreach (float s in data)
        {
            volumeSum += Mathf.Abs(s);
        }

        return volumeSum / SAMPLE_WINDOW;
    }
}