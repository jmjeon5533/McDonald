using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public Transform cam;
    Rigidbody rigid;
    [SerializeField] float CamSpeed;
    [SerializeField] float MoveSpeed;
    [SerializeField] float JumpPower;
    
    public float CamTickPos = 0;
    public int Heart = 4;

    public List<ItemS.WeaponType> Weapon = new List<ItemS.WeaponType>();
    public List<GameObject> WeaponObj = new List<GameObject>();

    [SerializeField] GameObject[] Bullet;

    public Transform WeaponArm;
    public int SelectWeaponNum; //선택한 무기 번호(Alpha N)

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
        if (SceneManager.instance.isGame)
        {
            CamRot();
            Jump();
            Movement();
            InitWeapon();
        }
        // else
        // {
        //     cam.transform.position = transform.position + new Vector3(0, 7, -6);
        //     cam.transform.eulerAngles = new Vector3(50, 0, 0);
        // }
    }

    void CamRot()
    {
        float h = Input.GetAxis("Mouse X") * CamSpeed * Time.deltaTime;
        float v = Input.GetAxis("Mouse Y") * CamSpeed * Time.deltaTime;

        // 현재 카메라의 각도
        Vector3 currentRotation = cam.localEulerAngles - new Vector3(CamTickPos,0,0);
        float newRotationX = currentRotation.x - v;

        // 각도를 -180 ~ 180 범위로 조정
        if (newRotationX > 180f)
            newRotationX -= 360f;
        else if (newRotationX < -180f)
            newRotationX += 360f;

        // 각도를 제한 범위 내로 조정
        newRotationX = Mathf.Clamp(newRotationX, minCamAngle, maxCamAngle);

        // 카메라 회전 적용
        cam.localEulerAngles = new Vector3(newRotationX - CamTickPos, 0f, 0f);
        transform.Rotate(new Vector3(0, h, 0));
    }

    void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal") * MoveSpeed * 10 * Time.deltaTime;
        float v = Input.GetAxisRaw("Vertical") * MoveSpeed * 10 * Time.deltaTime;

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
    void InitWeapon()
    {
        for(int i = 0; i < 4; i++)
            if(Input.GetKeyDown((KeyCode)(i + 49))) 
            {
                SelectWeaponNum = i;
                ActiveWeapon(i);
            }
    }
    public void ActiveWeapon(int index)
    {
        var UI = UIManager.instance;
        if (index + 1 > WeaponObj.Count) return;
        for (int i = 0; i < WeaponObj.Count; i++)
        {
            UI.WeaponUI[i].GetChild(0).gameObject.SetActive(false);
            WeaponObj[i].SetActive(false);
        }
        UI.WeaponUI[index].GetChild(0).gameObject.SetActive(true);
        WeaponObj[index].SetActive(true);
        CamTickPos = 0;
        //if (Weapon[index] == ItemS.WeaponType.Cola) WeaponObj[index].GetComponent<Cola>().ColaWeapon.Stop();
    }

    public void Damage()
    {
        Heart--;
        UIManager.instance.MinusHeartUI(Heart);
        if (Heart <= 0)
        {
            print("죽었슴");
            SceneManager.instance.isGame = false;
            SpawnManager.instance.GameOver();
            // cam.transform.SetParent(null);
            // rigid.freezeRotation = false;
            // transform.Rotate(Random.insideUnitSphere * 5);
            return;
        }
    }
}
