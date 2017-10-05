using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public partial class GameFormControl : ControlBase
{
    public UILabel firstPlayerTime;
    public UILabel secondPlayerTime;
    public UILabel setupRemaining;
    public BoardScriptController controller;
    private int black;




    public void UpdateAfterState(ResponseBase rb, int index)
    {
        ShowAllButtons();
        UpdateAfterStateInit();
        UpdateAfterStateCommon(rb, index);

        if (rb is GameResponse)
        {
            SetUpdateAfterStateGameResponce((GameResponse)rb, index);
        }

        if (rb is CountingResponse)
        {
            SetUpdateAfterStateCountingResponce((CountingResponse)rb, index);
        }

        if (rb is HiddenMoveGoSetupResponse)
        {
            SetUpdateAfterStateHMSetupResponce((HiddenMoveGoSetupResponse)rb, index);
        }
        HideDisabledButtons();
    }

    public void HideDisabledButtons()
    {
        UIButton[] buttonsList = new UIButton[] { goButton, this.moveButton, this.askButton, agreeButton, passButton, seOpenButton };
        for (int i = 0; i < buttonsList.Length; i++)
        {
            if (!buttonsList[i].isEnabled)
            {
                if (!Utils.IsUnvisible(buttonsList[i].transform))
                {
                    Utils.MakeUnvisible(buttonsList[i].transform);
                }
            }
        }
    }

    public void ShowAllButtons()
    {
        UIButton[] buttonsList = new UIButton[] { goButton, this.moveButton, this.askButton, agreeButton, passButton, seOpenButton };
        for (int i = 0; i < buttonsList.Length; i++)
        {
            if (!buttonsList[i].isEnabled)
            {
                if (Utils.IsUnvisible(buttonsList[i].transform))
                {
                    Utils.MakeVisible(buttonsList[i].transform);
                }
            }
        }
    }

    public void UpdateAfterStateCommon(ResponseBase rb, int index)
    {
        this.black = rb.black;
        if (rb.handi != 0)
        {
            handicapLabel.text = "Handicap: " + rb.handi;
        }
        if (rb.black == 0)
        {
            this.firstPlayerColor.spriteName = BoardStateParser.GetSprite(Stone.BLACK);
            this.secondPlayerColor.spriteName = BoardStateParser.GetSprite(Stone.WHITE);
        }
        else
        {
            this.firstPlayerColor.spriteName = BoardStateParser.GetSprite(Stone.WHITE);
            this.secondPlayerColor.spriteName = BoardStateParser.GetSprite(Stone.BLACK);
        }
        if (rb.komi1 != 0)
        {
            this.firstPlayerKomi.text = "Komi: " + rb.komi1 + "";
        }
        if (rb.komi2 != 0)
        {
            this.secondPlayerKomi.text = "Komi: " + rb.komi2 + "";
        }

        if (rb.me == MeEnum.OBSERVER)
        {
            resignButton.isEnabled = false;
        }

        if (rb.me == MeEnum.OBSERVER)
        {
            chatButton.isEnabled = false;
        }

        controller.SetState(rb);
    }

    public void SetUpdateAfterStateCountingResponce(CountingResponse rb, int index)
    {
        if (rb.me != MeEnum.OBSERVER)
        {
            this.agreeButton.isEnabled = true;
        }
    }

    public void SetUpdateAfterStateHMSetupResponce(HiddenMoveGoSetupResponse rb, int index)
    {
        if (rb.me != MeEnum.OBSERVER)
        {
            this.moveButton.isEnabled = true;
        }
        string text = "Set up: " + rb.placed + "/" + rb.total;
        if (rb.placed == rb.total)
        {
            if (rb.me != MeEnum.OBSERVER)
            {
                goButton.isEnabled = true;
            }
        }
        this.setupRemaining.text = text;
    }


    public void SetUpdateAfterStateGameResponce(GameResponse rb, int index)
    {
        if (rb.me != MeEnum.OBSERVER)
        {
            this.passButton.isEnabled = true;
        }
        if (rb is RevealableGameResponse)
        {
            if (!((RevealableGameResponse)rb).isRevealed)
            {
                int first = ((RevealableGameResponse)rb).reveal1;
                int second = ((RevealableGameResponse)rb).reveal2;
                for (int i = 0; i < first; i++)
                {
                    firstPlayerTries[i].spriteName = "_eye";
                }

                for (int i = 0; i < second; i++)
                {
                    secondPlayerTries[i].spriteName = "_eye";
                }

                int meReveals = ((RevealableGameResponse)rb).reveal1;
                if (index == 1)
                {
                    meReveals = ((RevealableGameResponse)rb).reveal2;
                }
                if (meReveals > 0)
                {
                    if (rb.me != MeEnum.OBSERVER)
                    {
                        this.askButton.isEnabled = true;
                    }
                }
            }
            else
            {
                this.seOpenButton.isEnabled = true;
            }
        }
        else
        {
            this.seOpenButton.isEnabled = false;
            if (rb is RegularGoGameResponse)
            {
                this.seOpenButton.isEnabled = true;
            }
        }

        int current = rb.current;
        if (rb.me != MeEnum.OBSERVER)
        {
            moveButton.isEnabled = true;
        }

        if (current == 0)
        {
            this.currentPlayerHightlight.transform.localPosition = this.firstPlayerColor.transform.localPosition;
        }
        else
        {
            this.currentPlayerHightlight.transform.localPosition = this.secondPlayerColor.transform.localPosition;
        }

        if (rb.time1 != null)
        {
            this.firstPlayerTime.text = TimeToString(rb.time1);
        }

        if (rb.time2 != null)
        {
            this.secondPlayerTime.text = TimeToString(rb.time2);
        }

        if ((rb is RevealableGameResponse && ((RevealableGameResponse)rb).isRevealed) || !(rb is RevealableGameResponse)) //-V3031
        {
            if (!(rb is HiddenMoveGoGameResponse))
            {
                this.SetResponse(rb);
            }
        }
    }

    public string TimeToString(String t)
    {
        if (t[0] == 'b')
        {
            string[] ps = t.Split('#');
            int firstMainTime = int.Parse(ps[0].Substring(1));
            int firstOvertime = int.Parse(ps[1]);
            int firstPeriods = int.Parse(ps[2]);
            string res = GetByoyomiString(firstMainTime, firstOvertime, gameSettings.getOvertime(), firstPeriods);
            return res;
        }
        if (t[0] == 'a')
        {
            int firstMainTime = int.Parse(t.Substring(1));
            string res = GetAbsoluteString(firstMainTime);
            return res;
        }
        return "";
    }

    public void UpdateAfterStateInit()
    {
        for (int i = 0; i < firstPlayerTries.Length; i++)
        {
            firstPlayerTries[i].spriteName = "empty";
        }

        for (int i = 0; i < this.secondPlayerTries.Length; i++)
        {
            secondPlayerTries[i].spriteName = "empty";
        }
        this.goButton.isEnabled = false;
        this.moveButton.isEnabled = false;
        this.askButton.isEnabled = false;
        this.agreeButton.isEnabled = false;
        this.passButton.isEnabled = false;

        this.currentPlayerHightlight.transform.localPosition = new UnityEngine.Vector3(-1000, -1000, 0);
        handicapLabel.text = "";
        this.firstPlayerKomi.text = "";
        this.secondPlayerKomi.text = "";
        this.firstPlayerTime.text = "";
        this.secondPlayerTime.text = "";
        setupRemaining.text = "";

    }


    public static String GetByoyomiString(int mainTime, int overtimeTime, int overtime, int periods)
    {
        int mainTimeMinutes = mainTime / 60;
        int mainTimeSeconds = mainTime % 60;
        return expand(mainTimeMinutes) + ":" + expand(mainTimeSeconds) + " + " + expand(overtimeTime) + "/" + overtime + "(" + periods + ")";
    }

    public static String GetAbsoluteString(int mainTime)
    {
        int mainTimeMinutes = mainTime / 60;
        int mainTimeSeconds = mainTime % 60;
        return expand(mainTimeMinutes) + ":" + expand(mainTimeSeconds);
    }



    public static String expand(int val)
    {
        if (val < 10)
        {
            return "0" + val;
        }
        else
        {
            return val + "";
        }
    }


}
