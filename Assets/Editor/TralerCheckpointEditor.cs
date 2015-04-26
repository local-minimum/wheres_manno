using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Checkpoint))]
public class TralerCheckpointEditor : Editor {

	public override void OnInspectorGUI ()
	{
		Checkpoint mytarget = (Checkpoint) target;
		base.OnInspectorGUI ();
		if (GUILayout.Button("Automatic edges")) {
			mytarget.edges.AddRange(mytarget.GetAutoDiscovery());
			serializedObject.ApplyModifiedProperties();
		}
	}
}
