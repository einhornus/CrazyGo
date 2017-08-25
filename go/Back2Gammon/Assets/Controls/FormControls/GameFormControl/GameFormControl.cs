using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using BrainDuelsLib.threads;
using BrainDuelsLib.model.entities;
using UnityEngine;
//using admob;

public partial class GameFormControl : ControlBase
{
    /*
    public UI2DSprite myAvatar;
    public UI2DSprite opponentAvatar;
    public UILabel myNameLabel;
    public UILabel opponentNameLabel;
    */
    public Camera camera;
    public float rightX;
    public float leftX;
    private bool isTest = false;

    public void setX()
    {
        double screenEdge = (float)Screen.width / (float)Screen.height * 720 / 2f;
        double possibleEdge = screenEdge - 30;
        double alpha = 0.6;
        float x = (float)(450 * (1 - alpha) + possibleEdge * alpha);


        this.resignButton.transform.localPosition = new Vector3(x, this.resignButton.transform.localPosition.y, 0);
        this.settingsButton.transform.localPosition = new Vector3(x, this.settingsButton.transform.localPosition.y, 0);


        this.firstPlayerTime.transform.localPosition = new Vector3(-x, this.firstPlayerTime.transform.localPosition.y, 0);
        this.secondPlayerTime.transform.localPosition = new Vector3(-x, this.secondPlayerTime.transform.localPosition.y, 0);

        this.firstName.transform.localPosition = new Vector3(-x, this.firstName.transform.localPosition.y, 0);
        this.secondName.transform.localPosition = new Vector3(-x, this.secondName.transform.localPosition.y, 0);

    }



    void Start()
    {
        //return;
        this.chatPanel.transform.localPosition = new Vector3(0, 0, 0);
        this.gameEndDialog.transform.localPosition = new Vector3(0, 0, 0);
        this.resignPopup.transform.localPosition = new Vector3(0, 0, 0);
        this.tryPopup.transform.localPosition = new Vector3(0, 0, 0);
        //this.backgammonTutorial.transform.localPosition = new Vector3(0, 0, 0);
        //camera.orthographicSize = (float)Screen.height / (float)600.0 * 1.05f;

        Utils.MakeUnvisible(resignPopup.transform);
        Utils.MakeUnvisible(tryPopup.transform);
        InitGameWidgetPart();
        SetOnGameEndDialog();
        InitChatPart();
        SetScoreEstimatorPart();
        this.SetSettingsPart();



        /*
        if (isTest)
        {

            String text1 = "game|1|19|go|0999999909999999999999991999919999999999190999909999999999990999999999999999999910999999909999999999999991999919999999999190999909999999999990999999999999999999910999999909999999999999991999919999999999190999909999999999990999999999999999999910999999909999999999999991999919999999999190999909999999999990999999999999999999919999999999999999999999999999990000000|2-6";
            String text2 = "counting|1|9|go|999999999999999999999999199991999999999919099990999999999999099999999999999999999|111111111111111111111111011111111111111101111111111111111111111111111111111111111|";
            String text3 = "game|1|9|one-color-go|999999999999999999999999199991999999999919099990999999999999099999999999999999999|2-6";
            String text4 = "counting|1|9|one-color-go|999999999999999999999999199991999999999919099990999999999999099999999999999999999|111111111111111111111111011111111111111101111111111111111111111111111111111111111|";
            String text5 = "game|1|9|blind-go|999999999999999999999999199991999999999919099990999999999999099999999999999999999|2-6";
            String text6 = "counting|1|9|blind-go|999999999999999999999999199991999999999919099990999999999999099999999999999999999|111111111111111111111111011111111111111101111111111111111111111111111111111111111|";
            String text7 = "setup|1|9|hidden-move-go|999999999999999999999999999999999999999999999999999999999999999999999999999999999|4%4%1%5%4%1%4%4%0%4%3%0%";
            String text8 = "game|1|9|hidden-move-go|999999999999999999999999999999919999999099999999919999999099999999999999999999999|5%4%1%4%3%0%";

            ResponseBase response = BoardStateParser.Parse(text1, 0, 0);
            response.Reveal();
            board.SetState(response);
        }
         * */

        //DataPasser.SetAdmob();
        //DataPasser.ShowAds();
    }
}

