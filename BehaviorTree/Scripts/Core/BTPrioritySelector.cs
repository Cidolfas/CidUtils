using UnityEngine;
using System.Collections;

/// <summary>
/// The BT Priority Selector will run through each child in order until one of the
/// children returns a result other than failure, at which time the node will return
/// the result. If it gets failures from all of it's children it will return failure.
/// </summary>
public class BTPrioritySelector : BTNode {

	public BTPrioritySelector() {}
	
	public override BTStatusCode Tick ()
	{
		mp_lastChild = 0;
		
		while (mp_lastChild < children.Count) {
			BTStatusCode code = children[mp_lastChild].Tick();
			if (code != BTStatusCode.Failure) {
				status = code;
				return code;
			}
			mp_lastChild++;
		}
		
		status = BTStatusCode.Failure;
		return status;
	}
	
}
