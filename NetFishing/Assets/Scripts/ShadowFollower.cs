using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFollower : MonoBehaviour {

	public Transform player; // Reference to the player's transform
    public Vector3 offset = new Vector3(0, -5f, 0); // Offset to prevent z-fighting with the ground

    void Update()
    {
        // Position the shadow directly beneath the player
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y + offset.y, player.position.z);
        }
    }
}
