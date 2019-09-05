using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;
    }

}
