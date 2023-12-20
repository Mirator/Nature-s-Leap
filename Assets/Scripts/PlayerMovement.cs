using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float tiltSensitivity = 15.0f;  // Adjust this for more sensitive tilting
    public float maxSpeed = 10f;  // Maximum horizontal speed
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get tilt input and adjust sensitivity
        float tilt = Input.acceleration.x * tiltSensitivity;

        // Apply the tilt input to the Rigidbody velocity, clamping the max speed
        rb.velocity = new Vector2(Mathf.Clamp(tilt, -maxSpeed, maxSpeed), rb.velocity.y);

        // Automatic jump when grounded
        if (isGrounded && rb.velocity.y <= 0)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player is grounded and falling, or hitting the platform from above
        if (collision.gameObject.tag == "Platform")
        {
            Collider2D platformCollider = collision.collider;
            float playerBottom = transform.position.y - (transform.localScale.y / 2);
            float platformTop = platformCollider.bounds.max.y;

            if (playerBottom > platformTop)
            {
                isGrounded = true;
                Physics2D.IgnoreCollision(platformCollider, GetComponent<Collider2D>(), false);
                Debug.Log("Landed on platform");
            }
            else
            {
                Physics2D.IgnoreCollision(platformCollider, GetComponent<Collider2D>(), true);
                Debug.Log("Hit platform from below or while going up");
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isGrounded = false;
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), false);
        }
    }
}
