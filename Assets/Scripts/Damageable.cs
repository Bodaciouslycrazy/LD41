using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    public const int BLUE = 0;
    public const int GREEN = 1;
    public const int RED = 2;

    [SerializeField]
    protected int MaxHealth = 5;
    [SerializeField]
    protected int Health = 5;

    [SerializeField]
    private int Color = GREEN;

    public virtual int GetColor()
    {
        return Color;
    }

    public virtual void SetColor(int col)
    {
        Color = col;
    }

    public virtual int GetHealth()
    {
        return Health;
    }

    public virtual int GetMaxHealth()
    {
        return MaxHealth;
    }

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
