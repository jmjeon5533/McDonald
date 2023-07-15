using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public GameObject Bullet;
    public float BulletSpeed;
    public Transform player;
    protected Transform weaponArm;
    public float recovery = 25f; //반동 회복
    public AudioClip FireSound;
    public Camera cam;
    public float CamFieldValue = 60;

    public Transform WeaponPos;
    protected Vector3 Weaponloc;

    [SerializeField] protected GameObject BulletParticle; //총구 화염
    [SerializeField] protected Transform Muzzle; //총구 위치
    
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
            Attack();
            WeaponPos.localRotation 
                = Quaternion.Lerp(WeaponPos.localRotation,Quaternion.Euler(Vector3.zero),0.2f);
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

    protected virtual void Attack()
    {
        StartCoroutine(CamTick());
        SoundManager.instance.SetAudio(FireSound, SoundManager.SoundState.SFX, false);
    }
    public abstract IEnumerator CamTick();
}
