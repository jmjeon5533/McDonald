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
        transform.Rotate(new Vector3(0, h, 0));
        cam.Rotate(new Vector3(-v, 0, 0));
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
    private void OnDrawGizmos() {
        Debug.DrawRay(transform.position,Vector3.down * 1.02f,Color.red);
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
