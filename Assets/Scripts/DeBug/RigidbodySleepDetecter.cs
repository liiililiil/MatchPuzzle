using UnityEngine;

public class RigidbodySleepDetecter : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody2D.IsAwake())
        {
            // Debug.Log($"{gameObject.name}가 깨어났습니다!", this);
        }
    }
}
