using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportWPF.Model
{
    [Serializable]
    public class Plane
    {
        /// <summary>
        /// Название модели самолета
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Груподъемность самолета
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Средняя скорость самолета
        /// </summary>
        public int Speed { get; set; }

        public Plane()
        { }

        /// <summary>
        /// Новый самолет
        /// </summary>
        /// <param name="name">Название модели самолета</param>
        /// <param name="capacity">Груподъемность самолета</param>
        /// <param name="speed">Средняя скорость самолета</param>
        public Plane(string name, int capacity, int speed)
        {
            this.Name = name;
            this.Capacity = capacity;
            this.Speed = speed;
        }

    }
}
