using System.Text;

using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using BrainDuelsLib.threads;
using BrainDuelsLib.model.entities;
using UnityEngine;
using System;
using System.Collections.Generic;



public partial class GameFormControl : ControlBase
{
    public UISprite sePopup;
    public BoardScript seBoard;
    public UIButton seOpenButton;
    public UIButton seCloseButton;
    public UILabel blackScore;
    public UILabel whiteScore;

    public UIPopupList modePopup;

    private ResponseBase seResponse;
    private ResponseBase seResponseSealed;
    private List<Move> userTouches;

    public UILabel diffLabel;

    private bool isActive = true;

    public void SetScoreEstimatorPart()
    {
        this.seOpenButton.isEnabled = false;
        sePopup.transform.localPosition = new Vector3(0, 0, 0);
        Utils.MakeUnvisible(sePopup.transform);
        seOpenButton.onClick.Add(new EventDelegate(
            delegate
            {
                pressButtonSound.Play();
                if (seResponse != null)
                {
                    if (Utils.IsUnvisible(sePopup.transform))
                    {
                        Utils.MakeVisible(sePopup.transform);
                        seResponseSealed = this.seResponse;
                        isActive = true;
                        StartEstimation();
                    }
                }
            }
        ));

        seCloseButton.onClick.Add(new EventDelegate(
            delegate
            {
                pressButtonSound.Play();
                Utils.MakeUnvisible(sePopup.transform);
                isActive = false;
                userTouches = new List<Move>();
            }
        ));

        modePopup.onChange.Add(new EventDelegate(
            delegate
            {
                Recount();
            }
        ));
    }

    public void StartEstimation()
    {
        userTouches = new List<Move>();
        seBoard.SetUserTogglePoint(
            delegate (Point x)
            {
                bool contains = false;
                for (int k = 0; k < seResponseSealed.board.blacks.Count; k++)
                {
                    if (seResponseSealed.board.blacks[k].x == (int)x.x && seResponseSealed.board.blacks[k].y == (int)x.y)
                    {
                        contains = true;
                    }
                }


                for (int k = 0; k < seResponseSealed.board.whites.Count; k++)
                {
                    if (seResponseSealed.board.whites[k].x == (int)x.x && seResponseSealed.board.whites[k].y == (int)x.y)
                    {
                        contains = true;
                    }
                }

                if (contains)
                {
                    userTouches.Add(new Move((int)x.x, (int)x.y));
                    Recount();
                }
            }
        );
        Recount();
    }

    public void Recount()
    {
        int[,] brd = CookBoard(seResponseSealed);
        ResponseBase newRes = CookRes(brd);
        BoardStateParser.Dump(newRes);
        this.seBoard.SetState(newRes);
    }

    public void SetResponse(ResponseBase seResponse)
    {
        if (seResponse is GameResponse)
        {
            this.seOpenButton.isEnabled = true;
            this.seResponse = seResponse;
        }
    }

    public int[,] CookBoard(ResponseBase response)
    {
        int n = response.board.n;
        int[,] res = new int[n, n];
        for (int i = 0; i < response.board.blacks.Count; i++)
        {
            int x = response.board.blacks[i].x;
            int y = response.board.blacks[i].y;
            res[x, y] = -1;
        }

        for (int i = 0; i < response.board.whites.Count; i++)
        {
            int x = response.board.whites[i].x;
            int y = response.board.whites[i].y;
            res[x, y] = 1;
        }
        return res;
    }

    public ResponseBase CookRes(int[,] brd)
    {
        RegularGoCountingResponse res = new RegularGoCountingResponse();
        res.board = new Board();
        res.board = seResponseSealed.board;
        int n = res.board.n;
        List<Move> deadStones = ScoreEstimatorLogic.GetDeadStones(this.userTouches, res.board.whites, res.board.blacks, res.board.n);
        res.dead = deadStones;


        for (int i = 0; i < deadStones.Count; i++)
        {
            brd[deadStones[i].x, deadStones[i].y] = 0;
        }
        double[,] map = null;

        if (CreateNewGamePopup.GetSelection(modePopup) == 0)
        {
            /*
            UnityEngine.Debug.Log(Utils.Measure(delegate {
                double[,] d = ScoreEstimatorLogic.CountTerritory(brd);
                Console.WriteLine("fuck");
            }));
            UnityEngine.Debug.Log(Utils.Measure(delegate {
                double[,] d = ScoreEstimatorLogic.CountTerritoryC(brd);
                Console.WriteLine("fuck");
            }));
            */
            map = ScoreEstimatorLogic.CountTerritory(brd);
        }
        else
        {
            map = ScoreEstimatorLogic.CountInfluence(brd);
        }



        double limit = 0.01;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                bool contains = false;
                for (int k = 0; k < seResponseSealed.board.blacks.Count; k++)
                {
                    if (seResponseSealed.board.blacks[k].x == i && seResponseSealed.board.blacks[k].y == j)
                    {
                        contains = true;
                    }
                }


                for (int k = 0; k < seResponseSealed.board.whites.Count; k++)
                {
                    if (seResponseSealed.board.whites[k].x == i && seResponseSealed.board.whites[k].y == j)
                    {
                        contains = true;
                    }
                }

                if (!contains)
                {
                    if (brd[i, j] == 0 && map[i, j] > limit)
                    {
                        res.whiteTerritory.Add(new Move(i, j));
                        res.whiteTerritorySize.Add(map[i, j]);
                    }
                    if (brd[i, j] == 0 && map[i, j] < -limit)
                    {
                        res.blackTerritory.Add(new Move(i, j));
                        res.blackTerritorySize.Add(-map[i, j]);
                    }
                }
            }
        }


        double whiteScore = 0;
        double blackScore = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (map[i, j] > 0)
                {
                    whiteScore += map[i, j];
                }
                else
                {
                    blackScore += -map[i, j];
                }
            }
        }

        whiteScore += seResponseSealed.komi1 + seResponseSealed.komi2;

        double diff = blackScore - whiteScore;



        if (diff > 0)
        {
            diffLabel.text = "B+" + Math.Round(diff, 0);
        }
        else
        {
            diffLabel.text = "W+" + Math.Round(-diff, 0);
        }

        this.whiteScore.text = Math.Round(whiteScore, 0) + "";
        this.blackScore.text = Math.Round(blackScore, 0) + "";


        return res;
    }
}

