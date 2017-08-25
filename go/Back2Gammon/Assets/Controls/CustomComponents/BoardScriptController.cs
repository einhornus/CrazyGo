using UnityEngine;
using UnityEditor;

public class BoardScriptController
{
    public IBoardScript boardScript;
    private MeEnum me;


    private bool isInited = false;

    public void SetState(ResponseBase response)
    {
        me = response.me;
        /*
        this._board = response.board;
        this._response = response;
        Init(this._board.n);
        DrawStones(response);
        */
    }

    public void Init(int size)
    {
        /*
        if (!isInited)
        {
            this.RedrawGrid(_response.board);
            matrix = new UISprite[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Point center = GetWorldLocation(_board, new Point(i, j));
                    float rad = GetRad(_board);
                    UISprite stoneSprite = null;
                    string spriteName = BoardStateParser.GetSprite(Stone.EMPTY);
                    stoneSprite = AddSprite("stones", spriteName, (int)center.x, (int)center.y, (int)rad, (int)rad);
                    matrix[i, j] = stoneSprite;
                }
            }
            isInited = true;
        }
        */
    }

}