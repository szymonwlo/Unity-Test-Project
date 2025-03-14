using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        return Machines.Select( x => (IMachineView) x).ToList();
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
    public float CraftingTime { get; private set; }
    public bool IsDone { get; private set; }
    public string Name => ForgingRulesSO.Name;
    public bool Unlocked => Quest == null || Quest.IsDone ? true : false;


    public Machine(ForgingRulesSO forgingRulesSO)
    {
        ForgingRulesSO = forgingRulesSO;
        IsDone = true;
    }

    public void AddQuest(IQuest _Quest)
    {
        Quest = _Quest;
    }

    public void Update(float DeltaTime)
    {
        if (!IsDone)
        {
            CraftingTime += DeltaTime;

        }
    }
}

public interface IMachineView
{
    IQuest Quest { get; }
    bool IsDone { get; }
    string Name { get; }
    bool Unlocked { get; }
}