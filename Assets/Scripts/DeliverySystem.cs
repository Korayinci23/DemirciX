using UnityEngine;

// Bu script'in bir Collider'a (tetikleyici olarak ayarlanmýþ) ve Rigidbody'ye ihtiyacý olacaktýr.
public class DeliverySystem : MonoBehaviour
{
    [Header("Skor ve Ýtibar Ayarlarý")]
    public float baseScorePerDelivery = 100f;
    public float excellentBonus = 50f;
    public float reputationScore { get; private set; } = 0f; // Toplam Ýtibar Puaný

    // Baðlantý: CustomerOrderManager'a eriþim
    private CustomerOrderManager orderManager;

    void Start()
    {
        // Singleton referansýný Start'ta alýr.
        orderManager = CustomerOrderManager.Instance;
        if (orderManager == null)
        {
            Debug.LogError("CustomerOrderManager sahne üzerinde bulunamadý! Ýletiþim kurulamýyor.");
        }
    }

    // Bir nesne teslimat bölgesine girdiðinde tetiklenir
    private void OnTriggerEnter(Collider other)
    {
        // 1. Sipariþ Aktif mi?
        if (!orderManager.isOrderActive) return;

        // 2. Gelen nesne bir Üretilmiþ Ürün mü?
        CraftedItem deliveredItem = other.GetComponent<CraftedItem>();

        if (deliveredItem != null)
        {
            // 3. Teslimatý Ýþle ve Puanla
            ProcessDelivery(deliveredItem);

            // Teslim edilen fiziksel nesneyi sahneden kaldýr
            Destroy(other.gameObject);
        }
    }

    // Ana Puanlama ve Memnuniyet Mekaniði
    private void ProcessDelivery(CraftedItem deliveredItem)
    {
        CustomerOrder currentOrder = orderManager.currentOrder;
        float earnedScore = 0f;
        string satisfactionMessage;

        // --- 1. Ürün Türü Kontrolü ---
        if (deliveredItem.itemType != currentOrder.requestedItem)
        {
            satisfactionMessage = $"[HATA] Yanlýþ ürün! {currentOrder.requestedItem} istemiþtim, bu bir {deliveredItem.itemType}.";
            earnedScore = -50f; // Hatalý teslimattan ceza
        }
        else
        {
            // --- 2. Kalite Kontrolü ---
            int requiredQuality = (int)currentOrder.minQuality;
            int deliveredQuality = (int)deliveredItem.itemQuality;

            if (deliveredQuality >= requiredQuality)
            {
                // BAÞARILI: Ýstenen Kaliteye Ulaþýldý
                earnedScore = baseScorePerDelivery;
                satisfactionMessage = "Teþekkürler, tam istediðim gibi!";

                // Ýstenen kalitenin üzerine çýkýldýysa bonus puan
                if (deliveredQuality > requiredQuality)
                {
                    earnedScore += (deliveredQuality - requiredQuality) * 20f; // Fazla kalite baþýna bonus
                    satisfactionMessage += " Beklediðimden bile iyi bir iþçilik!";
                }
            }
            else
            {
                // BAÞARISIZ: Kalite Gereksinimi Karþýlanmadý
                satisfactionMessage = $"[DÜÞÜK KALÝTE] Ürün türü doðru ama kalitesi düþük. En az {currentOrder.minQuality} istemiþtim, seninki {deliveredItem.itemQuality}.";
                earnedScore = baseScorePerDelivery * 0.3f; // Düþük puan ama sýfýr deðil
            }
        }

        // --- 3. Skoru Güncelle ve Sipariþi Kapat ---
        reputationScore += earnedScore;
        orderManager.CompleteOrder();

        Debug.Log($"*** TESLÝMAT SONUCU ({currentOrder.customerName}) ***\n" +
                  $"Müþteri Geri Bildirimi: {satisfactionMessage}\n" +
                  $"Kazanýlan Puan: **{earnedScore:F0}**\n" +
                  $"Yeni Toplam Ýtibar Puanýnýz: **{reputationScore:F0}**");
    }
}