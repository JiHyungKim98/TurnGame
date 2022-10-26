using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform cam;
    public GameObject hpBar;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(-0.4f, 2.5f, 0.3f));
    }
}
