using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_stat : MonoBehaviour
{
    public static Enemy_stat Instance;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        
    }
}
