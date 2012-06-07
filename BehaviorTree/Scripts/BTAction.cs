using UnityEngine;
using System.Collections;

/// <summary>
/// The BT Action node is a leaf node that executes the delegate function assigned to it
/// and passes the return of that function to its parent.
/// </summary>
public class BTAction : BTNode {
	
	public delegate BTStatusCode Action();
	protected Action mp_action;
	
	public BTAction(Action act)
	{
		mp_action = act;
	}
	
	public override BTStatusCode Tick ()
	{
		return mp_action();
	}
	
}
