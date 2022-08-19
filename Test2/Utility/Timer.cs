using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
namespace Test2.Utility
{
    public class GeneratorTimer
    {
        private Action Function;
        public int Milliseconds { get; private set; }

        private DispatcherTimer Timer;

        private int milliseconds { get => Milliseconds % 1000; }
        private int seconds { get => Milliseconds / 1000 % 60; }
        private int minutes { get => Milliseconds / (60 * 1000) % 60; }
        private int hours { get => Milliseconds / (60 * 60 * 1000) % 24; }
        private int days { get => Milliseconds / (60 * 60 * 24 * 1000); }

        public GeneratorTimer(int milliseconds)
        {
            Milliseconds = milliseconds;
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(timerTick);
            Timer.Interval = new TimeSpan(days, hours, minutes, seconds, this.milliseconds);
            Timer.Start();
        }

        public GeneratorTimer(int milliseconds, Action function)
            : this(milliseconds)
        {
            Function = function;
        }

        public void AssignFunction(Action function)
        {
            Function += function;
        }
        private void timerTick(object sender, EventArgs e)
        {
            if(Function != null)
                Function();
        }
    }
}
