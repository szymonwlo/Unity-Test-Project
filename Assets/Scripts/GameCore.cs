using UnityEngine;
using UnityEngine.SceneManagement;


public class GameCore : MonoBehaviour
{
    [SerializeField] private GameDataSO DataSO;
    private ItemsManager ItemsManager;
    private QuestManager QuestManager;
    private MachinesManager MachinesManager;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        ItemsManager = new ItemsManager(DataSO.Items);
        QuestManager = new QuestManager(ItemsManager);
        MachinesManager = new MachinesManager(DataSO.ForgingRules, QuestManager);

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    private void Update()
    {
        MachinesManager.Update(Time.deltaTime);
    }


}

[System.Serializable]
public class GamePrefs
{

}

[System.Serializable]
public class InventoryPrefs
{

}