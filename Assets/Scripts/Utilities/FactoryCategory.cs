using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FactoryCategory")]
public class FactoryCategory : ScriptableObject //Contains all object that will share the same pool id
{
    public List<GameObject> CategoryItems;
    public GameObject GetRandomIten => CategoryItems[Random.Range(0, CategoryItems.Count)];
}