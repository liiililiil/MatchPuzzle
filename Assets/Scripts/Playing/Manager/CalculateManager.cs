using System.Collections;
using UnityEngine;

public class CalculateManager : MonoBehaviour
{

    public static CalculateManager Instance { get; private set; }
    public int Calculating = 0;

    private void Awake()
    {
        //싱글톤
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Calculate()
    {
        EventManager.Instance.OnCalReset.Invoke();
        EventManager.Instance.OnCalculate.Invoke();

        if (Calculating != 0) StartCoroutine(CalculateWait());
    }

    IEnumerator CalculateWait()
    {
        while (true)
        {
            if (Calculating == 0)
            {
                Calculate();
                break;
            }

            yield return null;
        }
    }
}
