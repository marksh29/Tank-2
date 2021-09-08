using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    [SerializeField] GameObject expl_prefab, player, body;
    [SerializeField] float speed;
    bool dead;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }    
    void Update()
    {
        if (!dead)
        {
            Vector3 targetDirection = player.transform.position - gameObject.transform.position;
            float singleStep = Player_stats.Instance.up_speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetDirection, singleStep, 0.0f);
            gameObject.transform.rotation = Quaternion.LookRotation(newDirection);

            if (transform.position.z - player.transform.position.z < Player_stats.Instance.enemy_distance)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }        
    }
    public void Dead()
    {
        dead = true;
        GetComponent<CapsuleCollider>().enabled = false;
        expl_prefab.SetActive(true);
        body.SetActive(false);        
    }  
    IEnumerator Off()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player_hp.Instance.Damage(0.2f);
            Dead();
        }
    }
}