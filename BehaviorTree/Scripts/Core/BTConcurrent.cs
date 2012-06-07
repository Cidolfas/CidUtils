using UnityEngine;
using System.Collections;

/// <summary>
/// The BT Concurrent node will run through each child node in order until a specified
/// number have failed. If a child returns an error the checks will be stopped and the
/// error returned. If all children are checked without the failure threshold being reached
/// the node will return success.
/// </summary>
public class BTConcurrent : BTNode {
	
	public int failLimit;
	
	// A negative value or 0 will result in no fail limiting
	public BTConcurrent(int fails = 1)
	{
		failLimit = fails;
	}
	
	public override BTStatusCode Tick ()
	{
		mp_lastChild = 0;
		int fails = 0;
		
		while (mp_lastChild < children.Count) {
			BTStatusCode code = children[mp_lastChild].Tick();
			if (code == BTStatusCode.Error) {
				return BTStatusCode.Error;
			} else if (code == BTStatusCode.Failure) {
				fails++;
				if (failLimit > 0 && fails >= failLimit)
					return BTStatusCode.Failure;
			}
			mp_lastChild++;
		}
		
		status = BTStatusCode.Success;
		return status;
	}
	
}
