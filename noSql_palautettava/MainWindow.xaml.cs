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
            var database = client.GetDatabase("test");
            var collection = database.GetCollection<BsonDocument>("products");

            //await collection.InsertOneAsync(new BsonDocument("Name", "Jack"));

            var list = await collection.Find(new BsonDocument("tuote", "kortti")).ToListAsync();

            foreach (var document in list)
            {
                MessageBox.Show(document["lukumaara"].ToString());
            }
        }
    }
}
