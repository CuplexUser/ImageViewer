namespace ImageViewer.Utility;

internal static class DelayOperation
{
    public static void DelayAction(Action action, int delayTime)
    {
        ThreadPool.QueueUserWorkItem(CallBack, new DelayedAction(action, delayTime));
    }

    private static void CallBack(object state)
    {
        if (state is DelayedAction action)
        {
            action.Execute();
        }
    }

    private class DelayedAction
    {
        private readonly Action _action;
        private readonly int _delayTime;

        public DelayedAction(Action action, int delayTime)
        {
            _action = action;
            _delayTime = delayTime;
        }

        public void Execute()
        {
            Thread.Sleep(_delayTime);
            _action.Invoke();
        }
    }
}