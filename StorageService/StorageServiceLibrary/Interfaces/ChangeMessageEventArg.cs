

using System;

namespace StorageServiceLibrary
{
    [Serializable]
    public class ChangeMessageEventArgs<T> : EventArgs
    {

        public Actions Action { get; }
        public T[] Argument { get; }

        public ChangeMessageEventArgs(Actions action, params T[] arg)
        {
            Action = action;
            Argument = arg;
        }
    }

    public enum Actions
    {
        Remove = 0,
        Add = 1
    }

}
