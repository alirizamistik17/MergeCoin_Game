using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public ItemData firstLevelItem; 
    public Vector2 spawnRangeX = new Vector2(-2.5f, 2.5f);
    public Vector2 spawnRangeY = new Vector2(-4f, 4f);

    [Header("Otomatik Spawn Ayarları")]
    public float spawnInterval = 5f; 
    public int maxItemCount = 20; // En fazla kaç para olabilir?
    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Kontrol: Eğer 20'den az para varsa doğur
            if (GetCurrentItemCount() < maxItemCount)
            {
                SpawnRandom();
            }
            timer = spawnInterval; 
        }

        // Manuel spawn testi (Space) - Burada da limiti kontrol ediyoruz
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GetCurrentItemCount() < maxItemCount)
            {
                SpawnRandom();
            }
            else
            {
                Debug.Log("Limit doldu! Daha fazla para doğurulamaz.");
            }
        }
    }

    // Sahnede aktif olan tüm 'Item' scriptine sahip objeleri sayar
    int GetCurrentItemCount()
    {
        // Unity 6'da en hızlı sayma yöntemi budur
        return Object.FindObjectsByType<Item>(FindObjectsSortMode.None).Length;
    }

    public void SpawnRandom()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(spawnRangeX.x, spawnRangeX.y),
            Random.Range(spawnRangeY.x, spawnRangeY.y),
            0
        );

        GameObject newItem = Instantiate(itemPrefab, randomPos, Quaternion.identity);
        newItem.GetComponent<Item>().SetupItem(firstLevelItem);
    }
}