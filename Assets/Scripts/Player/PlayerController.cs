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

	public PlayerAttributes playerAttributes;

    void Start()
    {
		playerAttributes = DataManager.GetPlayerAttributes();
		List<BaseSkill> charSkills = playerAttributes.GetSkills ();

        anim = GetComponent<Animation>();

		delays = new float[5];
		cooldowns = new float[5];

		cooldowns [0] = 1.1f;
		for (int i = 0; i < charSkills.Count; i++)
			cooldowns [i + 1] = charSkills [i].GetCooldown ();
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
		if (!playerAttributes.IsAlive ()) {
			Destroy (this.gameObject);
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
		BaseSkill skill = playerAttributes.GetSkills () [index];

		if (delays[0] <= 0 && delays[index+1] <= 0) {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			Vector3 currentPos = transform.position;
			foreach (GameObject t in enemies)
			{
				float dist = Vector3.Distance(t.transform.position, currentPos);
				if (dist < skill.GetRange())
				{
					PhotonView photonView = PhotonView.Get(t);
					photonView.RPC("TakeDamage", PhotonTargets.All, skill.GetDamage());
				}
			}
			anim.CrossFade (skill.GetAnimationName());
			delays[0] = cooldowns[0];
			delays [index + 1] = cooldowns [index + 1];
		}
	}

	[PunRPC]
	public void TakeDamage(float damage){
		this.playerAttributes.TakeDamage (damage);
	}
}
