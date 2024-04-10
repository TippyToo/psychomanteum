using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour
{
    private bool detectsPlayer;
    public GameObject jukeBoxUI;
    private JournalManager journal;
    public AudioSource audSource;
    public AudioClip[] songs;
    private int currentSong;
    private Queue<int> songQue = new Queue<int>();
    private PlayerController player = null;
    
    // Start is called before the first frame update
    void Start()
    {
        journal = FindObjectOfType<JournalManager>();
        currentSong = Random.Range(0, songs.Length);
        audSource.clip = songs[currentSong];
        audSource.time = Random.Range(0, audSource.clip.length);
        audSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (detectsPlayer && Input.GetButtonUp("Interact")) {
            Open();
        }

        if (!audSource.isPlaying) {
            PlayNextSong();
        }
    }
    public void QueueSong(int song) {
        songQue.Enqueue(song);
    }

    public void PlayNextSong() {
        if (songQue.Count >= 1) { currentSong = songQue.Dequeue(); }
        else { currentSong++; }

        if (currentSong >= songs.Length) { currentSong = 0; }
        audSource.clip = songs[currentSong];
        audSource.Play();
    }
    public void Open()
    {
        jukeBoxUI.SetActive(true);
        player.canMove = false;
        journal.canOpen = false;
    }
    public void Close() {
        jukeBoxUI.SetActive(false);
        player.canMove = true;
        journal.canOpen = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (player == null)
                player = other.GetComponent<PlayerController>();
            detectsPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            detectsPlayer = false;
        }

    }
}
