
using System.Collections.Generic; using System;
using System.Text;

using BrainDuelsLib.view.forms;
using BrainDuelsLib.view;
using BrainDuelsLib.web;
using BrainDuelsLib.web.exceptions;
using BrainDuelsLib.delegates;
using System;

namespace BrainDuelsLib.widgets
{
    public class RegistrationWidget : Widget
    {
        private RegistrationWidgetCallbackStore store = new RegistrationWidgetCallbackStore();
        public RegistrationWidgetCallbackStore Callbacks
        {
            get
            {
                return store;
            }
        }

        public class RegistrationWidgetCallbackStore : CallbackStore
        {
			public BrainDuelsLib.delegates.Action succesfullRegistrationCallback = delegate { };
			public BrainDuelsLib.delegates.Action loginAlreadyExistsCallback = delegate { };
			public BrainDuelsLib.delegates.Action passwordsDoNotMatchCallback = delegate { };
			public BrainDuelsLib.delegates.Action invalidLoginPasswordCallback = delegate { };
			public BrainDuelsLib.delegates.Action<Server.TokenAndId> succesfullAuthorizationCallback = delegate { };
        }

        public class RegistrationWidgetControlsStore : CallbackStore
        {
            public InputTextControl loginField;
            public InputTextControl passwordField;
            public InputTextControl confirmPasswordField;

            public ButtonControl registrationButton;
            public ButtonControl authorizationButton;
        }

        private RegistrationWidgetControlsStore controlsStore = new RegistrationWidgetControlsStore();
        public RegistrationWidgetControlsStore Controls
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
                store.succesfullRegistrationCallback();
            }
            catch (LoginExists e1)
            {
                store.loginAlreadyExistsCallback();
            }
            catch (WebException e2)
            {
                store.errorCallback(e2);
            }
        }

        private void RegistrationClick(object sender, EventArgs e)
        {
            //UnityEngine.Debug.Log("registraton");
            String login = Controls.loginField.GetText();
            String password = Controls.passwordField.GetText();
            String confirmPassword = Controls.confirmPasswordField.GetText();
            if (!password.Equals(confirmPassword))
            {
                store.passwordsDoNotMatchCallback();
                return;
            }
            Register(login, password);
        }

        public void Authorize(String login, String password)
        {
            try
            {
                Server.TokenAndId tai = Server.Authorize(login, password);
                store.succesfullAuthorizationCallback(tai);
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

        private void AuthorizationClick(object sender, EventArgs e)
        {
            //UnityEngine.Debug.Log("authorization");
            String login = Controls.loginField.GetText();
            String password = Controls.passwordField.GetText();
            String confirmPassword = Controls.confirmPasswordField.GetText();
            Authorize(login, password);
        }

        public RegistrationWidget()
            : base()
        {
        }

        public String GetName(int len)
        {
            string res = "";
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                res += (char)(random.Next() % 26 + 'a');
            }
            return res;
        }

        public override void Go()
        {
            base.Go();
            this.Controls.authorizationButton.SetOnClick(AuthorizationClick);
            this.Controls.registrationButton.SetOnClick(RegistrationClick);
            
            string name = GetName(9);
            Register(name, name+"_password");
            Authorize(name, name + "_password");
            //Authorize("hui8", "hui_pass");
        }
    }
}
