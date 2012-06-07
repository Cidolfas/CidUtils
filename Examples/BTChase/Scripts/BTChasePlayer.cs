using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is an example of a player controller using a BT.
/// I don't recommend this method, a FSM is easier for simple things like this.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class BTChasePlayer : MonoBehaviour {
	
	public BehaviorTree bt;
	
	public Blackboard blackboard = new Blackboard();
	
	public float normalSpeed = 10f;
	public float hulkSpeed = 7f;
	public Color normalColor;
	public Color hulkColor;
	
	// This feels kinda hacky. It's state, but using node traversal instead of FSM transitions
	public enum PlayerState { Normal, Hulk };
	public PlayerState state = PlayerState.Normal;
	
	// Use this for initialization
	void Awake ()
	{
		// This is where we build the tree
		BTPrioritySelector root = new BTPrioritySelector();
		bt = new BehaviorTree(root);
		
			// Should we transform?
			// Decorator to make sure this never wins the priority selection
			BTDecAlwaysFail transformCheckDec = new BTDecAlwaysFail();
			root.AddChild(transformCheckDec);
				// Do the actual check
				BTAction transformCheckAction = new BTAction(delegate(){
					if (state == PlayerState.Normal && Input.GetKeyDown(KeyCode.Space)) {
						BecomeHulk();
					}
					return BTStatusCode.Success;
				});
				transformCheckDec.AddChild(transformCheckAction);
		
			// Normal sequence
			BTSequence normalSeq = new BTSequence();
			root.AddChild(normalSeq);
				// Are we in Normal mode?
				BTCondition inNormalModeCond = new BTCondition(delegate(){ return state == PlayerState.Normal; }); // Anonymous methods are great for this!
				normalSeq.AddChild(inNormalModeCond);
				// Get input
				BTAction getInputAction = new BTAction(GetInput);
				normalSeq.AddChild(getInputAction);
				// Move player
				BTAction moveNormalAction = new BTAction(NormalMove);
				normalSeq.AddChild(moveNormalAction);
			
			// Hulk sequence
			BTSequence hulkSeq = new BTSequence();
			root.AddChild(hulkSeq);
				// Assume we're in hulk mode if we're here
				// Get input
				BTAction hulkInputAction = new BTAction(GetInput); // New instance b/c each instance has it own status
				hulkSeq.AddChild(hulkInputAction);
				// Move player
				BTAction moveHulkAction = new BTAction(HulkMove);
				hulkSeq.AddChild(moveHulkAction);
	}
	
	void Start()
	{
		renderer.material.color = normalColor;
	}
	
	// Update is called once per frame
	void Update ()
	{
		bt.Tick();
	}
	
	// Misc functions
	void BecomeHulk()
	{
		state = PlayerState.Hulk;
		transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
		transform.position = transform.position + new Vector3(0f, 0.2f, 0f);
		renderer.material.color = hulkColor;
		Invoke("BecomeNormal", 5f);
	}
	
	void BecomeNormal()
	{
		state = PlayerState.Normal;
		transform.localScale = new Vector3(1f, 1f, 1f);
		renderer.material.color = normalColor;
	}
	
	// BT functions
	BTStatusCode GetInput()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
		if (input.sqrMagnitude > 0)
			input.Normalize();
		blackboard.Put("Input", input);
		
		return BTStatusCode.Success;
	}
	
	BTStatusCode NormalMove()
	{
		Vector3 input;
		
		try {
			input = (Vector3)blackboard.Look("Input");
		} catch {
			Debug.LogError("Blackboard info for 'Input' does not match type 'Vector3'");
			return BTStatusCode.Error;
		}
		
		CharacterController cc = GetComponent<CharacterController>();
		cc.SimpleMove(input * normalSpeed);
		return BTStatusCode.Success;
	}
	
	BTStatusCode HulkMove()
	{
		Vector3 input;
		
		try {
			input = (Vector3)blackboard.Look("Input");
		} catch {
			Debug.LogError("Blackboard info for 'Input' does not match type 'Vector3'");
			return BTStatusCode.Error;
		}
		
		CharacterController cc = GetComponent<CharacterController>();
		cc.SimpleMove(input * hulkSpeed);
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
	
}
