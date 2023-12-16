using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSubjectHealth : MonoBehaviour
{
    [SerializeField] float m_MaxHealth;
    [SerializeField] float m_AnimCoolDown;
    float timer;
    [SerializeField] float health;
    [SerializeField] Transform m_HipTransform;
    [SerializeField] Transform m_RootTransform;
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
    public void TakeBulletDamage(float damage, string triggerName)
    {
        health -= damage;
        if (health <= 0.0f)
        {
            foreach (var b in ragdollBodies)
            {
                b.isKinematic = false;
            }
            animator.enabled = false;
            // Now add force.
        }
        else
        {
            animator.SetTrigger(triggerName);
        }

    }
    public void TakeAbsoluteDamage(float damage)
    {
        health -= damage;
        if (health <= 0.0f)
        {
            foreach (var b in ragdollBodies)
            {
                b.isKinematic = false;
            }
            animator.enabled = false;
            // Now add force.
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

    public void AddExplosionForce(Vector3 position, float force, float blastRadius)
    {
        foreach (var b in ragdollBodies)
        {
            b.AddExplosionForce(force, position, blastRadius);
        }
    }

    public void MakeRagdoll()
    {
        if (health <= 0.0f)
            return;
        foreach (var b in ragdollBodies)
        {
            b.isKinematic = false;
        }
        animator.enabled = false;
        // start an Ienumurator to blend between ragdoll and animation
        //GetComponent<CharacterController>().enabled = false;
        StartCoroutine(TransitionFromRagdoll(3.0f));
        
    }

    IEnumerator TransitionFromRagdoll(float time)
    {
        float _t = 0;
        while(_t < time)
        {
            _t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        // get hip bone front and compute the cross with World UP
        // if cross.y > 0 then we raise from front
        // else we raise from back
        transform.position = m_HipTransform.position;
        // need to add ground plane detection
        m_HipTransform.transform.localPosition = new Vector3(0, 0, 0);
        Vector3 _cross = Vector3.Cross(m_HipTransform.forward, Vector3.up);
        foreach (var b in ragdollBodies)
        {
            b.isKinematic = true;
        }
        animator.enabled = true;
        animator.SetTrigger(_cross.y > 0.0f ? "Raise_Back" : "Raise_Front");
        yield return null;
    }
}
