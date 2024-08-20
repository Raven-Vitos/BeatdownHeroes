using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform Player;

    [SerializeField]
    float speed = 5.0f;

    [SerializeField]
    float cameraHeight = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, Player.position.y + cameraHeight, Player.position.z), Time.deltaTime * speed);  
    }
}
