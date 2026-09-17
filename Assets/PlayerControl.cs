using UnityEngine;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public enum PlayerState
    {
        EXHAUSTED,
        MOVING,
        AIRBORNE
    }
    public PlayerState mystate;
    #region private variables
    private float moveSpeed = 8f;
    private float jumpForce = 5f;
    private Rigidbody2D playerRB;
    private bool canJump = true;
    private bool canDoubleJump = false;
    private float jumpCount = 0;
    private int floorCount = 0;
    private SpriteRenderer playerSprite;
    private KeyCode leftKey = KeyCode.A;
    private KeyCode rightKey = KeyCode.D;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        playerRB = GetComponent<Rigidbody2D>();
    }

    public void SwitchState()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        #region StateMachine
        SwitchState();
        
        switch (mystate)
        {
            case PlayerState.EXHAUSTED:
                moveSpeed = 4f;
                jumpForce = 3f;
                break;
        }
        #endregion
        #region jump/double jump
        if(floorCount > 0 && jumpCount < 1)
        {
            canJump = true;
            canDoubleJump = false;
        }
        else { canJump = false; }


        if(floorCount <= 0 && jumpCount < 2)
        {
            canDoubleJump = true;
        }
        else {canDoubleJump = false; }

        
        if(canJump || canDoubleJump)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump(moveSpeed);
                //jumpSound.PlaySoundEffect1();
                //playeraudio.Play();
            }
        }
        #endregion
            
    }

    void FixedUpdate()
    {
        #region Movement
        Vector3 myInput = Vector3.zero;

        if(Input.GetKey(leftKey))
        {
            myInput = myInput + Vector3.left;
        }
        if(Input.GetKey(rightKey))
        {
            myInput = myInput + Vector3.right;
        }

        playerRB.AddForce(myInput*moveSpeed*Time.fixedDeltaTime);
        #endregion
    }

    void Jump(float jumpForce)
    { 
        Vector3 jump = new Vector3(0, jumpForce, 0);
        playerRB.AddForce(jump);
        jumpCount += 1;

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Charger"))
        {
            floorCount += 1;
            jumpCount = 0;
        }
        
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        floorCount -= 1;
    }

    void allowJump()
    {
        if (canJump == true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                jumpCount += 1;
            }
        }
        
        if(jumpCount >= 2)
        {
            canJump = false;
        }
        //Debug.Log("Jump Count: " + jumpCount);
    }
}
