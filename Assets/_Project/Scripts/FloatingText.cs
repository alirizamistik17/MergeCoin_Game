using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 200f; // UI koordinatları için hız (100-300 arası dene)
    public float duration = 1f;    // 1 saniye sonra yok ol

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void Update()
    {
        // Yazıyı her karede yukarı doğru kaydır
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }
}