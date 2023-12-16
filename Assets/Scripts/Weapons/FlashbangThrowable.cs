using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashbangThrowable : Weapon
{
    [SerializeField] float m_fuseTimer;
    [SerializeField] float m_influenceRadius;
    [SerializeField] GameObject m_ExplosionFX;
    CinemachineImpulseSource impulseSource;
    // Start is called before the first frame update
    protected override void Start()
    {
        // yeah lets just create a new grenade and we call it in start to avoid unnecessary stuff, once its the last grenade we simply distroy the inventory item
        base.Start();
        if (!weaponReady)
            return;
        StartCoroutine(explode());
        impulseSource = GetComponent<CinemachineImpulseSource>();
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
        impulseSource.GenerateImpulse();
        Collider[] people = Physics.OverlapSphere(transform.position, m_influenceRadius);
        for (int i = 0; i < people.Length; i++)
        {
            // if npc play agony animation
            // if player invoke event on player 
            // best invoke an event using weapons singleton ?
            // pefectplace to make use of interface.
            if (people[i].GetComponent<IFlashbangObsorver>() != null)
            {
                people[i].GetComponent<IFlashbangObsorver>().ReactToFlashbang(transform.position);
            }
        }
        Destroy(gameObject);
        // A public function is the only way i see
        // A reminder, decrement thr count as soon as you drop the grenade
        // use overlap sphere to deal damage and add ragdoll physics.
        // finally reduce the count in inventory.
        // Only commmit Audio configration
    }
}
