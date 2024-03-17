using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDataPersistance
{
    //Movement
    public float moveSpeed;
    //Raycast Stuff
        //Offset 
    public float groundDistance;
    public LayerMask groundLayer;
    //Debug Stuff
    public bool seeRayCast;
    [HideInInspector]
    public bool canMove;

    private Animator anim;
    private Rigidbody rigidBody;
    private Vector3 scale;

    public void LoadData(SaveData data) { if (data != null) transform.position = data.playerPosition; }
    public void SaveData(ref SaveData data) { if (data != null) data.playerPosition = transform.position; }
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        scale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovement();
        anim.SetFloat("Speed", (Mathf.Abs(rigidBody.velocity.x) + Mathf.Abs(rigidBody.velocity.z)));
    }

    void UpdateMovement() {
        //Creates an invisible downward facing ray that detects the ground layer and adjusts character height accordingly
        RaycastHit targ;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out targ, Mathf.Infinity, groundLayer)) {
            if (seeRayCast) { Debug.DrawLine(castPos, new Vector3(transform.position.x, transform.position.y - 200, transform.position.z), Color.white, 5.0f, false); }
            if (targ.collider != null) {
                Vector3 movPos = transform.position;
                movPos.y = targ.point.y + groundDistance;
                transform.position = movPos;
            }
        }

        //Character movement controlled here
        if (CanMove()) {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            rigidBody.velocity = new Vector3((x * moveSpeed), 0.0f, (z * moveSpeed));
            if (x < 0) { transform.localScale = new Vector3(-scale.x, scale.y, scale.z); }
            else if (x > 0) { transform.localScale = new Vector3(scale.x, scale.y, scale.z); }
        }
    }

    private bool CanMove() {
        if (canMove)
            return true;
        else
            return false;
    }

    public void EnableMovement() { canMove = true; }
    public void DisableMovement() { canMove = false; }
}
