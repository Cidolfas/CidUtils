using UnityEngine;
using System.Collections;

/// <summary>
/// The BT Condition node is a leaf node that executes the delegate function assigned to it
/// and returns success if the function returns true or failure if the function returns false.
/// </summary>
public class BTCondition : BTNode {
	
	public delegate bool Condition();
	protected Condition mp_condition;
	
	public BTCondition(Condition cond)
	{
		mp_condition = cond;
	}
	
	public override BTStatusCode Tick ()
	{
		status = (mp_condition()) ? BTStatusCode.Success : BTStatusCode.Failure;
		return status;
	}
	
}
