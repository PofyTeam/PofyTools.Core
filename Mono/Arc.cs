//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class Arc : MonoBehaviour
//{
//	public const float ArcCQ = 1.0708f;
////	private BoxCollider _collider = null;
//
////	public BoxCollider collider {
////		get {
////			if (_collider == null)
////				_collider = gameObject.AddComponent<BoxCollider> ();
////			return _collider;
////		}
////	}
//
//	public enum Origin : int
//	{
//		START = 1,
//		MIDDLE = 0,
//		END = -1,
//	}
//
//	public List<Currency> spawnedObjects = new List<Currency> ();
//	[ContextMenu("Spawn Arc")]
//	public void SpawnArc (float length, float height, float objectsPerUnit = 1, Origin origin = Origin.MIDDLE)
//	{
//		if (length == 0) {
//			length = 1f;
//		}
//
//		float originZOffset = length * 0.5f * (float)origin;
//		
//		float sumAB = length + height;
//
//		//HACK Fast Ellipse Circumference
//		float positiveCircumferenceLength = sumAB * ArcCQ + sumAB; 
//
//		float theta = 0; // angle to increment
//		int totalIncrements = (int)(positiveCircumferenceLength * objectsPerUnit);
//		float deltaAngle = Mathf.PI / totalIncrements;
//		float localRotationY = 0f;
//		//float pitch = 1f + (totalIncrements * PITCH_OFFSET);
//		while (theta < Mathf.PI || Mathf.Approximately(theta, Mathf.PI)) {
//			float z = length * Mathf.Cos (theta) + originZOffset;
//			float y = height * Mathf.Sin (theta);
//			Vector3 newPosition = new Vector3 (0f, y, z);
//			//float clampedPitch = Mathf.Min (2f, pitch);
//			SpawnCurrency (newPosition, localRotationY, true);
//
//			localRotationY += 10f;
//			//pitch -= PITCH_OFFSET;
//
//			theta += deltaAngle;
//		}
//	}
//	public const float PITCH_OFFSET = 0.015f;
//	public Currency SpawnCurrency (Vector3 position, float rotationAngle = 0f, bool isInPitchChain = false)
//	{
//		Currency newCurrency = Pickup.Create ("SoftCurrency") as Currency;
//		newCurrency.ResetFromPool ();
//
//		this.spawnedObjects.Add (newCurrency);
//
//
//		newCurrency.transform.SetParent (this.transform);
//
//		newCurrency.transform.localPosition = position;
//
//		newCurrency.transform.localScale = Vector3.one;
//
//		newCurrency.transform.localEulerAngles = Vector3.up * rotationAngle;
//
//		//newCurrency.pitch = pitch;
//		newCurrency.isPitchChained = isInPitchChain;
//		return newCurrency;
//	}
//
//	public void FreeAll ()
//	{
//		foreach (Currency currency in this.spawnedObjects)
//			//currency.pool.Free (currency, currency.id);
//			Pickup.FreeToPool (currency);
//
//		this.spawnedObjects.Clear ();
//	}
//
//	private float _jumpTimer;
//	private float _lastJumpCurveEvaluation;
//	private float _currentZ;
//	[ContextMenu ("Spawn Jump Trajectory")]
//	public void SpawnJumpTrajectory (float speed, float waitTime, float jumpHeight, float jumpDuration, float objectPerUnit = 2)
//	{
//		if (waitTime >= jumpDuration || jumpDuration == 0f || objectPerUnit == 0)
//			return;
//		float totalDistance = speed * jumpDuration;
//		int totalIcrements = (int)Mathf.Ceil (totalDistance * objectPerUnit);
//		float timeIncrement = jumpDuration / totalIcrements;
//
//		this._jumpTimer = waitTime;
//		float localRotationY = 0f;
//		//float pitch = 1f;
//		while (this._jumpTimer < jumpDuration) {
//			this._jumpTimer += timeIncrement;
//			float normalizedJumpTime = Mathf.Clamp01 (this._jumpTimer / jumpDuration);
//			float y = GameRuntime.instance.jumpCurve.Evaluate (normalizedJumpTime) * jumpHeight;
//			float z = normalizedJumpTime * jumpDuration * speed;
//
//			//TODO: will spawn on each fixed frame
//			Vector3 newPosition = new Vector3 (0f, y, z);
//
//			SpawnCurrency (newPosition, localRotationY, true);//, pitch);
//			//pitch += PITCH_OFFSET;
//
//			localRotationY += 10f;
//		}
//	}
//
//	[ContextMenu("Spawn Sky Line")]
//	public void SpawnSkyLine (float length, float height, float forwardOffset, int initalCount = 10, float chanceToKeepLane = 0.5f, float objectsPerUnit = 1)
//	{
//		if (length == 0) {
//			length = 1f;
//		}
//
//		int totalIncrements = (int)Mathf.Ceil (length * objectsPerUnit);
//		float lengthSegment = length / totalIncrements;
//		float localRotationY = 0f;
//
//		int i = 0;
//		Lane lane0 = PlayerController.player.activeWaypoint.lane;
//		Lane lane1 = lane0;
//
//		float x = 0;
//		float z = 0;
//		float y = 0;
//		Vector3 newPosition = new Vector3 (x, y, z);
//
//		//float pitch = 1 + PITCH_OFFSET * totalIncrements;
//
//		while (totalIncrements > 0) {
//
//			Lane newLane = lane1;
//
//			if (totalIncrements > initalCount && i >= initalCount && lane1 == lane0 && Random.Range (0f, 1f) > chanceToKeepLane) {
//				newLane = Lane.MIDDLE;
//
//				if (lane1 == Lane.MIDDLE) {
//					newLane = Lane.LEFT;
//					if (Random.Range (0, 1f) > 0.5f)
//						newLane = Lane.RIGHT;
//				}
//				i = 0;
//
//				//mid lane coina
//				x = ((float)(((int)newLane >> 1) + ((int)lane1 >> 1)) / 2f) - 1;
//				z = lengthSegment * (totalIncrements + 0.5f) + forwardOffset;
//				y = height;
//				newPosition = new Vector3 (x, y, z);
//				//float clampedPitch = Mathf.Min (2f, pitch);
//				SpawnCurrency (newPosition, localRotationY, true);//, clampedPitch);
//				localRotationY += 10f;
//				//pitch -= PITCH_OFFSET;
//			}
//
//			lane0 = lane1;
//			lane1 = newLane;
//
//			x = ((int)newLane >> 1) - 1;
//			z = lengthSegment * totalIncrements + forwardOffset;
//			y = height;
//			newPosition = new Vector3 (x, y, z);
//
//			SpawnCurrency (newPosition, localRotationY, true);
//			localRotationY += 10f;
//			--totalIncrements;
//			++i;
//		}
//	}
//
//	[ContextMenu("Spawn Line")]
//	public void SpawnLine (float length, float height, float forwardOffset, float objectsPerUnit = 2)
//	{
//		if (length == 0) {
//			length = 1f;
//		}
//		int totalIncrements = (int)Mathf.Ceil (length * objectsPerUnit);
//		float lengthSegment = length / totalIncrements;
//		float localRotationY = 0f;
//
//		while (totalIncrements > 0) {
//			float z = lengthSegment * totalIncrements + forwardOffset;
//			float y = height;
//			Vector3 newPosition = new Vector3 (0f, y, z);
//			
//			SpawnCurrency (newPosition, localRotationY);
//			
//			localRotationY += 10f;
//			--totalIncrements;
//		}
//	}
//
//#if UNITY_EDITOR
//	public void SpawnArc (float length, float height, float objectsPerUnit, Currency currency, Origin origin = Origin.MIDDLE)
//	{
//		if (length == 0) {
//			length = 1f;
//		}
//		
//		float originZOffset = length * 0.5f * (float)origin;
//		
//		float sumAB = length + height;
//		
//		//HACK Fast Ellipse Circumference
//		float positiveCircumferenceLength = sumAB * 1.0708f + sumAB; 
//		
//		float theta = 0; // angle to increment
//		int totalIncrements = (int)(positiveCircumferenceLength * objectsPerUnit);
//		float deltaAngle = Mathf.PI / totalIncrements;
//		float localRotationY = 0f;
//
//		while (theta < Mathf.PI || Mathf.Approximately(theta, Mathf.PI)) {
//			float z = length * Mathf.Cos (theta) + originZOffset;
//			float y = height * Mathf.Sin (theta);
//			Vector3 newPosition = this.transform.position + new Vector3 (0f, y, z);
//			
//			GameObject newInstance = null;
//			Currency newCurrency = null;
//			if (Application.isPlaying)
//				newCurrency = currency.CreateInstance (currency.id) as Currency;
//			else {
//				newCurrency = GameObject.Instantiate (currency.gameObject).GetComponent<Currency> ();
//				newCurrency.gameObject.SetActive (true);
//				newCurrency.collider.enabled = false;
//			}
//			newInstance = newCurrency.gameObject;
//			
//			this.spawnedObjects.Add (newCurrency);
//			
//			newInstance.transform.position = newPosition;
//			newInstance.transform.SetParent (this.transform);
//			newInstance.transform.localScale = Vector3.one;
//
//			newInstance.transform.localEulerAngles = Vector3.up * localRotationY;
//			localRotationY += 10f;
//
//			theta += deltaAngle; 
//		}
//	}
//#endif
//}
