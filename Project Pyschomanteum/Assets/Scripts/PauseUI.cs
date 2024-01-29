using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    private bool paused;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) { Pause(); }
    }

    public void Resume() { Pause(); }

    public void Quit()  { SceneManager.LoadScene(0); }

    private void Pause() {
        //Pauses or unpauses the game
        paused = !paused;
        if (paused) {
            Time.timeScale = 0;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else {
            Time.timeScale = 1;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
