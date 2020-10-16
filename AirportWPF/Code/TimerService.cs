using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirportWPF
{
    //Всё необходимое для работы таймера и часов
    class TimerService
    {
        // Объявляем делегат
        public delegate void DateTimeHandler(DateTime step);
        // Событие, возникающее при обновлении времени
        public event DateTimeHandler OnChangeTime;

        //таймер для отсчета реального времени
        Timer timer;
        //обратный вызов таймера
        TimerCallback timerCall;

        //частота обновления
        private int frequency { get; set; }

        //шаг времени
        private int stepSecond { get; set; }

        public void SetFrequency(int newStep)
        {
            //определяем новую частоту           
            /*if (newStep >= 1 && newStep < 100)
            {
                frequency = 1000 / newStep;
                stepSecond = 1;
            }
            else if (newStep >= 100 && newStep < 300)
            {
                frequency = 1000 / newStep;
                stepSecond = 10;
            }
            else if (newStep >= 300 && newStep < 600)
            {
                frequency = 1000 / newStep;
                stepSecond = 100;
            }
            else if (newStep >= 600 && newStep < 900)
            {
                frequency = 1000 / newStep;
                stepSecond = 1000;
            }
            else if (newStep >= 900)
            {
                frequency = 1000 / newStep;
                stepSecond = 3000;
            }
            else if (newStep >= 1000)
            {
                frequency = 1000 / newStep;
                stepSecond = 5000;
            }*/
          //  frequency = 10000 / newStep;
            stepSecond = newStep;
            //timer.Change(0, frequency < 100 ? 100 : frequency);
        }

        /*public int GetFrequency()
        {
            return frequency;
        }*/

        DateTime RealDateTime;

        public TimerService()
        {
            RealDateTime = DateTime.Now;
            //настраиваем таймер реального времени
            frequency = 100;
            stepSecond = 1;
            timerCall = new TimerCallback(TimerStep);
            timer = new Timer(timerCall, null, 0, frequency);
        }  

        private void TimerStep(object obj)
        {            
            RealDateTime = RealDateTime.AddSeconds(stepSecond);
            OnChangeTime(RealDateTime);
        }
    }
}
