using UnityEngine;



[CreateAssetMenu(fileName = "QuestSO", menuName = "ScriptableObjects/QuestSO", order = 2)]
public class QuestSO : ScriptableObject
{
    public string Name;
    public QuestType Type = QuestType.Craft;
    public ItemSO Item;
    public int amount;

}


public enum QuestType
{
    Craft
}