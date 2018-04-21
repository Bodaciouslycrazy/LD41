using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    [SerializeField]
    private int MaxHealth = 5;
    [SerializeField]
    private int Health = 5;

	public virtual int Damage(int amt)
    {
        Health -= amt;

        if(Health <= 0)
        {
            Kill();
            return 0;
        }

        return Health;
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }
}
