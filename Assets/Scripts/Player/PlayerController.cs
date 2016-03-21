using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
	
	public bool isControllable = false;

    public float speed = 30;
	private float[] delays;
	private float[] cooldowns;

    Animation anim;

    void Start()
    {
		List<Skill> charSkills = Player.character.characterClass.skills;

        anim = GetComponent<Animation>();

		delays = new float[5];
		cooldowns = new float[5];

		cooldowns [0] = 1.1f;
		for (int i = 0; i < charSkills.Count; i++)
			cooldowns [i + 1] = charSkills [i].cooldown;
    }

    void FixedUpdate() {
		if (isControllable) {
			float x = CrossPlatformInputManager.GetAxis ("Horizontal"),
			y = CrossPlatformInputManager.GetAxis ("Vertical");
			if (x != 0 || y != 0) {
				transform.LookAt (transform.position + new Vector3 (x, 0f, y));
				transform.Translate (new Vector3 (0, 0, 1) * speed * Time.deltaTime);
				anim.CrossFade ("run");
			} else if (CrossPlatformInputManager.GetButton ("Attack")) {
				UseSkill(0);
			} else {
				anim.CrossFade ("idle");
			}
		}
    }

    void onGUI() {

    }

	void Update(){
		if (!Player.character.IsAlive()) {
			Debug.Log ("Dead");
		} else {
			UpdateDelays ();
		}
	}

	void UpdateDelays(){
		for (int i = 0; i < delays.Length; i++) {
			if (delays[i] > 0)
				delays[i] -= Time.deltaTime;
		}
	}

	void UseSkill(int index){
		Skill skill = Player.character.characterClass.skills[index];

		if (delays[0] <= 0 && delays[index+1] <= 0) {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			Vector3 currentPos = transform.position;

			List<GameObject> enemiesInRange = new List<GameObject> ();
			List<GameObject> enemiesInTarget;
			foreach (GameObject t in enemies)
			{
				float dist = Vector3.Distance(t.transform.position, currentPos);
				if (dist < skill.range) {
					enemiesInRange.Add(t);
				}
			}
			enemiesInTarget = GetEnemiesInTarget (enemiesInRange);

			if (enemiesInRange.Count > 0 && enemiesInTarget.Count == 0 ) {
				GameObject target = GetNearestEnemy (enemiesInRange);
				transform.LookAt (target.transform);
				enemiesInTarget = GetEnemiesInTarget (enemiesInRange);
			}

			foreach (GameObject t in enemiesInTarget) {
				PhotonView photonView = PhotonView.Get(t);
				photonView.RPC("TakeDamage", PhotonTargets.All, skill.Damage());
			}

			anim.CrossFade (skill.animationName);
			delays[0] = cooldowns[0];
			delays [index + 1] = cooldowns [index + 1];
		}
	}

	private List<GameObject> GetEnemiesInTarget(List<GameObject> enemies){
		List<GameObject> enemiesInTarget = new List<GameObject> ();
		foreach (GameObject t in enemies) {
			Vector3 directionToTarget = t.transform.position - transform.position;
			float angle = Vector3.Angle(transform.forward, directionToTarget);
			if (Mathf.Abs(angle) < 45)
			{
				enemiesInTarget.Add (t);
			}
		}
		return enemiesInTarget;
	}

	private GameObject GetNearestEnemy(List<GameObject> enemies){
		GameObject tMin = null;
		float minDist = Mathf.Infinity;
		Vector3 currentPos = transform.position;
		foreach (GameObject t in enemies)
		{
			float dist = Vector3.Distance(t.transform.position, currentPos);
			if (dist < minDist)
			{
				tMin = t;
				minDist = dist;
			}
		}
		return tMin;
	}

	[PunRPC]
	public void TakeDamage(float damage){
		Player.character.TakeDamage (damage);
	}
}
