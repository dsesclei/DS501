using UnityEngine;
using System.Collections;

public class Task_Rotate_Object : Task
{
	public GameObject targetObject;
	public GameObject finishedObject;

	public GameObject referenceObject;

	public Task_Rotate_Object(GameObject targetObject, GameObject finishedObject)
	{
		this.targetObject = targetObject;
		this.finishedObject = finishedObject;
		this.referenceObject = GameObject.Instantiate(targetObject);
	}
	
	public void start()
	{
		targetObject.SetActive(true);
		referenceObject.SetActive(true);
		finishedObject.SetActive(true);

		targetObject.transform.position = new Vector3(8, 0, 0);
		referenceObject.transform.position = new Vector3(-8, 0, 0);

		referenceObject.transform.Rotate(new Vector3(50, 20, 120));

		misc.setColor (targetObject, Color.yellow);
		misc.setColor (referenceObject, Color.yellow);
		misc.setColor (finishedObject, Color.red);
	}

	public void rotate(Vector3 dragDelta) {
		Quaternion fromRotation = targetObject.transform.rotation;
		Quaternion toRotation = Quaternion.Euler(Input.mousePosition.x, Input.mousePosition.y, 0);
		targetObject.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, 100);
	}
	
	public void end()
	{
		targetObject.SetActive(false);
		finishedObject.SetActive(false);
	}
}