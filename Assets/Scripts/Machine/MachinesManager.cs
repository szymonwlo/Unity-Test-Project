using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class MachinesManager : IMachinesManagerView
{
    public static IMachinesManagerView MachinesManagerView { get; private set; }
    private List<Machine> Machines;
    private QuestManager QuestManager;

    public MachinesManager(List<ForgingRulesSO> ForgingRules, QuestManager QuestManager)
    {
        MachinesManagerView = this;
        this.QuestManager = QuestManager;
        Machines = new List<Machine>();
        for (int i = 0; i < ForgingRules.Count; i++)
        {
            Machine machine = new Machine(ForgingRules[i]);
            if (ForgingRules[i].RequiredQuest != null)
                machine.AddQuest(this.QuestManager.CreateOrGetQuest(ForgingRules[i].RequiredQuest));

            Machines.Add(machine);
        }
    }

    public void Update(float DeltaTime)
    {
        Machines.ForEach(x => x.Update(DeltaTime));
    }

    public List<IMachineView> GetAllMachines()
    {
        return Machines.Select(x => (IMachineView)x).ToList();
    }

}

public interface IMachinesManagerView
{
    List<IMachineView> GetAllMachines();
}


[System.Serializable]
public class Machine : IMachineView
{
    private ForgingRulesSO ForgingRulesSO;
    public IQuest Quest { get; private set; }
    public string Name => ForgingRulesSO.Name;
    public Sprite Icon => ForgingRulesSO.Icon;
    public bool Unlocked => Quest == null || Quest.IsDone ? true : false;
    public bool Working => CurrentCrafting != null;
    public float Progress => CurrentCrafting == null ? 0 : Mathf.InverseLerp(MaxTime, 0, Time);

    private FullRecipe? CurrentCrafting;
    private float Time;
    private float MaxTime;

    public Machine(ForgingRulesSO forgingRulesSO)
    {
        ForgingRulesSO = forgingRulesSO;

    }

    public void AddQuest(IQuest _Quest)
    {
        Quest = _Quest;
    }

    public void Update(float DeltaTime)
    {
        if (CurrentCrafting != null)
        {
            Time -= DeltaTime;

            if (Time <= 0)
            {
                if (CurrentCrafting.Value.Success == 1 || new System.Random().NextDouble() <= CurrentCrafting.Value.Success)
                    ItemsManager.ItemsManagerView.IncreaseAmount(CurrentCrafting.Value.ItemOut.ID);
                

                CurrentCrafting = null;
            }
        }
    }

    public void Craft(FullRecipe _FullRecipe)
    {
        CurrentCrafting = _FullRecipe;

        BonusItem bonusItem = ItemsManager.ItemsManagerView.GetGlobalBonus();

        MaxTime = Time = Mathf.Max(_FullRecipe.Time - bonusItem.ReducesCraftingTime,0);

        ItemsManager.ItemsManagerView.ReduceAmount(_FullRecipe.ItemsIn.Select(x => x.ID).ToList());
    }

    public List<FullRecipe> GetFullRecipe()
    {
        List<FullRecipe> fullRecipes = new List<FullRecipe>();

        foreach (Recipe recipe in ForgingRulesSO.Recipes)
        {
            FullRecipe r = new FullRecipe();
            r.Time = recipe.Time;
            r.Success = recipe.Success;
            r.ItemOut = ItemsManager.ItemsManagerView.FindItem(recipe.ItemOut.ID);

            r.ItemsIn = new List<IItem>();
            foreach (ItemSO item in recipe.ItemsIn)
            {
                r.ItemsIn.Add(ItemsManager.ItemsManagerView.FindItem(item.ID));
            }

            fullRecipes.Add(r);
        }


        return fullRecipes;
    }
}

public interface IMachineView
{
    IQuest Quest { get; }
    string Name { get; }
    bool Unlocked { get; }
    Sprite Icon { get; }
    bool Working { get; }
    float Progress { get; }
    List<FullRecipe> GetFullRecipe();
    void Craft(FullRecipe _FullRecipe);
}


public struct FullRecipe
{
    public List<IItem> ItemsIn;
    public IItem ItemOut;
    public float Time;
    public float Success;
}