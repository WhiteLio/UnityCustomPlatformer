using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactToVolume : MonoBehaviour
{
    [SerializeField] private AudioInput audioInput;
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;

    void Update()
    {
        transform.localScale = Vector3.Lerp(minScale, maxScale, audioInput.GetVolumeOfMic());
    }
}
