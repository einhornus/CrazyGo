using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class BoardScriptController
{
    public IBoardScript boardScript;
    private MeEnum me;

    private bool isInited = false;
    private ResponseBase response;
    private Stone[,] stonesMatrix;

    private Action<Point> action;
    private Point moveIndicator = null;

    public Point GetCurrentMoveIndicator()
    {
        return moveIndicator;
    }


    public BoardScriptController(IBoardScript script)
    {
        boardScript = script;
    }

    public void SetState(ResponseBase response)
    {
        this.response = response;
        me = response.me;
        Init(this.response.board.n);
        ResetStones(response);
    }

    public void SetAction(Action<Point> action)
    {
        this.action = action;
    }

    public void Init(int size)
    {
        if (!isInited)
        {
            stonesMatrix = new Stone[size, size];
            boardScript.Init(size);
        }
        boardScript.SetOnUserTogglePoint(delegate (Point p)
        {
            this.moveIndicator = p;
            action(p);
        });
    }


    public void ResetStones(ResponseBase response)
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

                if (stonesMatrix[i,j] != initialStone) {
                    boardScript.ChangeStone(i, j, initialStone);
                    stonesMatrix[i, j] = initialStone;
                }
            }
        }

        boardScript.DeleteLastMove();
        if (response is GameResponse)
        {
            GameResponse gr = (GameResponse)response;
            if (gr.lastMove != null) {
                boardScript.SetLastMove(gr.lastMove.x, gr.lastMove.y);
            }
        }
    }
}