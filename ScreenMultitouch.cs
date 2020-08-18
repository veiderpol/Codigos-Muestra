using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMultitouch : MonoBehaviour {

    //--------Declaracion Inicial---------//
    [Header("Objeto")]
    public GameObject obj;
    [Header("Rotacion")]
    public int DedosRotacion = 3;

    public bool rotacionX = false;    
    public bool rotacionY = false;
    public float delayRotacion = 100;
    private float delayRX = 0;
    private float delayRY = 0;
    private bool AllowRX, AllowRY = false;
    public float speedRotacion = 0;
    [Header("Traslacion")]


    public int DedosTraslacion = 1;

    public bool BothXY = false;

    public bool traslacionX = false;

    public bool traslacionY = false;
    public bool convert_YtoZ = false;


    public float delayTraslacion = 100;
    private float delayTX = 0;
    private float delayTY = 0;
    private bool AllowTX, AllowTY = false;

    public float speedTraslacion = 0;
    [Header("Escalado")]
    public bool escalado = false;
    public float speedEscalado = 0;

    public float escalaMin = 1;
    public float escalaMax = 10;

    private ControlCircuitos controCircuitos;
    //--------Declaracion Inicial---------//



    void Awake()
    {
        Application.targetFrameRate=60;
    }

    // Use this for initialization
    void Start () {
        controCircuitos = FindObjectOfType<ControlCircuitos>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount == DedosTraslacion && Input.GetTouch(0).phase == TouchPhase.Ended) {
            controCircuitos.PosicionOriginal();
        }
        //Rotacion
        if (Input.touchCount == DedosRotacion && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            delayRX = delayRX + touchDeltaPosition.x;
            delayRY = delayRY + touchDeltaPosition.y;

            if (BothXY == false && rotacionX == true && AllowRY != true && (delayRX >= delayRotacion || delayRX<= -delayRotacion))
            {
                AllowRX = true;
            }

            if (rotacionY == true && AllowRX != true && (delayRY >= delayRotacion || delayRY <= -delayRotacion))
            {
                AllowRY = true;
            }          
            

            // Move object across XY plane
            if (AllowRX == true)
            {
                obj.transform.Rotate(0, -touchDeltaPosition.x * speedRotacion, 0, Space.World);
            }

            if (AllowRY == true)
            {
                obj.transform.Rotate(+touchDeltaPosition.y * speedRotacion, 0, 0, Space.World);  
            }

        }


        //Traslacion
        if (Input.touchCount == DedosTraslacion && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            delayTX = delayTX + touchDeltaPosition.x;
            delayTY = delayTY + touchDeltaPosition.y;

            if (BothXY == false && traslacionX == true && AllowTY != true && (delayTX >= delayTraslacion || delayTX <= -delayTraslacion))
            {
                AllowTX = true;
            }

            if (BothXY == false && traslacionY == true && AllowTX != true && (delayTY >= delayTraslacion || delayTY <= -delayTraslacion))
            {
                AllowTY = true;
            }

            if (BothXY == true && (delayTX >= delayTraslacion || delayTX <= -delayTraslacion))
            {
                AllowTX = true;
                AllowTY = true;
            }

            // Move object across XY plane
            if (AllowTX == true)
            {
                obj.transform.Translate(+touchDeltaPosition.x * speedTraslacion, 0, 0, Space.World);
            }

            if (AllowTY == true)
            {
                
				if (convert_YtoZ == true) {
					obj.transform.Translate (0, 0, +touchDeltaPosition.y * speedTraslacion, Space.World);
				} else {
					obj.transform.Translate(0, +touchDeltaPosition.y * speedTraslacion, 0, Space.World);
				}
            }
        }


        //Escalado
        if (Input.touchCount == 2 && escalado==true)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 toucOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - toucOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


            

            if (obj)
            {
                obj.transform.localScale += new Vector3(-(deltaMagnitudeDiff * speedEscalado), -(deltaMagnitudeDiff * speedEscalado), -(deltaMagnitudeDiff * speedEscalado));

                obj.transform.localScale = Vector3.Max(obj.transform.localScale, new Vector3(escalaMin, escalaMin, escalaMin));
                obj.transform.localScale = Vector3.Min(obj.transform.localScale, new Vector3(escalaMax, escalaMax, escalaMax));
            }
            else
            {
                obj.transform.localScale = new Vector3(Mathf.Clamp(obj.transform.localScale.x, (escalaMin+0.1f), (escalaMax-0.1f)), Mathf.Clamp(obj.transform.localScale.y, (escalaMin + 0.1f), (escalaMax - 0.1f)), Mathf.Clamp(obj.transform.localScale.z, (escalaMin + 0.1f), (escalaMax - 0.1f)));

                
            }
        }


        if (Input.touchCount == 0)
        {
            AllowRX = false;
            AllowRY = false;
            delayRX = 0;
            delayRY = 0;


            AllowTX = false;
            AllowTY = false;
            delayTX = 0;
            delayTY = 0;
        }
    }
}
