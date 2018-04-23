using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    private static List<Enemy> Enemies;

    public GameObject RedShipPref;
    public GameObject GreenShipPref;
    public GameObject BlueShipPref;
    public GameObject SmallShipPref;
    public GameObject BossShip;

    [SerializeField]
    private int StartPhase = 0;

    private static int Phase = 0;
    private static float PhaseTime = 0;
    private static int Spawned = 0;

    // Use this for initialization
    private void Awake()
    {
        Enemies = new List<Enemy>();
        Phase = StartPhase;
    }

    void Start () {
        //Enemies = new List<Enemy>();

	}
	
	// Update is called once per frame
	void Update () {

        PhaseTime += Time.deltaTime;

        if(Phase == 0)
        {
            //Placeholders?

            NextPhase();
        }
        else if(Phase == 1) //One small ship
        {
            if(Spawned > 0 && Enemies.Count == 0)
            {
                NextPhase();
            }
            else if (Spawned == 0)
            {
                SpawnEnemy(SmallShipPref, new Vector2(0, 3.5f));
            }
        }
        else if(Phase == 2) //One green ship
        {
            if(Enemies.Count < 1 && Spawned == 1)
            {
                NextPhase();
            }
            else if (Spawned == 0)
            {
                SpawnEnemy(GreenShipPref, new Vector2(0, 3.5f));
            }

        }
        else if(Phase == 3)// blue ship and small ship
        {
            if(Spawned == 2 && Enemies.Count == 0)
            {
                NextPhase();
            }
            else if(Spawned == 0)
            {
                SpawnEnemy(BlueShipPref, new Vector2(-2.5f, 3f));
            }
            else if(Spawned == 1 && PhaseTime > .8f)
            {
                SpawnEnemy(SmallShipPref, new Vector2(2, 3.5f));
            }
        }
        else if(Phase == 4)//Two red ships
        {
            if(Spawned == 2 && Enemies.Count  == 0)
            {
                NextPhase();
            }
            else
            {
                if(Spawned == 0)
                {
                    SpawnEnemy(RedShipPref, new Vector2(2, 3.5f));
                }
                else if(PhaseTime > 1f && Spawned < 2)
                {
                    SpawnEnemy(RedShipPref, new Vector2(-2, 3.5f));
                }
            }
        }
        else if(Phase == 5)// Green ship and two small ships
        {
            if (Spawned > 1 && Enemies.Count == 0)
                NextPhase();
            else if(Spawned == 0)
            {
                //Spawn two small ships on left and right
                SpawnEnemy(SmallShipPref, new Vector2(3f, 3.5f));
                SpawnEnemy(SmallShipPref, new Vector2(-3f, 3.5f));
            }
            else if(Spawned == 2 && PhaseTime > 1.5f)
            {
                SpawnEnemy(GreenShipPref, new Vector2(0, 4f));
            }
        }
        else if(Phase == 6)//Two blue ships, one small
        {
            if(Spawned == 3 && Enemies.Count == 0)
            {
                NextPhase();
            }
            else if(Spawned == 0)
            {
                SpawnEnemy(BlueShipPref, new Vector2(2, 2.5f));
                SpawnEnemy(BlueShipPref, new Vector2(-2, 2.5f));
                SpawnEnemy(SmallShipPref, new Vector2(0, 3.5f));
            }
        }
        else if(Phase == 7)//Four small ships
        {
            if(Spawned == 4 && Enemies.Count == 0)
            {
                NextPhase();
            }
            else if(Spawned == 0)
            {
                SpawnEnemy(SmallShipPref, new Vector2(-1, 3.5f));
                SpawnEnemy(SmallShipPref, new Vector2(2, 3f));
            }
            else if(Spawned == 2 && PhaseTime > .5f)
            {
                SpawnEnemy(SmallShipPref, new Vector2(-2, 3f));
                SpawnEnemy(SmallShipPref, new Vector2(1, 3.5f));
            }
        }
        else if(Phase == 8)
        {
            if (Spawned == 1 && Enemies.Count == 0)
                NextPhase();
            else if(Spawned == 0)
            {
                SpawnEnemy(BossShip, new Vector2(0, 3.5f));
            }
        }
	}

    private void SpawnEnemy(GameObject enemy, Vector2 target)
    {
        Vector2 startPos = new Vector2(target.x, 8);

        GameObject obj = Instantiate(enemy, startPos, Quaternion.identity);
        obj.GetComponent<IPositionable>().SetTarget(target);
    }

    private void NextPhase()
    {
        Phase += 1;
        Spawned = 0;
        PhaseTime = 0f;
    }

    public static void AddEnemy( Enemy e)
    {
        Spawned++;
        Enemies.Add(e);
    }

    public static void RemoveEnemy(Enemy e)
    {
        Enemies.Remove(e);
    }


}
