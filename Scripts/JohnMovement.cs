using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnMovement : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public float AttackCooldown;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    private bool IsAttacking;
    private int Health = 6;
    private float attackTimer;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Movement
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Animator.SetBool("running", Horizontal != 0.0f);

        // Ground detection
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.F) && !IsAttacking && attackTimer <= 0.0f)
        {
            StartCoroutine(Attack());
        }

        // Cooldown timer
        if (attackTimer > 0.0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    private IEnumerator Attack()
    {
        IsAttacking = true;
        Animator.SetTrigger("attack");

        // Perform attack logic here

        // Wait for attack animation to finish
        yield return new WaitForSeconds(0.5f);

        IsAttacking = false;
        attackTimer = AttackCooldown;
    }

    public void Hit()
    {
        Health -= 1;
        if (Health == 0)
        {
            Destroy(gameObject);
        }
    }
}
