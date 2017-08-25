using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using UnityEngine;
using BrainDuelsLib.web.socket_requests;


public class UnitySignupFormControl : ControlBase
{
    BrainDuelsLib.widgets.UnitySignupWidget widget;

    public CrazyGoButton signUpButton;

    public CrazyGoInputText loginInputText;
    public CrazyGoInputText confirmPasswordInputText;
    public CrazyGoInputText passwordInputText;
    public UILabel errorMessage;

    public UIButton back;
    public Camera camera;

    void Start()
    {
        //camera.orthographicSize = (float)Screen.height / (float)600.0 * 1.0f;

        back.onClick.Add(new EventDelegate(new EventDelegate.Callback(
    delegate
    {
        Application.LoadLevel(0);
    }
    )));

        widget = new BrainDuelsLib.widgets.UnitySignupWidget();
        widget.Controls.button = signUpButton;
        widget.Controls.loginField = loginInputText;
        widget.Controls.passwordField = passwordInputText;
        widget.Controls.confirmPasswordFirls = confirmPasswordInputText;

        widget.Callbacks.errorCallback = delegate
        {
            UnityEngine.Debug.Log("error");
            errorMessage.text = "Unexpected error";
        };

        widget.Callbacks.userExists = delegate
        {
            UnityEngine.Debug.Log("error");
            errorMessage.text = "Login is not available";
        };


        widget.Callbacks.incorrectLoginCallback = delegate
        {
            UnityEngine.Debug.Log("Incorrect login");
            errorMessage.text = UnityLoginFormControl.INCORRECT_LOGIN_MESSAGE;

        };

        widget.Callbacks.incorrectPasswordCallback = delegate
        {
            UnityEngine.Debug.Log("Incorrect password");
            errorMessage.text = UnityLoginFormControl.INCORRECT_PASSWORD_MESSAGE;
        };

        widget.Callbacks.passwordsDoNotMatchCallback = delegate
        {
            UnityEngine.Debug.Log("Passwords do not match");
            errorMessage.text = "Passwords do not match";
        };

        widget.Callbacks.succesfullSignUpCallback = delegate (Server.TokenAndId tai)
        {
            errorMessage.text = "Logging in...";
            UnityEngine.Debug.Log("sign up success");
            UnityEngine.Debug.Log("login success");
            DataPasser.Get().Set("tai", tai);
            Application.LoadLevel(2);
        };

        widget.Go();
    }

    void Awake()
    {
    }


    public void OnApplicationQuit()
    {
        DBSocketRequest.Close();
    }
}