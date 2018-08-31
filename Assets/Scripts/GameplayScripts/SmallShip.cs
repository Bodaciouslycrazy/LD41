using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletSpawner))]
public class SmallShip : CommonEnemy
{
	static int RandInit = 0;

	public override void Start()
	{
		//Random.InitState((int)System.DateTime.UtcNow.Ticks + RandInit);
		Random.InitState(RandInit);
		SetColor((ColorEnum)Random.Range(0, 3));
		RandInit = Random.Range(int.MinValue, int.MaxValue);

		base.Start();
	}

	public override void SetColor(ColorEnum col)
    {
        Color spriteCol = new Color(0,0,0);

        switch(col)
        {
            case ColorEnum.BLUE:
                spriteCol.b = .85f;
                break;
            case ColorEnum.GREEN:
                spriteCol.g = .85f;
                break;
            case ColorEnum.RED:
                spriteCol.r = .85f;
                break;
        }

        transform.GetChild(0).GetComponent<SpriteRenderer>().color = spriteCol;

        base.SetColor(col);
    }
}
