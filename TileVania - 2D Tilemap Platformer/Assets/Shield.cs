using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    [SerializeField] float TimeToDestroyShield = 0.5f;
    // Use this for initialization
    void Awake()
    {
        
        Destroy(gameObject,TimeToDestroyShield);

    }
}
