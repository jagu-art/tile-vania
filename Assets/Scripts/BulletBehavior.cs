using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    float xSpeed;
    Rigidbody2D myRigidbody;
    PlayerMovement player;  // we can find this because it is the only object of this type

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();    // we can find this because it is the only object of this type
        xSpeed = player.transform.localScale.x * bulletSpeed;   // localScale x will be 1 or -1 depending on player orientation
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);  
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(this.gameObject);
    }
}
