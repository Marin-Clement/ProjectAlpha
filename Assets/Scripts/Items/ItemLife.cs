using UnityEngine;

public class ItemLife : ItemEntity
{
    [SerializeField] private GameObject healEffect;

    protected override void Action(Collider2D hit)
    {
        hit.GetComponentInParent<Health>().Heal(Random.Range(2, 5));
        Instantiate(healEffect, transform.position, Quaternion.identity);
    }
}