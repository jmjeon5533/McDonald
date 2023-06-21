using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemS : MonoBehaviour
{
    public enum WeaponType
    {
        Hamberger,
        Cola,
        French_fries
    }
    public WeaponType weaponType;
    [SerializeField] GameObject WeaponObj;
    [SerializeField] GameObject WeaponUI;
    [SerializeField] GameObject GetParticle;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var o = other.GetComponent<FirstPersonController>();
            o.Weapon.Add(weaponType);
            var Obj = Instantiate(WeaponObj,o.WeaponArm);
            SpawnManager.instance.player.WeaponObj.Add(Obj);
            var ui = Instantiate(WeaponUI,UIManager.instance.WeaponPanel).transform;
            UIManager.instance.WeaponUI.Add(ui);
            //Instantiate(GetParticle,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
