using UnityEngine;
using System.Collections;

public class BTSendMessage : BTAction {
	
	protected GameObject mp_target;
	protected string mp_message;
	
	public BTSendMessage(GameObject target, string message)
	{
		mp_target = target;
		mp_message = message;
		mp_action = Send;
	}
	
	protected BTStatusCode Send()
	{
		mp_target.SendMessage(mp_message);
		return BTStatusCode.Success;
	}
	
}
