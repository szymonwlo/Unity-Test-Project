using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using NUnit.Framework;

[System.Serializable]
public class ItemsManager : IItemsManagerView
{
    public static IItemsManagerView ItemsManagerView { get; private set; }
    private List<Item> Items;

    public Action RefreshItems { get; set; }

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

        Items = Items.OrderBy(x => x.ID).ToList();
    }

    public IItem FindItem(int ID)
    {
        return Items.Find(x => x.ID == ID);
    }


    public List<IItem> GetItems()
    {
        return Items.Select(x => (IItem)x).ToList();
    }

    public BonusItem GetGlobalBonus()
    {
        BonusItem globalBonus = new BonusItem();

        foreach (Item item in Items)
        {
            if (item.CurrentAmount > 0)
            {
                globalBonus.IncreasesSuccessRate += item.BonusItem.IncreasesSuccessRate;
                globalBonus.ReducesCraftingTime += item.BonusItem.ReducesCraftingTime;
            }
        }

        return globalBonus;
    }

    private bool HasAnyBonus(Item item)
    {
        return item.BonusItem.IncreasesSuccessRate > 0 || item.BonusItem.ReducesCraftingTime > 0;
    }

    public List<IBonusItemDescription> GetAllBonusItems()
    {
        List<IBonusItemDescription> items = new List<IBonusItemDescription>();

        foreach (Item item in Items)
        {
            if (item.CurrentAmount > 0 && HasAnyBonus(item))
            {
                items.Add(new BonusItemDescription(item));
            }
        }

        return items;
    }


    public void ReduceAmount(List<int> _ID)
    {
        foreach (int id in _ID)
        {
            Items.Find(x => x.ID == id).CurrentAmount--;
        }

        RefreshItems?.Invoke();
    }

    public void IncreaseAmount(int ID)
    {
        Item item = Items.Find(x => x.ID == ID);
        item.CurrentAmount++;
        item.CraftingAmount++;
        RefreshItems?.Invoke();
    }

}

public interface IItemsManagerView
{
    List<IBonusItemDescription> GetAllBonusItems();
    List<IItem> GetItems();
    IItem FindItem(int ID);
    BonusItem GetGlobalBonus();
    void ReduceAmount(List<int> ID);
    void IncreaseAmount(int ID);
    Action RefreshItems {get; set;}
}



[System.Serializable]
public class Item : IItem
{
    private ItemSO ItemSO;
    public string Name => ItemSO.Name;
    public int CurrentAmount { get; set; }
    public int CraftingAmount { get; set; }
    public int ID => ItemSO.ID;
    public Sprite Icon => ItemSO.Icon;
    public BonusItem BonusItem => ItemSO.BonusItem;

    public Item(ItemSO itemSO)
    {
        ItemSO = itemSO;

        CraftingAmount = 0;
    }
}


public interface IItem
{
    string Name { get; }
    int CurrentAmount { get; }
    int CraftingAmount { get; }
    int ID { get; }
    Sprite Icon { get; }
}