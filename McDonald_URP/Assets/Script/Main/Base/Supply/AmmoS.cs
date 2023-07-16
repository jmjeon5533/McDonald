using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoS : SupplyBase
{
    public override void Buff(Collider other)
    {
        var player = other.GetComponent<FirstPersonController>();
        for(int i = 0; i < player.WeaponObj.Count; i++)
        {
            var w = player.WeaponObj[i].GetComponent<WeaponBase>();
            w.megazine += w.maxAmmo * 2;
        }
    }
}
