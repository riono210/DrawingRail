using UnityEngine;
using UnityEngine.AI;

public class BuildNavmesh : MonoBehaviour {

	private void Start () { }

	void Update () {
		CreateNavMesh ();
	}

	// NavMeshの生成
	public void CreateNavMesh () {
		if (RailCreateManager.Instance.railExistence) {
			GetComponent<NavMeshSurface> ().BuildNavMesh ();

			RailCreateManager.Instance.railExistence = false;
			//RailCreateManager.Instance.rootExistence = true;
			RailCreateManager.Instance.shapeRail = true;
		}
	}
}