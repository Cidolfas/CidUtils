using UnityEngine;
using System.Collections;

public class BTWaitForAnim : BTAction {
	
	protected GameObject mp_target;
	protected string mp_anim;
	
	public BTWaitForAnim(GameObject target, string anim = null)
	{
		mp_target = target;
		mp_anim = anim;
		mp_action = Wait;
	}
	
	protected BTStatusCode Wait()
	{
		if (string.IsNullOrEmpty(mp_anim)) {
			return (mp_target.animation.isPlaying) ? BTStatusCode.Running : BTStatusCode.Success;
		} else {
			return (mp_target.animation.IsPlaying(mp_anim)) ? BTStatusCode.Running : BTStatusCode.Success;
		}
	}
	
}
