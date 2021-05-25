using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Spawnable
    {
        public GameObject gameObject;
        public float weight;
    }

    public List<Spawnable> items = new List<Spawnable>();
    float totalWeight;
    public GameObject room;
    void Awake()
    {
        totalWeight = 0;
        foreach (var spawnable in items)
        {
            totalWeight += spawnable.weight;
        }
    }

    void Start()
    { 
        var pick = Random.value * totalWeight;
        var chosenIndex = 0;
        var cumulativeWeight = items[0].weight;

        while(pick > cumulativeWeight && chosenIndex < items.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += items[chosenIndex].weight;
        }

        var i = Instantiate(items[chosenIndex].gameObject, transform.position, Quaternion.identity) ;
        i.transform.SetParent(room.transform);
 
    }
    
}
