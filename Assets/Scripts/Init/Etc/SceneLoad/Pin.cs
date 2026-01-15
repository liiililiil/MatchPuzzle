using UnityEngine;

public class Pin : MonoBehaviour
{
    RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,SceneLoadManager.Instance.progress * 800);
    }
}
