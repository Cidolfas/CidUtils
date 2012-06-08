using UnityEngine;
using System.Collections;

/// <summary>
/// The BT Switch node will run the first child, and then run either the second
/// or third child branches depending on the outcome of the first child. If the first
/// child results in anything other than success or failure, further operations are
/// halted and that result is returned. If the first child results in success, the second
/// branch is run and its result is returned. If the first child results in failure,
/// the third branch is run and its result is returned.
/// </summary>
public class BTSwitch : BTNode {

	public override BTStatusCode Tick ()
	{
		if (children.Count < 3) {
			Debug.LogError("BTSwitch ticked without at least 3 children");
			return BTStatusCode.Error;
		}
		
		BTStatusCode which = children[0].Tick();
		if (which != BTStatusCode.Success && which != BTStatusCode.Failure) {
			return which;
		} else if (which == BTStatusCode.Success) {
			return children[1].Tick();
		} else {
			return children[2].Tick();
		}
	}
	
}
