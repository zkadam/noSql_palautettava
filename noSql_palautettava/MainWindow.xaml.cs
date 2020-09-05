using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace noSql_palautettava
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("henkilorekisteri");
            var collection = database.GetCollection<BsonDocument>("henkilot");

            //await collection.InsertOneAsync(new BsonDocument("Name", "Jack"));

            //var list = await collection.Find(new BsonDocument("Etunimi", "Timo")).ToListAsync();
            var list =await collection.Find(new BsonDocument()).ToListAsync();

            foreach (var document in list)
            {
                //MessageBox.Show(document["Sukunimi"].ToString());
                MessageBox.Show(document.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("henkilorekisteri");
            var collection = database.GetCollection<BsonDocument>("henkilot");

            //{ Etunimi: "Adam", Sukunimi: "Zakar", Osoite: "Testikatu 42", Postinumero: "06100", Sähköposti: "zkadam@vmillle.fi", JäsenyydenAlkuPvm: 24 - 04 - 2020  },

            var document = new BsonDocument  {
                { "Etunimi",  txt_etunimi.Text.ToString() },
                { "Sukunimi", txt_sukumimi.Text.ToString()  },
                { "Osoite",  txt_osoite.Text.ToString()  },
                { "Postinumero",  txt_postinum.Text.ToString()  },
                { "Sähköposti",  txt_sahkoposti.Text.ToString()  },
                { "JäsenyydenAlkuPvm", 2020},
               
            };
            collection.InsertOneAsync(document);
        }
    }
}
