using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 12f;
    public float gravity = -20f;
    public float jumpHeight = 1.5f;

    PlayerControls controls;
    CharacterController controller;
    Animator animator;
    Transform cam;
    public bool isAttacking;

    Vector3 moveInput;
    Vector3 finalInput;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Movement.Move.performed += OnMove;
        controls.Movement.Move.canceled += OnCancelMove;
        controls.Movement.Attack.performed += OnAttack;
    }

    void OnDisable()
    {
        controls.Movement.Move.performed -= OnMove;
        controls.Movement.Move.canceled -= OnCancelMove;
        controls.Movement.Attack.performed -= OnAttack;
        controls.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector3>();
    }

    void OnCancelMove(InputAction.CallbackContext context)
    {
        moveInput = Vector3.zero;
    }
    
    void OnAttack(InputAction.CallbackContext context)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    void Update()
    {
        animator.SetFloat("Speed", moveInput.magnitude);
        animator.SetBool("isGrounded", controller.isGrounded);

        Vector3 forward = cam.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = cam.right;
        right.y = 0f;
        right.Normalize();

        finalInput = forward * moveInput.z + right * moveInput.x;
        if (finalInput.sqrMagnitude > 1f) finalInput.Normalize();

        //Booleana controlada ao fim da animação de ataque (animation):
        if (isAttacking)
            finalInput = Vector3.zero;

        // Rotação: só gira quando há input
        if (finalInput.sqrMagnitude > 0.01f)
        {
            Quaternion target = Quaternion.LookRotation(finalInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }

        //Move() não depende do passo de física.
        //Ele calcula a colisão e move o objeto na hora,
        //então o lugar natural é o Update, que roda a cada frame.
        Vector3 move = finalInput * speed;
        controller.Move(move * Time.deltaTime);
    }
}