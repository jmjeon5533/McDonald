using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public GameObject Bullet; //총알 프리팹
    public float BulletSpeed; //총알 속도
    public Transform player; //플레이어 위치
    protected Transform weaponArm; //생성 위치 (플레이어 팔 부분)
    public float recovery = 25f; //반동 회복
    public AudioClip FireSound; //격발음
    public AudioClip ReloadSound; //장전음
    public Camera cam; //카메라
    public float CamFieldValue = 60; //카메라 fieldOfView

    public Transform WeaponPos;
    protected Vector3 Weaponloc;

    [SerializeField] protected GameObject BulletParticle; //총구 화염
    [SerializeField] protected Transform Muzzle; //총구 위치

    public Sprite WeaponImage; //무기 이미지
    [Space(20)]
    public int Ammo; //탄약 (현재 탄 갯수)
    public int megazine; //탄창 (구비된 탄 갯수)
    public int maxAmmo; //최대 탄약 (총에 들어갈 수 있는 최대 탄 갯수)
    public int Damage; //공격력

    public float ReloadTime; //장전시간
    public bool isReload = false; //장전중
    public float AttackTime; //공격속도
    public bool isAttack = false; //공격 가능 유무
    void Awake()
    {
        SpawnManager.instance.player.WeaponImage.Add(WeaponImage);
    }
    protected virtual void Start()
    {
        player = SpawnManager.instance.player.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localEulerAngles = new Vector3(0, 0, 0);
        weaponArm = player.GetComponent<FirstPersonController>().WeaponArm;
        cam = Camera.main.GetComponent<Camera>();
        cam.fieldOfView = 60;
        Weaponloc = WeaponPos.localPosition;
    }
    protected virtual void Update()
    {
        if (SceneManager.instance.isGame && Time.timeScale != 0)
        {
            if (Ammo > 0 && !isReload)
            {
                AttackKey();
            }
            WeaponPos.localRotation
                = Quaternion.Lerp(WeaponPos.localRotation, Quaternion.Euler(Vector3.zero), 0.2f);
        }
        if (Mathf.Abs(cam.fieldOfView - CamFieldValue) > 0.1f)
        {
            FieldSet();
        }
    }
    protected virtual void FieldSet()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, CamFieldValue, 15 * Time.deltaTime);
    }
    protected virtual void AttackKey()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttack)
            {
                isAttack = true;
                Attack();
                StartCoroutine(AttackWait());
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }
    IEnumerator AttackWait()
    {
        yield return new WaitForSeconds(AttackTime);
        isAttack = false;
    }
    protected virtual void Attack()
    {
        print("attack");
        StartCoroutine(CamTick());
        SoundManager.instance.SetAudio(FireSound, SoundManager.SoundState.SFX, false);
        minusAmmo();
    }
    public void minusAmmo()
    {
        Ammo--;
        if (Ammo <= 0)
        {
            print("1");
            Reload();
        }
        UIManager.instance.InitAmmoUI();
    }
    public virtual void Reload()
    {
        if (megazine > 0 && Ammo < maxAmmo && !isReload)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }
    IEnumerator ReloadCoroutine()
    {
        print("2");
        isReload = true;
        SoundManager.instance.SetAudio(ReloadSound, SoundManager.SoundState.SFX, false);
        UIManager.instance.ReloadImage.fillAmount = 0;
        UIManager.instance.ReloadImage.enabled = true;
        for (float f = 0; f < ReloadTime; f += Time.deltaTime)
        {
            yield return null;
            UIManager.instance.ReloadImage.fillAmount = Mathf.InverseLerp(0, ReloadTime, f);
        }
        UIManager.instance.ReloadImage.enabled = false;
        int add;
        if (megazine >= maxAmmo)
        {
            add = maxAmmo - Ammo;
        }
        else
        {
            add = megazine;
        }
        Ammo += add;
        megazine -= add;
        UIManager.instance.InitAmmoUI();
        isReload = false;
    }
    public abstract IEnumerator CamTick();
}
