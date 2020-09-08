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
        //--------------------------------------------------------------------------------------------henkilö tulostus
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("henkilorekisteri");
            var collection = database.GetCollection<BsonDocument>("henkilot");

            //await collection.InsertOneAsync(new BsonDocument("Name", "Jack"));

            //var list = await collection.Find(new BsonDocument("Etunimi", "Timo")).ToListAsync();
            var list =await collection.Find(new BsonDocument()).ToListAsync();
            tblock_results.Text = "";
            foreach (var document in list)
            {
                //MessageBox.Show(document["Sukunimi"].ToString());
                //MessageBox.Show(document.ToString());
                tblock_results.Text = tblock_results.Text +
                document["_id"].ToString()+ "    "+
                document["Etunimi"].ToString() + "     " +
                document["Sukunimi"].ToString() + "     "  +
                document["Osoite"].ToString() + "     " +
                document["Postinumero"].ToString() + "     " +
                document["Sähköposti"].ToString() + "     " +
                document["JäsenyydenAlkuPvm"].ToString() + "     "   +"\r\n";

            }
        }

        //-------------------------------------------------------------------------------------------henkilö lisäys/päivitys
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            //--------------------------------------------------------------------------------------lisäys
            if (txt_idresult.Text.Length < 0)
            {
                try
                {
                    var client = new MongoClient("mongodb://localhost:27017");
                    var database = client.GetDatabase("henkilorekisteri");
                    var collection = database.GetCollection<BsonDocument>("henkilot");

                    //{ Etunimi: "Adam", Sukunimi: "Zakar", Osoite: "Testikatu 42", Postinumero: "06100", Sähköposti: "zkadam@vmillle.fi", JäsenyydenAlkuPvm: 24 - 04 - 2020  },

                    var document = new BsonDocument  {
                        { "Etunimi",  txt_etunimi.Text.ToString() },
                        { "Sukunimi", txt_sukunimi.Text.ToString()  },
                        { "Osoite",  txt_osoite.Text.ToString()  },
                        { "Postinumero",  txt_postinum.Text.ToString()  },
                        { "Sähköposti",  txt_sahkoposti.Text.ToString()  },
                        { "JäsenyydenAlkuPvm", datePicker_pvm.SelectedDate},

                    };
                    collection.InsertOneAsync(document);

                }
                catch (Exception)
                {

                    MessageBox.Show("Tallennuus epäonnistui");
                }

            }

            //--------------------------------------------------------------------------------------päivitys

            else
            {
                try
                {
                    var client = new MongoClient("mongodb://localhost:27017");
                    var database = client.GetDatabase("henkilorekisteri");
                    var collection = database.GetCollection<BsonDocument>("henkilot");
                    var id = new ObjectId(txt_id.Text.ToString());
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                    var update = Builders<BsonDocument>.Update.Set("Etunimi", txt_etunimi.Text.ToString()).Set("Sukunimi", txt_sukunimi.Text.ToString()).Set("Osoite", txt_osoite.Text.ToString()).Set("Postinumero", txt_postinum.Text.ToString()).Set("Sähköposti", txt_sahkoposti.Text.ToString()).Set("JäsenyydenAlkuPvm", datePicker_pvm.SelectedDate);
                    collection.UpdateOneAsync(filter, update);

                    Button_Click_1(sender,e);
                }
                catch (Exception)
                {

                    MessageBox.Show("Päivitys epäonnistui");
                }

            }
        }
        //--------------------------------------------------------------------------------------------henkilö haku
        private async void Btn_id_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("henkilorekisteri");
                var collection = database.GetCollection<BsonDocument>("henkilot");
                var id = new ObjectId(txt_id.Text.ToString());
                var list = await collection.Find(new BsonDocument("_id", id)).ToListAsync();
                tblock_results.Text = "";
                foreach (var document in list)
                {

                    txt_idresult.Text = document["_id"].ToString();
                    txt_etunimi.Text = document["Etunimi"].ToString();
                    txt_sukunimi.Text = document["Sukunimi"].ToString();
                    txt_osoite.Text = document["Osoite"].ToString();
                    txt_postinum.Text = document["Postinumero"].ToString();
                    txt_sahkoposti.Text = document["Sähköposti"].ToString();
                    DateTime dt = document["JäsenyydenAlkuPvm"].ToUniversalTime();

                    
                    datePicker_pvm.SelectedDate = dt;


                };
            }
            catch (Exception)
            {
                MessageBox.Show("Haku epäonnistui");
                
            }

                
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            txt_idresult.Text = "";
            txt_etunimi.Text = "";
            txt_sukunimi.Text = "";
            txt_osoite.Text = "";
            txt_postinum.Text = "";
            txt_sahkoposti.Text = "";
            datePicker_pvm.SelectedDate = DateTime.Today;

        }
        //--------------------------------------------------------------------------------------------henkilön poisto

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("henkilorekisteri");
            var collection = database.GetCollection<BsonDocument>("henkilot");
            try
            {
            var id = new ObjectId(txt_id.Text.ToString());
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
            Button_Click_1(sender, e);
             MessageBox.Show("Henkilö poistettu!");

            }
            catch (Exception)
            {

                MessageBox.Show("Poistaminen ei onnistunut!");

            }

        }
    }
}
