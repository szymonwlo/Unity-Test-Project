using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "GameDataSO", menuName = "ScriptableObjects/GameDataSO", order = 0)]
public class GameDataSO : ScriptableObject
{
    public List<ItemSO> Items;

    public List<ForgingRulesSO> ForgingRules;
}


