using System.Collections;
using System.Linq;
using JungleCore;
using UnityEngine;

public class Master : MonoBehaviour
{
    public  Target   targetPrefab;
    public  Game     game;
    private Target[] _targets;
    private Animal   lastAnimal;
    private Animal[] _animals;

    private void Awake()
    {
        game = new Game();
    }
    
    void Start()
    {
        _animals = FindObjectsOfType<Animal>();
        foreach (var item in _animals)
        {
            item.selected += ShowMoves;
            game.someoneDead += item.Die;
        }
        UnblockActive();

        _targets = new Target[4];
        for (var i = 0; i < _targets.Length; i++)
        {
            _targets[i] = Instantiate(targetPrefab);
            _targets[i].clicked += Move;
        }
    }

    private void ShowMoves(Animal animal)
    {
        foreach (var item in _targets)
        {
            item.gameObject.SetActive(false);
        }

        lastAnimal = animal;
        var points = game.MovesFor(animal.rank);
        for (var i = 0; i < points.Count(); i++)
        {
            _targets[i].SetPosition(points[i]);
            _targets[i].gameObject.SetActive(true);
        }
    }

    private void Move(Point point) => StartCoroutine(MoveRoutine(point));
    private IEnumerator MoveRoutine(Point point)
    {
        foreach (var item in _targets)
        {
            item.gameObject.SetActive(false);
        }

        BlockAll();
        yield return StartCoroutine(lastAnimal.Move(point));
        game.Move(lastAnimal.rank, point);
        UnblockActive();
    }

    private void UnblockActive()
    {
        foreach (var item in _animals)
        {
            item.IsActive = item.side == game.Board.ActiveSide;
        }
    }  
    private void BlockAll()
    {
        foreach (var item in _animals)
        {
            item.IsActive = false;
        }
    }
}