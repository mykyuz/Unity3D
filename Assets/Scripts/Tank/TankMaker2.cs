﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankMaker2 : MonoBehaviour
{
    //private GameObject enemyPrefab;
    private GameObject tankPrefabs;
    private float maketime = 4f;
    private int currentCount = 0;
    public List<GameObject> listEnemy = new List<GameObject>();
    void Start()
    {
        tankPrefabs = Resources.Load<GameObject>("Tank/Tank2");
       
    }

    void Update()
    {
        if (currentCount < 4)
        {
            maketime -= Time.deltaTime;
            if (maketime < 0)
            {
                currentCount += 1;
                GameObject go = Instantiate(tankPrefabs, this.transform.position, Quaternion.identity) as GameObject;
                listEnemy.Add(go);
                maketime = 5f;
            }
        }
    }
}
    
