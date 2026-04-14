using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    private SpriteRenderer spriteRenderer;

    [Header("Rastgele Hareket Ayarları")]
    public float minWaitTime = 10f;    
    public float maxWaitTime = 20f;    
    public float moveDistance = 0.2f;  
    public float moveDuration = 0.5f;

    // --- MÜHENDİSLİK EKLEMESİ: AKILLI DUVAR SINIRLARI ---
    private float minX, maxX;
    private float minY = -4.5f, maxY = 4.5f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (itemData != null)
        {
            SetupItem(itemData);
        }
        StartCoroutine(RandomIdleMovement());
    }

    public void SetupItem(ItemData newData)
    {
        itemData = newData;
        spriteRenderer.sprite = itemData.itemIcon;
        gameObject.name = itemData.itemName;

        // Her setup yapıldığında duvar sınırlarını yeniden hesapla
        CalculateBoundaries();
    }

    // Yeni oda sistemine göre duvarları hesaplayan fonksiyon
    void CalculateBoundaries()
    {
        if (StageManager.Instance == null) return;

        // Bu item'ın hangi odaya (0, 1, 2...) ait olduğunu öğreniyoruz
        int myStageIndex = StageManager.Instance.GetStageIndexForItem(itemData.itemLevel);
        float offset = StageManager.Instance.stageOffset; // Senin 6.15 değerin
        float centerXPoz = myStageIndex * offset;

        // Duvarları odanın genişliğine göre belirliyoruz (0.5f pay bırakarak)
        minX = centerXPoz - (offset / 2f) + 0.5f;
        maxX = centerXPoz + (offset / 2f) - 0.5f;
    }

    // DragAndDrop scriptinin duvarlara çarpmayı kontrol etmesi için lazım:
    public Vector3 GetClampedPosition(Vector3 targetPos)
    {
        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);
        return new Vector3(clampedX, clampedY, targetPos.z);
    }

    public void TriggerFakeClick()
    {
        if (itemData == null) return;
        double amount = itemData.goldPerSecond;
        CurrencyManager.Instance.totalMoney += amount;
        CurrencyManager.Instance.UpdateUI();

        DragAndDrop dnd = GetComponent<DragAndDrop>();
        if (dnd != null) dnd.PerformClick(); 
    }

    IEnumerator RandomIdleMovement()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            Vector3 startPos = transform.position;
            
            // HEDEF BELİRLEME: Artık sabit 2.5 yerine minX ve maxX kullanıyor!
            Vector3 potentialTarget = startPos + new Vector3(
                Random.Range(-moveDistance, moveDistance), 
                Random.Range(-moveDistance, moveDistance), 
                0);

            potentialTarget.x = Mathf.Clamp(potentialTarget.x, minX, maxX);
            potentialTarget.y = Mathf.Clamp(potentialTarget.y, minY, maxY);

            float elapsed = 0;
            while (elapsed < moveDuration)
            {
                transform.position = Vector3.Lerp(startPos, potentialTarget, elapsed / moveDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}