using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Checkpoint : MonoBehaviour {

	static HashSet<Checkpoint> nodes = new HashSet<Checkpoint>();

	[SerializeField] float autoDiscoverRadius = 2f;

	public List<Checkpoint> edges = new List<Checkpoint>();

	public List<Checkpoint> GetAutoDiscovery() {
		List<Checkpoint> autoDiscovery = new List<Checkpoint>();
		foreach (Checkpoint node in Checkpoint.nodes) {

			if (!edges.Contains(node) && node != this &&
			    Vector3.Distance(transform.position, node.transform.position) < autoDiscoverRadius) {

				autoDiscovery.Add(node);
			}
		}

		return autoDiscovery;
	}

	void OnEnable() {
		Checkpoint.nodes.Add(this);
	}

	void OnDisable() {
		Checkpoint.nodes.Remove(this);
	}

	// Use this for initialization
	void Start () {
		if (edges.Count == 0)
			GetAutoDiscovery();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		foreach (Checkpoint pt in edges)
			Gizmos.DrawLine(transform.position, pt.transform.position);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, autoDiscoverRadius);
		foreach (Checkpoint pt in edges)
			Gizmos.DrawLine(transform.position, pt.transform.position);
	}
}
