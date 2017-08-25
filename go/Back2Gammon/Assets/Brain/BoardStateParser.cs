using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib;
using BrainDuelsLib.utils.json;
using Newtonsoft;


public enum MeEnum
{
    BLACK, WHITE, OBSERVER
};

public enum Stone
{
    BLACK, WHITE, BLACK_DEAD, WHITE_DEAD, WHITE_HIDDEN, BLACK_HIDDEN, UNKNOWN, EMPTY, BLACK_TERRITORY, WHITE_TERRITORY
}

public class ResponseBase
{
    public Board board;
    public MeEnum me;
    public double komi1;
    public double komi2;
    public int black;
    public int handi;
}

public class GameResponse : ResponseBase
{
    public Move lastMove;
    public int current;
    public String time1;
    public String time2;
}

public class CountingResponse : ResponseBase
{
    public List<Move> blackTerritory = new List<Move>();
    public List<Move> whiteTerritory = new List<Move>();
    public List<Move> dead = new List<Move>();
    public List<double> blackTerritorySize = new List<double>();
    public List<double> whiteTerritorySize = new List<double>();
}

public class RegularGoGameResponse : GameResponse
{

}

public class RegularGoCountingResponse : CountingResponse
{
}

public class RevealableGameResponse : GameResponse
{
    public bool isRevealed = false;
    public bool isAsking;
    public int reveal1;
    public int reveal2;
}

public class OneColorGoGameResponse : RevealableGameResponse
{
}

public class OneColorGoCountingResponse : CountingResponse
{
}

public class BlindGoGameResponse : RevealableGameResponse
{
}

public class BlindGoCountingResponse : CountingResponse
{
}

public class HiddenMoveGoGameResponse : GameResponse
{
    public List<Move> hiddens = new List<Move>();
    public List<int> colors;
}

public class HiddenMoveGoSetupResponse : ResponseBase
{
    public List<Move> hiddens = new List<Move>();
    public int total;
    public int placed;
    public List<int> colors;
}

public class HiddenMoveGoCountingResponse : CountingResponse
{
}

public class Move
{
    public int x;
    public int y;

    public Move(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Move()
    {

    }
}

public class Point
{
    public float x;
    public float y;

    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}


public class Board
{
    public int n;
    public List<Move> blacks = new List<Move>();
    public List<Move> whites = new List<Move>();
}

public class HiddenMoveBoard : Board
{
    public List<Move> hiddenMoves = new List<Move>();
}

public class BoardStateParser
{
    public static string GetSprite(Stone stone)
    {
        if (stone == Stone.BLACK_HIDDEN)
        {
            return "_black_alpha";
        }

        if (stone == Stone.WHITE_HIDDEN)
        {
            return "_white_alpha";
        }

        if (stone == Stone.BLACK)
        {
            return "_black";
        }

        if (stone == Stone.BLACK_DEAD)
        {
            return "_black_dead";
        }

        if (stone == Stone.WHITE)
        {
            return "_white";
        }

        if (stone == Stone.WHITE_DEAD)
        {
            return "_white_dead";
        }



        if (stone == Stone.WHITE_TERRITORY)
        {
            return "_white_territory";
        }

        if (stone == Stone.BLACK_TERRITORY)
        {
            return "_black_territory";
        }

        if (stone == Stone.UNKNOWN)
        {
            return "_question";
        }

        return "empty";
    }


    public static void ParseHiddenMoves(ResponseBase _in, ResponseJson rj)
    {
        string[] content = rj.hidden_moves;
        if (_in is HiddenMoveGoSetupResponse)
        {
            ((HiddenMoveGoSetupResponse)_in).hiddens = new List<Move>();
            ((HiddenMoveGoSetupResponse)_in).colors = new List<int>();
        }
        if (_in is HiddenMoveGoGameResponse)
        {
            ((HiddenMoveGoGameResponse)_in).hiddens = new List<Move>();
            ((HiddenMoveGoGameResponse)_in).colors = new List<int>();
        }
        for (int i = 0; i < content.Length; i++)
        {
            string[] sss = content[i].Split('#');
            int x = int.Parse(sss[0]);
            int y = int.Parse(sss[1]);
            int color = int.Parse(sss[2]);

            if (_in is HiddenMoveGoSetupResponse)
            {
                ((HiddenMoveGoSetupResponse)_in).hiddens.Add(new Move(x, y));
                ((HiddenMoveGoSetupResponse)_in).colors.Add(color);
            }
            if (_in is HiddenMoveGoGameResponse)
            {
                ((HiddenMoveGoGameResponse)_in).hiddens.Add(new Move(x, y));
                ((HiddenMoveGoGameResponse)_in).colors.Add(color);
            }
        }
    }

    public static List<int> ParseHiddenMovesColors(string content)
    {
        List<int> list = new List<int>();
        string[] set = content.Split('%');
        for (int i = 0; i < set.Length / 3; i++)
        {
            int x = int.Parse(set[3 * i]);
            int y = int.Parse(set[3 * i + 1]);
            int color = int.Parse(set[3 * i + 2]);
            list.Add(color);
        }
        return list;
    }

    public class ResponseJson
    {
        public String phase;
        public int black_player;
        public double komi1;
        public double komi2;
        public int handi;
        public int n;
        public String board;
        public String title;
        public int asking;
        public int rev1;
        public int rev2;
        public String[] moves;
        public String life_table;
        public String territory_table;
        public bool is_revealed;
        public String last_move;
        public String[] hidden_moves;
        public int current;
        public String time1;
        public String time2;
        public int hm_count;
    }

    public static ResponseBase Parse(string str, int index)
    {
        ResponseBase res = null;

        ResponseJson rj = JsonConvert.DeserializeObject<ResponseJson>(str);

        string phase = rj.phase;
        int boardSize = rj.n;

        int nPlayers = 2;
        string gameTitle = rj.title;
        string boardContent = rj.board;

        Board board = new Board();
        board.n = boardSize;
        int cnt = 0;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int val = (int)(boardContent[cnt] - '0');
                if (val != 9)
                {
                    if (val == rj.black_player)
                    {
                        board.blacks.Add(new Move(i, j));
                    }
                    else
                    {
                        board.whites.Add(new Move(i, j));
                    }
                }
                cnt++;
            }
        }
        if (phase == "game")
        {
            bool ask = rj.asking == 1;
            int rev1 = rj.rev1;
            int rev2 = rj.rev2;
            String time1 = rj.time1;
            String time2 = rj.time2;
            if (gameTitle.Equals("go"))
            {
                res = new RegularGoGameResponse();
            }

            if (gameTitle.Equals("one-color-go"))
            {
                res = new OneColorGoGameResponse();
                ((OneColorGoGameResponse)res).isAsking = ask;
                ((OneColorGoGameResponse)res).reveal1 = rev1;
                ((OneColorGoGameResponse)res).reveal2 = rev2;
            }

            if (gameTitle.Equals("blind-go"))
            {
                res = new BlindGoGameResponse();
                ((BlindGoGameResponse)res).isAsking = ask;
                ((BlindGoGameResponse)res).reveal1 = rev1;
                ((BlindGoGameResponse)res).reveal2 = rev2;
            }

            if (gameTitle.Equals("hidden-move-go"))
            {
                string[] cont = rj.hidden_moves;
                res = new HiddenMoveGoGameResponse();
                ParseHiddenMoves(res, rj);
            }

            if (!rj.last_move.Equals("nada"))
            {
                string lastMove = rj.last_move;
                string[] ps = lastMove.Split('-');
                int a = int.Parse(ps[0]);
                int b = int.Parse(ps[1]);
                Move lm = new Move(a, b);
                ((GameResponse)res).lastMove = lm;
            }

            ((GameResponse)res).time1 = rj.time1;
            ((GameResponse)res).time2 = rj.time2;
            ((GameResponse)res).current = rj.current;

        }

        if (phase == "counting")
        {
            string lifeTableContent = rj.life_table;
            List<Move> dead = new List<Move>();
            bool[,] lifeTable = new bool[boardSize, boardSize];
            cnt = 0;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    lifeTable[i, j] = (lifeTableContent[cnt] == '0');
                    if (lifeTable[i, j])
                    {
                        dead.Add(new Move(i, j));
                    }
                    cnt++;
                }
            }

            string countingTableContent = rj.territory_table;
            List<Move> whiteTerritory = new List<Move>();
            List<Move> blackTerritory = new List<Move>();
            List<double> whiteTerritorySize = new List<double>();
            List<double> blackTerritorySize = new List<double>();
            cnt = 0;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (countingTableContent[cnt] != '9')
                    {
                        if (countingTableContent[cnt] == '0' && rj.black_player == 0)
                        {
                            blackTerritory.Add(new Move(i, j));
                            blackTerritorySize.Add(1.0);
                        }

                        if (countingTableContent[cnt] == '1' && rj.black_player == 1)
                        {
                            blackTerritory.Add(new Move(i, j));
                            blackTerritorySize.Add(1.0);
                        }

                        if (countingTableContent[cnt] == '0' && rj.black_player == 1)
                        {
                            whiteTerritory.Add(new Move(i, j));
                            whiteTerritorySize.Add(1.0);
                        }

                        if (countingTableContent[cnt] == '1' && rj.black_player == 0)
                        {
                            whiteTerritory.Add(new Move(i, j));
                            whiteTerritorySize.Add(1.0);
                        }
                    }
                    cnt++;
                }
            }

            if (gameTitle.Equals("go"))
            {
                res = new RegularGoCountingResponse();
                ((RegularGoCountingResponse)res).dead = dead;
                ((RegularGoCountingResponse)res).whiteTerritory = whiteTerritory;
                ((RegularGoCountingResponse)res).blackTerritory = blackTerritory;
                ((RegularGoCountingResponse)res).whiteTerritorySize = whiteTerritorySize;
                ((RegularGoCountingResponse)res).blackTerritorySize = blackTerritorySize;
            }

            if (gameTitle.Equals("one-color-go"))
            {
                res = new OneColorGoCountingResponse();
                ((OneColorGoCountingResponse)res).dead = dead;
                ((OneColorGoCountingResponse)res).whiteTerritory = whiteTerritory;
                ((OneColorGoCountingResponse)res).blackTerritory = blackTerritory;
                ((OneColorGoCountingResponse)res).whiteTerritorySize = whiteTerritorySize;
                ((OneColorGoCountingResponse)res).blackTerritorySize = blackTerritorySize;

            }

            if (gameTitle.Equals("blind-go"))
            {
                res = new BlindGoCountingResponse();
                ((BlindGoCountingResponse)res).dead = dead;
                ((BlindGoCountingResponse)res).whiteTerritory = whiteTerritory;
                ((BlindGoCountingResponse)res).blackTerritory = blackTerritory;
                ((BlindGoCountingResponse)res).whiteTerritorySize = whiteTerritorySize;
                ((BlindGoCountingResponse)res).blackTerritorySize = blackTerritorySize;

            }

            if (gameTitle.Equals("hidden-move-go"))
            {
                res = new HiddenMoveGoCountingResponse();
                ((HiddenMoveGoCountingResponse)res).dead = dead;
                ((HiddenMoveGoCountingResponse)res).whiteTerritory = whiteTerritory;
                ((HiddenMoveGoCountingResponse)res).blackTerritory = blackTerritory;
                ((HiddenMoveGoCountingResponse)res).whiteTerritorySize = whiteTerritorySize;
                ((HiddenMoveGoCountingResponse)res).blackTerritorySize = blackTerritorySize;

            }

        }

        if (phase.Equals("setup"))
        {
            if (gameTitle.Equals("hidden-move-go"))
            {
                res = new HiddenMoveGoSetupResponse();
                HiddenMoveGoSetupResponse tr = ((HiddenMoveGoSetupResponse)res);
                tr.total = rj.hm_count;
                ParseHiddenMoves(tr, rj);

                for (int i = 0; i < tr.colors.Count; i++)
                {
                    if (tr.colors[i] == index)
                    {
                        tr.placed++;
                    }
                }
            }
        }


        if (rj.black_player == index)
        {
            res.me = MeEnum.BLACK;
        }

        if (rj.black_player == 1 - index)
        {
            res.me = MeEnum.WHITE;
        }

        if (index == 9)
        {
            res.me = MeEnum.OBSERVER;
        }

        res.board = board;
        res.black = rj.black_player;
        res.komi1 = rj.komi1;
        res.komi2 = rj.komi2;

        if (res is RevealableGameResponse)
        {
            ((RevealableGameResponse)res).isRevealed = rj.is_revealed;
        }

        res.handi = rj.handi;


        Dump(res);

        return res;
    }


    public static void Dump(object output)
    {
        //NGUIDebug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(output));
    }
}