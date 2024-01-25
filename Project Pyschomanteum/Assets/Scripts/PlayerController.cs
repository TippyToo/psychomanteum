using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement
    public bool canMove;
    public float moveSpeed;
    //Raycast Stuff
        //Offset 
    public float groundDistance;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovement();
    }

    void UpdateMovement() {
        //Creates an invisible downward facing ray that detects the ground layer and adjusts character height accordingly
        RaycastHit targ;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out targ, Mathf.Infinity, groundLayer)) {
            //Uncomment to see the raycast in the scene viewport
            //Debug.DrawLine(castPos, new Vector3(transform.position.x, transform.position.y - 200, transform.position.z), Color.white, 5.0f, false);
            if (targ.collider != null) {
                Vector3 movPos = transform.position;
                movPos.y = targ.point.y + groundDistance;
                transform.position = movPos;
            }
        }

        //Character movement controlled here
        if (canMove) {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            transform.position = new Vector3(transform.position.x + (x * moveSpeed), transform.position.y, transform.position.z + (z * moveSpeed));
        }
    }
}
