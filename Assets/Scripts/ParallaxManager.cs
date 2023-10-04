using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour{
    
    public GameObject targetCamera;
    public float spriteHeight;
    public GameObject enemySpawner;
    private int countChild;

    void Start(){
        countChild = this.transform.childCount;
    }

    void Update(){
        for (int i = 0; i < countChild; i++){
            GameObject currentSprite = transform.GetChild(i).gameObject;
            if (targetCamera.transform.position.y - currentSprite.transform.position.y >= spriteHeight){
                currentSprite.transform.position = new Vector2(0, currentSprite.transform.position.y + transform.childCount * spriteHeight);
                enemySpawner.transform.position = new Vector2(0, currentSprite.transform.position.y + transform.childCount * spriteHeight);
            }
        }
    }
}
