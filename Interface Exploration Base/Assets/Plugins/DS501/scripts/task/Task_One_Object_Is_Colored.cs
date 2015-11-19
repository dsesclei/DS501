using UnityEngine;
using System.Collections;


public class Task_One_Object_Is_Colored : Task
{
	public bool isOver = false;

	// objects to select from
	public GameObject targetObject;
	public GameObject resetObject;
	public int numTargets = 9;
	public int targetsSelected;

	private GameObject[] children;
	private int[] order;

	public Task_One_Object_Is_Colored( GameObject targetObject, GameObject resetObject )
	{
		this.targetObject = targetObject;
		this.resetObject = resetObject;
	}

	// call to start the task
	public void start ()
	{
		isOver = false;
		targetsSelected = 0;

		order = new int[numTargets];
		for (int i = 0; i < numTargets; i++)
			order[i] = i;
		new System.Random ().Shuffle (order);

		resetObject.SetActive (true);
		setColor (resetObject, Color.red);

		targetObject.SetActive (false);
		setColor (targetObject, Color.red);
	}

	public void onTargetSelected ()
	{
		targetsSelected++;
		isOver = targetsSelected >= numTargets;
		resetObject.SetActive (true);
		targetObject.SetActive (false);
	}

	public void onResetSelected ()
	{
		Vector3 position = targetObject.gameObject.transform.position;
		// Would be nice to remove these magic numbers, but probably not worth the time
		position.x = order[targetsSelected] * 2 - 8;
		position.y = order[targetsSelected] - 3;
		targetObject.gameObject.transform.position = position;
		resetObject.SetActive (false);
		targetObject.SetActive (true);
	}

	public void end ()
	{
		isOver = true;
		resetObject.SetActive (false);
		targetObject.SetActive (false);
	}

	
	private void setColor( GameObject obj, Color color )
	{
		foreach( Renderer r in obj.GetComponents<Renderer>() )
		{
			foreach( Material m in r.materials )
			{
				m.color = color;
			}
		}
	}
}
