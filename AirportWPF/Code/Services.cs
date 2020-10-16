using AirportWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AirportWPF
{
    public class Services
    {
        #region LoadSaveXML


        /// <summary>
        /// Сгенерировать случайные рейсы в xml
        /// </summary>
        /// <param name="Count">количество рейсов</param>
        public static void CreateVoyagesXML(int Count)
        {
            var rnd = new Random();

            try
            {
                //генерируем рейсы
                var startDate = DateTime.Now.AddHours(-1);
                Model.Voyage[] voyages = new Model.Voyage[Count];
                for (int i = 0; i < Count; i++)
                {
                    var rndplane = Services.RandomPlane(rnd);
                    var startCity = Services.RandomCity("", rnd);
                    voyages[i] = new Voyage
                        (
                        Services.RandomString(6, rnd),
                        rndplane,
                        startCity,
                        Services.RandomCity(startCity, rnd),
                        startDate = Services.RandomDate(startDate, rnd, true),
                        startDate = Services.RandomDate(startDate, rnd, false),
                        rnd.Next(1, rndplane.Capacity)
                        );

                }

                XmlSerializer formatter = new XmlSerializer(typeof(Model.Voyage[]));

                using (System.IO.FileStream fs = new System.IO.FileStream("voyages.xml", System.IO.FileMode.Create))
                {
                    formatter.Serialize(fs, voyages);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// загрузить коллекцию рейсов из xml файла
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Voyage> LoadVoyagesXML()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Model.Voyage[]));
            ObservableCollection<Voyage> result = null;
            try
            {
                using (FileStream fs = new FileStream("voyages.xml", FileMode.OpenOrCreate))
                {
                    result = new ObservableCollection<Voyage>((Voyage[])formatter.Deserialize(fs));
                }
            }
            catch (Exception) { }

            return result;
        }

        #endregion

        /// <summary>
        /// Генератор названий рейсов
        /// </summary>
        /// <param name="maxlength">Максимальная длина</param>
        /// <param name="rn"></param>
        /// <returns></returns>
        public static string RandomString(int maxlength, Random rn)
        {
            //АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ
            StringBuilder sb = new StringBuilder();             
            char[] allowedChars = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789".ToArray();
            for (int i = 0; i < maxlength; i++)
            {
                
                if (i >= maxlength / 2)
                {
                    int n = rn.Next(allowedChars.Length - 9, allowedChars.Length);
                    if (char.IsLetter(allowedChars[n]))
                    {
                        if (rn.Next(0, 2) == 0)
                        {
                            sb.Append(allowedChars[n].ToString().ToUpper());
                        }
                        else
                        {
                            sb.Append(allowedChars[n]);
                        }
                    }
                    else
                    {
                        sb.Append(allowedChars[n]);
                    }
                }
                else
                {
                    int n = rn.Next(0, allowedChars.Length - 9);
                    if (char.IsLetter(allowedChars[n]))
                    {
                        if (rn.Next(0, 2) == 0)
                        {
                            sb.Append(allowedChars[n].ToString().ToUpper());
                        }
                        else
                        {
                            sb.Append(allowedChars[n]);
                        }
                    }
                    else
                    {
                        sb.Append(allowedChars[n]);
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Генератор моделей самолетов
        /// </summary>
        /// <param name="rn"></param>
        /// <returns></returns>
        public static Model.Plane RandomPlane(Random rn)
        {
            Model.Plane[] plane = new Model.Plane[] 
            {
                 new Model.Plane("Ту-204",300,0),
                 new Model.Plane("ТУ-204-300", 300,0),
                 new Model.Plane("Superjet 100", 87,0),
                 new Model.Plane("МС-21", 180,0),
                 new Model.Plane("Ан-148",83,0),
                 new Model.Plane("Ил-96",300,0)
            };

            return plane[rn.Next(0, plane.Count() - 1)];
        }

        /// <summary>
        /// Генератор даты
        /// </summary>
        /// <param name="start">основа даты</param>
        /// <param name="rn"></param>
        /// <returns></returns>
        public static DateTime RandomDate(DateTime start, Random rn, bool after)
        {
            DateTime result = start.AddMinutes(rn.Next(after ? -5 : 1, 45));

            return result;
        }

        /// <summary>
        /// Генератор названий городов 
        /// </summary>
        /// <param name="notThisCity">исключение города</param>
        /// <param name="rn"></param>
        /// <returns></returns>
        public static string RandomCity(string notThisCity, Random rn)
        {
            //ниже список всех городов с аеропортами
            //Да их можно было занести просто в текстовый файл
            //но вдруг его удалят
            //для тестового задания думаю пока сойдет и так
            string[] cities = new string[] 
            {                                      
                 "Ярославль                        ",
                 "Ямбург                           ",
                 "Якутск                           ",
                 "Южно-Сахалинск                   ",
                 "Южно-Курильск                    ",
                 "Элиста                           ",
                 "Экимчан                          ",
                 "Щёлково                          ",
                 "Шушенское                        ",
                 "Шахтерск                         ",
                 "Чумикан                          ",
                 "Чокурдах                         ",
                 "Чита                             ",
                 "Черский                          ",
                 "Череповец                        ",
                 "Челябинск                        ",
                 "Чебоксары                        ",
                 "Чара                             ",
                 "Херпучи                          ",
                 "Хатанга                          ",
                 "Ханты-Мансийск                   ",
                 "Хандыга                          ",
                 "Хабаровск                        ",
                 "Ухта                             ",
                 "Уфа                              ",
                 "Усть-Нера                        ",
                 "Усть-Кут                         ",
                 "Усть-Куйга                       ",
                 "Усинск                           ",
                 "Урай                             ",
                 "Ульяновск                        ",
                 "Ульяновск                        ",
                 "Улан-Удэ                         ",
                 "Удачный                          ",
                 "Тюмень                           ",
                 "Тюмень                           ",
                 "Тында                            ",
                 "Тымовское                        ",
                 "Томск                            ",
                 "Тиличики                         ",
                 "Тикси                            ",
                 "Тверь                            ",
                 "Тарко-Сале                       ",
                 "Тамбов                           ",
                 "Таганрог                         ",
                 "Сыктывкар                        ",
                 "Сургут                           ",
                 "Сунтар                           ",
                 "Стрежевой                        ",
                 "СтарыйОскол                      ",
                 "Ставрополь                       ",
                 "Сочи                             ",
                 "Советский                        ",
                 "Симферополь                      ",
                 "Саратов                          ",
                 "Саранск                          ",
                 "Санкт-Петербург                  ",
                 "Самара                           ",
                 "Салехард                         ",
                 "Сабетта                          ",
                 "Ростов-на-Дону                   ",
                 "Псков                            ",
                 "Провидения                       ",
                 "Пионерный                        ",
                 "Петрозаводск                     ",
                 "Пермь                            ",
                 "Пенза                            ",
                 "Певек                            ",
                 "Палана                           ",
                 "Охотск                           ",
                 "Оха                              ",
                 "Орск                             ",
                 "Оренбург                         ",
                 "Омск                             ",
                 "Олёкминск                        ",
                 "Обь                              ",
                 "Нягань                           ",
                 "Нюрба                            ",
                 "Ноябрьск                         ",
                 "Норильск                         ",
                 "Ноглики                          ",
                 "НовыйУренгой                     ",
                 "Новосибирск                      ",
                 "Новокузнецк                      ",
                 "Никольское                       ",
                 "Николаевск-на-Амуре              ",
                 "НижнийНов                        ",
                 "Нижнекамск                       ",
                 "Нижневартовск                    ",
                 "Нижнеангарск                     ",
                 "Нерюнгри                         ",
                 "Нарьян-Мар                       ",
                 "Нальчик                          ",
                 "Надым                            ",
                 "Мурманск                         ",
                 "Москва                           ",
                 "Мирный                           ",                 
                 "Махачкала                        ",
                 "Мама                             ",
                 "Магнитогорск                     ",
                 "Магас                            ",
                 "Магадан                          ",
                 "Липецк                           ",
                 "Кызыл                            ",
                 "Курск                            ",
                 "Курган                           ",
                 "Кубинка                          ",
                 "Красноярск                       ",
                 "Краснодар                        ",
                 "Кострома                         ",
                 "Кондинское                       ",                 
                 "Когалым                          ",
                 "Киров                            ",
                 "Кемерово                         ",
                 "Калуга                           ",
                 "Калининград                      ",
                 "Казань                           ",
                 "Йошкар-Ола                       ",
                 "Итуруп                           ",
                 "Иркутск                          ",
                 "Ижевск                           ",
                 "Игарка                           ",
                 "Иванов                           ",
                 "Зея                              ",
                 "Жиганск                          ",
                 "Елизово                          ",
                 "Екатеринбург                     ",
                 "Ейск                             ",
                 "Грозный                          ",
                 "Горно-Атайск                     ",
                 "Геленджик                        ",
                 "Воронеж                          ",
                 "Воркута                          ",
                 "Вологда                          ",
                 "Волгоград                        ",
                 "Владимир                         ",
                 "Владикавказ                      ",
                 "Владивосток                      ",
                 "Витим                            ",
                 "ВеликийУстюг                     ",
                 "Варандей                         ",
                 "Бугульма                         ",
                 "Брянск                           ",
                 "Братск                           ",
                 "Бор                              ",
                 "Бодайбо                          ",
                 "Боскоесело                       ",
                 "Бованенково                      ",
                 "Благовещенск                     ",
                 "Билибино                         ",
                 "Беринговский                     ",
                 "Белоярский                       ",
                 "Бел                              ",
                 "БелаяГора                        ",
                 "Батагай                          ",
                 "Барнаул                          ",
                 "Байконур                         ",
                 "Аянсело                          ",
                 "Астрахань                        ",
                 "Архангельск                      ",
                 "Архангельск                      ",
                 "Апатиты                          ",
                 "Анапа                            ",
                 "Анадырь                          ",
                 "Амдерма                          ",
                 "Алдан                            ",
                 "Айхал                            ",
                 "Абакан                           "
            };

            var result = cities[rn.Next(0, cities.Count() - 1)].Trim();
            if (result == notThisCity)
                result = cities[rn.Next(0, cities.Count() - 1)].Trim();

            return result;
        }
    }
}
