using UnityEngine;

public class DamageText : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1);
        transform.LookAt(Camera.main.transform);
        transform.Rotate(Vector3.up, 180);
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime);
            transform.Translate(Vector3.up * 0.1f);
        }
    }
}
