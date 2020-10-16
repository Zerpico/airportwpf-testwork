using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AirportWPF
{
    public class DTimeService
    {
        //таймер
        private readonly DispatcherTimer _countTimer;
        //счётчик
        private double sumInterval;

        //обновляемое время
        private DateTime upTime;
        //шаг времени
        private double step;

        // делегат по времени
        public delegate void DateTimeHandler(DateTime step);
        // Событие, возникающее при обновлении времени
        public event DateTimeHandler OnChangeTime;

        public DTimeService(DateTime realTime)
        {
            //инициализируем всё
            upTime = realTime;
            step = 1;
            _countTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100),
                IsEnabled = false
            };
            _countTimer.Tick += CountTimerTick;
        }
               
        /// <summary>
        /// Сменить шаг(ход) времени
        /// </summary>
        /// <param name="newStep"></param>
        public void SetStep(int newStep)
        {
            step = newStep;
        }

        /// <summary>
        /// Запустить сервис
        /// </summary>
        public void Start()
        {
            _countTimer.Start();
        }

        /// <summary>
        /// Остановить сервис
        /// </summary>
        public void Stop()
        {
            _countTimer.Stop();
        }

      
        /// <summary>
        /// Обновление данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CountTimerTick(object sender, EventArgs e)
        {            
            sumInterval += (sender as DispatcherTimer).Interval.TotalMilliseconds;
            if (sumInterval >= 300)
            {
                //обновляем время и событие
                upTime = upTime.AddMilliseconds(sumInterval*step);
                OnChangeTime(upTime);

                //обнуляем счётчик
                sumInterval = 0;
            }          
        }
      
    }
}
