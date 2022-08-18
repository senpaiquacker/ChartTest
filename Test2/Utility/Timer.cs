using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace Test2.Utility
{
    public class GeneratorTimer
    {
        public Action Function { get; private set; }
        public int Time { get; private set; }

        public GeneratorTimer(int milliseconds, Action func)
        {
            Time = milliseconds;
            Function = func;
            LoopTime();
        }
        
        public void LoopTime()
        {
            TimerCallback tm = new TimerCallback(CallFunction);
            Timer timer = new Timer(tm, null, 0, Time);
        }
        private void CallFunction(object obj)
        {
            Function.Invoke();
        }
    }
}
