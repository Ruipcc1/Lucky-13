using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        float xyz = Random.Range(.5f, 2.5f);
        gameObject.transform.localScale = new Vector3(xyz, xyz, xyz);    
    }

    void Update()
    {
        transform.LookAt(target);
    }
}
