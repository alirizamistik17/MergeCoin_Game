using UnityEngine;
using System.Collections;

public class AutoClickManager : MonoBehaviour
{
    public float autoClickInterval = 15f;

    void Start()
    {
        StartCoroutine(AutoClickRoutine());
    }

    IEnumerator AutoClickRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoClickInterval);

            // Sahnede o an var olan tüm Item'ları bul
            Item[] allItems = Object.FindObjectsByType<Item>(FindObjectsSortMode.None);

            foreach (Item item in allItems)
            {
                // Her birine "sahte tıklama" yaptır
                item.TriggerFakeClick();
            }
            
            Debug.Log("Otomatik hasat tamamlandı!");
        }
    }
}