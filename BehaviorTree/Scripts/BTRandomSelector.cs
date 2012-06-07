using UnityEngine;
using System.Collections;

/// <summary>
/// The BT Random Selector node will choose one of its child nodes to visit at random,
/// and will return the child node's result. If the last result was running, the node
/// will return to the previous child until that node returns a value other than running.
/// </summary>
public class BTRandomSelector : BTNode {

	BTRandomSelector() {}
	
	public override BTStatusCode Tick ()
	{
		if (status != BTStatusCode.Running) {
			mp_lastChild = Random.Range(0, children.Count);
		}
		
		status = children[mp_lastChild].Tick();
		return status;
	}
	
}
