using UnityEngine;
using System.Collections.Generic;

public class CustomerOrderManager : MonoBehaviour
{
    // Singleton (Tekil Örnek): Diðer script'lerin kolayca eriþimi için
    public static CustomerOrderManager Instance;

    [Header("Müþteri ve Sipariþ Durumu")]
    public List<string> customerNames = new List<string> { "Ahmet", "Ayþe", "Mehmet", "Fatma", "Ali" };

    // Müþterinin aktif sipariþi
    public CustomerOrder currentOrder { get; private set; }
    public bool isOrderActive { get; private set; } = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Diðer script'ler tarafýndan çaðrýlarak yeni bir sipariþ baþlatýlýr
    public void GenerateNewOrder()
    {
        if (isOrderActive)
        {
            Debug.LogWarning("Mevcut bir sipariþ zaten aktif. Lütfen önce onu teslim edin.");
            return;
        }

        // Rastgele bir Ürün Türü ve minimum Kalite seçimi
        ItemType randomItem = (ItemType)Random.Range(0, System.Enum.GetNames(typeof(ItemType)).Length);

        // Poor (0) kalitesinin üstünde bir kalite isteme ihtimali (Normal, Good, Excellent)
        ItemQuality minDesiredQuality = (ItemQuality)Random.Range(1, System.Enum.GetNames(typeof(ItemQuality)).Length);
        string randomCustomer = customerNames[Random.Range(0, customerNames.Count)];

        currentOrder = new CustomerOrder
        {
            requestedItem = randomItem,
            minQuality = minDesiredQuality,
            customerName = randomCustomer
        };

        isOrderActive = true;
        Debug.Log($"*** YENÝ SÝPARÝÞ BAÞLADI ***\n" +
                  $"{currentOrder.customerName}: Bana **{currentOrder.requestedItem}** gerekiyor. " +
                  $"Kalitesi en az **{currentOrder.minQuality}** olmalý.");
    }

    // Teslimat sistemi tarafýndan çaðrýlýr, sipariþi kapatýr
    public void CompleteOrder()
    {
        isOrderActive = false;
        Debug.Log("Sipariþ tamamlandý. Yeni müþteri bekleniyor...");
    }
}