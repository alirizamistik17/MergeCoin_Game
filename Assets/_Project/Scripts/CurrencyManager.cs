using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    
    [Header("Ekonomi Verisi")]
    public double totalMoney = 0; // 'double' büyük sayılar için şart.
    public TextMeshProUGUI moneyText;

    void Awake()
    {
        // Singleton Deseni
        if (Instance == null) 
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Oyun açıldığında UI'ı hemen güncelle
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (moneyText != null)
        {
            // CurrencyFormatter içindeki AA, BB sistemini kullanır
            moneyText.text = CurrencyFormatter.Format(totalMoney);
        }
    }
}