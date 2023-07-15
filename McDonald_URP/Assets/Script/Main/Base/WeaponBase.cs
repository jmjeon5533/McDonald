using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public GameObject Bullet;
    public Transform player;
    protected Transform weaponArm;
    public float recovery = 25f; //반동 회복
    public AudioClip FireSound;
    protected virtual void Start()
    {
        player = SpawnManager.instance.player.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localEulerAngles = new Vector3(0, 0, 0);
        weaponArm = player.GetComponent<FirstPersonController>().WeaponArm;
    }
    void Update()
    {
        if (SceneManager.instance.isGame)
            Attack();
    }
    protected virtual void Attack()
    {
        StartCoroutine(CamTick());
        SoundManager.instance.SetAudio(FireSound,SoundManager.SoundState.SFX, false);
    }
    public IEnumerator CamTick()
    {
        var controller = player.GetComponent<FirstPersonController>();
        controller.CamTickPos = 75f * Time.deltaTime;
        while(controller.CamTickPos >= -0.75f)
        {
            controller.CamTickPos -= Time.deltaTime * recovery;
            yield return null;
        }
        controller.CamTickPos = 0 * Time.deltaTime;
    }
}
