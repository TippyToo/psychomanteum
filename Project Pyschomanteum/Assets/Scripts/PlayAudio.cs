using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public float playTime;
    public float waitTime;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator StartTime() {
        yield return new WaitForSeconds(Random.Range(10, 30));
        StartCoroutine(playSound());
    }
    public IEnumerator playSound() {
        while (true)
        {
            audioSource.Play();
            yield return new WaitForSeconds(playTime);
            audioSource.Stop();
            yield return new WaitForSeconds(waitTime);
        }
    }
}
