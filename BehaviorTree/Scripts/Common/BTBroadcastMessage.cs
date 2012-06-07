using UnityEngine;
using System.Collections;

public class BTBroadcastMessage : BTAction {
	
	protected GameObject mp_target;
	protected string mp_message;
	
	public BTBroadcastMessage(GameObject target, string message)
	{
		mp_target = target;
		mp_message = message;
		mp_action = Broadcast;
	}
	
	protected BTStatusCode Broadcast()
	{
		mp_target.BroadcastMessage(mp_message);
		return BTStatusCode.Success;
	}
	
}
