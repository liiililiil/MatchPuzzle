using UnityEngine;

public class SceneLoadEnable : MonoBehaviour
{
    private void Start() {
        SceneLoadManager.Instance.OnStartLoading.AddListener(Enable);
        SceneLoadManager.Instance.OnCompleteLoading.AddListener(Disable);
    }

    private void Enable()
    {
        gameObject.SetActive(true);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
