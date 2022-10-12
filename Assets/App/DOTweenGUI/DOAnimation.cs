using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOAnimation : MonoBehaviour
{
    public bool animateOnStart;
    public UpdateType updateType;
    public DOLocationType locationType;
    [Space]
    public bool isLooped;
    public int loopCount;
    public LoopType loopType;
    [Space]
    public List<TweenData> tweenDatas = new List<TweenData>();

    private Sequence sequence;

    private void Start()
    {
        if (animateOnStart)
        {
            Animate();
        }
    }

    public void Animate()
    {
        if(tweenDatas.Count == 0)
        {
            //Debug.LogWarning("> Warning: has no tweens to animate");
            return;
        }
        if (tweenDatas.Count > 1)
        {
            //Debug.Log("> DOExtension: Sequence");
            sequence = DOTween.Sequence();
            if (isLooped)
            {
                sequence.SetLoops(loopCount, loopType);
            }
            sequence.SetUpdate(updateType);
            foreach (TweenData tweenData in tweenDatas)
            {
                if(tweenData.sequenceType == DOSequenceType.Append)
                {
                    //Debug.Log("> DOExtension: Append");
                    sequence.Append(GetTween(tweenData, locationType));
                }
                else
                {
                    //Debug.Log("> DOExtension: Join");
                    sequence.Join(GetTween(tweenData, locationType));
                }
            }
        }
        else
        {
            //Debug.Log("> DOExtension: Simple");
            GetTween(tweenDatas[0], locationType).SetUpdate(updateType).SetLoops(loopCount, loopType);
        }
    }

    private Tween GetTween(TweenData tweenData, DOLocationType locationType)
    {
        Tween tween = null;
        if (tweenData.type == DOTweenType.Move)
        {
            if(locationType == DOLocationType.Local)
            {
                tween = transform.DOLocalMove(tweenData.additive ? transform.localPosition + tweenData.value : tweenData.value, tweenData.duration);
            }
            else
            {
                tween = transform.DOMove(tweenData.additive ? transform.position + tweenData.value : tweenData.value, tweenData.duration);
            }
        }
        else if (tweenData.type == DOTweenType.Rotate)
        {

            if (locationType == DOLocationType.Local)
            {
                tween = transform.DOLocalRotate(tweenData.additive ? transform.localEulerAngles + tweenData.value : tweenData.value, tweenData.duration, tweenData.rotateMode);
            }
            else
            {
                tween = transform.DORotate(tweenData.additive ? transform.eulerAngles + tweenData.value : tweenData.value, tweenData.duration, tweenData.rotateMode);
            }
        }
        else if (tweenData.type == DOTweenType.Scale)
        {
            tween = transform.DOScale(tweenData.additive ? transform.localScale + tweenData.value : tweenData.value, tweenData.duration);
        }
        tween.SetEase(tweenData.ease);
        return tween;
    }
}
