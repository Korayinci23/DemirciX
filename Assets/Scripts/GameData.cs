using UnityEngine;

public enum ItemType { Sword, Dagger, Axe, Shield }

public enum ItemQuality { Poor, Normal, Good, Excellent }

public struct CustomerOrder
{
    public ItemType requestedItem;
    public ItemQuality minQuality; // Müþterinin istediði minimum kalite
    public string customerName;
}