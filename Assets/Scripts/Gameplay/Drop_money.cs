using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_money : MonoBehaviour
{
    [SerializeField] GameObject money_prefab;
    [SerializeField] int xx;
    void Start()
    {
        
    }    
   public void Spawn()
    {
        for(int i = 0; i < Random.Range(3, 5); i++)
        {
            GameObject obj = Instantiate(money_prefab, transform.position, money_prefab.transform.rotation) as GameObject;
            obj.transform.position = new Vector3(transform.position.x + (-3 + (i *2)), 0, transform.position.z + Random.Range(-4, 4));
            //obj.GetComponent<Money>().Drop();
        }
    }
}
