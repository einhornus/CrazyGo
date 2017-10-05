using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using UnityEngine;
using Assets;

public class BoardScript : ControlBase, IBoardScript
{
    public UISprite aimingSprite;
    public UISprite lastStoneSprite;


    public void SetOnUserTogglePoint(Action<Point> p)
    {
        userTogglePoint = p;
    }

    private Action<Point> userTogglePoint = delegate { };
    private Action<Point> userHovering = delegate { };


    private static float HEIGHT = 720;
    private static float[,] offsetTable =
    {
        {50.0f/720.0f, 46f/720.0f, 51f/720.0f, 51f/720.0f },
        {33f/720.0f, 34f/720.0f, 33f/720.0f, 35f/720.0f },
        {25f/720.0f, 25f/720.0f, 25f/720.0f, 25f/720.0f },
    };

    public int GetRow()
    {
        if (size == 9)
        {
            return 0;
        }

        if (size == 13)
        {
            return 1;
        }

        if (size == 19)
        {
            return 2;
        }
        throw new System.ArgumentException();
    }

    public float GetLeftOffset()
    {
        return offsetTable[GetRow(), 0] * this.rootSprite.width;
    }

    public float GetRightOffset()
    {
        return offsetTable[GetRow(), 1] * this.rootSprite.width;
    }

    public float GetBottomOffset()
    {
        return offsetTable[GetRow(), 2] * this.rootSprite.height;
    }


    public float GetTopOffset()
    {
        return offsetTable[GetRow(), 3] * this.rootSprite.height;
    }

    void Start()
    {
        aimingSprite.spriteName = "_black";
    }

    UISprite[,] spriteMatrix;
    int size;
    public void Init(int size)
    {
        this.size = size;
        spriteMatrix = new UISprite[size, size];
    }


    public void ChangeStone( int x, int y, Stone stone)
    {
        spriteMatrix[x, y].spriteName = BoardStateParser.GetSprite(stone);
    }

    public static string GetGobanSpriteName(int size)
    {
        if (size == 9)
        {
            return "g9";
        }

        if (size == 13)
        {
            return "g13";
        }

        if (size == 19)
        {
            return "g19";
        }

        throw new System.ArgumentException();
    }

    private bool isAiming = false;


    public float GetActualWidth()
    {
        float width = this.rootSprite.width - (GetLeftOffset() + GetRightOffset());
        return width;
    }

    public float GetActualHeight()
    {
        float width = this.rootSprite.width - (GetBottomOffset() + GetTopOffset());
        return width;
    }

    public int GetBoardSize()
    {
        return size;
    }

    public float GetStart()
    {
        float start = -GetActualWidth() / 2f;
        return start;
    }

    public float GetEnd()
    {
        float end = GetActualWidth() / 2f;
        return end;
    }

    public float GetCellSize()
    {
        return GetActualWidth() / (GetBoardSize() - 1f);
    }

    public Point GetLogicalLocation(float x, float y)
    {
        float xc = (x - GetStart()) / GetCellSize();
        float yc = (y - GetStart()) / GetCellSize();
        int resx = (int)Mathf.Round(xc);
        int resy = (int)Mathf.Round(yc);
        return new Point(resx, resy);
    }

    public Point GetWorldLocation(Point point)
    {
        int size = GetBoardSize();
        float width = GetActualWidth();
        float height = GetActualHeight();
        float centerX = (float)width * (float)point.x / (float)(size - 1) + GetLeftOffset() - this.rootSprite.width / 2f;
        float centerY = (float)height * (float)point.y / (float)(size - 1) + GetBottomOffset() - this.rootSprite.width / 2f;
        return new Point((int)centerX, (int)centerY);
    }

    public float GetRad()
    {
        float rad = GetActualWidth() / (float)GetBoardSize() * 1.15f;
        return rad;
    }


    public void SetLastMove(int x, int y)
    {
        Point center = GetWorldLocation(new Point(x, y));
        lastStoneSprite.transform.localPosition = new Vector3(center.x, center.y, 0);
        lastStoneSprite.spriteName = "_cyan";
    }

    public void DeleteLastMove()
    {
        lastStoneSprite.spriteName = "invisible";
    }


    void OnPress(bool isDown)
    {
        BoxCollider collider = this.GetComponent<BoxCollider>();
        Vector2 mouse = UICamera.lastHit.point;
        float x = (mouse.x * this.rootSprite.width / 2.0f) / this.rootSprite.transform.localScale.x * (HEIGHT / this.rootSprite.height) - this.rootSprite.transform.localPosition.x;
        float y = (mouse.y * this.rootSprite.width / 2.0f) / this.rootSprite.transform.localScale.y * (HEIGHT / this.rootSprite.height);

        Point boardPosition = GetLogicalLocation(x, y);

        bool good = true;
        if (boardPosition.x < 0 || boardPosition.x >= GetBoardSize())
        {
            good = false;
        }

        if (boardPosition.y < 0 || boardPosition.y >= GetBoardSize())
        {
            good = false;
        }

        if (isDown && good)
        {
            this.userTogglePoint(boardPosition);
        }

    }

    public void SetAimBlack()
    {
        aimingSprite.spriteName = "_black";
    }

    public void SetAimWhite()
    {
        aimingSprite.spriteName = "_white";
    }

    public void SetAimInvisible()
    {
        aimingSprite.spriteName = "invisible";
    }

    void Update()
    {
    }
}
