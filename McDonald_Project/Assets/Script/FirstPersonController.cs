using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    Transform cam;
    [SerializeField] float CamSpeed;
    [SerializeField] float MoveSpeed;
    [SerializeField] float JumpPower;
    Rigidbody rigid;

    // 제한할 카메라 각도 범위
    [SerializeField] float minCamAngle = -30f;
    [SerializeField] float maxCamAngle = 80f;

    void Start()
    {
        cam = transform.GetChild(0).transform;
        rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CamRot();
        Jump();
        Movement();
    }

    void CamRot()
    {
        float h = Input.GetAxis("Mouse X") * CamSpeed * Time.deltaTime;
        float v = Input.GetAxis("Mouse Y") * CamSpeed * Time.deltaTime;

        // 현재 카메라의 각도
        Vector3 currentRotation = cam.localEulerAngles;
        float newRotationX = currentRotation.x - v;

        // 각도를 -180 ~ 180 범위로 조정
        if (newRotationX > 180f)
            newRotationX -= 360f;
        else if (newRotationX < -180f)
            newRotationX += 360f;

        // 각도를 제한 범위 내로 조정
        newRotationX = Mathf.Clamp(newRotationX, minCamAngle, maxCamAngle);

        // 카메라 회전 적용
        cam.localEulerAngles = new Vector3(newRotationX, 0f, 0f);
        transform.Rotate(new Vector3(0, h, 0));
    }

    void Movement()
    {
        float h = Input.GetAxis("Horizontal") * MoveSpeed * 10 * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * MoveSpeed * 10 * Time.deltaTime;

        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 movement = forward * v + right * h;
        rigid.velocity = new Vector3(movement.x, rigid.velocity.y, movement.z);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * 1.02f, Color.red);
    }

    void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.02f, LayerMask.GetMask("Ground")))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigid.AddForce(Vector3.up * JumpPower);
            }
        }
    }
}
