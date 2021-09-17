using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] float speed, force;
    [SerializeField] GameObject move_pos;
    bool move;

    void Start()
    {
        move_pos = GameObject.FindGameObjectWithTag("MoneyPos");
        //Drop();
        speed = Random.Range(50f, 200f);
    }
    private void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, move_pos.transform.position, force * Time.deltaTime);
            if (transform.position == move_pos.transform.position)
            {
                gameObject.SetActive(false);
                Game_Controll.Instance.Add_money(1);
            }
        }
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        if (coll.gameObject.tag == "Player")
        {
            move = true;
            //GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            //StartCoroutine(DoMove(1, move_pos.transform.position));
        }
    }
    private IEnumerator DoMove(float time, Vector3 pos)
    {
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            transform.position = Vector3.Lerp(transform.position, pos, fraction);
            yield return null;
        }
        gameObject.SetActive(false);
        Game_Controll.Instance.Add_money(1);
    }
}
