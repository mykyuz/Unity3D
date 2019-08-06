using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
namespace Complete
{
    public class TankMaker : MonoBehaviour
    {
        private GameObject tankPrefabs;
        private float maketime = 4f;
        private int currentCount = 0;
        public List<GameObject> listEnemy = new List<GameObject>();
        public GameObject next;
        private int i = 0;
        void Start()
        {
            tankPrefabs = Resources.Load<GameObject>("Tank/Tank2");
        }

        void Update()
        {
            if (currentCount < 5)
            {
                maketime -= Time.deltaTime;
                if (maketime < 0)
                {
                    currentCount += 1;
                    GameObject go = Instantiate(tankPrefabs, this.transform.position, Quaternion.identity) as GameObject;
                    listEnemy.Add(go);
                    maketime = 4f;
                }

            }
            if (i == 5)
            {
                next.SetActive(true);
                
            }
        }
        public void MinusTank()
        {
            i += 1;
        }
    }
}
    
