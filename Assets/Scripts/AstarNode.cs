using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode
{

    public int x;
    public int y;

    public int h;
    public int g;
    public int f;

    public AstarNode parent;

    public AstarNode(int x, int y)
    {
        this.x = x;
        this.y = y;

        this.parent = null;
    }

    public bool Equals(AstarNode autre)
    {
        return this.x == autre.x && this.y == autre.y;
    }

    public Vector2Int GetVector2()
    {
        return new Vector2Int(this.x, this.y);
    }
}
