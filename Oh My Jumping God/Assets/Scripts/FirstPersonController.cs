using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public Camera PlayerCamera;

    public float xSensitivity;
    public float ySensitivity;
    public float speed;
    public float jumpForce;
    public float sprintMultiplier;
    public int jumpCharges;

    public bool canMove;
    public bool canRotate;
    public bool canJump;
    public bool canSprint;
    public bool canDoubleJump;

    public float groundCheckSize;
    public float groundCheckLength;

    private float x;
    private float z;
    private float mouseX;
    private float mouseY;
    private float xRotation;
    private float yRotation;
    private Vector3 feetPosition;
    private RaycastHit groundHitInfo;
    private float baseSpeed;

    [SerializeField]
    public bool isGrounded;
    [SerializeField]
    private bool isSprinting = false;
    [SerializeField]
    int currentJumpCharges;

    private Vector3 LocalVelocity;
    private Rigidbody rb;


    void Start()
    {
        baseSpeed = speed;
        rb = GetComponent<Rigidbody>();
        PlayerCamera.transform.position = transform.position;
        PlayerCamera.transform.SetParent(this.gameObject.transform);

        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;

        GetMovementInfo();

        if (canRotate)
            Rotation();

        if (canMove)
            Move();

        if (canJump)
            Jump();

        if (canSprint)
            Sprint();
    }

    void GetMovementInfo()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        yRotation = mouseX * xSensitivity;
        xRotation = mouseY * ySensitivity;

        if (Physics.SphereCast(transform.position, groundCheckSize, Vector3.down, out groundHitInfo, groundCheckLength, Physics.DefaultRaycastLayers))
        {
            //Si el frame anterior estaba en el suelo, reinicia las cargas de salto
            if (!isGrounded)
            {
                currentJumpCharges = jumpCharges;
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void Move()
    {
        LocalVelocity.x = x * Time.timeScale * speed;
        LocalVelocity.y = rb.velocity.y;
        LocalVelocity.z = z * Time.timeScale * speed;

        rb.velocity = transform.TransformDirection(LocalVelocity);
    }

    void Rotation()
    {
        //Rotate player on Y axis
        transform.eulerAngles += new Vector3(0, yRotation, 0);

        //Rotate camera on X axis
        PlayerCamera.transform.eulerAngles += new Vector3(-xRotation, 0, 0);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && currentJumpCharges > 0 && canDoubleJump)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            currentJumpCharges--;
        }
    }

    void Sprint()
    {
        if (isGrounded && Input.GetKey(KeyCode.LeftControl))
        {
            if (!isSprinting)
            speed *= sprintMultiplier;

            isSprinting = true;
        }
        else
        {
            speed = baseSpeed;
            isSprinting = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, groundCheckSize);
    }
}
