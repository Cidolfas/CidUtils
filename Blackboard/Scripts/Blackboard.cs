using UnityEngine;
using System.Collections;

public class Blackboard {
	
	public Hashtable data = new Hashtable();
	
	public Blackboard() {}
	
	public void Put(string name, object info)
	{
		data[name] = info;
	}
	
	public object Look(string name)
	{
		if (!data.ContainsKey(name)) {
			Debug.LogWarning("Blackboard: look key not found: " + name);
			return null;
		}
		
		try
		{
			object obj = data[name];
			return obj;
		}
		catch
		{
			Debug.LogWarning("Blackboard: info for key '" + name + "' is not expected type");
			return null;
		}
	}
	
	public void Remove(string name)
	{
		data.Remove(name);
	}
	
	public void Clear()
	{
		data.Clear();
	}
	
}
