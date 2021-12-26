using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public GameObject Cube;

    public void Start()
    {
        var render = Cube.GetComponentInChildren<MeshRenderer>();
        render.enabled = false;
    }
}

