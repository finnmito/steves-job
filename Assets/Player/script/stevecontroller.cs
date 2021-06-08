using UnityEngine;
using UnityEngine.Events;
using System;

public class stevecontroller : MonoBehaviour {

//member variablen
//===================================================================================================================================
    
    public float movement_speed = 100f;
    float horizontal_move = 0f;
    public float jump_hight = 3;
    private bool m_facing_right = true; 
    private Vector3 m_velocity = Vector3.zero;
    [Range(0, 1f)] [SerializeField] private float m_movement_smoothing = .05f;    // How much to smooth out the movement

    public Animator m_animator;
    private Rigidbody2D m_rigitbody;
    [SerializeField] private Transform m_ground_check;
    [SerializeField] private LayerMask ground_layer;
    //public UnityEvent OnLandEvent;
    [SerializeField] private bool is_grounded = false;
    const float k_GroundedRadius = .2f;
    [SerializeField] private bool crouch = false;
    [SerializeField] private bool shift;
    


//Methoden
//===================================================================================================================================


    private void Flip(){
        // Switch the way the player is labelled as facing.
        m_facing_right = !m_facing_right;
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Move(float move){
        // the actual movement
        //Debug.Log(move);
        //Debug.Log(m_ridgitbody.velocity.y);

        Vector3 player_velocity = new Vector2(move * 10f, m_rigitbody.velocity.y);
        m_rigitbody.velocity = Vector3.SmoothDamp(m_rigitbody.velocity, player_velocity, ref m_velocity, m_movement_smoothing);

        if (move < 0 && m_facing_right){
            Flip();
        }
        else if (move > 0 && !m_facing_right){
            Flip();
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground")){
            is_grounded = true;
        }
        Debug.Log(collision.gameObject.name);
    }

    void OnCollisionExit2D(Collision2D collision){
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground")){
            is_grounded = false;
            
        }
        Debug.Log(collision.collider.gameObject.layer);
        
    }
/*     private void Ground_check(){

        Collider2D[] collider = Physics2D.OverlapCircleAll(m_ground_check.position, k_GroundedRadius, ground_layer);
        is_grounded = false;
        Debug.Log(collider.Length);
        if(collider.Length > 0){
            is_grounded = true;
        }
        
    } */

//Start and Update
//===================================================================================================================================

    private void Start(){
        m_rigitbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    private void Update(){
        //Debug.Log(Time.deltaTime);
        //Debug.Log(horizontal_move);
        //Debug.Log(is_grounded);
        horizontal_move = Input.GetAxis("Horizontal") * movement_speed;
        m_animator.SetFloat("Speed", Mathf.Abs(horizontal_move));
        m_animator.SetBool("is_grounded", is_grounded);  
        m_animator.SetBool("crouch", crouch);   
        while (Input.GetKeyDown(KeyCode.LeftShift) && Mathf.Abs(m_rigitbody.velocity.y) < 0.001f){
            shift = true;
        }  
        if (!Input.GetKeyDown(KeyCode.LeftShift) && Mathf.Abs(m_rigitbody.velocity.y) < 0.001f){
            shift = false;
        } 
    }


    private void FixedUpdate() {
        //Ground_check();
        Move(horizontal_move * Time.fixedDeltaTime);
        //Debug.Log(horizontal_move);
        //Debug.Log(Time.fixedDeltaTime);
        //Debug.Log(m_ridgitbody.velocity.y);

        if (Input.GetButtonDown("Jump") && Mathf.Abs(m_rigitbody.velocity.y) < 0.001f) 
        {
            m_rigitbody.AddForce(new Vector2(0, jump_hight), ForceMode2D.Impulse);
        }
        if (shift){
            crouch = true;
            Debug.Log("crouch");
        } 
        if (!shift){
            crouch = false;
            Debug.Log("no crouch");
        } 

    }
}
