using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour, IOnLoadAndSave
{
    private Enemy enemy;
    [field: SerializeField] private Enemy prefab;
    [SerializeField] private float respawnTime;
    [SerializeField] private int minLvl;
    [SerializeField] private int maxLvl;
    [SerializeField] private bool isBoss;
    private float respawning;
    private bool isSpawned;
    private bool playerInRange;
    [field: HideInInspector] public bool IsDead = false;

    private void Start()
    {
        if(enemy == null)
        {
            if (isBoss && IsDead)
            {

            }
            else if(this.gameObject.GetComponent<CircleCollider2D>().bounds.Contains(GameObject.Find("Player").transform.position))
            {
                enemy = Instantiate(prefab, this.gameObject.transform);
                enemy.GiveLevelBetweenRange(minLvl, maxLvl);
                isSpawned = true;
            }
        }

        GoalEventHandler.OnEnemyDeath += CheckForBossDeath;
        respawning = respawnTime;
    }

    private void Update()
    {
        if (enemy == null)
        {
            if (!isBoss)
            {
                if(!IsDead)
                {
                    if (playerInRange)
                    {
                        if (!isSpawned)
                        {
                            Transform transform = this.gameObject.transform;
                            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

                            enemy = Instantiate(prefab, transform);
                            enemy.GiveLevelBetweenRange(minLvl, maxLvl);
                            isSpawned = true;
                        }
                        else
                        {
                            IsDead = true;
                        }

                    }     
                }
                else
                {
                    IsDead = true;
                    respawning -= Time.deltaTime;
                    if (respawning < 0 && enemy == null && playerInRange)
                    {
                        enemy = Instantiate(prefab, this.gameObject.transform);
                        enemy.GiveLevelBetweenRange(minLvl, maxLvl);
                        IsDead = false;
                        respawning = respawnTime;
                    }
                }
            }
            else
            {
                if (!IsDead && playerInRange)
                {
                    enemy = Instantiate(prefab, this.gameObject.transform);
                    enemy.GiveLevelBetweenRange(minLvl, maxLvl);
                    isSpawned = true;
                }
            }
        }
        else
        {
            if (!playerInRange)
            {
                isSpawned = false;
                Destroy(enemy.gameObject);
            }
        }
    }

    public void CheckForBossDeath(EnemyID _enemyId)
    {
        if(_enemyId == prefab.EnemyID && isBoss)
        {
            IsDead = true;
        }
    }

    public void Save()
    {

    }

    public void Load()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == GameObject.Find("Player").GetComponent<CircleCollider2D>())
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == GameObject.Find("Player").GetComponent<CircleCollider2D>())
        {
            playerInRange = false;
        }
    }

    public void OnEnable()
    {
        if (!DataControl.control.Spawners.Contains(this))
        DataControl.control.Spawners.Add(this);
        DataControl.control.OnLoad += Load;
        DataControl.control.OnSafe += Save;
    }

    public void OnDisable()
    {
        if (DataControl.control.Spawners.Contains(this))
        DataControl.control.Spawners.Remove(this);
        DataControl.control.OnLoad -= Load;
        DataControl.control.OnSafe -= Save;
    }
}
