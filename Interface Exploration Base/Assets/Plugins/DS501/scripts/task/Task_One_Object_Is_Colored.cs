using UnityEngine;
using System.Collections;


public class Task_One_Object_Is_Colored : Task
{
	public bool isRunning = false;

	// objects to select from
	public GameObject target_group;
	public GameObject target_object;
	public GameObject resetObject;

	private GameObject[] children;

	public Task_One_Object_Is_Colored( GameObject target_group, GameObject resetObject )
	{
		this.target_group = target_group;
		this.resetObject = resetObject;
	}

	// call to start the task
	public void start ()
	{
		Debug.Log ("Task Select Object: start()");
		isRunning = true;

		resetObject.SetActive (true);
		Renderer resetRenderer = resetObject.GetComponent<Renderer> ();
		resetRenderer.material.color = Color.red;

		target_group.SetActive (true);
	}

	public void onResetSelected ()
	{
		Renderer resetRenderer = resetObject.GetComponent<Renderer> ();
		resetRenderer.material.color = Color.yellow;

		// get array of all children
		// NOTE: this is apparently pretty awkward to do in Unity:
		// http://answers.unity3d.com/questions/594210/get-all-children-gameobjects.html
		// http://forum.unity3d.com/threads/hiow-to-get-children-gameobjects-array.142617/
		// TODO: the second link had a way to add this as a method to GameObject
		children = new GameObject[target_group.transform.childCount];
		for ( int i = 0; i < target_group.transform.childCount; i++ ) 
		{
			children[i] = target_group.transform.GetChild( i ).gameObject;
		}
		
		// select a random item from the group
		int num_children = children.Length;
		int selected = Mathf.FloorToInt(Random.value * num_children);
		
		// record the selection
		target_object = children[selected];
		
		// set the colors of everything in the group to "not selected"
		foreach (GameObject child in children) 
		{
			foreach( Renderer r in child.GetComponents<Renderer>() )
			{
				foreach( Material m in r.materials )
				{
					m.color = Color.yellow;	
				}
			}
		}
		
		// set the color of the selected item to "selected"
		foreach( Renderer r in target_object.GetComponents<Renderer>() )
		{
			foreach( Material m in r.materials )
			{
				m.color = Color.red;
			}
		}
	}

	public void end ()
	{
		isRunning = false;
		resetObject.SetActive (false);
		target_group.SetActive (false);

		Renderer resetRenderer = resetObject.GetComponent<Renderer> ();
		resetRenderer.material.color = Color.red;
		
		// set the colors of everything in the group to "not selected"
		foreach( Renderer r in target_object.GetComponents<Renderer>() )
		{
			foreach( Material m in r.materials )
			{
				m.color = Color.yellow;
			}
		}
	}
}
