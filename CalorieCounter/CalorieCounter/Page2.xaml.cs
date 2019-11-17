﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace CalorieCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : TabbedPage
    {
        public static string BaseAddress =
            Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        public static string apiEndpoint = $"{BaseAddress}/api.asmx/";
        RestService _restService;

        public Page2()
        {
            InitializeComponent();
            _restService = new RestService();
            //SearchingFoods.Text = "Searching Foods";
        }
        
        string CreateFoodQuery()
        {
            //api.asmx/GetCaloriesByFood?food=string
            //? id = string & date = string & token = string
            //string food = testEntry.Text.Replace(" ", string.Empty);
            string food = Uri.EscapeUriString(testEntry.Text);
            string requestUri = apiEndpoint;
            //requestUri += $"/GetCaloriesByFood";
            requestUri += "GetCaloriesByFood";
            requestUri += $"?food={food}";
            //requestUri += $"?id={id}";
            //requestUri += $"&date={date}";
            //requestUri += $"&token={token}";


            return requestUri;
        }

        async void QueryClick_Clicked(object sender, EventArgs e)
        {
            //ProcessFoodQuery();
            if (!string.IsNullOrWhiteSpace(testEntry.Text))
            {
                FoodItem foodItem = await _restService.GetFoodDataAsync(CreateFoodQuery());
                BindingContext = foodItem;
            }
        }

        public void ProcessFoodQuery()
        {
            FoodItem data = new FoodItem
            {
                Calories = 8,
               
            };

            string stringData = JsonConvert.SerializeObject(data);
            FoodItem jsonData = JsonConvert.DeserializeObject<FoodItem>(stringData);
            Console.WriteLine(stringData);
            Console.WriteLine(jsonData);

        }


        private void Entry_Completed(object sender, TextChangedEventArgs e)
        {

            textCompleted.Text = ((Entry)sender).Text;

        }



        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextOldValue.Text = e.OldTextValue;
            String connectionString;
            String output1 = "";
            String output2 = "";

            connectionString = "Data Source=cec-stahrm-ws12.it.muohio.edu; Initial Catalog=CalorieCounter;User ID=CalorieCounter_Admin_99$;Password=CC_ADm1n_$$";



            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                
                sqlConnection.Open();
                Console.WriteLine("Connection Open  !");



                TextNewValue.Text = "Connection Open!";
                SqlCommand command = new SqlCommand("Select * from Food where foodname like '%" + e.OldTextValue + "%' order by FoodName", sqlConnection);
                 
                SqlDataReader reader = null;
                reader = command.ExecuteReader();
                DataTable schemaTable = reader.GetSchemaTable();


                foreach (DataRow row in schemaTable.Rows)
                {
                    foreach (DataColumn column in schemaTable.Columns)
                    {
                        //  output1+=String.Format("{0} = {1}",
                        //     column.ColumnName, row[column])+"\n";

                    }
                }




                while (reader.Read())
                {
                    String locName;
                    int location = reader.GetInt32(1);
                    switch (location)
                    {
                        case 1:
                            locName = ("UDF");
                            break;
                        case 2:
                            locName = ("Bell Tower");
                            break;
                        case 3:
                            locName = ("Null");
                            break;
                        case 4:
                            locName = ("Garden Commons");
                            break;
                        case 5:
                            locName = ("King Cafe");
                            break;
                        case 6:
                            locName = ("Market Street Cafe");
                            break;
                        case 7:
                            locName = ("Martin Dining Hall");
                            break;
                        case 8:
                            locName = ("ScoreBoard Market");
                            break;
                        case 9:
                            locName = ("ScoreBoard Market");
                            break;
                        case 10:
                            locName = ("MapleStreet Commons");
                            break;

                        default:
                            locName = ("Unknown Location");
                            break;

                    }//end switch

                    output2 += reader.GetString(12) + "  |\t" + locName + "\n";


                }

                //(String.Format("ID{0}  Loc{1} Type{2} Cal{3} Trans{4} Sat{5} chol{6} Sodium{",
                //  column.ColumnName, row[column]));
                TextOldValue.Text = output2;
                sqlConnection.Close();
            }//end using
        }//end emthod entrychanged

        
    }
}