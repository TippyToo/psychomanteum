using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour, IDataPersistance
{
    public bool followPlayer;
    public float followDistance;
    public float moveSpeed;
    private GameObject player;
    private Rigidbody rigidBody;
    private Animator anim;

    public bool canMove = true;

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        rigidBody = transform.GetComponent<Rigidbody>();
    }

    #region Save and Load
    public void SaveData(ref SaveData data)
    {
        if (data != null)
        {
            if (!GameObject.Find("Save Manager").GetComponent<SaveManager>().loadingSubWorld) {
                if (data.companionPosition.ContainsKey(GetComponent<Dialogue>().npcName)) { data.companionPosition.Remove(GetComponent<Dialogue>().npcName); }
                data.companionPosition.Add(GetComponent<Dialogue>().npcName, transform.position);
            }

            if (data.followingStatus.ContainsKey(GetComponent<Dialogue>().npcName)) { data.followingStatus.Remove(GetComponent<Dialogue>().npcName); }
            data.followingStatus.Add(GetComponent<Dialogue>().npcName, followPlayer);
        }
        else
            Debug.Log("No Save Slot Found");
    }
    public void LoadData(SaveData data) {
        if (data != null)
        {
            data.followingStatus.TryGetValue(GetComponent<Dialogue>().npcName, out followPlayer);
            Vector3 pos = new Vector3();
            data.companionPosition.TryGetValue(GetComponent<Dialogue>().npcName, out pos);
            if (pos != new Vector3())
            transform.position = pos;
        }
    }
    #endregion

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer) {
            DetermineBehaviour();
        }
        anim.SetFloat("Speed", (Mathf.Abs(rigidBody.velocity.x) + Mathf.Abs(rigidBody.velocity.z)));
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
        { 
            rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * -1, transform.localScale.z, transform.localScale.z);
        }
        else if (transform.position.x < followPoint.position.x - followDistance)
        { 
            rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.z, transform.localScale.z);
        }
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
