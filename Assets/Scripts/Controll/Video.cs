using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Video : MonoBehaviour
{
    [SerializeField] GameObject[] tank;
    [SerializeField] GameObject _cam, _pos, fire_pos;
    [SerializeField] float speed, time;
    void Start()
    {
        //StartCoroutine(T());
        StartCoroutine(Fire(5));
        StartCoroutine(Back());
    }
    //IEnumerator T()
    //{
    //    yield return new WaitForSeconds(2);
    //    tank[0].SetActive(false);
    //    tank[1].SetActive(true);
    //    yield return new WaitForSeconds(2);
    //    tank[1].SetActive(false);
    //    tank[2].SetActive(true);

    //}

    // Update is called once per frame
    void Update()
    {
        _cam.transform.RotateAround(_pos.transform.position, Vector3.up, speed * Time.deltaTime);

    }
    IEnumerator Fire(float time)
    {
        yield return new WaitForSeconds(time);
        fire_pos.SetActive(true);
        yield return new WaitForSeconds(1);
        fire_pos.SetActive(false);
        StartCoroutine(Fire(Random.RandomRange(5, 10)));
    }
    IEnumerator Back()
    {
        yield return new WaitForSeconds(time);
        speed = -speed;
        StartCoroutine(Back());
    }
}
