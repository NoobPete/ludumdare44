using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableBox : MonoBehaviour
{
    //The box's current health point total
    public int currentHealth = 3;
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    public void Damage(int damageAmount)
    {        
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;

        //Check if health has fallen below zero
        if (currentHealth <= 0)
        {
            if (audioSource2 != null)
            {
                audioSource2.Play();
            }
            //if health has fallen below zero, deactivate it 
            Destroy(gameObject);
        } else
        {
            if (audioSource1 != null)
            {
                audioSource1.Play();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
