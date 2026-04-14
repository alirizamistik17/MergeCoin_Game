using UnityEngine;
using System.Collections;
using TMPro;

public class DragAndDrop : MonoBehaviour
{
    // Global değişken: SwipeController'ın parayı tutarken ekranı kaydırmasını engeller
    public static bool IsAnyItemDragging = false; 

    private bool isDragging;
    private Vector3 offset;
    private Camera mainCamera;
    private Vector3 originalScale;

    [Header("Efekt Prefabları")]
    public GameObject floatingTextPrefab; 
    public GameObject mergeVFXPrefab;    

    void Awake() => mainCamera = Camera.main;
    void Start() => originalScale = transform.localScale;

    void OnMouseDown()
    {
        IsAnyItemDragging = true; // Sürükleme başladı, ekran kayması kilitlendi
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
        PerformClick();
    }

    public void PerformClick()
    {
        StopCoroutine("ClickSquashEffect"); 
        StartCoroutine(ClickSquashEffect());

        Item myItem = GetComponent<Item>();
        if (myItem != null && myItem.itemData != null)
        {
            double amount = myItem.itemData.goldPerSecond;
            CurrencyManager.Instance.totalMoney += amount;
            CurrencyManager.Instance.UpdateUI();

            SpawnFloatingText(amount);
        }
    }

    IEnumerator ClickSquashEffect()
    {
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.8f, originalScale.z);
        yield return new WaitForSeconds(0.1f);
        transform.localScale = originalScale;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 newPos = GetMouseWorldPosition() + offset;

            // HAPİS SİSTEMİ: Item scriptindeki akıllı duvar sınırlarını kullanıyoruz
            Item myItem = GetComponent<Item>();
            if (myItem != null)
            {
                // Paranın sadece kendi odası içinde kalmasını zorunlu kılıyoruz
                transform.position = myItem.GetClampedPosition(newPos);
            }
            else
            {
                transform.position = newPos;
            }
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        IsAnyItemDragging = false; // Sürükleme bitti, ekran kayması açıldı
        CheckForMerge();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    private void CheckForMerge()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hit in hitColliders)
        {
            if (hit.gameObject == gameObject) continue;
            Item otherItem = hit.GetComponent<Item>();
            Item myItem = GetComponent<Item>();

            if (otherItem != null && myItem != null)
            {
                if (otherItem.itemData == myItem.itemData && myItem.itemData.nextLevelItem != null)
                {
                    Merge(otherItem);
                    return; 
                }
            }
        }
    }

    void Merge(Item other)
    {
        if (mergeVFXPrefab != null)
        {
            GameObject vfx = Instantiate(mergeVFXPrefab, other.transform.position, Quaternion.identity);
            Destroy(vfx, 0.5f);
        }

        ItemData nextLevel = other.itemData.nextLevelItem;
        int targetStageIndex = StageManager.Instance.GetStageIndexForItem(nextLevel.itemLevel);
        
        Vector3 spawnPos = other.transform.position;
        
        if (targetStageIndex > StageManager.Instance.GetStageIndexForItem(other.itemData.itemLevel))
        {
            float targetX = targetStageIndex * StageManager.Instance.stageOffset;
            spawnPos = new Vector3(targetX + Random.Range(-1f, 1f), Random.Range(-2f, 2f), 0);
        }

        // --- KRİTİK DÜZELTME: SCALE HATASI ÇÖZÜLDÜ ---
        GameObject newItem = Instantiate(gameObject, spawnPos, Quaternion.identity);
        // Yeni oluşan paranın ölçeğini zorla orijinal haline getiriyoruz
        newItem.transform.localScale = originalScale; 
        
        newItem.GetComponent<Item>().SetupItem(nextLevel);
        
        StageManager.Instance.CheckStageTransition(nextLevel.itemLevel);

        Destroy(other.gameObject);
        Destroy(gameObject);
    }

    void SpawnFloatingText(double amount)
    {
        if (floatingTextPrefab == null) return;
        Canvas canvas = Object.FindAnyObjectByType<Canvas>();
        if (canvas == null) return;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        GameObject ftObj = Instantiate(floatingTextPrefab, screenPos, Quaternion.identity, canvas.transform);
        
        TextMeshProUGUI txt = ftObj.GetComponent<TextMeshProUGUI>();
        if (txt != null)
        {
            txt.text = CurrencyFormatter.Format(amount) + " <sprite=39>";
        }
    }
}