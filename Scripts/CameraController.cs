using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	public Camera[] cameras;// cameras in scene
	public GameObject[] cameraOrigins; // reset point of cameras

	public float MovementSpeed = 1.0f;// camera move speed
	
	// movement check
	private bool mForward = false;
	private bool mBack = false;
	private bool mLeft = false;
	private bool mRight = false;

	[HideInInspector]
	public Camera currentCam;
	private int currentCameraIndex;
	private TowerManager towerManager;
	private CameraBounds currentBounds;
	 
	 void Awake () 
	 {
		 if ((towerManager = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<TowerManager>()) == null)
			 Debug.Log("Error: TowerManager could not be found on Game Logic, in Camera Controller.");

		 if (cameras.Length != cameraOrigins.Length)
		 {
			 Debug.Log("Error: Missing cameras or origins in CameraController!");
		 }
		 currentCameraIndex = 0;
		 
		 //Turn all cameras off, except the first default one
		 for (int i=1; i<cameras.Length; i++) 
		 {
			 cameras[i].gameObject.SetActive(false);
		 }
		 
		 //If any cameras were added to the controller, enable the first one
		 if (cameras.Length>0)
		 {
			 currentCam = cameras[0];
			 currentCam.gameObject.SetActive(true);
			 //currentCam.transform.position = cameraOrigins[0].transform.position;
			 currentBounds = currentCam.gameObject.GetComponent<CamLimitScript>().bounds;
		 }

		 currentCam.transform.position = cameraOrigins[0].transform.position;
		 cameras[1].transform.position = cameraOrigins[1].transform.position;
	 }
	 
	 // Update is called once per frame
	 void Update () {

		 //If either shift button is pressed, switch to the next camera
		 if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && towerManager.hasPlaced)
		 {
			 // Disable current, switch to next OR first camera && enable/disable tower buttons
			 cameras[currentCameraIndex].gameObject.SetActive(false);
			 currentCameraIndex ++;
			 if (currentCameraIndex < cameras.Length)
			 {
				 currentCam = cameras[currentCameraIndex]; // set current camera control
				 //currentCam.transform.position = cameraOrigins[currentCameraIndex].transform.position; // set camera to origin
				 currentCam.gameObject.SetActive(true);
				 currentBounds = currentCam.gameObject.GetComponent<CamLimitScript>().bounds;
			 }
			 else
			 {
				 currentCameraIndex = 0;
				 currentCam = cameras[currentCameraIndex];
				 //currentCam.transform.position = cameraOrigins[currentCameraIndex].transform.position;
				 currentCam.gameObject.SetActive(true);
				 currentBounds = currentCam.gameObject.GetComponent<CamLimitScript>().bounds;
			 }
		 }

		 // set bool for move state on key code
		 if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			mForward = true;
		else if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
			mForward = false;
		
		if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
			mBack = true;
		else if(Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
			mBack = false;
		
		if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			mLeft = true;
		else if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
			mLeft = false;
		
		if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			mRight = true;
		else if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
			mRight = false;

		 //Set camera movement:
		if (mForward && cameras[currentCameraIndex].transform.position.z <= currentBounds.max_z)
			 cameras[currentCameraIndex].transform.position += (transform.forward * Time.deltaTime * MovementSpeed);

		if (mBack && cameras[currentCameraIndex].transform.position.z >= currentBounds.min_z)
			cameras[currentCameraIndex].transform.position += (-transform.forward * Time.deltaTime * MovementSpeed);

		if (mLeft && cameras[currentCameraIndex].transform.position.x >= currentBounds.min_x)
			cameras[currentCameraIndex].transform.position += (-transform.right * Time.deltaTime * MovementSpeed);

		if (mRight && cameras[currentCameraIndex].transform.position.x <= currentBounds.max_x)
			cameras[currentCameraIndex].transform.position += (transform.right * Time.deltaTime * MovementSpeed);
	 }
}
