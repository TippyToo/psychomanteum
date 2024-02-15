using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraSettings : MonoBehaviour
{
    private GameObject player;
    public float cameraHeight;
    public float cameraAngle;
    public float startingZPosition;
    private float zPosition;
    public bool followPlayerZ;
    public bool lockMovement;
    private bool matched = true;
    // Start is called before the first frame update
    void Start()
    {
        zPosition = startingZPosition;
        player = GameObject.Find("Player");
        transform.position = new Vector3(player.transform.position.x, cameraHeight, zPosition);
        transform.eulerAngles = new Vector3(cameraAngle, 0.0f, 0.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }



    //Camera Position
    //Exact x y z coordinates to set position 
    public void SetCameraPosition(float x = 0.0f, float y = 0.0f, float z = 0.0f) {
        transform.position = new Vector3(x, y, z);
    }
    //x y z values to add/subtract to the current posiiton instantly and over time
    public void ShiftCameraPosition(float x, float y, float z, float time = 0.0f) {
        if (time > 0.0f) { StartCoroutine(Move(x, y, z, time)); }
        else { transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z); }
    }
    private IEnumerator Move(float x, float y, float z, float time) {
        for (int i = 0; i < time * 4; i++) {
            transform.position = new Vector3(transform.position.x + (x / (time / 4)), transform.position.y + (y / (time / 4)), transform.position.z + (z / (time / 4)));
            yield return new WaitForSecondsRealtime(time / 4);
        }
    }

    //Toggles whether or not to follow the players z position
    public void FollowPlayer() {
        followPlayerZ = !followPlayerZ;
    }
    private void UpdateMovement() {
        float targetPosition = player.transform.position.z + zPosition;
        if (transform.position.z >= (targetPosition - 0.5f) && transform.position.z <= (targetPosition + 0.5)) { matched = true;  } else { matched = false; }
        if (followPlayerZ) {
            if (matched) {
                transform.parent = GameObject.Find("Player").transform;
            } else {
                if (transform.position.z > targetPosition) {
                    transform.position = new Vector3(player.transform.position.x, player.transform.position.y + cameraHeight, transform.position.z - 0.1f);
                }
                else if (transform.position.z < targetPosition) {
                    transform.position = new Vector3(player.transform.position.x, player.transform.position.y + cameraHeight, transform.position.z + 0.1f); ;
                }
            }
        } 
        else if (!lockMovement) { transform.parent = null; transform.position = new Vector3(player.transform.position.x, player.transform.position.y + cameraHeight, zPosition); }
    }

    //Camera Rotation
    //Exact x y z angles to set rotation
    public void SetCameraRotation(float x = 0.0f, float y = 0.0f, float z = 0.0f) {
        transform.eulerAngles = new Vector3(x, y, z);
    }
    //x y z angles to add/subtract to the current rotation instantly and over time
    public void RotateCamera(float x = 0.0f, float y = 0.0f, float z = 0.0f, float time = 0.0f) {
        if (time > 0.0f) { StartCoroutine(Rotate(x, y, z, time)); }
        else { transform.eulerAngles = new Vector3(transform.rotation.x + x, transform.rotation.y + y, transform.rotation.z + z); }
    }
    private IEnumerator Rotate(float x, float y, float z, float time) {
        for (int i = 0; i < time * 4; i++) {
            transform.eulerAngles = new Vector3(transform.rotation.x + (x / (time / 4)), transform.rotation.y + (y / (time / 4)), transform.rotation.z + (z / (time / 4)));
            yield return new WaitForSecondsRealtime(time / 4);
        }
    }
}
