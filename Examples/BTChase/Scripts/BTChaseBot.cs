using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class BTChaseBot : MonoBehaviour {
	
	public Transform target;
	public float chaseRadius = 2f;
	public float runRadius = 15f;
	public float speed = 10f;
	
	public Blackboard blackboard = new Blackboard();
	public BehaviorTree bt;
	
	// Use this for initialization
	void Awake ()
	{
		// Begin the tree
		BTNode root = new BTPrioritySelector();
		bt = new BehaviorTree(root);
		
			// Player in Hulk mode branch
			BTNode hulkModeSeq = root.AddChild(new BTSequence());
				// Is the player in hulk mode?
				hulkModeSeq.AddChild(new BTCondition(delegate(){
						if (target == null) return false;
						else return target.GetComponent<BTChasePlayer>().state == BTChasePlayer.PlayerState.Hulk;
					}));
				// Are we safe?
				BTNode botNotSafeSwitch = hulkModeSeq.AddChild(new BTSwitch());
					// Are we far enough away yet?
					botNotSafeSwitch.AddChild(new BTCondition(delegate(){
							Vector3 dist = target.transform.position - transform.position;
							if (dist.sqrMagnitude > runRadius * runRadius) return false;
							else return true;
						}));
					// Yes, let's move away
					botNotSafeSwitch.AddChild(new BTAction(MoveAway));
					// No, let's idle
					botNotSafeSwitch.AddChild(new BTDecAlwaysSucceed());
			
			// Player in Normal mode branch
			
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	// BT functions
	BTStatusCode MoveAway()
	{
		Vector3 dist = target.transform.position - transform.position;
		dist.Normalize();
		
		GetComponent<CharacterController>().SimpleMove(dist * -speed);
		
		return BTStatusCode.Success;
	}
	
	BTStatusCode MoveTowards()
	{
		Vector3 dist = target.transform.position - transform.position;
		dist.Normalize();
		
		GetComponent<CharacterController>().SimpleMove(dist * speed);
		
		return BTStatusCode.Success;
	}
	
	// Decorators
	class BTDecAlwaysFail : BTNode {
		
		public override BTStatusCode Tick ()
		{
			// Call the first child and then return failure
			if (children.Count > 0)
				children[0].Tick();
			return BTStatusCode.Failure;
		}
		
	}
	
	class BTDecAlwaysSucceed : BTNode {
		
		public override BTStatusCode Tick ()
		{
			// Call the first child and then return failure
			if (children.Count > 0)
				children[0].Tick();
			return BTStatusCode.Success;
		}
		
	}
}
