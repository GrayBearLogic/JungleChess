using System;
using System.Collections;
using System.Collections.Generic;
using JungleCore;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class Animal : MonoBehaviour
{
    private const float MoveDuration = 1f;
    
    public event Action<Animal> selected;
    
    [SerializeField]  private SpriteRenderer _spriteRenderer;
    [HideInInspector] private Animator       _animator;

    [SerializeField] public Rank  rank;
    [SerializeField] public Side  side;
    
    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }
    private bool _isActive;

    private static readonly int DieId  = Animator.StringToHash("Die");
    private static readonly int MoveId = Animator.StringToHash("Move");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        if (_isActive)
            selected?.Invoke(this);
    }

    public void Die(Game sender, DeathEventArgs e)
    {
        if (e.Owner == side && e.Rank == rank)
        {
            sender.someoneDead -= Die;
            _animator.SetTrigger(DieId);
        }
    }

    public void DestroyWrapper() => Destroy(gameObject);

    public IEnumerator Move(Point endPos)
    {
        _spriteRenderer.sortingOrder++;
        
        var startPos = transform.position;
        var startScale = transform.localScale;
        var time = 0f;
        
        _animator.SetTrigger(MoveId);
        while (time < MoveDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, time / MoveDuration);

            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        _spriteRenderer.sortingOrder--;
    }
}