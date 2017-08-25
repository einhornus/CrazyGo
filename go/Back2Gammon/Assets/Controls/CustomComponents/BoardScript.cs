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
    private MeEnum me;
    public UISprite aimingSprite;

    private InputMethod currentInputMethod = InputMethod.Desktop;
    public ResponseBase _response;


    public Point GetCurrentMoveIndicator()
    {
        return currentMoveIndicator;
    }

    public int GetN()
    {
        return this._board.n;
    }



    public void SetUserTogglePoint(Action<Point> p)
    {
        userTogglePoint = p;
    }



    public InputMethod GetCurrentInputMethod()
    {
        return currentInputMethod;
    }

    public void SetCurrentInputMethod(InputMethod im)
    {
        currentInputMethod = im;
    }

    private Action<Point> userTogglePoint = delegate { };

    private static float HEIGHT = 720;
    private static float[,] offsetTable =
    {
        {50.0f/720.0f, 46f/720.0f, 51f/720.0f, 51f/720.0f },
        {33f/720.0f, 34f/720.0f, 33f/720.0f, 35f/720.0f },
        {25f/720.0f, 25f/720.0f, 25f/720.0f, 25f/720.0f },
    };

    public int GetRow(Board board)
    {
        if (board.n == 9)
        {
            return 0;
        }

        if (board.n == 13)
        {
            return 1;
        }

        if (board.n == 19)
        {
            return 2;
        }
        throw new System.ArgumentException();
    }

    public float GetLeftOffset(Board board)
    {
        return offsetTable[GetRow(board), 0] * this.rootSprite.width;
    }

    public float GetRightOffset(Board board)
    {
        return offsetTable[GetRow(board), 1] * this.rootSprite.width;
    }

    public float GetBottomOffset(Board board)
    {
        return offsetTable[GetRow(board), 2] * this.rootSprite.height;
    }


    public float GetTopOffset(Board board)
    {
        return offsetTable[GetRow(board), 3] * this.rootSprite.height;
    }

    void Start()
    {
        aimingSprite.spriteName = "_black";
    }

    public void SetState(ResponseBase response)
    {
        
        me = response.me;
        this._board = response.board;
        this._response = response;
        Init(this._board.n);
        DrawStones(response);
    }

    bool isInited = false;

    public void Init(int size)
    {
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
    }

    public UISprite[,] matrix;

    public void DrawStones(ResponseBase response)
    {
        for (int i = 0; i < response.board.n; i++)
        {
            for (int j = 0; j < response.board.n; j++)
            {
                Move move = new Move(i, j);
                Stone initialStone = Stone.EMPTY;
                double stoneSize = 1.0;
                for (int a = 0; a < response.board.blacks.Count; a++)
                {
                    if (response.board.blacks[a].x == i && response.board.blacks[a].y == j)
                    {
                        initialStone = Stone.BLACK;
                    }
                }


                for (int a = 0; a < response.board.whites.Count; a++)
                {
                    if (response.board.whites[a].x == i && response.board.whites[a].y == j)
                    {
                        initialStone = Stone.WHITE;
                    }
                }

                if (response is CountingResponse)
                {
                    CountingResponse counting = (CountingResponse)response;
                    for (int a = 0; a < counting.dead.Count; a++)
                    {
                        if (counting.dead[a].x == i && counting.dead[a].y == j)
                        {
                            if (initialStone == Stone.WHITE)
                            {
                                initialStone = Stone.WHITE_DEAD;
                            }
                            if (initialStone == Stone.BLACK)
                            {
                                initialStone = Stone.BLACK_DEAD;
                            }
                        }
                    }

                    for (int a = 0; a < counting.blackTerritory.Count; a++)
                    {
                        if (counting.blackTerritory[a].x == i && counting.blackTerritory[a].y == j)
                        {
                            if (initialStone != Stone.WHITE_DEAD)
                            {
                                initialStone = Stone.BLACK_TERRITORY;
                                stoneSize = counting.blackTerritorySize[a];
                            }
                        }
                    }

                    for (int a = 0; a < counting.whiteTerritory.Count; a++)
                    {
                        if (counting.whiteTerritory[a].x == i && counting.whiteTerritory[a].y == j)
                        {
                            if (initialStone != Stone.BLACK_DEAD)
                            {
                                initialStone = Stone.WHITE_TERRITORY;
                                stoneSize = counting.whiteTerritorySize[a];
                            }
                        }
                    }
                }
                if (response is HiddenMoveGoGameResponse || response is HiddenMoveGoSetupResponse)
                {
                    List<Move> hiddens = null;
                    if (response is HiddenMoveGoGameResponse)
                    {
                        hiddens = ((HiddenMoveGoGameResponse)response).hiddens;
                    }
                    if (response is HiddenMoveGoSetupResponse)
                    {
                        hiddens = ((HiddenMoveGoSetupResponse)response).hiddens;
                    }


                    for (int a = 0; a < hiddens.Count; a++)
                    {
                        if (hiddens[a].x == i && hiddens[a].y == j)
                        {
                            if (response is HiddenMoveGoGameResponse)
                            {
                                if (initialStone == Stone.WHITE)
                                {
                                    if (me == MeEnum.WHITE || me == MeEnum.OBSERVER)
                                    {
                                        initialStone = Stone.WHITE_HIDDEN;
                                    }
                                    else
                                    {
                                        initialStone = Stone.EMPTY;
                                    }
                                }
                                if (initialStone == Stone.BLACK)
                                {
                                    if (me == MeEnum.BLACK || me == MeEnum.OBSERVER)
                                    {
                                        initialStone = Stone.BLACK_HIDDEN;
                                    }
                                    else
                                    {
                                        initialStone = Stone.EMPTY;
                                    }
                                }
                            }
                            else
                            {
                                HiddenMoveGoSetupResponse setup = ((HiddenMoveGoSetupResponse)response);
                                bool w = true;
                                if (response.black == setup.colors[a])
                                {
                                    w = false;
                                }
                                if (initialStone == Stone.EMPTY)
                                {
                                    if ((me == MeEnum.WHITE || me == MeEnum.OBSERVER) && w)
                                    {
                                        initialStone = Stone.WHITE_HIDDEN;

                                    }
                                    if ((me == MeEnum.BLACK || me == MeEnum.OBSERVER) && !w)
                                    {
                                        initialStone = Stone.BLACK_HIDDEN;
                                    }
                                }
                                else
                                {
                                    initialStone = Stone.EMPTY;
                                }
                            }
                        }
                    }
                }

                if (initialStone == Stone.WHITE && response is OneColorGoGameResponse && !((OneColorGoGameResponse)response).isRevealed)
                {
                    initialStone = Stone.BLACK;
                }

                if (response is BlindGoGameResponse && !((BlindGoGameResponse)response).isRevealed)
                {
                    BlindGoGameResponse rr = (BlindGoGameResponse)response;
                    if (rr.lastMove != null && rr.lastMove.x == i && rr.lastMove.y == j)
                    {

                    }
                    else
                    {
                        initialStone = Stone.EMPTY;
                    }
                }

                DrawStone(response.board, i, j, initialStone, false, stoneSize);
            }
        }

        if (response is GameResponse)
        {
            GameResponse gr = (GameResponse)response;
            RemoveCategory("last");
            if (gr.lastMove != null)
            {
                Point center = GetWorldLocation(_board, new Point(gr.lastMove.x, gr.lastMove.y));
                float rad = GetRad(_board) * 0.4f;
                UISprite sparkSprite = null;
                sparkSprite = AddSprite("last", "_cyan", (int)center.x, (int)center.y, (int)rad, (int)rad);
            }
        }
        else
        {
            RemoveCategory("last");
        }
    }

    public void DrawStone(Board board, int x, int y, Stone stone, bool isAlphazed, double size)
    {
        if (!matrix[x, y].spriteName.Equals(BoardStateParser.GetSprite(stone)))
        {
            matrix[x, y].spriteName = BoardStateParser.GetSprite(stone);
        }
        if (isAlphazed)
        {
            matrix[x, y].alpha = 0.5f;
        }
        matrix[x, y].transform.localScale = new Vector2((float)size, (float)size);
    }

    public string GetGobanSpriteName(Board board)
    {
        if (board.n == 9)
        {
            return "g9";
        }

        if (board.n == 13)
        {
            return "g13";
        }

        if (board.n == 19)
        {
            return "g19";
        }

        throw new System.ArgumentException();
    }

    public void RedrawGrid(Board board)
    {
        rootSprite.spriteName = GetGobanSpriteName(board);
    }

    private bool isAiming = false;
    public Board _board;


    public float GetActualWidth(Board board)
    {
        float width = this.rootSprite.width - (GetLeftOffset(board) + GetRightOffset(board));
        return width;
    }

    public float GetActualHeight(Board board)
    {
        float width = this.rootSprite.width - (GetBottomOffset(board) + GetTopOffset(board));
        return width;
    }

    public int GetBoardSize(Board board)
    {
        return board.n;
    }

    public float GetStart(Board board)
    {
        float start = -GetActualWidth(board) / 2f;
        return start;
    }

    public float GetEnd(Board board)
    {
        float end = GetActualWidth(board) / 2f;
        return end;
    }

    public float GetCellSize(Board board)
    {
        return GetActualWidth(board) / (GetBoardSize(board) - 1f);
    }

    public Point GetLogicalLocation(Board board, float x, float y)
    {
        float xc = (x - GetStart(board)) / GetCellSize(board);
        float yc = (y - GetStart(board)) / GetCellSize(board);
        int resx = (int)Mathf.Round(xc);
        int resy = (int)Mathf.Round(yc);
        return new Point(resx, resy);
    }

    public Point GetWorldLocation(Board board, Point point)
    {
        int size = GetBoardSize(board);
        float width = GetActualWidth(board);
        float height = GetActualHeight(board);
        float centerX = (float)width * (float)point.x / (float)(size - 1) + GetLeftOffset(board) - this.rootSprite.width / 2f;
        float centerY = (float)height * (float)point.y / (float)(size - 1) + GetBottomOffset(board) - this.rootSprite.width / 2f;
        return new Point((int)centerX, (int)centerY);
    }

    public float GetRad(Board board)
    {
        float rad = GetActualWidth(board) / (float)GetBoardSize(board) * 1.15f;
        return rad;
    }

    //private MeEnum me;
    public Point currentMoveIndicator = null;

    void OnPress(bool isDown)
    {
        BoxCollider collider = this.GetComponent<BoxCollider>();
        Vector2 mouse = UICamera.lastHit.point;
        float x = (mouse.x * this.rootSprite.width / 2.0f) / this.rootSprite.transform.localScale.x * (HEIGHT / this.rootSprite.height) - this.rootSprite.transform.localPosition.x;
        float y = (mouse.y * this.rootSprite.width / 2.0f) / this.rootSprite.transform.localScale.y * (HEIGHT / this.rootSprite.height);

        Point boardPosition = GetLogicalLocation(_board, x, y);

        bool good = true;
        if (boardPosition.x < 0 || boardPosition.x >= GetBoardSize(_board))
        {
            good = false;
        }

        if (boardPosition.y < 0 || boardPosition.y >= GetBoardSize(_board))
        {
            good = false;
        }

        if (isDown && good)
        {
            currentMoveIndicator = boardPosition;
            if (this.currentInputMethod == InputMethod.Desktop)
            {
                NGUIDebug.Log("Toggle", boardPosition.x+" "+ boardPosition.y);
                this.userTogglePoint(boardPosition);
                currentMoveIndicator = null;
            }
        }

    }

    void Update()
    {
        if (_board != null)
        {
            if (currentMoveIndicator != null)
            {
                aimingSprite.spriteName = "invisible";
                if ((_response is GameResponse) || (_response is HiddenMoveGoSetupResponse))
                {
                    if (me != MeEnum.OBSERVER)
                    {
                        if (me == MeEnum.BLACK)
                        {
                            aimingSprite.spriteName = "_black";
                        }
                        if (me == MeEnum.WHITE)
                        {
                            aimingSprite.spriteName = "_white";
                        }

                        aimingSprite.alpha = 0.3f;
                        Point world = GetWorldLocation(_board, currentMoveIndicator);
                        aimingSprite.SetDimensions((int)GetRad(_board), (int)GetRad(_board));
                        aimingSprite.transform.localPosition = new Vector3(world.x, world.y, 0);
                    }
                }
                else
                {
                    aimingSprite.spriteName = "invisible";
                }
            }
        }
    }
}
