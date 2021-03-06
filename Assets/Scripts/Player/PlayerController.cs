﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
	
	public bool isControllable = false;

    public float speed = 30;
	private float[] delays;
	private float[] cooldowns;

    Animation anim;
	public AudioClip attack;
	public AudioClip spell;
	AudioSource audioSource;

	private bool crRunning = false;

    void Start()
    {
		audioSource = GameObject.Find("SFX").GetComponent<AudioSource> ();
		if (audioSource)
			Debug.Log ("Loaded Audio source");

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

			if (crRunning) {
				if (CrossPlatformInputManager.GetButtonUp ("Attack") || CrossPlatformInputManager.GetButtonUp ("Skill"))
					StopGlitch ();
			} else {
				if (CrossPlatformInputManager.GetButtonDown ("Attack")) {
					UseSkill (0);
				} else if (CrossPlatformInputManager.GetButtonDown ("Skill")) {
					UseSkill (1);
				} else if (x != 0 || y != 0) {
					transform.LookAt (transform.position + new Vector3 (x, 0f, y));
					transform.Translate (new Vector3 (0, 0, 1) * speed * Time.deltaTime);
					anim.CrossFade ("run");
				} else {
					anim.CrossFade ("idle");
				}
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
		if (index >= Player.character.characterClass.skills.Count)
			return;
		
		Skill skill = Player.character.characterClass.skills[index];

		if (delays[0] <= 0 && delays[index+1] <= 0) {
			delays[0] = cooldowns[0];
			delays [index + 1] = cooldowns [index + 1];
			crRunning = true;
			StartCoroutine ("Glitch", skill);
		}
	}

	IEnumerator Glitch(Skill skill){
		SendAnimation (skill.animationName);
		yield return new WaitForSeconds(1);
		SendSkill (skill);
		StopGlitch ();
	}

	private void StopGlitch(){
		crRunning = false;
		StopAllCoroutines ();
		SendStopAnimation ();
	}

	private void SendAnimation(string animationName){
		PhotonView photonView = PhotonView.Get (this);
		photonView.RPC ("PlayAnimation", PhotonTargets.All, animationName);
	}

	private void SendStopAnimation(){
		PhotonView photonView = PhotonView.Get (this);
		photonView.RPC ("StopAnimation", PhotonTargets.All);
	}

	private void SendSkill(Skill skill){
		List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
		Vector3 currentPos = transform.position;

		List<GameObject> enemiesInRange = GetEnemiesInRange (enemies, transform, skill.range);
		List<GameObject> enemiesInTarget = GetEnemiesInTarget (enemiesInRange, skill.width);
		GameObject target = null;
		if (enemiesInRange.Count > 0 && enemiesInTarget.Count == 0) {
			target = GetNearestEnemy (enemiesInRange);
			transform.LookAt (target.transform);
			enemiesInTarget = GetEnemiesInTarget (enemiesInRange, skill.width);
		}
		if (skill.isMelee) {
			audioSource.PlayOneShot (attack);
			foreach (GameObject t in enemiesInTarget) {
				PhotonView photonView = PhotonView.Get (t);
				photonView.RPC ("TakeDamage", PhotonTargets.All, skill.Damage ());
			}
		} else {
			audioSource.PlayOneShot (spell);
			if (enemiesInTarget.Count > 0 || (target == null && enemiesInRange.Count > 0)) {
				target = GetNearestEnemy (enemiesInTarget);
			}
			if (target != null)
			if (skill.isAoe) {
				enemiesInTarget = GetEnemiesInRange (enemies, target.transform, (float) skill.aoe);
				foreach (GameObject t in enemiesInTarget) {
					PhotonView photonView = PhotonView.Get (t);
					photonView.RPC ("TakeDamage", PhotonTargets.All, skill.Damage ());
				}
			} else {
				PhotonView photonView = PhotonView.Get (target);
				photonView.RPC ("TakeDamage", PhotonTargets.All, skill.Damage ());
			}
		}
	}

	private List<GameObject> GetEnemiesInTarget(List<GameObject> enemies, int skillWidth){
		List<GameObject> enemiesInTarget = new List<GameObject> ();
		foreach (GameObject t in enemies) {
			Vector3 directionToTarget = t.transform.position - transform.position;
			float angle = Vector3.Angle(transform.forward, directionToTarget);
			if (Mathf.Abs(angle) < skillWidth)
			{
				enemiesInTarget.Add (t);
			}
		}
		return enemiesInTarget;
	}

	private List<GameObject> GetEnemiesInRange(List<GameObject> enemies, Transform origin, float range){
		List<GameObject> enemiesInRange = new List<GameObject> ();
		foreach (GameObject t in enemies) {
			float dist = Vector3.Distance (t.transform.position, origin.position);
			if (dist < range) {
				enemiesInRange.Add (t);
			}
		}
		return enemiesInRange;
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

	[PunRPC]
	public void PlayAnimation(string animationName){
		anim.clip = anim.GetClip (animationName);	
		anim.Play ();
	}

	[PunRPC]
	public void StopAnimation(){
		anim.Stop ();
	}
}
