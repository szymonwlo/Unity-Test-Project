using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ForgingRulesSO", menuName = "ScriptableObjects/ForgingRulesSO", order = 2)]
public class ForgingRulesSO : ScriptableObject
{
    public List<Recipe> Recipes;
}


[System.Serializable]
public struct Recipe
{
    public List<ItemSO> ItemsIn;
    public ItemSO ItemOut;
    public float Time;
    public float Success;
}