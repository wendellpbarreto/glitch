using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {
	
	public float height = 10f;
    public float width = 10f;
    public float distance = 10f;
    Vector3 cameraPosition;
    public Transform target;
	public Camera camera;
 
    void Start()
    {
		cameraPosition = new Vector3(width, height, -distance);
		camera = Camera.main;
    }

    void Update() {
		if (target != null) {
			camera.transform.position = target.position + cameraPosition;
			camera.transform.LookAt(target.position);
		}
    }
}
