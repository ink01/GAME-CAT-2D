using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControls : MonoBehaviour
{
    public float speed = 5f;
    private GatherInput gatherInput;
    private Rigidbody2D rigidbody2D;
    private Animator animator;

    private int direction = 1; // to right-hand side

    public float jumpForce;
    public int maxJumpCount = 2; // จำนวนการกระโดดสูงสุด
    private int currentJumpCount; // เก็บจำนวนครั้งที่กระโดดในปัจจุบัน


    public float rayLength;
    public LayerMask groundLayer;
    public Transform leftPoint;  // จุดสำหรับตรวจสอบที่เท้าซ้าย
    public Transform rightPoint; // จุดสำหรับตรวจสอบที่เท้าขวา
    private bool grounded = false;
    
    // Start is called before the first frame update
    void Start()
    {
        gatherInput = GetComponent<GatherInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentJumpCount = maxJumpCount; // ตั้งค่าจำนวนครั้งที่กระโดดเริ่มต้น
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimatorValues();
    }
    private void FixedUpdate()
    {
        CheckStatus();
        Move();
        JumpPlayer();
    }
    private void Move() 
    {
        Flip();
        rigidbody2D.velocity = new Vector2(speed * gatherInput.valueX, rigidbody2D.velocity.y);
    }
    private void Flip() 
    { 
        if(gatherInput.valueX * direction < 0) 
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            direction *= -1;
        }
    }
    private void SetAnimatorValues() 
    {
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
        animator.SetFloat("vSpeed", rigidbody2D.velocity.y);
        animator.SetBool("Grounded", grounded);
    }
    private void JumpPlayer() 
    {
        if (gatherInput.jumpInput && (grounded || currentJumpCount > 0))
        {
            rigidbody2D.velocity = new Vector2(gatherInput.valueX * speed, jumpForce);
            currentJumpCount--;
        }
        gatherInput.jumpInput = false; // รีเซ็ตค่า input ของการกระโดด
    }
    private void CheckStatus() {
        RaycastHit2D leftCheckHit = Physics2D.Raycast(leftPoint.position, Vector2.down, rayLength, groundLayer);
        RaycastHit2D rightCheckHit = Physics2D.Raycast(rightPoint.position, Vector2.down, rayLength, groundLayer);

        grounded = leftCheckHit || rightCheckHit;
        
        if (grounded)
        {
            currentJumpCount = maxJumpCount; // รีเซ็ตจำนวนการกระโดดเมื่อสัมผัสพื้น
        }
    }
}
