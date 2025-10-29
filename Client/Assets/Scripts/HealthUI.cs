using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private RectTransform _filledImage;
    [SerializeField] private float _defaultWidht;

    void OnValidate()
    {
        _defaultWidht = _filledImage.sizeDelta.x;
    }
    public void UpdateHealth(float max, int current)
    {
        float percent = current / max;

        _filledImage.sizeDelta = new Vector2(_defaultWidht * percent, _filledImage.sizeDelta.y);
    }
}
