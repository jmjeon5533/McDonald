using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("카메라 관련")]
    public Transform cam; //카메라
    Rigidbody rigid;
    [SerializeField] float CamSpeed; //세로 화면 감도
    [SerializeField] float MoveSpeed; //이동속도
    [SerializeField] float JumpPower; //점프 파워
    // 제한할 카메라 각도 범위
    [SerializeField] float minCamAngle = -30f;
    [SerializeField] float maxCamAngle = 80f;

    [Space(10)]
    public float CamTickPos = 0; //총알 반동값

    public List<WeaponItem.WeaponType> Weapon = new List<WeaponItem.WeaponType>(); //무기 타입
    public List<GameObject> WeaponObj = new List<GameObject>(); //무기 오브젝트
    public List<Sprite> WeaponImage = new List<Sprite>(); //적용된 무기 이미지

    public Transform WeaponArm;
    public int SelectWeaponNum; //선택한 무기 번호(Alpha N)

    [Header("약점 모드")]
    public int Heart = 4; //목숨
    [Header("사격 모드")]
    public float HP;
    public float MaxHP;
    float CurShieldTime; //무적시간
    bool isDamage; //피격 유무
    [SerializeField] AudioClip HurtSound; //피격음


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MaxHP = HP;
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
        Vector3 currentRotation = cam.localEulerAngles - new Vector3(CamTickPos, 0, 0);
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
        for (int i = 0; i < 4; i++)
        {
            if (WeaponObj.Count <= 0) return;
            if (Input.GetKeyDown((KeyCode)(i + 49)))
            {
                SelectWeaponNum = i;
                ActiveWeapon(i);

            }
        }
    }
    public void ActiveWeapon(int index)
    {
        if (index > WeaponObj.Count - 1) return;
        SelectWeaponNum = index;
        var UI = UIManager.instance;
        if (index + 1 > WeaponObj.Count) return;
        for (int i = 0; i < WeaponObj.Count; i++)
        {
            UI.WeaponUI[i].GetChild(0).gameObject.SetActive(false);
            WeaponObj[i].SetActive(false);
        }
        UI.WeaponUI[index].GetChild(0).gameObject.SetActive(true);
        WeaponObj[index].SetActive(true);
        UIManager.instance.InitWeaponUI(index);
        UIManager.instance.InitAmmoUI();
        CamTickPos = 0;
        var w = WeaponObj[index].GetComponent<WeaponBase>();
        w.isReload = false;
        w.isAttack = false;
        if (w.Ammo <= 0 && w.megazine > 0) w.Reload();
        else UIManager.instance.ReloadImage.fillAmount = 0;
        //if (Weapon[index] == ItemS.WeaponType.Cola) WeaponObj[index].GetComponent<Cola>().ColaWeapon.Stop();
    }

    public void Damage(float damage = 0)
    {
        if (SceneManager.instance.FireMod)
        {
            if (!isDamage)
            {
                isDamage = true;
                HP -= damage;
                UIManager.instance.MinusHeartUI(HP);
                if (HP <= 0)
                {
                    Dead();
                }
                StartCoroutine(Protect());
                SoundManager.instance.SetAudio(HurtSound, SoundManager.SoundState.SFX, false);
            }
        }
        else
        {
            Heart--;
            UIManager.instance.MinusHeartUI(Heart);
            if (Heart <= 0)
            {
                Dead();
            }
        }
    }
    IEnumerator Protect()
    {
        yield return new WaitForSeconds(0.5f);
        isDamage = false;
    }
    void Dead()
    {
        print("죽었슴");
        SceneManager.instance.isGame = false;
        SpawnManager.instance.GameOver();
        return;
    }
}
