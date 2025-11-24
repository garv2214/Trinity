using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [Header("Movement Settings")]
  public float moveSpeed = 6f;
  public float rotationSpeed = 12f;

  [Header("Jump Settings")]
  public float jumpHeight = 1.5f;
  public float gravity = -9.81f;

  [Header("References")]
  public Transform cameraTransform;
  private CharacterController controller;
  private Animator animator;

  private Vector3 velocity;
  private bool isGrounded;

  void Start()
  {
    controller = GetComponent<CharacterController>();
    animator = GetComponent<Animator>();

    if (cameraTransform == null)
    {
      cameraTransform = Camera.main.transform;
    }
  }

  void Update()
  {
    HandleMovement();
    HandleGravity();
    HandleJump();
    ApplyAnimations();
  }

  void HandleMovement()
  {
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");

    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

    if (direction.magnitude >= 0.1f)
    {
      // Camera-relative movement
      float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
                          + cameraTransform.eulerAngles.y;
      float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle,
                                     rotationSpeed * Time.deltaTime);

      transform.rotation = Quaternion.Euler(0f, angle, 0f);

      Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

      controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
    }
  }

  void HandleJump()
  {
    if (controller.isGrounded && Input.GetButtonDown("Jump"))
    {
      velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
  }

  void HandleGravity()
  {
    if (controller.isGrounded && velocity.y < 0)
    {
      velocity.y = -2f;
    }

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
  }

  void ApplyAnimations()
  {
    if (animator == null) return;

    float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

    animator.SetFloat("Speed", speed);
    animator.SetBool("IsGrounded", controller.isGrounded);
  }
}
