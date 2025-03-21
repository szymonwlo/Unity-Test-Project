using UnityEngine;
using Unity.Mathematics;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObjects/ItemSO", order = 1)]
public class ItemSO : ScriptableObject
{
    public string Name;
    public string Description;
    public int ID;
    public ItemType Type;
    public Sprite Icon;
    public BonusItem BonusItem;

    [Header("Starting Inventory")]
    public int2 QuantityRange;
    public float Chance;
}


public enum ItemType
{
    Resource,
    Crafted,
    Bonus
}

[System.Serializable]
public struct BonusItem
{
    public float IncreasesSuccessRate;
    public float ReducesCraftingTime;
}