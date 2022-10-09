using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
	[Header("General Settings")]
	public GameObject entityPrefab;

	public GameObject spawn;

	[Range(0, 50)]
	public int numFlockingEntities;

	public GameObject[] allFlockingEntities;
	public GameObject entitiesTarget;
	public Vector3 limits;

	[Header("Flocking Entity Settings")]

	[Range(0.0f, 20.0f)]
	public float minSpeed;

	[Range(0.0f, 20.0f)]
	public float maxSpeed;

	public float neighbourDistance;

	[Range(0.0f, 5.0f)]
	public float rotationSpeed;

	// Start is called before the first frame update
	void Start()
	{
		allFlockingEntities = new GameObject[numFlockingEntities];
		for (int i = 0; i < numFlockingEntities; ++i)
		{
			Vector3 pos = RandomPosition();

			Vector3 randomize = RandomDirection();

			allFlockingEntities[i] = (GameObject)Instantiate(entityPrefab, pos, Quaternion.LookRotation(randomize));
			allFlockingEntities[i].GetComponent<FlockEntities>().myManager = this;
		}
	}

	// Update is called once per frame
	void Update()
	{
	}

	Vector3 RandomPosition()
	{
		Vector3 pos;
		pos.x = Random.Range(spawn.transform.position.x - limits.x, spawn.transform.position.x + limits.x);
		pos.y = Random.Range(spawn.transform.position.y - limits.y, spawn.transform.position.y + limits.y);
		pos.z = Random.Range(spawn.transform.position.z - limits.z, spawn.transform.position.z + limits.z);
		return pos;
	}

	Vector3 RandomDirection()
	{
		Vector3 dir;
		dir.x = Random.Range(-1.0f, 1.0f);
		dir.y = Random.Range(-1.0f, 1.0f);
		dir.z = Random.Range(-1.0f, 1.0f);
		dir.Normalize();
		return dir;
	}
}
