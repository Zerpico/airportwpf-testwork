using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using AirportWPF.Model;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;

namespace AirportWPF.ViewModel
{
    public class VoyageViewModel : INotifyPropertyChanged
    {     
      
        public VoyageViewModel()
        {
            //генерируем случайные рейсы
            Services.CreateVoyagesXML(100);
            //загружаем
            Voyages = Services.LoadVoyagesXML();

            // создаем коллекцию с источником
            startViewVoyage = new CollectionViewSource() { Source = Voyages };
            destViewVoyage = new CollectionViewSource() { Source = Voyages };

            
            //добавляем фильтры
            startViewVoyage.Filter += new FilterEventHandler(startFilter);
            destViewVoyage.Filter += new FilterEventHandler(destFilter);

            StartInProgress = Voyages.Where(d => d.Status >= -1 && d.Status < 3).Count();
            DestInProgress = Voyages.Where(d => d.Status >= 3).Count();

            //сервис для потокового обновления коллекций UI
            Executer.Initialize();

            //инициализция графиков
            graphVoyage = new GraphVoyage();           
            graphVoyage.ConfigChart();

            //настраиваем таймер и запускаем таймер (имитатор времени)          
            RealDateTime = DateTime.Now;
            dtimer = new DTimeService(RealDateTime);
            dtimer.OnChangeTime += Dtimer_OnChangeTime;
            dtimer.Start();
        }

        //Работа с данными графиков (гистограммы)
        public GraphVoyage graphVoyage { get; set; }

        //сервис таймера
        DTimeService dtimer;

        //событие при обновлении таймера, (имитация времени)
        //здесь происходят все обновления данных и коллекций
        private void Dtimer_OnChangeTime(DateTime step)
        {
            //ставим новое время
            RealDateTime = step;
            OnPropertyChanged("RealDateTime");

            //перебором коллекции обновляем статус рейса
            bool result = false;
            foreach (var vog in Voyages)
            {
                //сменился ли статус
                var tmp = vog.SetStatus(RealDateTime);
                if (tmp)
                { LastVoyage = vog; OnPropertyChanged("LastVoyage"); OnPropertyChanged("LastStatus"); }
                if (result == false)
                    result = tmp;


                //если рейс уже вылетел, добавляем инфу о пассажирах на график
                if (vog.Status == 2 || vog.Status == 3)
                {
                    if (names.Where(x => x == vog.Name).Count() == 0)
                    {
                        names.Add(vog.Name);
                        graphVoyage.AddSeries(new PasItem(RealDateTime, vog.Passenger, vog.Name));                       

                    }

                }

                //если рейс уже приземлился, добавляем инфу о пассажирах на график 2
                if (vog.Status == 5)
                {
                    if (names2.Where(x => x == vog.Name).Count() == 0)
                    {
                        names2.Add(vog.Name);
                        graphVoyage.AddSeries2(new PasItem(RealDateTime, vog.Passenger, vog.Name));
                       
                    }
                }
            }

            //если данные статусы рейсов поменялись, обновляем фильтры коллекций
            if (result)
                Executer.OnUIThread(UpdateUIViewCollection);

        }

        /// <summary>
        /// Обновление фильров коллекий
        /// </summary>
        void UpdateUIViewCollection()
        {
            StartInProgress = Voyages.Where(d => d.Status >= -1 && d.Status < 3).Count();
            DestInProgress = Voyages.Where(d => d.Status >= 3).Count();

            OnPropertyChanged("StartInProgress"); OnPropertyChanged("DestInProgress");

            if (StartViewVoyage.View != null)
                StartViewVoyage.View.Refresh();

            if (DestViewVoyage.View != null)
                DestViewVoyage.View.Refresh();

        }
      
        /// <summary>
        /// Количество рейсов на вылет
        /// </summary>
        public int StartInProgress { get; set; }

        /// <summary>
        /// Количество прилетевших рейсов
        /// </summary>
        public int DestInProgress { get; set; }

        //коллеция отображения для вылета
        private CollectionViewSource startViewVoyage;
        public CollectionViewSource StartViewVoyage
        {
            get { return startViewVoyage; }
            set
            {
                if (startViewVoyage == value) return;
                startViewVoyage = value;
                OnPropertyChanged("StartViewVoyage");
            }
        }

        //коллеция отображения для прилета
        private CollectionViewSource destViewVoyage;
        public CollectionViewSource DestViewVoyage
        {
            get { return destViewVoyage; }
            set
            {
                if (destViewVoyage == value) return;
                destViewVoyage = value;
                OnPropertyChanged("DestViewVoyage");
            }
        }

        //фильтр для вылета
        private void startFilter(object sender, FilterEventArgs e)
        {
            var voyage = e.Item as Voyage;
            if (voyage != null)
            {
                //статус 0(в ожидании)
                if (voyage.Status >= 0 && voyage.Status < 3 )
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        //фильтр для прилета
        private void destFilter(object sender, FilterEventArgs e)
        {
            var voyage = e.Item as Voyage;
            if (voyage != null)
            {
                //статус 3(в пути)
                if (voyage.Status>=3 && voyage.Status < 6)
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }


        List<string> names = new List<string>();
        List<string> names2 = new List<string>();


        /// <summary>
        /// информация о последнем рейсе
        /// </summary>
        public Voyage LastVoyage { get; set; }
        public string LastStatus { get { return LastVoyage != null ? StatusItems[LastVoyage.Status].Name : null; } }
       
        /// <summary>
        /// Обновляемое время, из сервиса TimerService
        /// </summary>
        public DateTime RealDateTime { get; set; }

        //шаг времени для Slider в ui привязка
        private int stepTimer { get; set; }
        public int StepTimer
        {
            get { return stepTimer; }
            set { stepTimer = value; dtimer.SetStep(value); }
        }
        
        /// <summary>
        /// //Коллекция всех рейсов
        /// </summary>
        public ObservableCollection<Voyage> Voyages { get; set; }

        /// <summary>
        /// Справочник статусов рейсов
        /// </summary>
        public ObservableCollection<StatusItem> StatusItems
        {
            get
            {
                return new ObservableCollection<StatusItem>()
                {
                    new StatusItem (0,"Ожидается"),
                    new StatusItem (1,"Регистрация"),
                    new StatusItem (2,"Вылетел"),
                    new StatusItem (3,"В пути"),
                    new StatusItem (4,"Идет на посадку"),
                    new StatusItem (5,"Прилетел"),
                    new StatusItem (6,"Ушёл")
                };
            }
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
}
