using UnityEngine;

public class Spin : MonoBehaviour
{
    float time;
    int frame;
    RectTransform rectTransform;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        frame += 4;

        if(time >= 1) time -=1;
        if(frame >= 360) frame -= 360;

        float rotateZ = Mathf.Lerp(0, 360, EaseMoveMent.Ease(EaseType.InOutSine, time));

        rectTransform.rotation = Quaternion.Euler(0, 0, rotateZ + frame);
    }
}
