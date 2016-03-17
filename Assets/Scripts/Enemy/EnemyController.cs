using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
	public float range = 5;
	public float delay = 0f;
	public float cooldown = 2f;
	public GameObject healthBar;

	private Transform playerTransform;
	private EnemyAttributes enemyAttributes;


	void Start () {
		enemyAttributes = new EnemyAttributes ();
	}

	void Update () {
		if (!enemyAttributes.IsAlive ())
			Destroy (this.gameObject);
		else {
			if (delay > 0)
				delay -= Time.deltaTime;

			if (InRange()) {
				if (delay <= 0)
					AttackPlayer ();
			} else {
				MoveToPlayer ();	
			}	
		}
	}

	void OnGUI(){
		gameObject.GetComponentInChildren<Canvas>().transform.LookAt (Camera.main.transform);

		float hpRatio = enemyAttributes.CurrentHP / enemyAttributes.MaxHP ();
		this.healthBar.transform.localScale = new Vector3 (hpRatio, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}

	Transform GetNearPlayer(){
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		Transform tMin = null;
		float minDist = Mathf.Infinity;
		Vector3 currentPos = transform.position;
		foreach (GameObject t in players)
		{
			float dist = Vector3.Distance(t.transform.position, currentPos);
			if (dist < minDist)
			{
				tMin = t.transform;
				minDist = dist;
			}
		}
		return tMin;
	}

	bool InRange(){
		UpdatePlayerTransform ();
		if (this.playerTransform != null && Vector3.Distance (this.playerTransform.position, transform.position) < range)
			return true;
		return false;
	}

	void MoveToPlayer(){
		UpdatePlayerTransform ();
		if (playerTransform != null) {
			transform.LookAt (playerTransform);
			GetComponent<CharacterController> ().SimpleMove (transform.forward);
		}
	}

	void UpdatePlayerTransform(){
		this.playerTransform = GetNearPlayer ();
	}

	void AttackPlayer(){
		PhotonView photonView = PhotonView.Get(this.playerTransform.gameObject);
		photonView.RPC("TakeDamage", PhotonTargets.All, enemyAttributes.GetDamage());
		delay = cooldown;
	}

	[PunRPC]
	public void TakeDamage(float damage){
		Debug.Log ("Enemy damaged");
		enemyAttributes.TakeDamage (damage);
	}
}
