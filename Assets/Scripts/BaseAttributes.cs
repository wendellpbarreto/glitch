using UnityEngine;
using System.Collections;

public abstract class BaseAttributes {

    protected float strenght;
    protected float dextrery;
    protected float inteligence;
    protected float vitality;
	protected float range;

	protected float currentHp;
	protected float currentMp;

    public float MaxHP() {
        return vitality * 100;
    }

    public float MaxMP() {
		return inteligence * 100;
	}

	public float Strenght {
		get{ 
			return this.strenght;		
		}
	}
	public float Dextrery {
		get{ 
			return this.dextrery;		
		}
	}
	public float Intelligence {
		get{ 
			return this.inteligence;		
		}
	}
	public float Vitallity {
		get{ 
			return this.vitality;		
		}
	}

	public float CurrentHP {
		get{ 
			return this.currentHp;		
		}
	}

	public float CurrentMP {
		get{ 
			return this.currentMp;		
		}
	}

	public bool IsAlive(){
		return this.currentHp > 0;
	}
}
