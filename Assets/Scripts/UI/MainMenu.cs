using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour, IMainMenu
{
    private IQuestManagerView _QuestManager;
    private IItemsManagerView _ItemsManager;
    private IMachinesManagerView _MachinesManager;
    private List<MachineElement> MachineElements;

    [SerializeField] private TextMeshProUGUI QuestsAndBonusesText;

    [Header("Inventory")]
    [SerializeField] private List<ItemGridElement> GridElements;
    [SerializeField] private TextMeshProUGUI CurrentItemName;

    [Header("Machines")]
    [SerializeField] private RectTransform ContainerMachines;
    [SerializeField] private GameObject MachineElementPrefab;


    void OnEnable()
    {
        ItemsManager.ItemsManagerView.RefreshItems += RefreshElement;
    }

    void OnDisable()
    {
        ItemsManager.ItemsManagerView.RefreshItems -= RefreshElement;
    }

    public void RefreshElement()
    {
        UpdateQuestsAndBonuses();
    }

    void Start()
    {
        _QuestManager = QuestManager.QuestManagerView;
        _ItemsManager = ItemsManager.ItemsManagerView;
        _MachinesManager = MachinesManager.MachinesManagerView;

        DebugLogs();

        UpdateQuestsAndBonuses();
        UpdateInventory(true);
        SpawnMachines();
    }

    private void SpawnMachines()
    {
        List<IMachineView> machines = _MachinesManager.GetAllMachines();

        MachineElements = new List<MachineElement>();
        foreach (IMachineView machine in machines)
        {
            GameObject gm = Instantiate(MachineElementPrefab, ContainerMachines);
            MachineElement machineElement = gm.GetComponent<MachineElement>();
            machineElement.Init(machine, this);

            MachineElements.Add(machineElement);
        }
    }

    private void UpdateInventory(bool first = false)
    {
        List<IItem> Items = _ItemsManager.GetItems();
        for(int i = 0; i < GridElements.Count; i++)
        {
            if(first)
                GridElements[i].Init(this, i);
            if(i > Items.Count - 1)
                GridElements[i].SetItem(null);
            else
                GridElements[i].SetItem(Items[i]);
        }
    }


    public void SelectItemInInventory(IItem Item)
    {
        if(Item != null)
            CurrentItemName.text = Item.Name;
    }

    public void DeselectItemInInventory()
    {
        CurrentItemName.text = string.Empty;
    }




    private void UpdateQuestsAndBonuses()
    {
        QuestsAndBonusesText.text = string.Empty;
        List<IQuest> activeQuests = _QuestManager.GetActiveQuests();
        if (activeQuests != null && activeQuests.Count > 0)
        {
            QuestsAndBonusesText.text += "<b>Quests:</b>\n";
            foreach (IQuest quest in activeQuests)
            {
                QuestsAndBonusesText.text += quest.Description + "\n";
            }
        }

        List<IBonusItemDescription> bonusItems = _ItemsManager.GetAllBonusItems();
        if (bonusItems != null && bonusItems.Count > 0)
        {
            QuestsAndBonusesText.text += "<b>Bonuses:</b>\n";
            foreach (IBonusItemDescription bonusItem in bonusItems)
            {
                //QuestsAndBonusesText.text += bonusItem.NameBonusItem + "\n";
                foreach (string effect in bonusItem.Effects)
                {
                    QuestsAndBonusesText.text += "+" + effect + "\n";
                }
            }
        }
    }

    private void DebugLogs()
    {
        Debug.Log("[QuestManager]");
        List<IQuest> activeQuests = _QuestManager.GetActiveQuests();
        foreach (IQuest quest in activeQuests)
        {
            Debug.Log(quest.Description);
        }

        Debug.Log("[Bonuses]");
        List<IBonusItemDescription> bonusItems = _ItemsManager.GetAllBonusItems();
        foreach (IBonusItemDescription bonusItem in bonusItems)
        {
            Debug.Log(bonusItem.NameBonusItem);
            foreach (string effect in bonusItem.Effects)
            {
                Debug.Log(effect);
            }
        }


        Debug.Log("[ItemsManager]");
        List<IItem> Items = _ItemsManager.GetItems();
        foreach (IItem item in Items)
        {
            Debug.Log("[" + item.ID + "] " + item.Name + " " + item.CurrentAmount);
        }

        Debug.Log("[MachinesManager]");
        List<IMachineView> machines = _MachinesManager.GetAllMachines();
        foreach (IMachineView machine in machines)
        {
            Debug.Log(machine.Name + " Unlocked: " + machine.Unlocked);
        }
    }


}


public interface IMainMenu
{
    void SelectItemInInventory(IItem Item);
    void DeselectItemInInventory();
}