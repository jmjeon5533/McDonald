using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    public enum WeaponType
    {
        Hamberger,
        Cola,
        French_fries
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * 25f * Time.deltaTime);
    }

    public WeaponType weaponType;
    [SerializeField] GameObject WeaponObj;
    [SerializeField] GameObject WeaponUI;
    [SerializeField] AudioClip PickSound;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var o = other.GetComponent<FirstPersonController>();
            o.Weapon.Add(weaponType);
            var Obj = Instantiate(WeaponObj,other.transform.position,Quaternion.identity,o.WeaponArm);
            SpawnManager.instance.player.WeaponObj.Add(Obj);
            var ui = Instantiate(WeaponUI,UIManager.instance.WeaponPanel).transform;
            UIManager.instance.WeaponUI.Add(ui);
            o.ActiveWeapon(UIManager.instance.WeaponUI.Count - 1);
            SoundManager.instance.SetAudio(PickSound,SoundManager.SoundState.SFX, false);
            Destroy(gameObject);
        }
    }
}
