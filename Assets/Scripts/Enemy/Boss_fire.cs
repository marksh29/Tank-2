using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_fire : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject player, fire_position;
    [SerializeField] int fire_id;
    [SerializeField] float[] min_max_pos, fire_pso;
    [SerializeField] float bullet_start_pos;
    public bool stay, dead, fire, end;
    [SerializeField] GameObject[] emojy;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");       
    }
    void Start()
    {
        Change_move();
    }
    void Update()
    {
        if (!dead)
        {          
            Vector3 targetDirection = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 5) - gameObject.transform.position;
            float singleStep = Player_stats.Instance.up_speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetDirection, singleStep, 0.0f);
            gameObject.transform.rotation = Quaternion.LookRotation(newDirection);         
        }
    }   
    void Change_move()
    {
        anim.SetTrigger("move");
        float pos = Random.Range(min_max_pos[0], min_max_pos[min_max_pos.Length - 1]);
        float time = Mathf.Abs(transform.position.x - pos) / 3;
        StartCoroutine(DoMove(time / Player_stats.Instance.enemy_change_speed, pos));
    }
    private IEnumerator DoMove(float time, float xx)
    {       
        Vector2 startPosition = transform.position;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            transform.position = Vector3.Lerp(new Vector3(startPosition.x, transform.position.y, transform.position.z), new Vector3(xx, transform.position.y, transform.position.z), fraction);
            yield return null;
        }
        switch(fire_id)
        {
            case (0):
                StartCoroutine(Fire_on(6));
                fire_id++;
                break;
            case (1):
                StartCoroutine(Fire_two(6));
                fire_id++;
                break;
            case (2):
                StartCoroutine(Fire_thre(6));
                fire_id = 0;
                break;
        }        
        anim.SetTrigger("fire");
    }
    IEnumerator Fire_on(int fire_count)
    {        
        for(int i = 0; i < fire_count; i++)
        {
            GameObject bull = PoolControll.Instance.Spawn_enemy_bullet();
            bull.transform.position = new Vector3(transform.position.x, transform.position.y + bullet_start_pos, transform.position.z);
            bull.transform.rotation = fire_position.transform.rotation;
            yield return new WaitForSeconds(0.3f);
        }        
        Change_move();        
    }
    IEnumerator Fire_two(int fire_count)
    {
        List<float> list = new List<float>(fire_pso);
        int p1 = Random.Range(0, list.Count);
        list.RemoveAt(p1);
        int p2 = Random.Range(0, list.Count);

        for (int i = 0; i < fire_count; i++)
        {
            GameObject bull = PoolControll.Instance.Spawn_enemy_bullet();
            bull.transform.position = new Vector3(transform.position.x, transform.position.y + bullet_start_pos, transform.position.z);
            bull.transform.rotation = fire_position.transform.rotation;
            bull.GetComponent<Enemy_bullet>().target = new Vector3(fire_pso[p1], 0, player.transform.position.z - 2);
            
            GameObject bull1 = PoolControll.Instance.Spawn_enemy_bullet();
            bull1.transform.position = new Vector3(transform.position.x, transform.position.y + bullet_start_pos, transform.position.z);
            bull1.transform.rotation = fire_position.transform.rotation;
            bull1.GetComponent<Enemy_bullet>().target = new Vector3(fire_pso[p2], 0, player.transform.position.z - 2);
            yield return new WaitForSeconds(0.3f);
        }

        Change_move();
    }
    IEnumerator Fire_thre(int fire_count)
    {
        for (int i = 0; i < fire_count; i++)
        {
            GameObject bull = PoolControll.Instance.Spawn_enemy_bullet();
            bull.transform.position = new Vector3(transform.position.x, transform.position.y + bullet_start_pos, transform.position.z);
            bull.transform.rotation = fire_position.transform.rotation;
            yield return new WaitForSeconds(1f);
        }
        Change_move();
    }

    public void Dead()
    {
        StopAllCoroutines();        
        dead = true;
        anim.SetTrigger("dead");
        Destroy(gameObject.GetComponent<CapsuleCollider>());
    }
}
