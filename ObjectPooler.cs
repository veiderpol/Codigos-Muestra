using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class ObjectPooler : MonoBehaviour{
    [System.Serializable]
    public class Pool{
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;
    public GameObject respoGameObj, canvasGanar, canvasMenu, canvasGO, canvasStore, canvasInGame, objPlayer, canvasInstrucciones;
    public GameObject[] RES, RES_MALO;

    public Slider sliderAvance;
    public Image imgFill;
    public Image[] imgCorazon;

    public int vidasRestante = 0, randRespo, puntuaje;
    float sliderValue = 0;
    

    #region Singleton
    private void Awake(){
        Instance = this;
    }
    #endregion
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public Transform pos_1, pos_1_2, pos_2, pos_2_2, posEsfera, posRespo_1, posRespo_2;
    public bool runing = true;
    public Animator animPlayer;
    public TextMeshProUGUI textMeshPro, textMeshScore;
    private static StoreControl storeControl;
    private static Data data;
    Scene scene;
    void Start(){
        objPlayer = GameObject.Find("CoolDude1");
        animPlayer = objPlayer.GetComponent<Animator>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        randRespo = Random.Range(2,5);
        foreach(Pool pool in pools){
            Queue<GameObject> ObjectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++){
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                ObjectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, ObjectPool); 
        }
        StartCoroutine("SpawnBuilding_I");
        StartCoroutine("SpawnBuilding_2_D");
        scene = SceneManager.GetActiveScene();
        if(scene.name == "Level01"){
            Invoke("StartRespo",3);
        }else{
            storeControl = FindObjectOfType<StoreControl>();
        }  
        data = FindObjectOfType<Data>();
    }
    void Update(){
        if(scene.name == "Level01"){
            textMeshPro.SetText("Score: " + puntuaje);
            sliderValue += Time.deltaTime / 100;
            sliderAvance.value = sliderValue;
            imgFill.color = Color.Lerp(Color.blue, Color.red, sliderValue);
        }
        if(sliderValue >= 1){
            Win();
            Debug.Log("GAME OVER");
        }
    }
    public GameObject SpawnFromPool(string tag, Vector3 position,Quaternion rotation){
        if(runing){
            if(!poolDictionary.ContainsKey(tag)){
                Debug.LogWarning("NO EXISTE EL OBJETO EN EL POOL");
                return null;
            }
            
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.transform.parent = posEsfera.transform;
            
            poolDictionary[tag].Enqueue(objectToSpawn);

            randRespo = Random.Range(2,5);
            return objectToSpawn;
        }else{
            Debug.Log("Entre que k px");
            return null;
        }
        
    }
   
    public IEnumerator SpawnBuilding_I(){
        float rand =  Random.Range(0,100);
        if(rand > 50){
            SpawnFromPool("Edificio_1_I", pos_1.position,Quaternion.Euler(-86.41701f,183.194f,-27.387f));
            yield return new WaitForSeconds(2f);
        }else{
            SpawnFromPool("Edificio_2_I", pos_1_2.position,Quaternion.Euler(-1.702f,65.856f,86.818f));
            yield return new WaitForSeconds(2f);
        }            

        StartCoroutine("SpawnBuilding_I");
    } 
    public IEnumerator SpawnBuilding_2_D(){
        float rand =  Random.Range(0,100);
        if(rand > 50){
            SpawnFromPool("Edificio_1_D", pos_2.position,Quaternion.Euler(85.90601f,35.803f,9.373f));
            yield return new WaitForSeconds(2f);
        }else{
            SpawnFromPool("Edificio_2_D", pos_2_2.position,Quaternion.Euler(-1.338f,-63.688f,-85.96001f));
            yield return new WaitForSeconds(2f);
        }            
    
        StartCoroutine("SpawnBuilding_2_D");
    }
    
    public IEnumerator SpawnRepoV2(){
        GameObject instantiateObj;

        int randB = Random.Range(0,5);
        int randM = Random.Range(0,5);
        int randRespo = Random.Range(0,100);
        int randTimeB = Random.Range(1,4);
        int randTimeM = Random.Range(1,2);

        if(randRespo >= 50){
            yield return new WaitForSeconds(randTimeM);
            instantiateObj = (GameObject) Instantiate(RES_MALO[randM],posRespo_2.position,posRespo_2.rotation);
        }else{
            yield return new WaitForSeconds(randTimeM);
            instantiateObj = (GameObject) Instantiate(RES_MALO[randM],posRespo_1.position,posRespo_1.rotation);
        }

        instantiateObj.transform.parent = posEsfera.transform;

        if(randRespo >= 50){
            yield return new WaitForSeconds(randTimeB);
            instantiateObj = (GameObject) Instantiate(RES[randB],posRespo_2.position,posRespo_2.rotation);
        }else{
            yield return new WaitForSeconds(randTimeB);
            instantiateObj = (GameObject) Instantiate(RES[randB],posRespo_1.position,posRespo_1.rotation);
        }

        instantiateObj.transform.parent = posEsfera.transform;

        StartCoroutine("SpawnRepoV2");  
    }
    
}
