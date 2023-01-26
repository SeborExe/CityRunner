using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isActive = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<Player>(out Player player) && isActive)
            {
                player.coins++;
                isActive = false;
                this.gameObject.SetActive(false);
            }
        }
    }
}
