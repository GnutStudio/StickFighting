using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class TextDameInGame : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private Vector3 ofset;
    [SerializeField] private float timeMove;
    private Tween tweenMove;
    //private Tween tweenFade;
    //public void Show(string txt,Vector3 startPosition) {
    //    //text.text = txt;
    //    //transform.position = startPosition;
    //    //Vector3 endPostion = startPosition + ofset;
    //    //tweenMove.CheckKillTween();
    //    //tweenMove = transform.DOMove(endPostion,timeMove).SetEase(Ease.OutExpo).OnComplete(()=> {
    //    //    this.Recycle();
    //    //});
    //    //Color colorText = Color.white;
    //    //text.color = colorText;
    //    //tweenFade.CheckKillTween();
    //    //tweenFade = DOTween.To(() => 1f, (value) => {
    //    //    colorText.a = value;
    //    //    text.color = colorText;
    //    //}, 0f, timeMove);
    //}

    public void Show(string txt,Vector3 startPostion,TypeTextShow type = TypeTextShow.UP) {
        tweenMove.CheckKillTween();
        if(type == TypeTextShow.UP) {
            text.text = txt;
            transform.position = startPostion;
            Vector3 endPostion = startPostion + Vector3.up * ofset.y;
            tweenMove = transform.DOMove(endPostion, timeMove).SetEase(Ease.OutCubic).OnComplete(() => {
                this.Recycle();
            });
        } else if(type == TypeTextShow.UP_DOWN) {
            text.text = txt;
            transform.position = startPostion;
            float random_X = Random.Range(-ofset.x,ofset.x);
            Vector3 newOfset = new Vector3(random_X,ofset.y,0);
            Vector3 endPostionUp = startPostion + newOfset;
            Vector3 endPostionDown = new Vector3(endPostionUp.x+random_X/2,endPostionUp.y-newOfset.y/2,0);
            tweenMove = transform.DOMove(endPostionUp, timeMove).SetEase(Ease.OutCubic).OnComplete(() => {
                tweenMove = transform.DOMove(endPostionDown, timeMove/2).SetEase(Ease.InCubic).OnComplete(() => {
                    this.Recycle();
                });
            });
        }
    }

    public enum TypeTextShow {
        UP,
        UP_DOWN,
    }

    private void OnDisable() {
        tweenMove.CheckKillTween();
        //tweenFade.CheckKillTween();
    }
}
