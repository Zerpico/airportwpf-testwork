using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AirportWPF.ViewModel
{
    public class GraphVoyage : INotifyPropertyChanged
    {
        //серия и формат для графиков
        public LiveCharts.SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }

        //коллекция данных графика прилёта/вылета
        LiveCharts.ChartValues<PasItem> items { get; set; }
        LiveCharts.ChartValues<PasItem> items2 { get; set; }

        public GraphVoyage()
        {
            //Коллекции данных графика
            items = new ChartValues<PasItem>(); items.Add(new PasItem(DateTime.Now, 0, ""));
            items2 = new ChartValues<PasItem>(); items2.Add(new PasItem(DateTime.Now, 0, ""));
        }

        public void ConfigChart()
        {
            var dayConfig = Mappers.Xy<PasItem>()
              .X(dateModel => dateModel.Time.Ticks / TimeSpan.FromHours(1).Ticks)
              .Y(dateModel => dateModel.Value);

            //добавления коллекции и стратовых данных
            SeriesCollection = new SeriesCollection(dayConfig)
            {
                new ColumnSeries
                {
                    Values = items,
                    Title = "Вылет",
                    MaxColumnWidth = 15
                },
                new ColumnSeries
                {
                    Values = items2,
                    Title = "Прилёт",
                    MaxColumnWidth = 15
                }
            };

            //настройка формата графика (взято из доков)
            Formatter = value => new DateTime((long)(value * TimeSpan.FromHours(1).Ticks)).ToString("t");
        }

        public void AddSeries(PasItem pasitem)
        {
            SeriesCollection[0].Values.Add(pasitem);
            var itemsForDel = items.Where(x => x.Time.AddDays(1) < pasitem.Time).ToArray();
            foreach (var dl in itemsForDel)
            {
                items.Remove(dl);
            }
            OnPropertyChanged("SeriesCollection");
        }

        public void AddSeries2(PasItem pasitem2)
        {
            SeriesCollection[1].Values.Add(pasitem2);
            var itemsForDel = items2.Where(x => x.Time.AddDays(1) < pasitem2.Time).ToArray();
            foreach (var dl in itemsForDel)
            {
                items2.Remove(dl);
            }
            OnPropertyChanged("SeriesCollection");
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

    //Класс данных для графика
    public class PasItem
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public int Value { get; set; }
        public PasItem() { }
        public PasItem(DateTime dt, int pas, string name) { this.Time = dt; this.Value = pas; this.Name = name; }

        public override string ToString()
        {
            return String.Format("{0:HH:mm} {1:0.0}", this.Time, this.Value);
        }
    }
}
