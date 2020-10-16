using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AirportWPF.Model
{
    

    [Serializable]
    public class Voyage: INotifyPropertyChanged
    {
        /*
         Из ТЗ:
         - тип самолёта (тип самолёта определяет его максимальную вместимость)
         - вылет/прилёт
         - времени вылета/прилёта
         - город (куда вылетел/откуда прилетел)
         */

        /// <summary>
        /// Тип самолета
        /// </summary>
        public Plane TypePlane { get; set; }

        /// <summary>
        /// Название рейса
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Пункт назначения
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Пункт отправки
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// Время прилета
        /// </summary>
        public DateTime DateDestination { get; set; }

        /// <summary>
        /// Время вылета
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// Количество пассажир
        /// </summary>
        public int Passenger { get; set; }

        // 0 - ожидается
        // 1 - регистрация
        // 2 - Вылетел
        // 3 - В пути
        // 4 - Идет на посадку
        // 5 - Прилетел

        /// <summary>
        /// Статус рейса в текущий момент
        /// </summary>        
        public int Status { get; set; }

        public bool SetStatus(DateTime date)
        {
            //полпути
            var halfPath = (DateDestination - DateStart).TotalMinutes / 2;
           
            bool result = false;
            if (date >= DateStart.AddMinutes(-60) && date < DateStart.AddMinutes(-30))
            {
                if (Status != 0) { result = true; Status = 0; OnPropertyChanged("Status"); }
            }
            else if (date >= DateStart.AddMinutes(-30) && date < DateStart)
            {
                if (Status != 1) { result = true; Status = 1; OnPropertyChanged("Status"); }
            }
            else if (date >= DateStart && date < DateDestination.AddMinutes(-(halfPath + halfPath / 2)))
            {
                if (Status != 2) { result = true; Status = 2; OnPropertyChanged("Status"); }
            }
            else if (date >= DateStart.AddMinutes(halfPath / 2) && date < DateDestination.AddMinutes(-(halfPath / 2)))
            {
                if (Status != 3) { result = true; Status = 3; OnPropertyChanged("Status"); }
            }
            else if (date >= DateDestination.AddMinutes(-(halfPath / 2)) && date < DateDestination)
            {
                if (Status != 4) { result = true; Status = 4; OnPropertyChanged("Status"); }
            }
            else if (date >= DateDestination)
            {
                if (Status != 5) { result = true; Status = 5; OnPropertyChanged("Status"); }
            }

            return result;
        }

        public Voyage()
        { }

        /// <summary>
        /// Новый рейс
        /// </summary>
        /// <param name="name">Название рейса</param>
        /// <param name="typePlane">Тип самолета</param>
        /// <param name="start">Пункт отправки</param>
        /// <param name="dest">Пункт назначения</param>
        /// <param name="dateStart">Время вылета</param>
        /// <param name="dateDest">Время прилета</param>
        /// <param name="passenger">Количество пассажир</param>
        public Voyage(string name, Plane typePlane, string start, string dest, 
            DateTime dateStart, DateTime dateDest, int passenger)
        {
            this.Name = name;
            this.TypePlane = typePlane;
            this.Destination = dest;
            this.Start = start;
            this.DateDestination = dateDest;
            this.DateStart = dateStart;
            this.Passenger = passenger;
            Status = -1;
        }

        #region NotifyProperty
        //методы для обновления отображения    

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }

   public class StatusItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public StatusItem(int id, string name)
        {
            this.Id = id; this.Name = name;
        }
    }
}
