using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Trap")]
    [SerializeField] float chanceToSpawn;

    protected virtual void Start()
    {
        if (UnityEngine.Random.Range(1, 100) > chanceToSpawn)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.Knockback();
        }
    }
}
