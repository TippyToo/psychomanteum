using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : MonoBehaviour
{
    public bool movingLeft;
    public float killXCoords;
    public float respawnXCoords;
    public float moveSpeed;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            rb.velocity = new Vector3(-moveSpeed, rb.velocity.y, rb.velocity.z);
            if (transform.position.x < killXCoords)
                transform.position = new Vector3(respawnXCoords, transform.position.y, transform.position.z);
        }
        else {
            rb.velocity = new Vector3(moveSpeed, rb.velocity.y, rb.velocity.z);
            if (transform.position.x > killXCoords)
                transform.position = new Vector3(respawnXCoords, transform.position.y, transform.position.z);
        }
    }
}
