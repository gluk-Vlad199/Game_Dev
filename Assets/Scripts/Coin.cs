using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _coinPoints;



    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();

        if (player != null)
        {
            player.AddCoin(_coinPoints);
            Destroy(gameObject);
        }
    }

}
