using UnityEngine;
using System;
using System.Collections;

public class Base_Run_Rotate : Run
{
	
	public GameObject targetObject;
	public GameObject finishedObject;
	
	protected Task_Rotate_Object task = null;
	
	//protected Record_Selection selections = null;

	public override void Start()
	{
		base.Start();
		task = new Task_Rotate_Object(targetObject, finishedObject);
	}
	
	public virtual void onDrag(Vector3 dragDelta)
	{
		task.rotate (dragDelta);
	}

	public virtual void onSelect(GameObject selected)
	{
		if (selected == task.finishedObject) {
			float dotProduct = Quaternion.Dot(task.referenceObject.transform.rotation, task.targetObject.transform.rotation);
			float similarity = 1 - dotProduct;
			Debug.Log ("Similarity: " + similarity.ToString());
		}
	}

}
