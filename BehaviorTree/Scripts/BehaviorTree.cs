using UnityEngine;
using System.Collections;

/// <summary>
/// The main behavior tree class.
/// </summary>
public class BehaviorTree {
	// If you're new to this, take a look at http://www.altdevblogaday.com/2011/02/24/introduction-to-behavior-trees/
	
	public BehaviorTree(BTNode rootNode)
	{
		mp_root = rootNode;
	}
	
	protected BTNode mp_root;
	public BTNode Root { get { return mp_root; } }
	
	public void Tick()
	{
		mp_root.Tick();
		mp_root.Reset();
	}
	
}
