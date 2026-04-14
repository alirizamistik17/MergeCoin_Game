using UnityEngine;
using System.Collections;

public class BoxSpawner : MonoBehaviour
{
    public static BoxSpawner Instance;

    [Header("Ayarlar")]
    public GameObject itemPrefab;      // Doğacak olan altın objesi
    public ItemData level1ItemData;    // Başlangıçta çıkacak olan Level 1 verisi
    
    [Header("Geliştirme Verileri")]
    public float autoSpawnDelay = 10f; // Marketten düşürülecek olan süre
    public int spawnLevel = 1;         // Marketten artırılacak olan çıkış seviyesi

    private Vector3 originalScale;

    void Awake() => Instance = this;

    void Start()
    {
        originalScale = transform.localScale;
        // Otomatik doğurma döngüsünü başlat
        StartCoroutine(AutoSpawnRoutine());
    }

    void OnMouseDown()
    {
        SpawnItem();
        // Kutuyu videodaki gibi zıplat/sallat
        StopCoroutine("ClickEffect");
        StartCoroutine(ClickEffect());
    }

    public void SpawnItem()
{
    // GÜVENLİK KONTROLLERİ
    if (itemPrefab == null) { Debug.LogError("HATA: itemPrefab atanmamış!"); return; }
    if (level1ItemData == null) { Debug.LogError("HATA: level1ItemData atanmamış!"); return; }
    if (StageManager.Instance == null) { Debug.LogError("HATA: StageManager bulunamadı!"); return; }

    float targetX = StageManager.Instance.currentStageIndex * StageManager.Instance.stageOffset;
    Vector3 spawnPos = new Vector3(targetX + Random.Range(-1.5f, 1.5f), Random.Range(-2f, 2f), 0);

    GameObject newItem = Instantiate(itemPrefab, spawnPos, Quaternion.identity);
    
    Item itemScript = newItem.GetComponent<Item>();
    if (itemScript != null)
    {
        itemScript.SetupItem(level1ItemData);
    }
    else
    {
        Debug.LogError("HATA: Prefab üzerinde 'Item' scripti bulunamadı!");
    }
}

    IEnumerator AutoSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSpawnDelay);
            SpawnItem();
        }
    }

    IEnumerator ClickEffect()
    {
        // Kutuyu anlık büyütüp küçültme efekti (Juicy feel)
        transform.localScale = originalScale * 1.2f;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = originalScale;
    }
}