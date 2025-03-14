using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class MachinesManager : IMachinesManagerView
{
    public static IMachinesManagerView MachinesManagerView {get; private set;}
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
                machine.Quest = this.QuestManager.CreateOrGetQuest(ForgingRules[i].RequiredQuest);

            Machines.Add(machine);
        }
    }

    public void Update(float DeltaTime)
    {
        Machines.ForEach(x => x.Update(DeltaTime));
    }

}

public interface IMachinesManagerView
{

}


[System.Serializable]
public class Machine : IMachine
{
    private ForgingRulesSO ForgingRulesSO;
    public IQuest Quest { get; set; }

    public float CraftingTime { get; set; }
    public bool IsDone { get; set; }



    public Machine(ForgingRulesSO forgingRulesSO)
    {
        ForgingRulesSO = forgingRulesSO;
        IsDone = true;
    }

    public void Update(float DeltaTime)
    {
        if(!IsDone)
        {
            CraftingTime += DeltaTime;

        }
    }
}

public interface IMachine
{
    bool IsDone { get; }
}