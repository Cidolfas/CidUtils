using UnityEngine;
using System.Collections;

/// <summary>
/// The BT Sequence node will run through each child in order until it hits a results other
/// than success, at which point it will return the result. If it gets successes from
/// all of it's children it will return success. If it was running on the previous frame
/// it will resume from the last checked child.
/// </summary>
public class BTSequence : BTNode {
	
	public BTSequence() {}
	
	public override BTStatusCode Tick ()
	{
		if (status != BTStatusCode.Running)
			mp_lastChild = 0;
		
		while (mp_lastChild < children.Count) {
			BTStatusCode code = children[mp_lastChild].Tick();
			if (code != BTStatusCode.Success) {
				status = code;
				return code;
			}
			mp_lastChild++;
		}
		
		status = BTStatusCode.Success;
		return status;
	}
}
