using UnityEngine;
using System.Collections;

public class ScorePopManager : MonoBehaviour
{
    public void playSelfAndDestroy() {
        var delay = 0.6f;
        gameObject.GetComponent<Animator>().Play("Pop");
        Destroy(gameObject, gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }
}
