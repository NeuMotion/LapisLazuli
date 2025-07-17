using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed;
    public float rotateSpeed = 75f;
    public float jumpForce;
    public AudioSource voice;
    public AudioSource bgMusic;
    public AudioClip erclip;

    [Header("Components")]
    public Rigidbody rig;
    ////////////////////////////
    public GameObject playerObj;
    ////////////////////////////
    public Animator anim;

    [Header("Statistics")]
    public int health;
    public int coinCount;

    [Header("Animation Booleans")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isIdle;

    void Move()
    {
        // Get the input axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //////////////////////////////////////////////////////////////////////////////////
        Vector3 moveDir = playerObj.transform.forward * z + playerObj.transform.right * x;
        rig.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        Vector3 flatVel = new Vector3(rig.velocity.x, 0f, rig.velocity.z);
        // Limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rig.velocity = new Vector3(limitedVel.x, rig.velocity.y, limitedVel.z);
        }
        //////////////////////////////////////////////////////////////////////////////////
        
        //isGrounded = CheckIsGrounded();
        if (CheckIsGrounded())
        {
            isGrounded = true;
            anim.SetTrigger("isLanded");
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Set 
            isSprinting = true;
            anim.SetBool("isSprinting", true);
            moveSpeed += 3;
            jumpForce += 1;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // Disable
            isSprinting = false;
            anim.SetBool("isSprinting", false);
            moveSpeed -= 3;
            jumpForce -= 1;
        }

        //Dance button stuff
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            bgMusic.Stop();
            voice.clip = erclip;
            voice.Play();
            anim.SetBool("isIdle", true);
            anim.SetBool("isTwerking", true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            voice.clip = erclip;
            voice.Stop();
            bgMusic.Play();
            anim.SetBool("isIdle", true);
            anim.SetBool("isTwerking", false);
        }
        
        /* Vector3 rotation = Vector3.up * x; */
        

        /* COMMENTED OUT FOR A GAME TEST
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        // Calculate a direction relative to where we are facing
        Vector3 dir = (transform.forward * z + transform.right * x) * moveSpeed;
        dir.y = rig.velocity.y;

        // Set that as our velocity
        rig.velocity = dir;
        
        // rig.MoveRotation(rig.rotation * angleRot); 
        END OF REDACTED AREA */


        // Move = play run animation, otherwise not
        if(Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    public bool CheckIsGrounded()
    {
        return rig.velocity.y < 0.1;
    }

    void TryJump()
    {

        // Create a ray facing down
        Ray ray = new Ray(transform.position, Vector3.down);

        // Shoot the raycast & tests for sprint jumping
        if (Physics.Raycast(ray, 1.5f) && !isSprinting) {
            anim.SetTrigger("isJumping");
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        }
        if (Physics.Raycast(ray, 1.5f) && isSprinting) {
            anim.SetTrigger("isSprintJumping");
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Input for movement
        Move();

        // Input for jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }

        if(health <= 0)
        {
            anim.SetBool("Die", true);
            StartCoroutine("DieButCool");
        }
    }

    IEnumerator DieButCool()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Killider")
        {
            health -= 2;
        }
        if(other.gameObject.name == "FallCollider")
        {
            SceneManager.LoadScene(0);
        }
    }
}
