using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{

    // Reference to player
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        // Grab a reference to the player once we start the game
        player = GameObject.Find("Steve").GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        // If the player collides with the coin, the coin will delete itself and add to the coin score
        if (other.name == "Steve")
        {
            player.coinCount++;

            PlayerController play = other.GetComponent<PlayerController>();
            if (play != null)
            {
                play.PlayCollectSound();
            }

            Destroy(this.gameObject);

        }
    }
}
