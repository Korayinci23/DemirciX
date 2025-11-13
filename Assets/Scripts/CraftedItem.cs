using UnityEngine;

public class CraftedItem : MonoBehaviour
{
    [Header("Ürün Verileri")]
    // Bu deðerler demircilik minigame'i sonunda belirlenir ve atanýr.
    [Tooltip("Ürünün türü (Kýlýç, Balta vb.)")]
    public ItemType itemType;

    [Tooltip("Ürünün kalitesi (Poor, Normal, Good, Excellent)")]
    public ItemQuality itemQuality;

    // Örneðin, bu metot bir minigame sonunda çaðrýlýp kaliteyi belirleyebilir
    public void SetQualityAndType(ItemType type, ItemQuality quality)
    {
        itemType = type;
        itemQuality = quality;
        Debug.Log($"Kýlýç hazýr! Tür: {itemType}, Kalite: {itemQuality}");
    }
}