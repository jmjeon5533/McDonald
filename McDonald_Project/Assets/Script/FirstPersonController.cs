using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    Transform cam;
    Rigidbody rigid;
    [SerializeField] float CamSpeed;
    [SerializeField] float MoveSpeed;
    [SerializeField] float JumpPower;

    public float HP;
    [HideInInspector] public float MaxHp;

    [SerializeField] GameObject[] Bullet;

    // 제한할 카메라 각도 범위
    [SerializeField] float minCamAngle = -30f;
    [SerializeField] float maxCamAngle = 80f;

    public enum Weapon
    {
        none,
        Hamberger,
        Cola,
        French_fries
    }
    public Weapon playerWeapon = Weapon.none;

    void Start()
    {
        cam = transform.GetChild(0).transform;
        rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerWeapon = Weapon.none;
        UIManager.instance.InitWeaponUI(0);

        MaxHp = HP;
    }

    void Update()
    {
        CamRot();
        Jump();
        Movement();
        WeaponSwap();
        Fire();
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
    void WeaponSwap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerWeapon = Weapon.Hamberger;
            UIManager.instance.InitWeaponUI(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerWeapon = Weapon.Cola;
            UIManager.instance.InitWeaponUI(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerWeapon = Weapon.French_fries;
            UIManager.instance.InitWeaponUI(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerWeapon = Weapon.none;
            UIManager.instance.InitWeaponUI(0);
        }
    }
    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (playerWeapon == Weapon.none) return;
            Instantiate(WeaponInit(playerWeapon), cam.position,
            Quaternion.Euler(new Vector3(cam.eulerAngles.x, transform.eulerAngles.y / 2, 0)));
        }
    }
    GameObject WeaponInit(Weapon weapon)
    {
        GameObject returnObj = null;
        switch (weapon)
        {
            case Weapon.none: returnObj = null; break;
            case Weapon.Hamberger: returnObj = Bullet[0]; break;
            case Weapon.Cola: returnObj = Bullet[1]; break;
            case Weapon.French_fries: returnObj = Bullet[2]; break;
        }
        return returnObj;
    }
    public void Damage()
    {

    }
}
