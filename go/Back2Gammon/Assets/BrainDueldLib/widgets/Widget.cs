
using System.Collections.Generic; using System;
using System.Text;

using BrainDuelsLib.view.forms;


using BrainDuelsLib.model.entities;
using BrainDuelsLib.utils;
using BrainDuelsLib.web;
using BrainDuelsLib.web.exceptions;
using BrainDuelsLib.view;
using BrainDuelsLib.utils.pic;
using BrainDuelsLib.threads;



namespace BrainDuelsLib.widgets
{
    public abstract class Widget
    {
        public abstract class CallbackStore
        {
            public Action<Exception> errorCallback = delegate { };
        }

        public abstract class ControlsStore
        {

        }

        public Widget()
        {
        }

        public virtual void Go()
        {

        }

        public IImage LightAvaToImage(LightImage li, int width, int height)
        {
            li = li.CropToSize(width, height);
            IImage avaImage = SocketManager.resourcesProvider.MakeIImage(li);
            return avaImage;
        }
    }
}
