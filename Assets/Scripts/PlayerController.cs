using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public Camera cam;
    protected Collider coll;

    public float fallMulti = 2.5f;
    public float lowJumpMulti = 2f;

    public bool isGrounded = true;

    private float speed;
    private float runSpeed = 10f;
    private float walkSpeed = 5f;
    private float jumpForce = 15f;

    public GameObject journal;
    private bool journalActive = false;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;

    private float xAxisClamp;

    [SerializeField]
    private float lookSensitivity = 3f;

    private Rigidbody rb;
    private bool locked = false;

    private void Awake()
    {
        LockCursor();
    }

    private void LockCursor()
    {
        if (locked == true)
        {
            Cursor.lockState = CursorLockMode.None;
            locked = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            locked = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        journal.SetActive(false);
        rb = GetComponent<Rigidbody>();
        speed = walkSpeed;
        xAxisClamp = 0.0f;
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.IsPaused == true)
        {
            isGrounded = Grounded();
            float _xMove = Input.GetAxisRaw("Horizontal");
            float _zMove = Input.GetAxisRaw("Vertical");
            float _yRot = Input.GetAxisRaw("Mouse X");
            float _xRot = Input.GetAxisRaw("Mouse Y");


            float yRot = Input.GetAxisRaw("Mouse Y") * lookSensitivity * Time.deltaTime;

            Vector3 _movHorizontal = transform.right * _xMove;
            Vector3 _movVertical = transform.forward * _zMove;

            Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;
            Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
            Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;

            velocity = _velocity;
            rotation = _rotation;
            cameraRotation = _cameraRotation;

            xAxisClamp += yRot;


            if (Input.GetKeyDown(KeyCode.Q) && !PauseMenu.IsPaused)
            {
                if (journalActive == true)
                {
                    journal.SetActive(false);
                    journalActive = false;
                    LockCursor();
                    Time.timeScale = 1;
                }
                else
                {
                    journal.SetActive(true);
                    journalActive = true;
                    LockCursor();
                    Time.timeScale = 0;
                }

            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = runSpeed;
                cam.fieldOfView = 65;

            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = walkSpeed;
                cam.fieldOfView = 60;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {

                if (rb != null)
                {
                    if (Grounded())
                    {
                        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                        isGrounded = false;
                    }
                }

            }

            if (velocity != Vector3.zero)
            {
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            }

            PerformRotation();

            if (xAxisClamp > 90.0f)
            {
                xAxisClamp = 90.0f;
                yRot = 0.0f;
                ClampXAxisRotationToValue(270.0f);
            }
            else if (xAxisClamp < -90.0f)
            {
                xAxisClamp = -90.0f;
                yRot = 0.0f;
                ClampXAxisRotationToValue(90.0f);
            }

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMulti - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMulti - 1) * Time.deltaTime;
            }
        }
    }
    public bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, coll.bounds.extents.y);
    }

    void PerformRotation()
    {
        if (!PauseMenu.IsPaused || locked)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

            if (cam != null)
            {
                cam.transform.Rotate(-cameraRotation);
            }
        }

    }


    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
