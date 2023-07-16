using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SupplyBase : MonoBehaviour
{
    [SerializeField] float Gravity;
    bool isDown = true;
    [SerializeField] AudioClip PickSound;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(0,1,0),new Vector3(2.3f,2.3f,2.3f));
    }
    void Update()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(0,1,0), new Vector3(2.3f, 2.3f, 2.3f));
        foreach (var i in colliders)
        {
            if (i.CompareTag("Player"))
            {
                Buff(i);
                SoundManager.instance.SetAudio(PickSound, SoundManager.SoundState.SFX, false);
                UIManager.instance.InitAmmoUI();
                Destroy(gameObject);
            }
        }
    }
    public abstract void Buff(Collider other);
}
