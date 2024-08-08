using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, ICollectible
{
    public enum CollectibleType
    {
        Gold,
        Health
    }

    [SerializeField] Material[] materials;

    [SerializeField] int points;

    PlayerStats stats;

    [SerializeField] CollectibleType type;

    private void Awake()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        gameObject.GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
    }
    public void Collect()
    {
        Destroy(gameObject);
        
        switch(type)
        {
            case CollectibleType.Health:
                break;
            case CollectibleType.Gold:
                stats.AddGold(points);
                break;
        }
    }

}
