using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableBox : MonoBehaviour
{
    //The box's current health point total
    public int currentHealth = 3;
    public AudioClip onHitSound;
    public AudioClip onKillSound;

    public GameObject killReward;

    public void Damage(int damageAmount)
    {        
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;

        //Check if health has fallen below zero
        if (currentHealth <= 0)
        {
            if (onKillSound != null)
            {
                AudioManeger.main.Play(onKillSound, this.transform.position);
            }
            //if health has fallen below zero, deactivate it 
            if (killReward != null)
            {
                Instantiate(killReward, transform.position + new Vector3(0, 1f), Quaternion.identity * Quaternion.Euler(new Vector3(-90,0)));
            }
            Destroy(gameObject);
        } else
        {
            if (onHitSound != null)
            {
                AudioManeger.main.Play(onHitSound, this.transform.position);
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
