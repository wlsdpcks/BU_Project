using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = player.position - transform.position;    
    }

    private void LateUpdate()
    {
        transform.position = player.position - offset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
