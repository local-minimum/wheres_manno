using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class Checkpoint : MonoBehaviour {

	static float acuteIntermediaryThreshold = 100f;
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

	public static Transform[] GetPathFromTo(Transform source, Transform sink) {
		Checkpoint entry = GetClosestNode(source.position);
		Checkpoint exit = GetClosestNode(sink.position);
		if (entry == exit) {
			if (SkipIntermediate(source, sink, entry.transform))
				return new Transform[] {source, sink};
			else
				return new Transform[] {source, entry.transform, sink};
		} else {
			List<Transform> path = ShortestPath(entry, exit);
			return AddSouceAndSinkToPath(path, source, sink).ToArray();
		}
	}

	static Checkpoint GetClosestNode(Vector3 position) {
		return nodes.OrderBy(node => Vector3.Distance(node.transform.position, position)).First();
	}

	static bool SkipIntermediate(Transform source, Transform sink, Transform intermediate) {
		return Vector3.Angle(source.position - intermediate.position,
		                     sink.position - intermediate.position) < acuteIntermediaryThreshold;
	}

	static List<Transform> ShortestPath(Checkpoint entry, Checkpoint exit) {
		List<List<Checkpoint>> paths = entry.ExtendPath(new List<Checkpoint>(), exit);
		return paths.OrderBy(path => GetPathLength(path.Select(node => node.transform).ToArray()))
			.First().Select(pnode => pnode.transform).ToList();
	}

	List<List<Checkpoint>> ExtendPath(List<Checkpoint> path, Checkpoint target) {
		List<List<Checkpoint>> paths = new List<List<Checkpoint>>();
		path.Add(this);
		if (target == this) {
			paths.Add(path);
		} else if (edges.Contains(target)) {
			path.Add(target);
			paths.Add(path);
		} else {
			foreach (Checkpoint edge in edges) {
				if (!path.Contains(edge)) {
					paths.AddRange(edge.ExtendPath(path.ToList(), target));
				}
			}
		}
		return paths;
	}

	static float GetPathLength(Transform[] path) {
		float length = 0;
		for (int i=0; i < path.Length - 1; i++)
			length += Vector3.Distance(path[i].position, path[i+1].position);
		return length;
	}

	static List<Transform> AddSouceAndSinkToPath(List<Transform> path, Transform source, Transform sink) {
		if (SkipIntermediate(source, path[1], path[0]))
			path[0] = source;
		else
			path.Insert(0, source);

		if (SkipIntermediate(sink, path[path.Count - 2], path[path.Count - 1]))
			path[path.Count - 1] = sink;
		else
			path.Add(sink);

		return path;
	}
}
