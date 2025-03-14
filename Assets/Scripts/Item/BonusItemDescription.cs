using System.Collections.Generic;
using UnityEngine;

public class BonusItemDescription : IBonusItemDescription
{
    private Item Item;

    public string NameBonusItem => Item.Name;

    public List<string> Effects { get; private set; }

    public BonusItemDescription(Item _item)
    {
        Item = _item;

        Effects = new List<string>();

        if(Item.BonusItem.IncreasesSuccessRate > 0)
            Effects.Add($"Increases success rate by {Item.BonusItem.IncreasesSuccessRate*100} percentage points");
        if(Item.BonusItem.ReducesCraftingTime > 0)
            Effects.Add($"Reduces crafting time by {Item.BonusItem.ReducesCraftingTime} seconds");
    }
}

public interface IBonusItemDescription
{
    string NameBonusItem { get; }
    List<string> Effects { get; }
}