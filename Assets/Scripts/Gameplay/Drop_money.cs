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
   public void Spawn(int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(money_prefab, transform.position, money_prefab.transform.rotation) as GameObject;
            obj.transform.position = new Vector3(transform.position.x + Random.Range(-xx, xx), 0, transform.position.z + Random.Range(-xx, xx));
            //obj.GetComponent<Money>().Drop();
        }
    }
}
