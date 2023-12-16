using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeThrowable : Weapon
{
    [SerializeField] float m_fuseTimer;
    [SerializeField] GameObject m_ExplosionFX;
    // Start is called before the first frame update
    protected override void Start()
    {
        // yeah lets just create a new grenade and we call it in start to avoid unnecessary stuff, once its the last grenade we simply distroy the inventory item
        base.Start();
        if (!weaponReady)
            return;
        StartCoroutine(explode());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!weaponReady)
            return;
    }

    IEnumerator explode()
    {
        yield return new WaitForSecondsRealtime(m_fuseTimer);
        Instantiate(m_ExplosionFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
        // A public function is the only way i see
        // A reminder, decrement thr count as soon as you drop the grenade
        // use overlap sphere to deal damage and add ragdoll physics.
        // finally reduce the count in inventory.
        // Only commmit Audio configration
    }
}
