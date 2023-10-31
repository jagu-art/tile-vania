using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] int value = 100;
    bool wasCollected = false;  // this was added for a bug where a coin is collected more than once
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            FindObjectOfType<GameSession>().IncreaseScoreBy(value);
            gameObject.SetActive(false);    // this was added for a bug where a coin is collected more than once
            Destroy(gameObject);
        }
    }
}
