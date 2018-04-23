using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable {

	// Use this for initialization
	public void Start () {
        EnemySpawner.AddEnemy(this);
	}


    public override void Kill()
    {
        EnemySpawner.RemoveEnemy(this);
        base.Kill();
    }
}
