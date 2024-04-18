﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This defines a "piece" of the track. This is attached to the prefab and contains data such as what obstacles can spawn on it.
/// It also defines places on the track where obstacles can spawn. The prefab is placed into a ThemeData list.
/// </summary>
public class TrackSegment : MonoBehaviour
{
    public Transform[] pathParents;

    public TrackManager manager;

    public List<Vector3> obstaclePositionsInWorld;
    
	public Transform objectRoot;
	public Transform collectibleTransform;

    public AssetReference[] possibleObstacles;

    public int selectedPathIndex = 0;

    public delegate void PathDecisionEvent(bool success);
    public static event PathDecisionEvent OnPathDecision;

    [HideInInspector]
    public float[] obstaclePositions;

    public float worldLength { get { return m_WorldLength; } }

    protected float m_WorldLength;

    void Update()
    {

        // Check if the current segment allows for a path change

        if (gameObject.name.Contains("Road left"))
        {
            // Assuming the device is being held upright in a landscape orientation
            // You may need to adjust the axis based on how you expect the device to be held
            float accelerationX = Input.acceleration.x;

            // Detect left tilt; negative x acceleration might indicate tilting to the left
            if (accelerationX < -0.5f)
            { // Threshold value, adjust based on your testing
                selectedPathIndex = 0;
                Debug.Log("Detected left tilt, path index set to 0.");
            }
        }
        else if (gameObject.name.Contains("Road right"))
        {
            // Assuming the device is being held upright in a landscape orientation
            // You may need to adjust the axis based on how you expect the device to be held
            float accelerationX = Input.acceleration.x;

            // Detect right tilt; positive x acceleration might indicate tilting to the right
            if (accelerationX > 0.5f)
            { // Threshold value, adjust based on your testing
                selectedPathIndex = 0; // Assuming 1 is the index for the right path
                Debug.Log("Detected right tilt, path index set to 1.");
            }
        }
    }

        void OnEnable()
    {
        UpdateWorldLength();

		GameObject obj = new GameObject("ObjectRoot");
		obj.transform.SetParent(transform);
		objectRoot = obj.transform;

		obj = new GameObject("Collectibles");
		obj.transform.SetParent(objectRoot);
		collectibleTransform = obj.transform;
    }

    // Same as GetPointAt but using an interpolation parameter in world units instead of 0 to 1.
    public void GetPointAtInWorldUnit(float wt, out Vector3 pos, out Quaternion rot)
    {
        float t = wt / m_WorldLength;
        GetPointAt(t, out pos, out rot);
    }


	// Interpolation parameter t is clamped between 0 and 1.
	public void GetPointAt(float t, out Vector3 pos, out Quaternion rot)
    {
        if (pathParents == null || pathParents.Length == 0)
        {
            Debug.LogWarning("pathParents is either not initialized or empty.");
            pos = Vector3.zero;
            rot = Quaternion.identity;
            return;
        }

        if (selectedPathIndex < 0 || selectedPathIndex >= pathParents.Length)
        {
            Debug.LogError($"selectedPathIndex {selectedPathIndex} is out of bounds for pathParents array.");
            pos = Vector3.zero;
            rot = Quaternion.identity;
            return;
        }
        float clampedT = Mathf.Clamp01(t);
        float scaledT = (pathParents[selectedPathIndex].childCount - 1) * clampedT;
        int index = Mathf.FloorToInt(scaledT);
        float segmentT = scaledT - index;

        Transform orig = pathParents[selectedPathIndex].GetChild(index);
        if (index == pathParents[selectedPathIndex].childCount - 1)
        {
            pos = orig.position;
            rot = orig.rotation;
            return;
        }

        Transform target = pathParents[selectedPathIndex].GetChild(index + 1);

        pos = Vector3.Lerp(orig.position, target.position, segmentT);
        rot = Quaternion.Lerp(orig.rotation, target.rotation, segmentT);
    }

    protected void UpdateWorldLength()
    {
        if (pathParents == null || pathParents.Length == 0)
        {
            Debug.LogWarning("pathParents is either not initialized or empty.");
            return;
        }

        // Check if selectedPathIndex is within the bounds of pathParents array
        if (selectedPathIndex < 0 || selectedPathIndex >= pathParents.Length)
        {
            Debug.LogWarning($"selectedPathIndex {selectedPathIndex} is out of bounds for pathParents array.");
            return;
        }
        m_WorldLength = 0;

        for (int i = 1; i < pathParents[selectedPathIndex].childCount; ++i)
        {
            Transform orig = pathParents[selectedPathIndex].GetChild(i - 1);
            Transform end = pathParents[selectedPathIndex].GetChild(i);

            Vector3 vec = end.position - orig.position;
            m_WorldLength += vec.magnitude;
        }
    }

	public void Cleanup()
	{
		while(collectibleTransform.childCount > 0)
		{
			Transform t = collectibleTransform.GetChild(0);
			t.SetParent(null);
            Coin.coinPool.Free(t.gameObject);
		}

	    Addressables.ReleaseInstance(gameObject);
	}

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (pathParents == null || pathParents.Length == 0)
            return;

        Color c = Gizmos.color;
        Gizmos.color = Color.red;
        foreach (Transform pathParent in pathParents)
        {
            if (pathParent == null) continue;

            // Draw the path
            Gizmos.color = pathParent == pathParents[selectedPathIndex] ? Color.green : Color.red;
            for (int i = 1; i < pathParent.childCount; ++i)
            {
                Transform orig = pathParent.GetChild(i - 1);
                Transform end = pathParent.GetChild(i);

                Gizmos.DrawLine(orig.position, end.position);
            }
        }

        Gizmos.color = Color.blue;
        for (int i = 0; i < obstaclePositions.Length; ++i)
        {
            Vector3 pos;
            Quaternion rot;
            GetPointAt(obstaclePositions[i], out pos, out rot);
            Gizmos.DrawSphere(pos, 0.5f);
        }

        Gizmos.color = c;
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(TrackSegment))]
class TrackSegmentEditor : Editor
{
    protected TrackSegment m_Segment;

    public void OnEnable()
    {
        m_Segment = target as TrackSegment;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Add obstacles"))
        {
            ArrayUtility.Add(ref m_Segment.obstaclePositions, 0.0f);
        }

        if (m_Segment.obstaclePositions != null)
        {
            int toremove = -1;
            for (int i = 0; i < m_Segment.obstaclePositions.Length; ++i)
            {
                GUILayout.BeginHorizontal();
                m_Segment.obstaclePositions[i] = EditorGUILayout.Slider(m_Segment.obstaclePositions[i], 0.0f, 1.0f);
                if (GUILayout.Button("-", GUILayout.MaxWidth(32)))
                    toremove = i;
                GUILayout.EndHorizontal();
            }

            if (toremove != -1)
                ArrayUtility.RemoveAt(ref m_Segment.obstaclePositions, toremove);
        }
    }
}

#endif