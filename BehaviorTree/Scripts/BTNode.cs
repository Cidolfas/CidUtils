using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BTStatusCode { Ready, Success, Failure, Running, Error };

public abstract class BTNode {
	
	protected int mp_lastChild = 0;
	
	public BTStatusCode status = BTStatusCode.Ready;
	
	public List<BTNode> children = new List<BTNode>();
	public void AddChild(BTNode child) { children.Add(child); }
	
	public abstract BTStatusCode Tick();
	
	public void Reset(bool resetRunning = false)
	{
		if (resetRunning) {
			status = BTStatusCode.Ready;
		} else if (status != BTStatusCode.Running) {
			status = BTStatusCode.Ready;
		}
		
		for (int i = 0; i < children.Count; i++) {
			children[i].Reset(resetRunning);
		}
	}
}
