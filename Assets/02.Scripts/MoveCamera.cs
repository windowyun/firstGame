using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    Transform target;

    float moveSpeed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime * moveSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y, -30f);
    }

    //private void LateUpdate()
    //{
    //    transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime * moveSpeed);
    //    transform.position = new Vector3(transform.position.x,transform.position.y, -30f);
    //}
}
