using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeVisualiser : MonoBehaviour
{
    [SerializeField] private AudioInput audioInput;
    [SerializeField] private Image rawVolumeBar;
    [SerializeField] private Transform thresholdMark;
    [SerializeField] private Transform averageMark;

    private RectTransform barRectTransform;

    void Start()
    {
        barRectTransform = rawVolumeBar.GetComponent<RectTransform>();
        rawVolumeBar.fillAmount = 0;
        UpdateMarker(averageMark, 0);
        UpdateMarker(thresholdMark, 0);
    }

    void Update()
    {
        rawVolumeBar.fillAmount = Mathf.Lerp(0f, 1f,
                audioInput.GetRawVolumeOfMic() / audioInput.loudestRawVolume);
        
        UpdateMarker(averageMark, audioInput.GetRecentAverageRawVolume());
        UpdateMarker(thresholdMark, audioInput.GetThreshold());
    }

    private void UpdateMarker(Transform markerTransform, float value)
    {
        Vector3 newPos = markerTransform.position;
        newPos.x = Mathf.Lerp(0, barRectTransform.rect.width,
                value / audioInput.loudestRawVolume);
        markerTransform.position = newPos;
    }
}
