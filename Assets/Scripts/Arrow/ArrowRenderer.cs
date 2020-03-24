using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRenderer : MonoBehaviour
{
	public float height = 0.5f;
	public float segmentLength = 0.5f;
	public float fadeDistance = 0.35f;
	public float speed = 1f;
	
	[SerializeField]
	GameObject arrowPrefab;
	[SerializeField]
	GameObject segmentPrefab;
	
	[Space]
	[SerializeField]
	Vector3 start;
	[SerializeField]
	Vector3 end;
	[SerializeField]
	Vector3 upwards = Vector3.up;

	Transform arrow;
	List<Transform> segments = new List<Transform>();

	public void SetPositions(Vector3 start, Vector3 end)
	{
		this.start = start;
		this.end = end;
	}
	
	void Update() {
		UpdateSegments();
	}
	
	void UpdateSegments()
	{
		Debug.DrawLine(start, end, Color.yellow);
		
		float distance = Vector3.Distance(start, end);
		float radius = height / 2f + distance * distance / (8f * height);
		float diff = radius - height;
		float angle = 2f * Mathf.Acos(diff / radius);
		float length = angle * radius;
		float segmentAngle = segmentLength / radius * Mathf.Rad2Deg;
		
		Vector3 center = new Vector3(0, -diff, distance / 2f);
		Vector3 left = Vector3.zero;
		Vector3 right = new Vector3(0, 0, distance);
		
		int segmentsCount = (int) (length / segmentLength) + 1;
		
		CheckSegments(segmentsCount);
		
		float offset = Time.time * speed * segmentAngle;
		Vector3 firstSegmentPos = Quaternion.Euler(Mathf.Repeat(offset, segmentAngle), 0f, 0f) * (left - center) + center;
		
		float fadeStartDistance = (Quaternion.Euler(segmentAngle / 2f, 0f, 0f) * (left - center) + center).z;
		
		for (int i = 0; i < segmentsCount; i++)
		{
			Vector3 pos = Quaternion.Euler(segmentAngle * i, 0f, 0f) * (firstSegmentPos - center) + center;
			segments[i].localPosition = pos;
			segments[i].localRotation = Quaternion.FromToRotation(Vector3.up, pos - center);
			MeshRenderer rend = segments[i].GetComponent<MeshRenderer>();
			
			if (!rend)
				continue;

			Color color = rend.material.GetColor("_Color");
			color.a = GetAlpha(pos.z - left.z, right.z - fadeDistance - pos.z, fadeStartDistance);
			rend.material.SetColor("_Color", color);
		}

		if (!arrow)
			arrow = Instantiate(arrowPrefab, transform).transform;

		arrow.localPosition = right;
		arrow.localRotation = Quaternion.FromToRotation(Vector3.up, right - center);
		
		transform.position = start;
		transform.rotation = Quaternion.LookRotation(end - start, upwards);
	}
	
	void CheckSegments(int segmentsCount)
	{
		while (segments.Count < segmentsCount)
			segments.Add(Instantiate(segmentPrefab, transform).transform);

		for (int i = 0; i < segments.Count; i++)
		{
			GameObject segment = segments[i].gameObject;
			if (segment.activeSelf != i < segmentsCount)
				segment.SetActive(i < segmentsCount);
		}
	}
	
	static float GetAlpha(float distance0, float distance1, float distanceMax)
	{
		return Mathf.Clamp01(distance0 / distanceMax) + Mathf.Clamp01(distance1 / distanceMax) - 1f;
	}
	
}
