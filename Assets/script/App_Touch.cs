using UnityEngine;
using System.Collections;

public class App_Touch : MonoBehaviour {
	private static Vector3 TouchPosition = Vector3.zero;
	private static Vector3 Position;
	private static Vector3 Worldposition;


	public enum TouchInfo{
		None = 99,
		Began = 0,
		Moved = 1,
		Stationary = 2,
		Ended = 3,
		Canceled = 4
	}

	public enum FlickInfo{
		right = 0,
		left = 1,
		up = 2,
		down = 3,
		touch = 4,
		none = 5
	}

	public static TouchInfo GetTouch(){
		if (Application.isEditor) {
			if (Input.GetMouseButtonDown (0)) {
				return TouchInfo.Began;
			}
			if (Input.GetMouseButton (0)) {
				return TouchInfo.Moved;
			}
			if (Input.GetMouseButtonUp (0)) {
				return TouchInfo.Ended;
			}
		} else {
			if (Input.touchCount > 0) {
				return (TouchInfo)((int)Input.GetTouch (0).phase);
			}
		}
		return TouchInfo.None;
	}
	public static Vector3 GetTouchPosition(){
		if (Application.isEditor) {
			TouchInfo touch = App_Touch.GetTouch ();
			if (touch != TouchInfo.None) {
				Position = Input.mousePosition;
				Position.z = 20.0f;
				Worldposition = Camera.main.ScreenToWorldPoint(Position);
				return Worldposition;

			}
		} else {
			if (Input.touchCount > 0) {
				Touch touch = Input.GetTouch (0);
				TouchPosition.x = touch.position.x;
				TouchPosition.y = touch.position.y;
				TouchPosition.z = 20.0f;
				Worldposition = Camera.main.ScreenToWorldPoint(TouchPosition);
				return Worldposition;
			}
		}
		return Vector3.zero;
	}
	public static FlickInfo FlickDirection(Vector3 flickStartPos, Vector3 flickEndPos){
		float directionX = flickEndPos.x - flickStartPos.x;
		float directionY = flickEndPos.y - flickStartPos.y;

		if (Mathf.Abs (directionY) < Mathf.Abs (directionX)) {
			if (3 < directionX) {
				return FlickInfo.right;
			} else if (-3 > directionX) {
				return FlickInfo.left;
			}
		} else if (Mathf.Abs (directionX) < Mathf.Abs (directionY)) {
			if (3 < directionY) {
				return FlickInfo.up;
			} else if (-3 > directionY) {
				return FlickInfo.down;
			}
		}
		Debug.Log (directionX);
		Debug.Log (directionY);

		return FlickInfo.touch;
	}



}




