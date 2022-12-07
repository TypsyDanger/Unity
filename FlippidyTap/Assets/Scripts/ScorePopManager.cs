using UnityEngine;
using System.Collections;

public class ScorePopManager : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
        
	}

    void Awake() {
        //print("ScorePopManager Created"); 
    }

	// Update is called once per frame
	void Update()
	{
			
	}

    public void playSelfAndDestroy() {
        var delay = 0.6f;
        gameObject.GetComponent<Animator>().Play("Pop");
        Destroy(gameObject, gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }
}
