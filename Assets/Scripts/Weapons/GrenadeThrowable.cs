using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrowable : Weapon
{
    [SerializeField] float m_fuseTimer;
    [SerializeField] float m_explosionRadius;
    [SerializeField] float m_maximumDamage;
    [SerializeField] float m_explosionForce;
    [SerializeField] GameObject m_ExplosionFX;
    [SerializeField] LayerMask m_LayerMask;
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
        Collider[] people = Physics.OverlapSphere(transform.position, m_explosionRadius);
        for (int i = 0; i < people.Length; i++)
        {
            if (people[i].GetComponent<TestSubjectHealth>())
            {
                people[i].GetComponent<Animator>().enabled = false;
                people[i].GetComponent<TestSubjectHealth>().MakeRagdoll();
                people[i].GetComponent<TestSubjectHealth>().AddExplosionForce(transform.position, m_explosionForce, m_explosionRadius);
            }
            if (people[i].GetComponent<Rigidbody>())
            {
                people[i].GetComponent<Rigidbody>().AddExplosionForce(m_explosionForce, transform.position, m_explosionRadius);
            }
        }
        Destroy(gameObject);
        // A public function is the only way i see
        // A reminder, decrement thr count as soon as you drop the grenade
        // use overlap sphere to deal damage and add ragdoll physics.
        // finally reduce the count in inventory.
    }
}
