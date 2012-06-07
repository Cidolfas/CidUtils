using UnityEngine;
using System.Collections;

/// <summary>
/// The BT Condition node is a leaf node that executes the delegate function assigned to it
/// and passes the return of that function to its parent.
/// </summary>
public class BTCondition : BTNode {
	
	public delegate BTStatusCode Condition();
	protected Condition mp_condition;
	
	public BTCondition(Condition cond)
	{
		mp_condition = cond;
	}
	
	public override BTStatusCode Tick ()
	{
		return mp_condition();
	}
	
}
