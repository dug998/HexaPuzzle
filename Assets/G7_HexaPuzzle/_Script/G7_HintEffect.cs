using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class G7_HintEffect : MonoBehaviour
{
    private CanvasGroup canvasG;
    private float startValue = 0.2f;
    private float endValue = 0.65f;
    // Start is called before the first frame update
    void Start()
    {
        canvasG = GetComponent<CanvasGroup>();
       
        float duration = 0.6f;

        DOTween.To(() => startValue, x => OnUpdate(x), endValue, duration)
            .SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    private void OnUpdate(float value)
    {
        canvasG.alpha = value;
    }
}
