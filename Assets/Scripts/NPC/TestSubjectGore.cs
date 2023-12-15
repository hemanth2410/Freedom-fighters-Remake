using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestSubjectGore : MonoBehaviour
{
    [SerializeField] float m_ColliderHealth;
    [SerializeField] float m_Multiplier;
    [SerializeField] bool m_overrideExplode;
    [SerializeField] GameObject[] m_GoreObjects;
    [SerializeField] Transform m_TransformToScale;
    [SerializeField] GameObject[] m_GoreDecals;
    [SerializeField] bool m_RagdollOnHit;
    [SerializeField] string m_ForwardTrigger;
    [SerializeField] string m_BackwardTrigger;
    Transform parenTransform;
    TestSubjectHealth health;
    float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = m_ColliderHealth;
        health = GetComponentInParent<TestSubjectHealth>();
        WeaponsSingleton.Instance.TakeDamageEvent += takeDamage;
        parenTransform = GetComponentInParent<TestSubjectHealth>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void takeDamage(int ID, int damage, bool explode, Vector3 direction)
    {
        if(gameObject.GetInstanceID() == ID)
        {
            currentHealth -= damage;
            if (currentHealth <= 0 && explode && !m_overrideExplode)
            {
                health.TakeAbsoluteDamage(damage * m_Multiplier);
                m_TransformToScale.localScale = Vector3.zero;
                int rand = Random.Range(5, 10);
                for (int i = 0; i < rand; i++)
                {
                    var k = Instantiate(m_GoreObjects[Random.Range(0, m_GoreObjects.Length)], transform.position, Quaternion.identity);
                    k.GetComponent<Rigidbody>().AddExplosionForce(20.0f, transform.position, 1.0f);
                }
                for (int j = 0; j < m_GoreDecals.Length; j++)
                {
                    m_GoreDecals[j].SetActive(true);
                }
            }
            else if(!m_RagdollOnHit)
            {
                
                Vector3 _value = Vector3.Cross(parenTransform.forward, direction);
                health.TakeBulletDamage(damage * m_Multiplier,_value.y < 0.0f ? m_BackwardTrigger : m_ForwardTrigger);
            }
            else
            {
                // make this hooman ragdoll and let him recover to next animation
                health.MakeRagdoll();
            }
            if(!GetComponent<Rigidbody>().isKinematic)
                GetComponent<Rigidbody>().AddForce(direction * 10.0f, ForceMode.Impulse);
        }
    }
}
