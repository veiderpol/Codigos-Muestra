using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Random_Puzzle : MonoBehaviour {
	[SerializeField] public GameObject[] Elementos;
	[SerializeField] private Vector3 [] array;
	[SerializeField] private Vector3[] arrayPosicionesHint;
    [SerializeField] public Vector3[] posiciones;
    [SerializeField] public Transform[] posicionesChecar;
    [SerializeField] public GameObject[] piezas;
    [SerializeField] public Collider[] col;
	private ScreenMultitouch screenMultitouch;
	public GameObject letra3d;
	int contador;
	public bool iniciarRotacion;
	public bool parar = false;
	public int numRandomPiezas;
	int contadorPiezasRandom = 0;
   public List<int> list = new List<int>();
    
    private 
    void Start(){
		screenMultitouch = FindObjectOfType<ScreenMultitouch> ();
        RandomPuzzle();
    }
    void Update() { 
		if (Input.touchCount == 1) {
			iniciarRotacion = false;
		}
        //Debug.Log("La distancia es: "+ dis);
		if(iniciarRotacion == true){
			letra3d.transform.Rotate (0, Time.fixedDeltaTime *20, 0);
		}
       
    }
    void RandomPuzzle() {
        for (int i = 0; i < Elementos.Length; i++)
        {
			
			int randPos = Random.Range (0,arrayPosicionesHint.Length);
            int rand = Random.Range(0, array.Length);
            while (list.Contains(rand))
            {
                rand = Random.Range(0, arrayPosicionesHint.Length);
            }
            list.Add(rand);
				piezas[i] = Instantiate(Elementos[i], array[rand], Quaternion.identity);
				posiciones[i] = piezas[i].transform.position;
				col[i] = piezas[i].GetComponent<Collider>();
        }
    }
    public void ChecarPosicion(int index) {
        if (Vector2.Distance(piezas[index].transform.position, posicionesChecar[index].transform.position) < 2.3){
            piezas[index].transform.localScale = new Vector3(2.4f, 2.4f, 2.4f);
            piezas[index].transform.position = posicionesChecar[index].transform.localPosition;
            col[index].enabled = false;
			contador++;
			Debug.Log (contador);
			if(contador == 9){
				StartCoroutine ("InicioAnimacion");
			}
        }
        else {
            piezas[index].transform.position = posiciones[index];
            Debug.Log("Incorrecto");
        }
        
    }
	IEnumerator InicioAnimacion(){
		letra3d.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		letra3d.gameObject.GetComponent<Animator> ().enabled = false;
		iniciarRotacion = true;

	}
}
