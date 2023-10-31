using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float jumpSpeed = 3.0f;
    [SerializeField] float climbSpeed = 3.0f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);

    [SerializeField]
    private GameObject bullet;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    Animator myAnimator;

    bool isAlive = true;

    float startingGravityScale;

    private CinemachineImpulseSource impulseSource;


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        startingGravityScale = myRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive) { return; }
        
        Run();   
        FlipSprite();
        ClimbLadder();
        Die();
        
        
    }

    void OnMove(InputValue value)
    {
        if(!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if(!isAlive) { return; }
        int groundLayerMask = LayerMask.GetMask("Ground");
        if(!feetCollider.IsTouchingLayers(groundLayerMask)) { return; }
        if(value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if(!isAlive) { return; }
        Transform gunTransform = this.transform.Find("Gun");
        Instantiate(bullet, gunTransform.position, gunTransform.rotation);
    }

    void Run()
    {
        float currentYVelocity = myRigidbody.velocity.y;
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, currentYVelocity);
        myRigidbody.velocity = playerVelocity;

        myAnimator.SetBool("isRunning", PlayerHasHorizontalSpeed());
    }

    void FlipSprite()
    {
        if(PlayerHasHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1.0f);
        }
    }

    void ClimbLadder()
    {
        int climbingLayerMask = LayerMask.GetMask("Climbing");
        if(!bodyCollider.IsTouchingLayers(climbingLayerMask)) 
        { 
            myRigidbody.gravityScale = startingGravityScale;
            myAnimator.SetBool("isClimbing", false);
            return; 
        }
        Debug.Log("Touching climb layer!");
        myRigidbody.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myAnimator.SetBool("isClimbing", PlayerHasVerticalSpeed());
    }

    void Die()
    {
        int hazardousLayersMask = LayerMask.GetMask("Enemies", "Hazards");
        if(!bodyCollider.IsTouchingLayers(hazardousLayersMask)) { return; }
        isAlive = false;
        myAnimator.SetTrigger("Dying");
        ShakeCamera();
        myRigidbody.velocity = deathKick;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    bool PlayerHasHorizontalSpeed()
    {
        return Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
    }

    bool PlayerHasVerticalSpeed()
    {
        return Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
    }

    void ShakeCamera()
    {
        CameraShakeManager.instance.CameraShake(impulseSource);
    }
}
