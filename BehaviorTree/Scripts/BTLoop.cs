using UnityEngine;
using System.Collections;

/// <summary>
/// The BT Loop node will run through each child in order until one has a result other
/// than success, at which time the node will return the result. When the node has checked
/// all the children nodes, it will loop around and begin checking at the first child until
/// one of the children returns a result other than success. If it was running on the previous
/// frame it will resume from the last checked child.
/// </summary>
public class BTLoop : BTNode {

	public BTLoop() {}
	
	// This number is to prevent an infinite loop condition.
	// If you want to ignore this, set it to a negative number.
	public int maxLoops = 1000;
	
	public override BTStatusCode Tick ()
	{
		int loops = 0;
		
		if (status != BTStatusCode.Running)
			mp_lastChild = 0;
		
		while (loops != maxLoops) {
			BTStatusCode code = children[mp_lastChild].Tick();
			if (code != BTStatusCode.Success) {
				status = code;
				return code;
			}
			
			mp_lastChild++;
			if (mp_lastChild >= children.Count)
				mp_lastChild = 0;
			
			loops++;
		}
		
		Debug.LogError("Maximum loop depth reached!");
		status = BTStatusCode.Error;
		return status;
	}
	
}
