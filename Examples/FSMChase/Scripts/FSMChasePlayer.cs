using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FSMChasePlayer : MonoBehaviour {
	
	public StateMachine<FSMChasePlayer> sm = new StateMachine<FSMChasePlayer>();
	
	public float normalSpeed = 10f;
	public float hulkSpeed = 7f;
	public Color normalColor;
	public Color hulkColor;
	
	// Use this for initialization
	void Awake ()
	{
		sm.AddState("Normal", new NormalState(this));
		sm.AddState("Hulk", new HulkState(this));
		sm.ChangeState("Normal");
	}
	
	void Start()
	{
		renderer.material.color = normalColor;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !sm.IsInState("Hulk")) {
			GoHulk();
			Invoke("GoNormal", 10f);
		}
		
		sm.Tick();
	}
	
	public void GoHulk()
	{
		sm.ChangeState("Hulk");
	}
	
	public void GoNormal()
	{
		sm.ChangeState("Normal");
	}
	
	protected class NormalState : State<FSMChasePlayer> {
		
		public NormalState(FSMChasePlayer owner) : base(owner)
		{
			
		}
		
		public override void Tick ()
		{
			float v = Input.GetAxisRaw("Vertical");
			float h = Input.GetAxisRaw("Horizontal");
			
			Vector3 direction = new Vector3(h, 0f, v);
			if (direction.sqrMagnitude != 0) direction.Normalize();
			
			CharacterController cc = mp_owner.GetComponent<CharacterController>();
			cc.SimpleMove(direction * mp_owner.normalSpeed);
		}
		
		public override void EnterState ()
		{
			
		}
		
		public override void ExitState ()
		{
			
		}
	}
	
	protected class HulkState : State<FSMChasePlayer> {
		
		public HulkState(FSMChasePlayer owner) : base(owner)
		{
			
		}
		
		public override void Tick ()
		{
			float v = Input.GetAxisRaw("Vertical");
			float h = Input.GetAxisRaw("Horizontal");
			
			Vector3 direction = new Vector3(h, 0f, v);
			if (direction.sqrMagnitude != 0) direction.Normalize();
			
			CharacterController cc = mp_owner.GetComponent<CharacterController>();
			cc.SimpleMove(direction * mp_owner.hulkSpeed);
		}
		
		public override void EnterState ()
		{
			Debug.Log("You won't like me when I'm angry...");
			FSMChaseWorldManager.current.BroadcastMessage("PlayerEnterHulkMode");
			mp_owner.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
			mp_owner.transform.position = mp_owner.transform.position + new Vector3(0f, 0.2f, 0f);
			mp_owner.renderer.material.color = mp_owner.hulkColor;
		}
		
		public override void ExitState ()
		{
			Debug.Log("Whoops, thought about puppies.");
			FSMChaseWorldManager.current.BroadcastMessage("PlayerExitHulkMode");
			mp_owner.transform.localScale = new Vector3(1f, 1f, 1f);
			mp_owner.renderer.material.color = mp_owner.normalColor;
		}
	}
}
