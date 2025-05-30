using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDataPersistance
{
    //Movement
    public float verticalMoveSpeed;
    public float horizontalMoveSpeed;

    public bool lock2D = false;

    //Raycast Stuff
        //Offset 
    public float groundDistance;
    public LayerMask groundLayer;


    public bool canInteract = true;

    //Debug Stuff
    public bool seeRayCast;


    [HideInInspector]
    public bool canMove;

    private Animator anim;
    [HideInInspector]
    public Rigidbody rigidBody;
    private Vector3 scale;

    private Vector3 checkpointPosition;
    private string saveScene;

    #region Save and Load
    public void LoadData(SaveData data) {
        if (data != null) {
            if (GameObject.Find("Save Manager").GetComponent<SaveManager>().freshLoad) {
                transform.position = data.playerPosition;
                GameObject.Find("Save Manager").GetComponent<SaveManager>().freshLoad = false;
            }
            else if (!GameObject.Find("Save Manager").GetComponent<SaveManager>().loadingSubWorld && GameObject.Find("Save Manager").GetComponent<SaveManager>().preSubWorldCoords.Count > 0 && !FindObjectOfType<LevelManager>().usingStarterCoords)
            { transform.position = GameObject.Find("Save Manager").GetComponent<SaveManager>().preSubWorldCoords.Pop(); }

        }
    
    }
    public void SaveData(ref SaveData data) {
        if (data != null) {
            if (GameObject.Find("Save Manager").GetComponent<SaveManager>().loadingSubWorld)
                { 
                GameObject.Find("Save Manager").GetComponent<SaveManager>().preSubWorldCoords.Push(transform.position);
                SavePosition();
                data.playerPosition = checkpointPosition;
                data.sceneToLoad = saveScene;
            }
            else {
                SavePosition();
                data.playerPosition = checkpointPosition;
                data.sceneToLoad = saveScene;
            }
        }
    
    }
    #endregion

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
            float z = 0;
            if (lock2D) { z = 0; }
            else { z = Input.GetAxis("Vertical"); }
            rigidBody.velocity = new Vector3((x * horizontalMoveSpeed), 0.0f, (z * verticalMoveSpeed));
            if (x < 0) { transform.localScale = new Vector3(-scale.x, scale.y, scale.z); }
            else if (x > 0) { transform.localScale = new Vector3(scale.x, scale.y, scale.z); }
        }
        else
        {
            rigidBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    private bool CanMove() {
        if (canMove)
            return true;
        else
            return false;
    }

    public void SavePosition() {
        checkpointPosition = transform.position;
        saveScene = SceneManager.GetActiveScene().name;
    }
    public void SavePosition(Vector3 position)
    {
        checkpointPosition = position;

        saveScene = SceneManager.GetActiveScene().name;
    }

    public void EnableMovement() { canMove = true; }
    public void DisableMovement() { canMove = false; }
}
