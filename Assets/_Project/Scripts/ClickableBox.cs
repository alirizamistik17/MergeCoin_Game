using UnityEngine;

public class ClickableBox : MonoBehaviour
{
    public GameObject itemPrefab;    // Buraya GERÇEK ALTIN prefabını koy
    public ItemData level1ItemData;  // Buraya Coin1 Data dosyasını koy

    private void OnMouseDown()
    {
        // Kutunun olduğu yerde altını oluştur
        GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        
        // Altının seviyesini ayarla
        Item itemScript = newItem.GetComponent<Item>();
        if (itemScript != null)
        {
            itemScript.SetupItem(level1ItemData);
        }
        
        Destroy(gameObject); // Kutuyu yok et
    }
}