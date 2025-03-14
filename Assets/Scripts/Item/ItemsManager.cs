using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using NUnit.Framework;

[System.Serializable]
public class ItemsManager: IItemsManagerView
{
    public static IItemsManagerView ItemsManagerView {get; private set;}
    private List<Item> Items;

    public ItemsManager(List<ItemSO> ItemSO)
    {
        ItemsManagerView = this;
        Items = new List<Item>();
        for (int i = 0; i < ItemSO.Count; i++)
        {

            Item item = new Item(ItemSO[i]);

            if (ItemSO[i].Chance == 1)
                item.CurrentAmount = UnityEngine.Random.Range(ItemSO[i].QuantityRange.x, ItemSO[i].QuantityRange.y);
            else if (ItemSO[i].Chance > 0 && new System.Random().NextDouble() <= ItemSO[i].Chance)
                item.CurrentAmount = UnityEngine.Random.Range(ItemSO[i].QuantityRange.x, ItemSO[i].QuantityRange.y);
            
            

            Items.Add(item);
        }
    }

    public IItem FindItem(int ID)
    {
        return Items.Find(x => x.ID == ID);
    }


    public List<IItem> GetItems()
    {
        return Items.Select( x => (IItem) x).ToList();
    }

    public BonusItem GetBonusItem()
    {
        BonusItem b = new BonusItem();

        foreach(Item item in Items)
        {
            if(item.CurrentAmount > 0)
            {
                b.IncreasesSuccessRate = Mathf.Max(b.IncreasesSuccessRate, item.BonusItem.IncreasesSuccessRate);
                b.ReducesCraftingTime = Mathf.Max(b.ReducesCraftingTime, item.BonusItem.ReducesCraftingTime);
            }
        }

        return b;
    }

}

public interface IItemsManagerView
{
    BonusItem GetBonusItem();
}



[System.Serializable]
public class Item : IItem
{
    private ItemSO ItemSO;
    public int CurrentAmount { get; set; }
    public int CraftingAmount { get; set; }
    public int ID => ItemSO.ID;
    public BonusItem BonusItem => ItemSO.BonusItem;

    public Item(ItemSO itemSO)
    {
        ItemSO = itemSO;

        CraftingAmount = 0;
    }
}


public interface IItem
{
    int CurrentAmount { get; }
    int CraftingAmount { get; }
    int ID { get; }
}