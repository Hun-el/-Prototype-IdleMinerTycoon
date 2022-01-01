using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseMiner : MonoBehaviour,IClickable
{
    [Header("Initial Values")]
    [SerializeField] float initialCollectCapacity;
    [SerializeField] float initialCollectPerSecond;
    [SerializeField] protected float moveSpeed;

    public float CurrentGold { get; set; }
    public float CollectCapacity { get; set; }
    public float CollectPerSecond { get; set; }
    public float MoveSpeed { get; set; }

    bool MinerClicked { get; set; }
    bool IsTimeToCollect {get; set;}

    protected Animator _animator;

    void Awake() 
    {
        _animator = GetComponent<Animator>();
        IsTimeToCollect = true;

        CollectPerSecond = initialCollectPerSecond;
        CollectCapacity = initialCollectCapacity;

        MoveSpeed = moveSpeed;
    }

    void OnMouseDown() 
    {
        if(!MinerClicked)
        {
            OnClick();
            MinerClicked = true;
        }
    }

    public virtual void OnClick()
    {

    }

    protected virtual void MoveMiner(Vector3 newPosition)
    {
        transform.DOMove(newPosition,MoveSpeed).SetEase(Ease.Linear).OnComplete(() => 
        {
            if(IsTimeToCollect)
            {
                CollectGold();
            }
            else
            {
                DepositGold();
            }
        }
        ).Play();
    }

    protected virtual void CollectGold()
    {

    }

    protected virtual IEnumerator IECollect(float gold,float collectTime)
    {
        yield return null;
    }

    protected virtual void DepositGold()
    {

    }
    protected virtual IEnumerator IEDeposit(float depositTime)
    {
        yield return null;
    }

    protected void ChangeGoal()
    {
        IsTimeToCollect = !IsTimeToCollect;
    }

    protected void RotateMiner(int direction)
    {   
        if(direction == 1)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else
        {
            transform.localScale = new Vector3(-1,1,1);
        }
    }
}
