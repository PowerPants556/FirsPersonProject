using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    private const float gravityScale = 9.8f, speedScale = 5f, jumpForce = 8f, turnSpeed = 90f;
    private float verticalSpeed = 0f, mouseX = 0f, mouseY = 0f, currentCameraAngelX = 0f;
    [SerializeField] private CharacterController charecterConn;
    [SerializeField] private GameObject playerCamera;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        RotateCharecter();
    }

    private void RotateCharecter()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(new Vector3(0f, mouseX * turnSpeed * Time.fixedDeltaTime, 0f));
        currentCameraAngelX += mouseY * Time.fixedDeltaTime * turnSpeed * -1;
        currentCameraAngelX = Mathf.Clamp(currentCameraAngelX, -60f, 60f);
        playerCamera.transform.localEulerAngles = new Vector3(currentCameraAngelX, 0f, 0f);
    }

    private void Move()
    {
        Vector3 velocity = new Vector3 (Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        velocity = transform.TransformDirection(velocity) * speedScale;
        if (charecterConn.isGrounded)
        {
            verticalSpeed = 0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalSpeed = jumpForce;
            }
        }
        verticalSpeed -= gravityScale * Time.deltaTime;
        velocity.y = verticalSpeed;
        charecterConn.Move(velocity * Time.deltaTime);
    }
}
