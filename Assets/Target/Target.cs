using System;
using System.Collections;
using System.Collections.Generic;
using JungleCore;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Point> clicked;
    private Point _point;

    public void SetPosition(Point position)
    {
        transform.position = position + Vector3.back;
        _point = position;
    }

    private void OnMouseDown()
    {
        clicked?.Invoke(_point);
    }
}
