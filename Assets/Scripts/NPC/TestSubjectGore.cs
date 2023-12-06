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
    TestSubjectHealth health;
    float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = m_ColliderHealth;
        health = GetComponentInParent<TestSubjectHealth>();
        WeaponsSingleton.Instance.TakeDamageEvent += takeDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void takeDamage(int ID, int damage, bool explode)
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
            else
            {
                health.TakeBulletDamage(damage * m_Multiplier);
                //GetComponent<Rigidbody>().AddForce(direction * 30.0f, ForceMode.Impulse);
            }
        }
    }
    //public void TakeDamage(int damage, bool explode)
    //{
    //    currentHealth -= damage;
    //    if(currentHealth <= 0 && explode && !m_overrideExplode)
    //    {
    //        health.TakeAbsoluteDamage(damage * m_Multiplier);
    //        m_TransformToScale.localScale = Vector3.zero;
    //        int rand = Random.Range(5, 10);
    //        for (int i = 0; i < rand; i++)
    //        {
    //            var k = Instantiate(m_GoreObjects[Random.Range(0, m_GoreObjects.Length)], transform.position, Quaternion.identity);
    //            k.GetComponent<Rigidbody>().AddExplosionForce(20.0f, transform.position, 1.0f);
    //        }
    //        for (int j = 0; j < m_GoreDecals.Length; j++)
    //        {
    //            m_GoreDecals[j].SetActive(true);
    //        }
    //    }
    //    else
    //    {
    //        health.TakeBulletDamage(damage * m_Multiplier);
    //        //GetComponent<Rigidbody>().AddForce(direction * 30.0f, ForceMode.Impulse);
    //    }
    //}
}
