using UnityEngine;
using System.Collections;

public class Group : MonoBehaviour {
	float lastFall = 0;
	private App_Touch.TouchInfo touch;
	private Vector3 touchStartPos;
	private Vector3 touchEndPos;
	private App_Touch.FlickInfo flick = App_Touch.FlickInfo.none;
	// Use this for initialization
	void Start () {
		if (!isValidGridPos ()) {
			Debug.Log ("Game OVER");
			Destroy (gameObject);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		touch = App_Touch.GetTouch ();
		if (touch == App_Touch.TouchInfo.Began) {
			touchStartPos = App_Touch.GetTouchPosition ();
		}
		if (touch == App_Touch.TouchInfo.Ended) {
			touchEndPos = App_Touch.GetTouchPosition ();
			flick = App_Touch.FlickDirection(touchStartPos, touchEndPos);
			Debug.Log(flick);
	
		}


		if (Input.GetKeyDown (KeyCode.LeftArrow) ||
		    (flick == App_Touch.FlickInfo.left)) {
			transform.position += new Vector3 (-1, 0, 0);
			if (isValidGridPos ()) {
				updateGrid ();
			} else {
				transform.position += new Vector3 (1, 0, 0);
			}
			flick = App_Touch.FlickInfo.none;
		} else if (Input.GetKeyDown (KeyCode.RightArrow) ||
		           (flick == App_Touch.FlickInfo.right)) {
			transform.position += new Vector3 (1, 0, 0);
			if (isValidGridPos ()) {
				updateGrid ();
			} else {
				transform.position += new Vector3 (-1, 0, 0);
			}
			flick = App_Touch.FlickInfo.none;
		} else if (Input.GetKeyDown (KeyCode.UpArrow) ||
		           (flick == App_Touch.FlickInfo.up)) {
			transform.Rotate (0, 0, -90);
			if (isValidGridPos ()) {
				updateGrid ();
			} else {
				transform.Rotate (0, 0, 90);
			}
			flick = App_Touch.FlickInfo.none;
		} else if (Input.GetKeyDown (KeyCode.DownArrow) ||
			(Time.time - lastFall >= 1) ||
		           (flick == App_Touch.FlickInfo.down)) {
			transform.position += new Vector3 (0, -1, 0);
			if (isValidGridPos ()) {
				updateGrid ();
			} else {
				transform.position += new Vector3 (0, 1, 0);
				Grid.deleteFullRows ();
				FindObjectOfType<Spawner> ().spawnNext ();
				enabled = false;
			}
			lastFall = Time.time;
			flick = App_Touch.FlickInfo.none;
		}
	}
	bool isValidGridPos(){
		foreach (Transform child in transform) {
			Vector2 v = Grid.roundVec2 (child.position);

			if (!Grid.insideBorder (v)) {
				return false;
			}
			if (Grid.grid [(int)v.x, (int)v.y] != null &&
				Grid.grid [(int)v.x, (int)v.y].parent != transform) {
				return false;
			}
		}
		return true;
	}
	void updateGrid(){
		for (int y = 0; y < Grid.h; ++y) {
			for (int x = 0; x < Grid.w; ++x) {
				if (Grid.grid [x, y] != null) {
					if (Grid.grid [x, y].parent == transform) {
						Grid.grid [x, y] = null;
					}
				}
			}
		}
		foreach (Transform child in transform) {
			Vector2 v = Grid.roundVec2 (child.position);
			Grid.grid [(int)v.x, (int)v.y] = child;
		}
	}
}
