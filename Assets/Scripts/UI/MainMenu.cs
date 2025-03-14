using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    private IQuestManagerView _QuestManager;
    private IItemsManagerView _ItemsManager;
    private IMachinesManagerView _MachinesManager;

    void Start()
    {
        _QuestManager = QuestManager.QuestManagerView;
        _ItemsManager = ItemsManager.ItemsManagerView;
        _MachinesManager = MachinesManager.MachinesManagerView;

        DebugLogs();
    }

    private void DebugLogs()
    {
        Debug.Log("[QuestManager]");
        List<IQuest> activeQuests = _QuestManager.GetActiveQuests();
        foreach(IQuest quest in activeQuests)
        {
            Debug.Log(quest.Description);
        }

        Debug.Log("[Bonuses]");
        List<IBonusItemDescription> bonusItems = _ItemsManager.GetAllBonusItems();
        foreach(IBonusItemDescription bonusItem in bonusItems)
        {
            Debug.Log(bonusItem.NameBonusItem);
            foreach(string effect in bonusItem.Effects)
            {
                Debug.Log(effect);
            }
        }


        Debug.Log("[ItemsManager]");
        List<IItem> Items = _ItemsManager.GetItems();
        foreach(IItem item in Items)
        {
            Debug.Log("[" + item.ID + "] " + item.Name + " " + item.CurrentAmount);
        }

        Debug.Log("[MachinesManager]");
        List<IMachineView> machines = _MachinesManager.GetAllMachines();
        foreach(IMachineView machine in machines)
        {
            Debug.Log(machine.Name + " Unlocked: " + machine.Unlocked);
        }
    }

    void Update()
    {
        
    }
}
