using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftwareCursor : MonoBehaviour {

	public float mouse_radius;
	public float deadzone;

	public Vector3 relative_mouse_position = new Vector3();
	public Vector3 cursor_magnitude = new Vector3();

	private Vector3 software_mouse_position = new Vector3();


	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Confined;			
		Cursor.visible = false;

		var software_mouse_position = FindCenterScreen();
		var offset = FindDistanceFromCenterOfScreen();
		software_mouse_position += offset;

		relative_mouse_position = ConstrictReletiveMousePosition(offset);
		cursor_magnitude = GetCursorMagnitude(relative_mouse_position);
	}
	
	// Update is called once per frame
	void Update () {

		var center_screen = FindCenterScreen();
		var offset = FindDistanceFromCenterOfScreen();
		software_mouse_position = center_screen + offset;

		transform.position = software_mouse_position;

		relative_mouse_position = ConstrictReletiveMousePosition(offset);
		cursor_magnitude = GetCursorMagnitude(relative_mouse_position);
	}

	private Vector3 FindCenterScreen(){
		var mouse_pos = Input.mousePosition;

		var center_screen_x = Mathf.Floor(Screen.width/2.0f);
		var center_screen_y = Mathf.Floor(Screen.height/2.0f);
		var center_screen = new Vector3(center_screen_x, center_screen_y, 0);

		return center_screen;
	}

	private Vector3 FindDistanceFromCenterOfScreen(){
		var resolution = Screen.currentResolution;
		var mouse_pos = Input.mousePosition;

		float mouse_x_percent = Mathf.Clamp(mouse_pos.x/Screen.width, 0f, 1f);
		float mouse_y_percent = Mathf.Clamp(mouse_pos.y/Screen.height, 0f, 1f);


		var mouse_x = (Screen.width *0.5f) - (Screen.width * mouse_x_percent);
		var mouse_y = (Screen.height *0.5f) - (Screen.height * mouse_y_percent);

		var scaled_mouse_pos = - new Vector3(mouse_x, mouse_y, 0f);
		scaled_mouse_pos = Vector3.ClampMagnitude(scaled_mouse_pos, mouse_radius * Screen.height);

		return scaled_mouse_pos;
	}

	private Vector3 GetCursorMagnitude(Vector3 offset){
		var percent_magnitude_x = (offset.x / Screen.height) / mouse_radius;
		var percent_magnitude_y = (offset.y / Screen.height) / mouse_radius;

		var percent_magnitude = new Vector3(percent_magnitude_x, percent_magnitude_y, 0);


		return Vector3.ClampMagnitude(percent_magnitude, mouse_radius * Screen.height);
	}

	private Vector3 ConstrictReletiveMousePosition(Vector3 offset){
		/* 
		var percent_magnitude = offset.magnitude / Screen.height;
		if(percent_magnitude < deadzone){
			return Vector3.zero;
		}

		//rescale percentage starting at deadzone;
		var diff = (Screen.height * deadzone);

		var y_percent = (offset.y + diff) / Screen.height;
		var x_percent = (offset.x + diff) / Screen.height;

		if(y_percent > 0f && Mathf.Abs(x_percent) < 0.05f){
			offset.x = 0f;
		}

		if(y_percent < 0f && Mathf.Abs(x_percent) < 0.05f){
			offset.x = 0f;
		}

		if(x_percent < 0f && Mathf.Abs(y_percent) < 0.05f){
			offset.y = 0f;
		}

		if(x_percent > 0f && Mathf.Abs(y_percent) < 0.05f){
			offset.y = 0f;
		}

*/

		return offset;
	}

}
