using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode
{
    public Vector2 position { get; set; }
    public Vector2 mapPosition { get; set; }
    private const int gDefaultValue = 9999999;

    public int g { get; set; }
    public int h { get; set; }
    public int f { get; set; }

    public bool isMoveable { get; set; }
    public AstarNode previousNode { get; set; }

    public AstarNode(Vector2 position, Vector2 mapPosition, bool moveable)
    {
        g = gDefaultValue;
        h = f = 0;
        isMoveable = moveable;
        previousNode = null;
        this.position = new Vector2((int)position.x, (int)position.y);
        this.position = position;
        this.mapPosition = mapPosition;
    }

    public AstarNode(Vector2 _position, bool _moveable)
    {
        g = gDefaultValue;
        h = f = 0;
        isMoveable = _moveable;
        previousNode = null;
        position = new Vector2((int)position.x, (int)position.y);
        position = _position;
        ConvertPosToMapPos();
    }

    public void ConvertPosToMapPos()
    {

    }

    public void CalculateF()
    {
        f = g + h;
    }
}
