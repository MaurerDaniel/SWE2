using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using App1.Core.Models;
using Microsoft.Data.Sqlite;

namespace App1.Core.Services
{
    // This class holds sample data used by some generated pages to show how they can be used.
    // TODO WTS: Delete this file once your app is using real data.
    public static class SampleDataService
    {
        #region DATABASE
        public static void InitializeDatabase()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=sqlite.db"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                    "Text_Entry NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(string inputText)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
                insertCommand.Parameters.AddWithValue("@Entry", inputText);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static List<String> GetData()
        {
            List<String> entries = new List<string>();

            using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand("SELECT Text_Entry from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }
                db.Close();
            }
            return entries;
        }

        #endregion


        private static ObservableCollection<PictureModel> AllPictures()
        {
            var tempPictures = new ObservableCollection<PictureModel>
            {
                new PictureModel
                {
                    Name = "1",
                    Path = "ms-appx:///Assets/SampleData/1.jpg",
                    Id = "1",
                    Owner = new FotographerModel
                    {
                        Name = "Harald",
                        Surname = "Wahl",
                        ID = "1",
                        Birthday = DateTime.Today,
                        Notes = "Objektiv gesehen gibt es keine Nachteile"
                    },
                    EXIF = new EXIFModel
                    {
                        Breite = "1200",
                        Hoehe = "800",
                        Aufloesungseinheit = "22",
                        BildTiefe = "113",
                        HorizontAufloesung = "20",
                        VertAufloesung = "20"
                    },
                    IPTC = new IPTCModel
                    {
                        Bundesland = "Wien",
                        ISO_Landescode = "AUT",
                        Land = "Austria",
                        Ort = "Wien"
                    }
                },
                    new PictureModel
                {
                    Name = "2",
                    Path = "ms-appx:///Assets/SampleData/2.jpg",
                    Id = "2",
                    Owner = new FotographerModel
                    {
                        Name = "Harald",
                        Surname = "Wahl",
                        ID = "1",
                        Birthday = DateTime.Today,
                        Notes = "Objektiv gesehen gibt es keine Nachteile"
                    },
                    EXIF = new EXIFModel
                    {
                        Breite = "1300",
                        Hoehe = "900",
                        Aufloesungseinheit = "2",
                        BildTiefe = "13",
                        HorizontAufloesung = "2",
                        VertAufloesung = "2"
                    },
                    IPTC = new IPTCModel
                    {
                        Bundesland = "Graz",
                        ISO_Landescode = "AUT",
                        Land = "Austria",
                        Ort = "Graz"
                    }
                },
                        new PictureModel
                {
                    Name = "3",
                    Path = "ms-appx:///Assets/SampleData/3.jpg",
                    Id = "3",
                    Owner = new FotographerModel
                    {
                        Name = "Bar",
                        Surname = "Code",
                        ID = "2",
                        Birthday = DateTime.Today,
                        Notes = "Objektiv gesehen gibt es nur Nachteile"
                    },
                    EXIF = new EXIFModel
                    {
                        Breite = "1900",
                        Hoehe = "500",
                        Aufloesungseinheit = "1",
                        BildTiefe = "3",
                        HorizontAufloesung = "12",
                        VertAufloesung = "12"
                    },
                    IPTC = new IPTCModel
                    {
                        Bundesland = "Krakau",
                        ISO_Landescode = "PL",
                        Land = "Polen",
                        Ort = "Krakau"
                    }
                }
            };

            return tempPictures;
        }

        private static ObservableCollection<FotographerModel> AllFotographers()
        {
            var tempFotoGraphers = new ObservableCollection<FotographerModel>
            {
                new FotographerModel
                    {
                        Name = "Harald",
                        Surname = "Wahl",
                        ID = "1",
                        Birthday = DateTime.Today,
                        Notes = "Objektiv gesehen gibt es keine Nachteile"
                    },
                new FotographerModel
                    {
                        Name = "Bar",
                        Surname = "Code",
                        ID = "2",
                        Birthday = DateTime.Today,
                        Notes = "Objektiv gesehen gibt es nur Nachteile"
                    }
            };

            return tempFotoGraphers;

        }

        private static IEnumerable<SampleOrder> AllOrders()
        {
            // The following is order summary data
            var data
                = new ObservableCollection<SampleOrder>
            {
                new SampleOrder
                {
                    OrderId = 70,
                    OrderDate = new DateTime(2017, 05, 24),
                    Company = "Company F",
                    ShipTo = "Francisco Pérez-Olaeta",
                    OrderTotal = 2490.00,
                    Status = "Closed",
                    Symbol = (char)57643 // Symbol.Globe
                },
                new SampleOrder
                {
                    OrderId = 71,
                    OrderDate = new DateTime(2017, 05, 24),
                    Company = "Company CC",
                    ShipTo = "Soo Jung Lee",
                    OrderTotal = 1760.00,
                    Status = "Closed",
                    Symbol = (char)57737 // Symbol.Audio
                },
                new SampleOrder
                {
                    OrderId = 72,
                    OrderDate = new DateTime(2017, 06, 03),
                    Company = "Company Z",
                    ShipTo = "Run Liu",
                    OrderTotal = 2310.00,
                    Status = "Closed",
                    Symbol = (char)57699 // Symbol.Calendar
                },
                new SampleOrder
                {
                    OrderId = 73,
                    OrderDate = new DateTime(2017, 06, 05),
                    Company = "Company Y",
                    ShipTo = "John Rodman",
                    OrderTotal = 665.00,
                    Status = "Closed",
                    Symbol = (char)57620 // Symbol.Camera
                },
                new SampleOrder
                {
                    OrderId = 74,
                    OrderDate = new DateTime(2017, 06, 07),
                    Company = "Company H",
                    ShipTo = "Elizabeth Andersen",
                    OrderTotal = 560.00,
                    Status = "Shipped",
                    Symbol = (char)57633 // Symbol.Clock
                },
                new SampleOrder
                {
                    OrderId = 75,
                    OrderDate = new DateTime(2017, 06, 07),
                    Company = "Company F",
                    ShipTo = "Francisco Pérez-Olaeta",
                    OrderTotal = 810.00,
                    Status = "Shipped",
                    Symbol = (char)57661 // Symbol.Contact
                },
                new SampleOrder
                {
                    OrderId = 76,
                    OrderDate = new DateTime(2017, 06, 11),
                    Company = "Company I",
                    ShipTo = "Sven Mortensen",
                    OrderTotal = 196.50,
                    Status = "Shipped",
                    Symbol = (char)57619 // Symbol.Favorite
                },
                new SampleOrder
                {
                    OrderId = 77,
                    OrderDate = new DateTime(2017, 06, 14),
                    Company = "Company BB",
                    ShipTo = "Amritansh Raghav",
                    OrderTotal = 270.00,
                    Status = "New",
                    Symbol = (char)57615 // Symbol.Home
                },
                new SampleOrder
                {
                    OrderId = 78,
                    OrderDate = new DateTime(2017, 06, 14),
                    Company = "Company A",
                    ShipTo = "Anna Bedecs",
                    OrderTotal = 736.00,
                    Status = "New",
                    Symbol = (char)57625 // Symbol.Mail
                },
                new SampleOrder
                {
                    OrderId = 79,
                    OrderDate = new DateTime(2017, 06, 18),
                    Company = "Company K",
                    ShipTo = "Peter Krschne",
                    OrderTotal = 800.00,
                    Status = "New",
                    Symbol = (char)57806 // Symbol.OutlineStar
                },
            };

            return data;
        }

        private static IEnumerable<SampleOrder> _allOrders;

        // TODO WTS: Remove this once your ContentGrid page is displaying real data.
        public static ObservableCollection<SampleOrder> GetContentGridData()
        {
            if (_allOrders == null)
            {
                _allOrders = AllOrders();
            }

            return new ObservableCollection<SampleOrder>(_allOrders);
        }

        private static string _localResourcesPath;

        private static ObservableCollection<PictureModel> _gallerySampleData;

        public static void Initialize(string localResourcesPath)
        {
            _localResourcesPath = localResourcesPath;
        }

        // TODO WTS: Remove this once your image gallery page is displaying real data.
        public static ObservableCollection<PictureModel> GetGalleryTempData()
        {
            if (_gallerySampleData == null)
            {
                _gallerySampleData = new ObservableCollection<PictureModel>();
                _gallerySampleData = AllPictures();
                //for (int i = 1; i <= 10; i++)
                //{
                //    _gallerySampleData.Add(new SampleImage()
                //    {
                //        ID = $"{i}",
                //        Source = $"{_localResourcesPath}/SampleData/SamplePhoto{i}.png",
                //        Name = $"Image sample {i}"
                //    });
                //}
            }

            return _gallerySampleData;
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data.
        public static async Task<IEnumerable<SampleOrder>> GetSampleModelDataAsync()
        {
            await Task.CompletedTask;

            return AllOrders();
        }
    }
}
