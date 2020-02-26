﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CalorieCounter
{
    public class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient(GetInsecureHandler());
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }



        public HttpClientHandler GetInsecureHandler()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        public async Task<FoodItem> GetFoodCaloriesAsync(string uri)
        {
            List<FoodItem> foodItems = null;
            FoodItem item = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    string c = await response.Content.ReadAsStringAsync();

                    foodItems = JsonConvert.DeserializeObject<List<FoodItem>>(c);

                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            if (foodItems == null)
            {
                return item;
            }
            else
            {
                return foodItems[0];
            }

        }

        public async Task<List<FoodItem>> GetFoodDataAsync(string uri)
        {
            List<FoodItem> listFood = null;
            FoodItem foodItem = null;
            
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;
                
                if (response.IsSuccessStatusCode)
                {

                    string c = await response.Content.ReadAsStringAsync();

                    listFood = JsonConvert.DeserializeObject<List<FoodItem>>(c);

                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
             return listFood; 
        }

        // not complete yet but does access Miami API
        public async Task<string> GetMiamiFoodDataAsync(string uri)
        {
            string foods = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    
                    string c = await response.Content.ReadAsStringAsync();
                    // need to start and end file with same word
                    string file = "<items>\n" + c + "\n</items>";
                    // XElement parse does not like '&'
                    string newFile = file.Replace("&", "and");
                    XElement items = XElement.Parse(newFile);
                    // all items start and end with '<item>' so put them in a list
                    List<XElement> itemNodes = items.Elements("item").ToList();
                    // get first item from list (for testing purposes)
                    var s = itemNodes[0].Element("formal_name");

                    // return list of foods to be placed into our db

                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }

            return foods;

        }

        public async Task<string> InsertFoodIntoLogForUser(string uri, FoodEaten data)
        {
            string inserted = "false";
            try
            {
                var json = JsonConvert.SerializeObject(data);
                using (var message = new HttpRequestMessage(HttpMethod.Post, uri))
                {
                    message.Version = HttpVersion.Version10;
                    message.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await _client.SendAsync(message))
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            inserted = "success";
                        }
                    }
                }
                //var content = new StringContent(data, Encoding.UTF8, "application/json");
                //HttpResponseMessage response = await _client.PostAsync(uri, content);
                //HttpStatusCode i = response.StatusCode;
                //string result = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    inserted = "success";
                //}
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return inserted;
        }
        
        public async Task UpdateDailyLogForUser(string uri, UserLogData data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                using (var message = new HttpRequestMessage(HttpMethod.Post, uri))
                {
                    message.Version = HttpVersion.Version10;
                    message.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _client.SendAsync(message);
                    
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                         //yay   
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }

        public async Task<List<DailyValues>> DisplayDailyValuesByUserDayAsync(string uri)
        {
            List<DailyValues> listFood = null;

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {

                   
                    string c = await response.Content.ReadAsStringAsync();

                    listFood = JsonConvert.DeserializeObject<List<DailyValues>>(c);


                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }


            return listFood;

        }
    }
}
