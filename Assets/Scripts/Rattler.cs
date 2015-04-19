using UnityEngine;
using System.Collections;

public class Rattler : MonoBehaviour {

	[SerializeField] float rattleDuration = 0.4f;
	[SerializeField] Vector3 rattleMax = Vector3.one;
	[SerializeField] float rattleSpacing = 1f;
	[Range(0,1)][SerializeField] float rattleSpacingVariation = 0.8f;

	float nextPunch;

	// Use this for initialization
	void Start () {
		nextPunch = GetNextPunchTime();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > nextPunch)
			Rattle();
	}

	void Rattle() {
		iTween.PunchPosition(gameObject, randomRattleVector, rattleDuration);
		nextPunch = GetNextPunchTime();
	}

	Vector3 randomRattleVector {
		get {
			return new Vector3(GetRandomMax(rattleMax.x), GetRandomMax(rattleMax.y), GetRandomMax(rattleMax.z));
		}
	}

	float GetRandomMax(float value) {
		if (value > 0)
			return Random.Range(-value, value);
		else if (value < 0)
			return Random.Range(value, -value);
		else
			return value;
	}

	float GetNextPunchTime() {
		return Time.timeSinceLevelLoad + rattleDuration + rattleSpacing * (1 + Random.Range(-rattleSpacingVariation, rattleSpacingVariation));
	}
}
