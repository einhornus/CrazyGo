using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using BrainDuelsLib.threads;
using BrainDuelsLib.model.entities;
using UnityEngine;

public partial class GameFormControl : ControlBase
{
    public UIInput messageInput;
    public UISprite chatPanel;
    public UIButton sendButton;
    public UITextList list;
    public UIButton closeChatButton;

    private ChatWidget chat;
    public CrazyGoTimerScript chatTimer;
    private int opponentId;

    private bool isOpened = false;
    public void InitChatPart()
    {

        Utils.MakeUnvisible(chatPanel.transform);
        chatButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                pressButtonSound.Play();
                Debug.Log(Utils.IsUnvisible(chatPanel.transform));
                if (Utils.IsUnvisible(chatPanel.transform))
                {
                    Utils.MakeVisible(chatPanel.transform);
                    isOpened = true;
                    //chatButton.normalSprite = "chat";
                }
                else
                {
                    Utils.MakeUnvisible(chatPanel.transform);
                    isOpened = false;
                }
            }

            )));


        closeChatButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                pressButtonSound.Play();
                isOpened = false;
                Utils.MakeUnvisible(chatPanel.transform);
            })));
    }

    public void StartChat()
    {
        chat = new ChatWidget(tai);
        chat.Callbacks.onMessageReceived = delegate (int id, string mes)
        {
            //Debug.Log("Received " + mes);
            User user = Server.GetUser(tai, id);
            string m = "";
            string begin = user.login + "(" + user.GetRankString() + ")";
            m = begin + ": " + mes + "\n";
            list.Add(m);

            if (!isOpened)
            {
                this.chatSound.Play();
                //chatButton.normalSprite = "chat_dot";
            }
        };
        chat.Controls.timer = new CrazyGoTimerControl(250, chatTimer);
        chat.Go();

        if (game.users[0] == tai.id)
        {
            opponentId = game.users[1];
        }
        else
        {
            opponentId = game.users[0];
        }

        messageInput.onSubmit.Add(new EventDelegate(new EventDelegate.Callback(

            delegate
            {
                chat.SendMessage(opponentId, messageInput.value);
                messageInput.value = "";
            }

            )));

        sendButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(

        delegate
        {
            chat.SendMessage(opponentId, messageInput.value);
            messageInput.value = "";
        }

        )));
    }

    public void StopChat()
    {
        if (chat != null)
        {
            Debug.Log("STOP CHAT");
            chat.Stop();
        }
    }
}
