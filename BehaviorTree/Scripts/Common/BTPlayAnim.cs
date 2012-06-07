using UnityEngine;
using System.Collections;

public class BTPlayAnim : BTAction {
	
	protected GameObject mp_target;
	protected string mp_anim;
	
	public BTPlayAnim(GameObject target, string anim)
	{
		mp_target = target;
		mp_anim = anim;
		mp_action = PlayAnim;
	}
	
	protected BTStatusCode PlayAnim()
	{
		mp_target.animation.Play(mp_anim);
		return BTStatusCode.Success;
	}
	
}
