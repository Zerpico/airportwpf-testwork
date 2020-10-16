using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AirportWPF
{
    // Велосипед для Dispatcher 
    public static class Executer
    {
        private static Action<Action> executor = action => action();

        /// <summary>
        /// Инициализация
        /// </summary>
        public static void Initialize()
        {

            var dispatcher = Dispatcher.CurrentDispatcher;

            executor = action => 
            {
                if (dispatcher.CheckAccess())
                    action();
                else dispatcher.BeginInvoke(action);
            };
        }

        /// <summary>
        /// Запускает действия UI в потоке
        /// </summary>
        /// <param name="action"></param>
        public static void OnUIThread(this System.Action action)
        {
            executor(action);
        }
    }
}
