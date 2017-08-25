
using System.Collections.Generic;
using System;
using System.Text;

using BrainDuelsLib.view.forms;
using BrainDuelsLib.view;
using BrainDuelsLib.web;
using BrainDuelsLib.web.exceptions;
using BrainDuelsLib.delegates;
using System;

namespace BrainDuelsLib.widgets
{
    public class UnityLoginWidget : Widget
    {
        private UnityLoginWidgetCallbackStore store = new UnityLoginWidgetCallbackStore();
        public UnityLoginWidgetCallbackStore Callbacks
        {
            get
            {
                return store;
            }
        }

        public class UnityLoginWidgetCallbackStore : CallbackStore
        {
            public BrainDuelsLib.delegates.Action<Server.TokenAndId> succesfullLoginCallback = delegate { };
            public BrainDuelsLib.delegates.Action invalidLoginPasswordCallback = delegate { };
            public BrainDuelsLib.delegates.Action signUpCallback = delegate { };
            public BrainDuelsLib.delegates.Action wrongLogin = delegate { };
            public BrainDuelsLib.delegates.Action wrongPassword = delegate { };
        }

        public class UnityLoginWidgetControlsStore : CallbackStore
        {
            public InputTextControl loginField;
            public InputTextControl passwordField;

            public ButtonControl loginButton;
            public ButtonControl signUpReferenceButton;
        }

        private UnityLoginWidgetControlsStore controlsStore = new UnityLoginWidgetControlsStore();
        public UnityLoginWidgetControlsStore Controls
        {
            get
            {
                return controlsStore;
            }
        }

        public void Authorize(String login, String password)
        {
            try
            {
                Server.TokenAndId tai = Server.Authorize(login, password);
                store.succesfullLoginCallback(tai);
            }
            catch (WrongLoginPassword e1)
            {
                store.invalidLoginPasswordCallback();
            }
            catch (WebException e2)
            {
                store.errorCallback(e2);
            }
        }

        public static bool checkLogin(string s)
        {
            if (s.Length < 4)
            {
                return false;
            }
            for (int i = 0; i < s.Length; i++)
            {
                if (!(Char.IsLetterOrDigit(s[i])))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool checkPassword(string s)
        {
            return checkLogin(s);
        }

        private void AuthorizationClick(object sender, EventArgs e)
        {
            String login = Controls.loginField.GetText();
            String password = Controls.passwordField.GetText();

            if(!checkLogin(login)){
                Callbacks.wrongLogin();
                return;
            }

            if(!checkPassword(password)){
                Callbacks.wrongPassword();
                return;
            }

            Authorize(login, password);
        }

        private void SignUpClick(object sender, EventArgs e)
        {
            Callbacks.signUpCallback();
        }


        public UnityLoginWidget()
            : base()
        {
        }

        public override void Go()
        {
            base.Go();
            this.Controls.loginButton.SetOnClick(AuthorizationClick);
            this.Controls.signUpReferenceButton.SetOnClick(SignUpClick);
        }
    }
}