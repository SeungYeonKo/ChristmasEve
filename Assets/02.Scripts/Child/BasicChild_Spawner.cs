using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicChild_Spawner : MonoBehaviour
{
    public GameObject Basic_ChildPrefab;
    public GameObject Sword_ChildPrefab;
    public GameObject Hammer_ChildPrefab;

    public float MinSpawnInterval = 4f;
    public float MaxSpawnInterval = 8f;
    public float SpawnTimer = 0f;
    public float SpawnInterval = 0f;

    public int PoolSize = 20;
    public List<Basic_Child> ChildPool;

    private void Awake()
    {
        ChildPool = new List<Basic_Child>();
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject basicChild = Instantiate(Basic_ChildPrefab);
            basicChild.SetActive(false);
            ChildPool.Add(basicChild.GetComponent<Basic_Child>());
        }
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject basicChild = Instantiate(Sword_ChildPrefab);
            basicChild.SetActive(false);
            ChildPool.Add(basicChild.GetComponent<Basic_Child>());
        }
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject basicChild = Instantiate(Hammer_ChildPrefab);
            basicChild.SetActive(false);
            ChildPool.Add(basicChild.GetComponent<Basic_Child>());
        }
    }
    void Start()
    {
        SetRandomSpawnInterval();
    }
    void Update()
    {
        SpawnTimer -= Time.deltaTime;

        if (SpawnTimer <= 0f)
        {
            Basic_Child child = null;
            int randomNumber = Random.Range(0, 3);
            if(randomNumber == 0)
            {
                foreach(Basic_Child c in ChildPool)
                {
                    if ( !c.gameObject.activeInHierarchy && c.CType == ChildType.Basic)
                    {
                        child = c; break;
                    }
                }
            }
            else if (randomNumber == 1)
            {
                foreach (Basic_Child c in ChildPool)
                {
                    if (!c.gameObject.activeInHierarchy && c.CType == ChildType.Sword)
                    {
                        child = c; break;
                    }
                }
            }
            if (randomNumber == 2)
            {
                foreach (Basic_Child c in ChildPool)
                {
                    if (!c.gameObject.activeInHierarchy && c.CType == ChildType.Hammer)
                    {
                        child = c; break;
                    }
                }
            }

            //SpawnChild();
            child.transform.position = this.transform.position;

            child.gameObject.SetActive(true);
            SetRandomSpawnInterval();
            SpawnTimer = SpawnInterval;
        }
    }

    /*private void SpawnChild()
    {
        Instantiate(ChildPrefab, transform.position, transform.rotation);
    }*/
    private void SetRandomSpawnInterval()
    {
        SpawnInterval = Random.Range(MinSpawnInterval, MaxSpawnInterval);
    }
}
