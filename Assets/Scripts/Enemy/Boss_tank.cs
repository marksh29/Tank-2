using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_tank : MonoBehaviour
{
    [SerializeField] float speed, fire_time;
    bool dead;
    GameObject player;
    [SerializeField] GameObject up;
    [SerializeField] GameObject[] fire_pos;
    void Start()
    {
        //fire_time = Player_stats.Instance.tank_fire_time;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (!dead && transform.position.z - player.transform.position.z < Player_stats.Instance.attack_distance)
        {
            speed = Player_controll.Instance.speed;
            transform.Translate(Vector3.forward * -speed * Time.deltaTime);

            Vector3 targetDirection = new Vector3(player.transform.position.x, up.transform.position.y, player.transform.position.z + 5) - gameObject.transform.position;
            float singleStep = Player_stats.Instance.up_speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(up.gameObject.transform.forward, targetDirection, singleStep, 0.0f);
            up.gameObject.transform.rotation = Quaternion.LookRotation(newDirection);

            fire_time -= Time.deltaTime;
            if (fire_time <= 0)
            {
                fire_time = Player_stats.Instance.tank_fire_time;
                StartCoroutine(Fire_on());
            }
        }
    }
    IEnumerator Fire_on()
    {
        fire_pos[0].SetActive(true);
        GameObject bull = PoolControll.Instance.Spawn_enemy_bullet();
        bull.transform.position = fire_pos[0].transform.position;
        bull.transform.rotation = fire_pos[0].transform.rotation;
        yield return new WaitForSeconds(0.2f);
        fire_pos[0].SetActive(false);
        fire_pos[1].SetActive(true);
        GameObject bull1 = PoolControll.Instance.Spawn_enemy_bullet();
        bull1.transform.position = fire_pos[1].transform.position;
        bull1.transform.rotation = fire_pos[1].transform.rotation;
        yield return new WaitForSeconds(0.2f);
        fire_pos[1].SetActive(false);
    }
    public void Dead()
    {
        StopAllCoroutines();
        dead = true;
        GetComponent<BoxCollider>().enabled = false;
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<Enemy>() != null)
            coll.gameObject.GetComponent<Enemy>().Damage(100);
        else
        {
            if (coll.gameObject.tag != "Ground" && coll.gameObject.tag != "Untagged")
            {
                Destroy(coll.gameObject);
            }
        }        
    }
}
