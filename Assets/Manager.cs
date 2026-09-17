using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Manager : MonoBehaviour
{
    public enum PlayerState
    {
        NORMAL,
        EXHAUSTED,
        MOVING,
        AIRBORNE
    }
    public PlayerState mystate;
    public bool grounded = true;
    #region private variables
    private float moveSpeed = 8f;
    private float jumpForce = 4f;
    private Rigidbody2D playerRB;
    private bool canJump = true;
    private bool canDoubleJump = false;
    private float jumpCount = 0;
    private int floorCount = 0;
    private SpriteRenderer playerSprite;
    private KeyCode leftKey = KeyCode.A;
    private KeyCode rightKey = KeyCode.D;
    #endregion
    //public GameObject playerRB;
    public float Charge = 100;
    public TextMeshProUGUI chargeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        playerRB = GetComponent<Rigidbody2D>();
    }
    public void SwitchState()
    {
        if(Charge <= 0)
        {
            mystate = PlayerState.EXHAUSTED;
        }
        else if(Charge > 0)
        {
            mystate = PlayerState.NORMAL;
        }
        if(grounded == false)
        {
            //floorCount <= 0 &&
            mystate = PlayerState.AIRBORNE;
        }
    }
    // Update is called once per frame
    void Update()
    {
        #region StateMachine

        SwitchState();
        
        switch (mystate)
        {
            case PlayerState.EXHAUSTED:
                moveSpeed = 1f;
                jumpForce = .1f;
                playerSprite.color = Color.gray;
                break;
            case PlayerState.NORMAL:
                moveSpeed = 8f;
                jumpForce = 4f;
                playerSprite.color = Color.blue;
                break;
            case PlayerState.AIRBORNE:
                //Charge = Charge - 5;
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
                if(mystate == PlayerState.AIRBORNE && Charge >= 5)
                {
                    Charge = Charge - 5;
                }
                Jump(moveSpeed);
                //jumpSound.PlaySoundEffect1();
                //playeraudio.Play();
            }
        }
        #endregion
        chargeText.text = Charge.ToString("0");
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

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if(Charge > 0)
            {
                moveSpeed = 12f;
                Charge--;
            }
        }
        else if(mystate == PlayerState.EXHAUSTED)
        {
            moveSpeed = 2f;
        }
        else{moveSpeed = 8f;}
        playerRB.AddForce(myInput*moveSpeed*Time.fixedDeltaTime);
        #endregion
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Charger"))
        {
            floorCount += 1;
            jumpCount = 0;
            grounded = true;
        }
        if(collision.gameObject.CompareTag("Charger") && Charge < 100)
        {
            Charge++;
        }
    }

    void Jump(float jumpForce)
    { 
        Vector3 jump = new Vector3(0, jumpForce, 0);
        playerRB.AddForce(jump);
        jumpCount += 1;

    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Charger"))
        {
            grounded = false;
            floorCount -= 1;
        }    
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
