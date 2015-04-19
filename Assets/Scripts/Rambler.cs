using UnityEngine;
using System.Collections;

public class Rambler : MonoBehaviour {

	[SerializeField] float timeBetweenRambles = 2f;
	[Range(0, 1)][SerializeField] float timeBetweenVariability = 0.9f;
	AudioSource player;
	float nextRamble;
	Story story;
	bool touching = false;
	[SerializeField][Range(0,1)] float volume = 0.5f;

	// Use this for initialization
	void Start () {
		player = GetComponentInChildren<AudioSource>();
		story = GameObject.FindObjectOfType<Story>();
		if (player == null)
			player = gameObject.AddComponent<AudioSource>();
		nextRamble = Random.Range(-timeBetweenRambles, timeBetweenRambles);
	}
	
	// Update is called once per frame
	void Update () {
		if (RambleTime)
			Ramble();
	}

	bool RambleTime {
		get {
			return touching && !player.isPlaying && Time.timeSinceLevelLoad > nextRamble; 
		}
	}

	void Ramble() {
		player.PlayOneShot(story.NextRambling, volume);
		SetNextRambleTime();
	}

	void SetNextRambleTime() {
		nextRamble = Time.timeSinceLevelLoad +
			timeBetweenRambles * (1 + Random.Range(-timeBetweenVariability, timeBetweenVariability));
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player")
			touching = true;
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player")
			touching = false;
	}
}
