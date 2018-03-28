using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {


	public float throttle = 0f;
	
	public float forward_thrust = 2f;
	public float backward_thrust = 1.5f;
	public float acceleration = 1.5f;

	public float pitch_yaw_thrust = 1f;
	public float roll_thrust = 1f;

	public bool invert_y = true;

	public bool use_exterior_camera = false;


	private SoftwareCursor swc;
	private Rigidbody rb;
	private Camera interior_camera;
	private Camera exterior_camera;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();

		swc = GameObject.Find("SoftwareCursor").GetComponent<SoftwareCursor>();

		interior_camera = Camera.main;
		interior_camera.enabled = true;

		exterior_camera = GameObject.Find("ExteriorCamera").GetComponent<Camera>();
		exterior_camera.enabled = false;

		rb.centerOfMass = interior_camera.transform.position;
	}

	void Update(){

		if(Input.GetKeyUp("=")){
			SwapCamera();
		}

		if(Input.GetKey("w")){
			throttle += 0.01f;
			throttle = Mathf.Clamp(throttle, -1, 1);
		}	
		else if(Input.GetKey("s")){
			throttle -= 0.01f;
			throttle = Mathf.Clamp(throttle, -1, 1);
		}	
		else if(Input.GetKey("x")){
			throttle = 0f;
		}

		if(throttle > 0){
			rb.AddRelativeForce( Vector3.forward * throttle * forward_thrust);
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, forward_thrust);
			rb.drag = 0f;
		}
		else if (throttle < 0){
			rb.AddRelativeForce(-Vector3.forward * throttle * backward_thrust);
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, backward_thrust);
			rb.drag = 0f;
		}
		else{
			rb.drag = 1f;
		}

		var dampen_rotation = false;
		if(swc.cursor_magnitude.magnitude > 0.05){
			var cursor_magnitude = swc.cursor_magnitude;
			Debug.Log(cursor_magnitude);
			
			if (invert_y){
				cursor_magnitude.y *= -1;
			}

			var torque_vector = new Vector3(cursor_magnitude.y, cursor_magnitude.x, 0f);

			rb.AddRelativeTorque(torque_vector * pitch_yaw_thrust);
		}
		else{
			dampen_rotation = true;
		}


		if(Input.GetKey("q")){
			rb.AddRelativeTorque(Vector3.forward * pitch_yaw_thrust);
		}
		else if(Input.GetKey("e")){
			rb.AddRelativeTorque(-Vector3.forward * pitch_yaw_thrust);
		}
		else{
			dampen_rotation = true;
		}

		if(dampen_rotation){
			rb.angularDrag = 5f;
		}
		else{
			rb.angularDrag = 1f;
		}

	}
	

	void SwapCamera(){
		if(!use_exterior_camera){
			use_exterior_camera = true;
			interior_camera.enabled = false;
			exterior_camera.enabled = true;
		}
		else{
			use_exterior_camera = false;
			interior_camera.enabled = true;
			exterior_camera.enabled = false;
		}
	}
 
       
}
