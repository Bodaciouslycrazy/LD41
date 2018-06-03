using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

	public enum ColorEnum
	{
		BLUE,
		GREEN,
		RED
	}


	[Header("Damageable Vars")]
    [SerializeField]
    protected int MaxHealth = 5;
    [SerializeField]
    protected int Health = 5;

    [SerializeField]
    private ColorEnum Color = ColorEnum.GREEN;

    public virtual ColorEnum GetColor()
    {
        return Color;
    }

    public virtual void SetColor(ColorEnum col)
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

	public virtual int Damage(int amt, ColorEnum damColor)
    {
        if (damColor != Color)
            return Health;

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
