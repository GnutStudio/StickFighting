using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] protected int originHeart;    
    [SerializeField] protected int originDame;
    public int OriginHeart => originHeart;
    public int OriginDame => originDame;
    public int curHeart;
    public int curDame;

    protected virtual void Awake() {
        curHeart = originHeart;
        curDame = originDame;
    }

    public virtual void GetDame(int dame,GameObject objMakeDame = null) {
        if(curHeart<=0) {
            return;
        }
        curHeart -= dame;
    }
}
