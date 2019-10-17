using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using App1.Core.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace App1.Core.Services
{
    // This class holds sample data used by some generated pages to show how they can be used.
    // TODO WTS: Delete this file once your app is using real data.
    public static class DataService
    {
        static String database = ConfigurationManager.AppSettings.Get("database");
        static String path = ConfigurationManager.AppSettings.Get("dbpath");
        #region DATABASE
        public static void InitializeDatabase()
        {
            
            using (SqliteConnection db = new SqliteConnection(@"Filename="+ path + database))
            {
                db.Open();

                //create pic table
                string tableCommand =
                    "CREATE TABLE IF NOT " +
                    "EXISTS Fotographers (Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "Firstname NVARCHAR(100), " +
                    "Lastname NVARCHAR(50) NOT NULL, " +
                    "Birtdate NVARCHAR(10) NOT NULL, " +
                    "Notice TEXT )";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();


                string tableCommand2 =
                    "CREATE TABLE IF NOT " +
                    "EXISTS Pictures (Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                    "Name NVARCHAR(100) NOT NULL, " +
                                    "Path TEXT NOT NULL, " +
                                    "FotographerId INTEGER NOT NULL, " +
                                    "EXIFwidth INTEGER," +
                                    "EXIFheight INTEGER," +
                                    "IPTCcountry NVARCHAR(100)," +
                                    "IPTCcity NVARCHAR(100), " +
                "FOREIGN KEY(FotographerId) REFERENCES Fotographers(Id) );";

                SqliteCommand createTable2 = new SqliteCommand(tableCommand2, db);

                createTable2.ExecuteReader();

                insertDataAsync();

                /*
                 * We place the ExecuteReader() call inside a try-catch block. 
                 * This is because SQLite will always throw a SqliteException whenever 
                 * it can’t execute the SQL command. Not getting the error confirms 
                 * that the command went through correctly.
                */

                ////Create fotographer table
                //tableCommand = "CREATE TABLE IF NOT " +
                //    "EXISTS Fotographers (Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                //    "Firstname NVARCHAR(100), " +
                //    "Lastname NVARCHAR(50) NOT NULL, " +
                //    "Birtdate NVARCHAR(10) NOT NULL, " +
                //    "Notice TEXT )";

                //createTable = new SqliteCommand(tableCommand, db);

                //createTable.ExecuteReader();
            }
        }

        public static void insertDataAsync()
        {
            var foGr = AllFotographers();
            var Img = AllPictures();

            using (var db = new SqliteConnection(@"Filename=" + path + database))
            {

                db.Open();
                //AddFotographer()
                foreach (var fogr in foGr)
                {
                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    insertCommand.CommandText = "INSERT INTO Fotographers (Firstname, Lastname, Birtdate, Notice) VALUES(@Firstname, @Lastname, @Birtdate, @Notice);";
                    insertCommand.Parameters.AddWithValue("@Firstname", fogr.Name);
                    insertCommand.Parameters.AddWithValue("@Lastname", fogr.Surname);
                    insertCommand.Parameters.AddWithValue("@Birtdate", fogr.Birthday);
                    insertCommand.Parameters.AddWithValue("@Notice", fogr.Notes);


                    insertCommand.ExecuteReader();
                }

                foreach (var img in Img)
                {

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    var parameters = new DynamicParameters();

                    insertCommand.CommandText = "INSERT INTO Pictures (Name, Path, FotographerId, EXIFwidth, EXIFheight, IPTCcountry, IPTCcity) VALUES(@Name, @Path, @FotographerId, @EXIFwidth, @EXIFheight, @IPTCcountry, @IPTCcity); SELECT LAST_INSERT_ROWID();";

                    insertCommand.Parameters.AddWithValue("@Name", img.Name);
                    insertCommand.Parameters.AddWithValue("@Path", img.Path);
                    insertCommand.Parameters.AddWithValue("@FotographerId", img.Owner.ID);
                    insertCommand.Parameters.AddWithValue("@EXIFwidth", img.EXIF.Breite);
                    insertCommand.Parameters.AddWithValue("@EXIFheight", img.EXIF.Hoehe);
                    insertCommand.Parameters.AddWithValue("@IPTCcountry", img.IPTC.ISO_Landescode);
                    insertCommand.Parameters.AddWithValue("@IPTCcity", img.IPTC.Ort);

                    insertCommand.ExecuteReader();
                }
                db.Close();
            }

        }

        public static void AddFog(FotographerModel fog)
        {
            using (SQLiteConnection db = new SQLiteConnection(@"Data Source=" + path + database + ";Version=3", true))
            {
                db.Open();

                SQLiteCommand insertCommand = new SQLiteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO Fotographers (Firstname, Lastname, Birtdate, Notice) VALUES(@Firstname, @Lastname, @Birtdate, @Notice); ; SELECT LAST_INSERT_ROWID();";
                insertCommand.Parameters.AddWithValue("@Firstname", fog.Name);
                insertCommand.Parameters.AddWithValue("@Lastname", fog.Surname);
                insertCommand.Parameters.AddWithValue("@Birtdate", fog.Birthday);
                insertCommand.Parameters.AddWithValue("@Notice", fog.Notes);


                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static ObservableCollection<PictureModel> GetPictures(string tablename)
        {
            ObservableCollection<PictureModel> entries = new ObservableCollection<PictureModel>();

            using (SQLiteConnection db = new SQLiteConnection(@"Data Source=" + path + database + ";Version=3", true))
            {
                db.Open();

                SQLiteDataAdapter ad;
                DataTable dt = new DataTable();

                SQLiteCommand cmd;
                cmd = db.CreateCommand();
                cmd.CommandText = "SELECT * from " + tablename;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource

                db.Close();
                foreach (DataRow row in dt.Rows)
                {
                    var a = row.ItemArray;

                    PictureModel pic = new PictureModel();
                    pic.Id = a[0].ToString();
                    pic.Name = a[1].ToString();
                    pic.Path = a[2].ToString();
                    pic.Owner = new FotographerModel { ID = a[3].ToString() };
                    pic.EXIF = new EXIFModel { Breite = a[4].ToString(), Hoehe = a[5].ToString() };
                    pic.IPTC = new IPTCModel { Land = a[6].ToString(), Ort = a[7].ToString() };

                    entries.Add(pic);
                }
            }

            return entries;
        }

        public static ObservableCollection<FotographerModel> GetFotographers(string tablename)
        {
            ObservableCollection<FotographerModel> entries = new ObservableCollection<FotographerModel>();

            using (SQLiteConnection db = new SQLiteConnection(@"Data Source=" + path + database + ";Version=3", true))
            {
                db.Open();

                SQLiteDataAdapter ad;
                DataTable dt = new DataTable();

                SQLiteCommand cmd;
                cmd = db.CreateCommand();
                cmd.CommandText = "SELECT * from " + tablename;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource

                db.Close();
                foreach (DataRow row in dt.Rows)
                {
                    var a = row.ItemArray;

                    FotographerModel fo = new FotographerModel();
                    fo.ID = a[0].ToString();
                    fo.Name = a[1].ToString();
                    fo.Surname = a[2].ToString();
                    fo.Birthday = DateTime.Parse(a[3].ToString());
                    fo.Notes = a[4].ToString();

                    entries.Add(fo);
                }
            }

            return entries;
        }

        public static void ChangeIPTC(int id, PictureModel newPic)
        {
            using (SqliteConnection db = new SqliteConnection(@"Data Source=" + path + database))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "UPDATE Pictures SET IPTCcountry = @Land, IPTCcity = @Ort WHERE Id = @id;";  //set the passed query
                insertCommand.Parameters.AddWithValue("@Land", newPic.IPTC.Land);
                insertCommand.Parameters.AddWithValue("@Ort", newPic.IPTC.Ort);
                insertCommand.Parameters.AddWithValue("@id", id);

                insertCommand.ExecuteReader();

                db.Close();

                //SQLiteDataAdapter ad;
                //DataTable dt = new DataTable();

                //SQLiteCommand cmd;
                //cmd = db.CreateCommand();
                //cmd.CommandText = "UPDATE Pictures SET IPTCcountry = "+ newPic.IPTC.Land +", IPTCcity = "+ newPic.IPTC.Ort +" WHERE StudentId = "+ id +"; " ;  //set the passed query
            }
        }

        public static void ChangeFog(int id, FotographerModel newFog)
        {
            using (SqliteConnection db = new SqliteConnection(@"Data Source=" + path + database))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "UPDATE Fotographers SET Firstname = @Firstname, Lastname = @Lastname, Birtdate = @Birthday, Notice = @Notice WHERE Id = @id;";  //set the passed query
                insertCommand.Parameters.AddWithValue("@Firstname", newFog.Name);
                insertCommand.Parameters.AddWithValue("@Lastname", newFog.Surname);
                insertCommand.Parameters.AddWithValue("@Birthday", newFog.Birthday);
                insertCommand.Parameters.AddWithValue("@Notice", newFog.Notes);
                insertCommand.Parameters.AddWithValue("@id", id);

                insertCommand.ExecuteReader();

                db.Close();

                //SQLiteDataAdapter ad;
                //DataTable dt = new DataTable();

                //SQLiteCommand cmd;
                //cmd = db.CreateCommand();
                //cmd.CommandText = "UPDATE Pictures SET IPTCcountry = "+ newPic.IPTC.Land +", IPTCcity = "+ newPic.IPTC.Ort +" WHERE StudentId = "+ id +"; " ;  //set the passed query
            }
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
        private static ObservableCollection<FotographerModel> _galleryFotographerData;
        private static ObservableCollection<PictureModel> _galleryImageData;



        public static void Initialize(string localResourcesPath)
        {
            _localResourcesPath = localResourcesPath;
        }

        // TODO WTS: Remove this once your image gallery page is displaying real data.
        public static ObservableCollection<FotographerModel> GetFotographerData()
        {
            if (_galleryFotographerData == null)
            {
                _galleryFotographerData = new ObservableCollection<FotographerModel>();
                _galleryFotographerData = GetFotographers("Fotographers");
                //AllPictures();
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

            return _galleryFotographerData;
        }

        public static ObservableCollection<PictureModel> GetImageData()
        {
            if (_galleryImageData == null)
            {
                _galleryImageData = new ObservableCollection<PictureModel>();
                _galleryImageData = GetPictures("Pictures");
            }

            return _galleryImageData;
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data.
        public static async Task<IEnumerable<SampleOrder>> GetSampleModelDataAsync()
        {
            await Task.CompletedTask;

            return AllOrders();
        }
    }
}
