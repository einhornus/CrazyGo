
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
    public class UnitySignupWidget : Widget
    {
        private UnitySignupWidgetCallbackStore store = new UnitySignupWidgetCallbackStore();
        public UnitySignupWidgetCallbackStore Callbacks
        {
            get
            {
                return store;
            }
        }

        public class UnitySignupWidgetCallbackStore : CallbackStore
        {
            public BrainDuelsLib.delegates.Action<Server.TokenAndId> succesfullSignUpCallback = delegate { };
            public BrainDuelsLib.delegates.Action passwordsDoNotMatchCallback = delegate { };
            public BrainDuelsLib.delegates.Action incorrectPasswordCallback = delegate { };
            public BrainDuelsLib.delegates.Action incorrectLoginCallback = delegate { };
            public BrainDuelsLib.delegates.Action userExists = delegate { };
        }

        public class UnitySignupWidgetControlsStore : CallbackStore
        {
            public InputTextControl loginField;
            public InputTextControl passwordField;
            public InputTextControl confirmPasswordFirls;

            public ButtonControl button;
        }

        private UnitySignupWidgetControlsStore controlsStore = new UnitySignupWidgetControlsStore();
        public UnitySignupWidgetControlsStore Controls
        {
            get
            {
                return controlsStore;
            }
        }

        public void Register(String login, String password)
        {
            try
            {
                Server.Register(login, password);
                Server.TokenAndId tai = Server.Authorize(login, password);
                Server.SetUser(tai, 0);
                store.succesfullSignUpCallback(tai);
            }
            catch (LoginExists e3)
            {
                store.userExists();
            }
            catch (WebException e2)
            {
                store.errorCallback(e2);
            }
        }

        private void RegistrationClick(object sender, EventArgs e)
        {
            String login = Controls.loginField.GetText();
            String password = Controls.passwordField.GetText();
            String confirmation = Controls.confirmPasswordFirls.GetText();

            if (!password.Equals(confirmation))
            {
                Callbacks.passwordsDoNotMatchCallback();
                return;
            }

            if (!CheckLogin(login))
            {
                Callbacks.incorrectLoginCallback();
                return;
            }

            if (!CheckPassword(password))
            {
                Callbacks.incorrectPasswordCallback();
                return;
            }

            Register(login, password);
        }

        public bool CheckLogin(string s)
        {
            return UnityLoginWidget.checkLogin(s);
        }

        public bool CheckPassword(string s)
        {
            return UnityLoginWidget.checkPassword(s);
        }

        public UnitySignupWidget()
            : base()
        {
        }

        public override void Go()
        {
            base.Go();
            this.Controls.button.SetOnClick(RegistrationClick);
        }
    }
}