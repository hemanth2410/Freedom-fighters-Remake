using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSubjectHealth : MonoBehaviour
{
    [SerializeField] float m_MaxHealth;
    [SerializeField] float m_AnimCoolDown;
    float timer;
    [SerializeField] float health;
    bool doAnimation;
    Animator animator;
    Rigidbody[] ragdollBodies;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var b in ragdollBodies)
        {
            b.isKinematic = true;
        }
        doAnimation = true;
        health = m_MaxHealth;
    }
    private void Update()
    {
        if (doAnimation)
            return;
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
            doAnimation = true;
    }

    public void SetAnimation(string  name)
    {
        if(doAnimation)
        {
            animator.SetTrigger(name);
            timer = m_AnimCoolDown;
            doAnimation = false;
        }
        
    }
    public void TakeDamage(float damage)
    {
        health -= damage * Time.deltaTime;
        if(health <= 0.0f)
        {
            foreach (var b in ragdollBodies)
            {
                b.isKinematic = false;
            }
            animator.enabled = false;
            // Now add force.
        }
    }
}
