using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool followPlayer;
    public float followDistance;
    public float moveSpeed;
    private GameObject player;
    private Rigidbody rigidBody;

    public bool canMove = true;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("Player");
        rigidBody = transform.GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer) {
            DetermineBehaviour();
        }
    }

    private void DetermineBehaviour() {
        //if at the follow point and player isn't moving, 
        if (canMove) {
            MoveToFollowPoint();
        }
        
        //else follow player
    }

    private void MoveToFollowPoint()
    {
        //Update x position until within x range
        Transform followPoint = player.transform.GetChild(0);
        if (transform.position.x > followPoint.position.x + followDistance)
        { rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z); }
        else if (transform.position.x < followPoint.position.x - followDistance)
        { rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z); }
        else { rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, rigidBody.velocity.z); }

        //Update z position until within z range
        if (transform.position.z > followPoint.position.z + followDistance)
        { rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed); }
        else if (transform.position.z < followPoint.position.z - followDistance)
        { rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed); }
        else { rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, 0); }

    }

    
    private void Idle() { 
    
    }
}
