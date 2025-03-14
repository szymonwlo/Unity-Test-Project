using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ForgingRulesSO", menuName = "ScriptableObjects/ForgingRulesSO", order = 2)]
public class ForgingRulesSO : ScriptableObject
{
    public string Name;
    public List<Recipe> Recipes;
    public QuestSO RequiredQuest;
}


[System.Serializable]
public struct Recipe
{
    public List<ItemSO> ItemsIn;
    public ItemSO ItemOut;
    public float Time;
    public float Success;
}