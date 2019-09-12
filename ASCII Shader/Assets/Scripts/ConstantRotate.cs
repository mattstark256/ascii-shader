using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 90;
    
    void Update()
    {
        transform.localRotation *= Quaternion.Euler(0, Time.deltaTime * rotateSpeed, 0);
    }
}
