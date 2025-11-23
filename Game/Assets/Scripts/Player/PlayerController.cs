using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.2f;
    public float dashDistance = 6f;
    public float dashCooldown = 2f;

    CharacterController cc;
    Vector3 velocity;
    float dashTimer = 0f;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        dashTimer -= Time.deltaTime;
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0; right.y = 0;
        forward.Normalize(); right.Normalize();

        Vector3 move = (forward * v + right * h).normalized;

        bool running = Input.GetKey(KeyCode.LeftShift);
        float speed = running ? runSpeed : walkSpeed;

        cc.Move(move * speed * Time.deltaTime);

        if (cc.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && cc.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Dash (space + direction)
        if (Input.GetKeyDown(KeyCode.Space) && dashTimer <= 0f)
        {
            Vector3 dashDir = move;
            if (dashDir.magnitude < 0.1f)
                dashDir = transform.forward;
            StartCoroutine(Dash(dashDir));
            dashTimer = dashCooldown;
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    System.Collections.IEnumerator Dash(Vector3 direction)
    {
        float elapsed = 0f;
        float duration = 0.2f;
        Vector3 start = transform.position;
        Vector3 target = start + direction.normalized * dashDistance;

        // simple lerp dash — you can replace with physics based impulse
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
    }
}
