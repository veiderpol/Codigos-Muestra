using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ZoomAndPan : MonoBehaviour {
	private static readonly float PanSpeed = 350;
	public Camera cam , cam_2;
	bool panActive, panActive_2;
	private Vector3 lastPanPosition, lastPanPosition_2;
	private int leftFingerId, rigthFingerId;
	Vector3 leftTouchPos, rigthTouchPos;
	private Continuar continuar;
	private float minX = -20,maxX = 20, minZ= -180,maxZ = 50;
	public GameObject planeimage, planeimage_2;
	public Texture[] textures;
	public Slider slider, slider_2;

	void OnDisable(){
		if(this.gameObject.name == "Camera"){
			slider.value = 60;
		}else{
			slider_2.value = 60;
		}
	}
	void Update() {
		HandleTouch_3();
	}
	public void ChangeImage(){
		continuar = FindObjectOfType<Continuar>();
		string nombreImgActual = continuar.nombreImg;
		string nombreImgActual_2 = continuar.nombreImg_2;
		for(int i = 0 ; i < textures.Length; i++){
			if(textures[i].name == nombreImgActual){
				Renderer rend_1 = planeimage.gameObject.GetComponent<Renderer>();
				rend_1.material.mainTexture  = textures[i]; 
				Debug.Log("Entre a el primero");
			}
			 if(textures[i].name == nombreImgActual_2){
				Renderer rend_2 = planeimage_2.gameObject.GetComponent<Renderer>();
				rend_2.material.mainTexture  = textures[i]; 
				Debug.Log("Entre a el segundo");
			}
			 if(nombreImgActual == null){
				Renderer rend_1 = planeimage.gameObject.GetComponent<Renderer>();
				rend_1.material.mainTexture  = null; 
				Debug.Log("Entre a el primero null");
			}
			 if(nombreImgActual_2 == null){
				Renderer rend_2 = planeimage_2.gameObject.GetComponent<Renderer>();
				rend_2.material.mainTexture  = null; 
				Debug.Log("Entre a el segundo null");
			}
		} 
	}
	void HandleTouch_3(){
		foreach(Touch touch in Input.touches){
			if(touch.phase == TouchPhase.Began && touch.position.x <= Screen.width/2){
				leftFingerId = touch.fingerId;
				lastPanPosition = touch.position;
			}
			if(touch.fingerId == leftFingerId && touch.position.x <= Screen.width/2){
				leftTouchPos = touch.position;
				panActive = true;
				if(leftTouchPos.x <= Screen.width / 2){
					RaycastHit2D hit = Physics2D.Raycast(new Vector3(leftTouchPos.x,leftTouchPos.y,leftTouchPos.z),Vector2.zero);
					if(hit.collider.tag.Equals("ImagenArea")){
						panActive = true;
						PanCamera(new Vector3(leftTouchPos.x,leftTouchPos.y,leftTouchPos.z));
					}	
				}
			}
			if(touch.phase == TouchPhase.Began && touch.position.x > Screen.width/2){
				rigthFingerId = touch.fingerId;
				lastPanPosition_2 = touch.position;
			}
			if(touch.fingerId == rigthFingerId && touch.position.x > Screen.width/2){
				rigthTouchPos = touch.position;
				panActive_2 = true;
				if(rigthTouchPos.x > Screen.width / 2){
					RaycastHit2D hit = Physics2D.Raycast(new Vector3(rigthTouchPos.x,rigthTouchPos.y,rigthTouchPos.z),Vector2.zero);
					if(hit.collider.tag.Equals("ImagenArea")){
						panActive_2 = true;
						PanCamera_2(new Vector3(rigthTouchPos.x,rigthTouchPos.y,rigthTouchPos.z));
					}
				}
			}
		} 
	}
	public void Slider_Cam_1(float newValue){
		cam.fieldOfView = newValue;
		ClampToBounds();
	}
	void PanCamera(Vector3 newPanPosition) {
		if(!panActive){
			return;
		}
		Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
		Vector3 move = new Vector3(-1*(offset.y * PanSpeed), 0, (offset.x * PanSpeed));
		cam.transform.Translate(move, Space.World);  
		ClampToBounds();
		lastPanPosition = newPanPosition;
	}
	void PanCamera_2(Vector3 newPanPosition) {
		if(!panActive_2){
			return;
		}
		Vector3 offset = cam_2.ScreenToViewportPoint(lastPanPosition_2 - newPanPosition);
		Vector3 move = new Vector3((offset.y * PanSpeed), 0, -1*(offset.x * PanSpeed));
		cam_2.transform.Translate(move, Space.World);  
		ClampToBounds_2();
		lastPanPosition_2 = newPanPosition;
	}
	public void ClampToBounds() {
		minX = 80;
		maxX = 80; 
		minZ= -70;
		maxZ = -70;
		Vector3 pos = cam.transform.position;
		float camSize = cam.fieldOfView;
		float camSizeRedux = 60;
		float hopeRedux = (cam.fieldOfView/10)+ 1;
		float aspect = cam.aspect;
		Vector3 posOrig = new Vector3(81.0f,891.6f,-69.5f);
		Vector3 offset = pos - posOrig;
		//float minXMod = offset.x - minX; 
		pos.x = Mathf.Clamp(cam.transform.position.x, minX + ((camSize - camSizeRedux)*4.45f) ,maxX  - ((camSize - camSizeRedux)*4.3f));
		pos.z = Mathf.Clamp(cam.transform.position.z, minZ + ((camSize - camSizeRedux) *5.75f), maxZ - ((camSize - camSizeRedux)* 5.82f));
		cam.transform.position = pos;
	}
	void ClampToBounds_2() {
		minX = -673.68f;
		maxX = -673.68f; 
		minZ= -63.57f;
		maxZ = -63.57f;
		Vector3 pos = cam_2.transform.position;
		float camSize = cam_2.fieldOfView;
		float camSizeRedux = 60;
		float hopeRedux = (cam_2.fieldOfView/10)+ 1;
		float aspect = cam_2.aspect;
		Vector3 posOrig = new Vector3(81.0f,891.6f,-69.5f);
		Vector3 offset = pos - posOrig;
		//float minXMod = offset.x - minX; 
		pos.x = Mathf.Clamp(cam_2.transform.position.x, minX + ((camSize - camSizeRedux)*4.45f) ,maxX  - ((camSize - camSizeRedux)*4.3f));
		pos.z = Mathf.Clamp(cam_2.transform.position.z, minZ + ((camSize - camSizeRedux) *5.75f), maxZ - ((camSize - camSizeRedux)* 5.82f));

		cam_2.transform.position = pos;
	}
}
