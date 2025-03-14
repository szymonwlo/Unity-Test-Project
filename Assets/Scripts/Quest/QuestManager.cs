using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class QuestManager : IQuestManagerView
{
    public static IQuestManagerView QuestManagerView {get; private set;}
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
}

public interface IQuestManagerView
{

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
    bool IsDone { get; }
    int Amount { get; }
    int RequiredAmount { get; }
}