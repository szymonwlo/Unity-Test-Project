using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class QuestManager : IQuestManagerView
{
    public static IQuestManagerView QuestManagerView { get; private set; }
    private ItemsManager ItemsManager;
    private List<Quest> Quests;

    public QuestManager(ItemsManager ItemsManager)
    {
        QuestManagerView = this;
        this.ItemsManager = ItemsManager;
        Quests = new List<Quest>();
    }

    public IQuest CreateOrGetQuest(QuestSO questSO)
    {
        Quest quest = Quests.Find(x => x.CompareQuestSO(questSO));

        if (quest == null)
        {
            IItem Item = ItemsManager.FindItem(questSO.Item.ID);
            quest = new Quest(questSO, Item);
            Quests.Add(quest);
        }


        return quest;
    }

    public List<IQuest> GetActiveQuests()
    {
        return Quests.FindAll(x => !x.IsDone).Select(x => (IQuest) x).ToList();
    }
}

public interface IQuestManagerView
{
    List<IQuest> GetActiveQuests();
}

[System.Serializable]
public class Quest : IQuest
{
    private QuestSO QuestSO;
    private IItem Item;

    public Quest(QuestSO QuestSO, IItem Item)
    {
        this.QuestSO = QuestSO;
        this.Item = Item;
    }


    public string Name => QuestSO.Name;
    public string Description => $"Craft {Item.CraftingAmount}/{QuestSO.amount} {Item.Name}";
    public bool IsDone => QuestSO.amount <= Item.CraftingAmount;
    public int Amount => Item.CraftingAmount;
    public int RequiredAmount => QuestSO.amount;

    public bool CompareQuestSO(QuestSO _questSO)
    {
        return QuestSO.amount == _questSO.amount && QuestSO.Item == _questSO.Item && QuestSO.Type == _questSO.Type;
    }

}

public interface IQuest
{
    string Name { get; }
    string Description { get; }
    bool IsDone { get; }
    int Amount { get; }
    int RequiredAmount { get; }
}