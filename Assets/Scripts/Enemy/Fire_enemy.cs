using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_enemy : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject player, fire_position;
    [SerializeField] int cur_pos;
    [SerializeField] float[] min_max_pos;
    [SerializeField] float speed, fire_timer, move_timer, bullet_start_pos;
    public bool stay, dead, fire, end, move;
    [SerializeField] GameObject[] emojy;
    [SerializeField] List<GameObject> list;
    Vector3 move_target;

    private void OnEnable()
    {
        anim.SetTrigger("fire");
        player = GameObject.FindGameObjectWithTag("Player");       
    }
    void Start()
    {
               
    }
    void Update()
    {
        if (!end && !dead && transform.position.z - player.transform.position.z < Player_stats.Instance.enemy_distance)
        {
            if(!stay)
            {
                if(move)
                {
                    Vector3 targetDirection = move_target - transform.position;
                    float singleStep = Player_stats.Instance.up_speed * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetDirection, singleStep, 0.0f);
                    gameObject.transform.rotation = Quaternion.LookRotation(newDirection);

                    transform.position = Vector3.MoveTowards(transform.position, move_target, speed * Time.deltaTime);
                    if (transform.position == move_target)
                    {
                        move_timer = Random.Range(2f, 5f);
                        anim.SetTrigger("fire");
                        move = false;
                    }
                }
                else
                {
                    move_timer -= Time.deltaTime;
                    if (move_timer <= 0)
                    {
                        Change_move();
                        anim.SetTrigger("move");
                    }                       
                   
                    fire_timer -= Time.deltaTime;
                    if (fire_timer <= 0 && (transform.position.z - player.transform.position.z < Player_stats.Instance.attack_distance))
                    {
                        fire_timer = Player_stats.Instance.enemy_fire_time;
                        StopAllCoroutines();
                        StartCoroutine(Fire_on(0.5f, false));
                    }
                }                              
            }
            else
            {
                    Vector3 targetDirection = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z + 5) - gameObject.transform.position;
                    float singleStep = Player_stats.Instance.up_speed * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetDirection, singleStep, 0.0f);
                    gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
            }

            if(transform.position.z <= player.transform.position.z)
            {                
                Player_controll.Instance.Cleare_enemy();                
                StopAllCoroutines();
                StartCoroutine(Fire_on(0.2f, true));
                end = true;
            }
        }
        else
        {
            if (!dead)
            {
                Vector3 targetDirection = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z + 5) - gameObject.transform.position;
                float singleStep = Player_stats.Instance.up_speed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetDirection, singleStep, 0.0f);
                gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
            }           
        }
    }  
    void Change_move()
    {
        anim.SetTrigger("move");
        Vector3 pos = new Vector3(Random.Range(min_max_pos[0], min_max_pos[min_max_pos.Length - 1]), 0, transform.position.z + Random.Range(3, 13));
        move_target = pos;
        move = true;        
    }     
    //private IEnumerator DoMove(float time, Vector3 pos)
    //{
    //    //Vector2 startPosition = transform.position;
    //    float startTime = Time.realtimeSinceStartup;
    //    float fraction = 0f;
    //    while (fraction < 1f)
    //    {
    //        fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
    //        transform.position = Vector3.Lerp(transform.position, pos, fraction);
    //        yield return null;
    //    }
    //    Full_stop();
    //}
    IEnumerator Fire_on(float time, bool end_bullet)
    {
        anim.SetTrigger("fire");
        stay = true;
        yield return new WaitForSeconds(time);
        GameObject bull = PoolControll.Instance.Spawn_enemy_bullet();
        bull.transform.position = new Vector3(transform.position.x, transform.position.y + bullet_start_pos , transform.position.z);
        bull.transform.rotation = fire_position.transform.rotation;
        if (end)
            bull.GetComponent<Enemy_bullet>().New_target();
        list.Add(bull);
        yield return new WaitForSeconds(1);
        fire_timer = Player_stats.Instance.enemy_fire_time;       
        stay = false;
        Change_move();
        //anim.SetTrigger("move");
    }


    public void Dead()
    {
        GetComponent<Drop_money>().Spawn();
        dead = true;
        //for (int i = 0; i < list.Count; i++)
        //{
        //    list[i].SetActive(false);
        //}
        StopAllCoroutines();       
        anim.SetTrigger("dead");
        Destroy(gameObject.GetComponent<CapsuleCollider>());
        Emojy();
    }
    public void Emojy()
    {
        GameObject obj = Instantiate(emojy[Random.Range(0, emojy.Length)], player.transform.parent) as GameObject;
        obj.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z + 5);
        Destroy(obj, 2);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player" && other.gameObject.tag != "Respawn")
        {
            print("stop");
            if(other.transform.position.x > transform.position.x)
            {
                move_target = new Vector3((other.transform.position.x > -9 ? other.transform.position.x - 4 : -13), move_target.y, move_target.z);
            }
            else
            {
                move_target = new Vector3((other.transform.position.x < 9 ? other.transform.position.x + 4 : 13), move_target.y, move_target.z);
            }
            //Full_stop();
        }
    }
    void Full_stop()
    {
        move = false;
        anim.SetTrigger("fire");
        //Vector3 targetDirection = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z + 5) - gameObject.transform.position;
        //float singleStep = Player_stats.Instance.up_speed * Time.deltaTime;
        //Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetDirection, singleStep, 0.0f);
        //gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
