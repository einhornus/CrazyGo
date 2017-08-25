using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;
using UnityEngine;
using BrainDuelsLib.web;

public class UnityLoginFormControl : ControlBase
{
    BrainDuelsLib.widgets.UnityLoginWidget widget;

    public CrazyGoButton loginButton;
    public CrazyGoButton signUpButton;
    public CrazyGoButton facebookButton;
    public CrazyGoInputText loginInputText;
    public CrazyGoInputText passwordInputText;

    public Camera camera;

    public UIToggle toggle;
    public UILabel errorMessage;

    public static string INCORRECT_LOGIN_MESSAGE = "Invalid login: it should be at least 4 characters long and consist of digits and letters only.";
    public static string INCORRECT_PASSWORD_MESSAGE = "Invalid password: it should be at least 4 characters long and consist of digits and letters only.";



    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void Start()
    {
        if (DataPasser.Get().dic.ContainsKey("repeat") && DataPasser.Get().Get("repeat").Equals("true"))
        {
            errorMessage.text = "You are logged in from another device";
        }
        Debug.Log(Screen.width + " " + Screen.height);
        //camera.orthographicSize = Screen.height / (float)600.0 * 1.0f;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Debug.Log("Logiiiin");

        /*
        if (UnityEngine.PlayerPrefs.HasKey("fb") && (UnityEngine.PlayerPrefs.GetString("fb").Equals("true")))
        {
            string fbLogin = UnityEngine.PlayerPrefs.GetString("fb_login");
            string fbPassword = UnityEngine.PlayerPrefs.GetString("fb_password");
            Server.TokenAndId tai = Server.Authorize(fbLogin, fbPassword);
            Debug.Log("to lobby, " + tai);
            return;
        }
        */

        this.toggle.value = false;
        if (UnityEngine.PlayerPrefs.HasKey("remember") && (UnityEngine.PlayerPrefs.GetString("remember").Equals("yes")))
        {
            loginInputText.text.value = UnityEngine.PlayerPrefs.GetString("login");
            passwordInputText.text.value = UnityEngine.PlayerPrefs.GetString("password");
            this.toggle.value = true;
        }
        else
        {

        }

        widget = new BrainDuelsLib.widgets.UnityLoginWidget();
        widget.Controls.loginButton = loginButton;
        widget.Controls.signUpReferenceButton = signUpButton;
        widget.Controls.loginField = loginInputText;
        widget.Controls.passwordField = passwordInputText;

        widget.Callbacks.errorCallback = delegate
        {
            UnityEngine.Debug.Log("error");
            errorMessage.text = "Unexpected error";
        };

        widget.Callbacks.wrongLogin = delegate
        {
            UnityEngine.Debug.Log("wrong login");
            errorMessage.text = INCORRECT_LOGIN_MESSAGE;
        };

        widget.Callbacks.wrongPassword = delegate
        {
            UnityEngine.Debug.Log("wrong password");
            errorMessage.text = INCORRECT_PASSWORD_MESSAGE;
        };

        widget.Callbacks.invalidLoginPasswordCallback = delegate
        {
            UnityEngine.Debug.Log("invalid login password");
            errorMessage.text = "Invalid login or password";

        };

        widget.Callbacks.signUpCallback = delegate
        {
            Application.LoadLevel(1);
            UnityEngine.Debug.Log("go to sign up");
        };

        widget.Callbacks.succesfullLoginCallback = delegate (Server.TokenAndId tai)
        {
            errorMessage.text = "Logging in";

            UnityEngine.Debug.Log("login success");
            DataPasser.Get().Set("tai", tai);



            if (toggle.value)
            {
                UnityEngine.PlayerPrefs.SetString("login", loginInputText.text.value);
                UnityEngine.PlayerPrefs.SetString("password", passwordInputText.text.value);
                UnityEngine.PlayerPrefs.Save();
            }
            else
            {
                //UnityEngine.PlayerPrefs.SetString("login", loginInputText.text.value);
                //UnityEngine.PlayerPrefs.SetString("password", passwordInputText.text.value);
                //UnityEngine.PlayerPrefs.Save();
            }
            Application.LoadLevel(2);
        };

        widget.Go();


        toggle.onChange.Add(new EventDelegate(new EventDelegate.Callback(

            delegate ()
            {
                if (toggle.value)
                {
                    UnityEngine.PlayerPrefs.SetString("remember", "yes");
                }
                else
                {
                    UnityEngine.PlayerPrefs.SetString("remember", "no");
                }
            }

        )));
    }



}