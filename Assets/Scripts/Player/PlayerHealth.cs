using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float m_maxHealth;
    float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        #region TODO
        // Just like in the project senses
        // I want to decouple Logic from UI
        // So we use Scriptable objects
        // Shared Health variable for player.
        // On start we initialize value on
        // shared variable with m_maxHealth of
        // If shaved sata exists then we will read from it.
        #endregion
        currentHealth = m_maxHealth;
    }
    /// <summary>
    /// This method reduces health by certain damage amount.
    /// Ideally we want this method to be called from child colliders of actual player object.
    /// </summary>
    /// <param name="damage">amount of damage to be inflicted</param>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0.0f)
        {
            // A perfect place to kill player.
        }
    }
}
