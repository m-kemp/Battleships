using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;
using ShipModel;
using Newtonsoft.Json;

namespace Ship
{
    class ShipEngine
    {
        public ShipEngine()
        {
        }

        #region Configuration Settings
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string clientId = ConfigurationManager.AppSettings["ida:TeamAppId"];
        private static string clientSecret = ConfigurationManager.AppSettings["ida:TeamAppSecret"];
        private static Uri redirectUri = new Uri(ConfigurationManager.AppSettings["ida:RedirectUri"]);
        private static string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        private static string GameAPIAppId = ConfigurationManager.AppSettings["todo:GameAPIAppId"];
        private static string GameAPIBaseAddress = ConfigurationManager.AppSettings["todo:GameAPIBaseAddress"];
        #endregion 
 
        #region Global Variables
        public string AccessToken { get; set; }

        // Declare an array for own ships called GameGrid; referenced by GameGrid[rowNumber, colNumber]
        public string[,] GameGrid = {
        { "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1", "I1", "J1" },
        { "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2", "I2", "J2" },
        { "A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3", "I3", "J3" },
        { "A4", "B4", "C4", "D4", "E4", "F4", "G4", "H4", "I4", "J4" },
        { "A5", "B5", "C5", "D5", "E5", "F5", "G5", "H5", "I5", "J5" },
        { "A6", "B6", "C6", "D6", "E6", "F6", "G6", "H6", "I6", "J6" },
        { "A7", "B7", "C7", "D7", "E7", "F7", "G7", "H7", "I7", "J7" },
        { "A8", "B8", "C8", "D8", "E8", "F8", "G8", "H8", "I8", "J8" },
        { "A9", "B9", "C9", "D9", "E9", "F9", "G9", "H9", "I9", "J9" },
        { "A10", "B10", "C10", "D10", "E10", "F10", "G10", "H10", "I10", "J10" }};

        // Declare an array for enemy ships called EnemyGameGrid; referenced by EnemyGameGrid[rowNumber, colNumber]
        public string[,] EnemyGameGrid = {
        { "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1", "I1", "J1" },
        { "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2", "I2", "J2" },
        { "A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3", "I3", "J3" },
        { "A4", "B4", "C4", "D4", "E4", "F4", "G4", "H4", "I4", "J4" },
        { "A5", "B5", "C5", "D5", "E5", "F5", "G5", "H5", "I5", "J5" },
        { "A6", "B6", "C6", "D6", "E6", "F6", "G6", "H6", "I6", "J6" },
        { "A7", "B7", "C7", "D7", "E7", "F7", "G7", "H7", "I7", "J7" },
        { "A8", "B8", "C8", "D8", "E8", "F8", "G8", "H8", "I8", "J8" },
        { "A9", "B9", "C9", "D9", "E9", "F9", "G9", "H9", "I9", "J9" },
        { "A10", "B10", "C10", "D10", "E10", "F10", "G10", "H10", "I10", "J10" }};

        private int ShotsTaken = 0;
        #endregion

        #region API Methods
        public void AuthenticateToAzureAD()
        {
            // Using Active Directory Authentication Library (ADAL).
            AuthenticationResult result = null;
            AuthenticationContext authContext = new AuthenticationContext(authority, new FileTokenCache());

            Console.WriteLine("Authenticating via Azure AD....");
            Task t1 = Task.Run(async () =>
            {
                // Make connection using application credientials: uses appID + secret.
                ClientCredential clientCredential = new ClientCredential(clientId, clientSecret);
                result = await authContext.AcquireTokenAsync(GameAPIAppId, clientCredential);

                AccessToken = result.AccessToken;
                Console.WriteLine("Access Token = " + AccessToken);
                Console.WriteLine();
            });
            t1.Wait();
        }

        public void DemoCallAPI()
        {
            Task t2 = Task.Run(async () =>
            {
                // Retrieve the data
                Console.WriteLine("Calling test API....");
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, GameAPIBaseAddress + "/api/test");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                Console.WriteLine("Call sucessfull = " + response.IsSuccessStatusCode);
                Console.WriteLine("Result: HTTP{0}", response.StatusCode);
                Console.WriteLine("Response success/error message = " + response.ReasonPhrase);

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response data = " + responseString);
                Console.WriteLine();
            });
            t2.Wait();
        }

        public void StatusAPI()
        {
            Task t2 = Task.Run(async () =>
            {
                // Retrieve the data
                Console.WriteLine("Calling Status API....");
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, GameAPIBaseAddress + "/api/status");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                Console.WriteLine("Call sucessfull = " + response.IsSuccessStatusCode);
                Console.WriteLine("Result: HTTP{0}", response.StatusCode);
                Console.WriteLine("Response success/error message = " + response.ReasonPhrase);

                string responseString = await response.Content.ReadAsStringAsync();
                MasterGameState state = JsonConvert.DeserializeObject<MasterGameState>(responseString);

                Console.WriteLine("Response data = " + responseString);
                Console.WriteLine();
            });
            t2.Wait();
        }
        #endregion

        private void ShowGameGrid()   // method to show the game grid in command prompt
        {
            for (int rowNumber = 0; rowNumber < 10; rowNumber++)
            {
                for (int colNumber = 0; colNumber < 10; colNumber++)
                {
                    Console.Write(GameGrid[rowNumber, colNumber]);
                }
                Console.WriteLine();
            }
        }

        public bool CheckPlacement(int colNumber, int rowNumber, int direction, int length)
        {
            // Out of bounds check --- direction: 0 = North, 1 = East, 2 = South, 3 = West

            if ((direction == 0 && rowNumber + length > 9)
            || (direction == 1 && colNumber - length < 0)
            || (direction == 2 && rowNumber - length < 0)
            || (direction == 3 && colNumber + length > 9)) { return false; }

            // Existing ship check

            for(int i = length; i > 0; i--)
            {
                if (GameGrid[rowNumber, colNumber].Length > 3) { return false; }

                switch (direction)
                {
                    case 0: // North
                        rowNumber = rowNumber + 1;
                        break;
                    case 1: // East
                        colNumber = colNumber - 1;
                        break;
                    case 2: // South
                        rowNumber = rowNumber - 1;
                        break;
                    case 3: // West
                        colNumber = colNumber + 1;
                        break;
                }
            }               

             return true;  //true is passed as result if inputs pass out of bounds and existing ship test
        }

        public void Run()
        {
            // Setup.
            AuthenticateToAzureAD();
            DemoCallAPI();

            PlaceAC1();
            PlaceD1();
            PlaceD2();
            PlaceF1();
            PlaceF2();
            PlaceF3();
            PlaceS1();

            ////Main engine loop.
            Task t = Task.Run(async () =>
            {
                while (true)
                {
                    // Take a Shot - Read API documentation at XXXXX
                    Shoot();

                    Console.WriteLine("{0} - Ship standing by for orders...", DateTime.Now.ToString());
                    await Task.Delay(15000);
                }
            });
            t.Wait();
        }

        #region Ship Placement

        private void PlaceAC1()
        {
            int rowNumber = 0;
            int colNumber = 0;
            int direction = 0;
            string facing = "";

            string shipType = "AircraftCarrier";
            int length = 5;
            string shipIndex = "1";
            string name = "AC1";

            do
            {
                rowNumber = new Random().Next(0, 10);
                colNumber = new Random().Next(0, 10);
                direction = new Random().Next(0, 4);
                Console.WriteLine(rowNumber);
                Console.WriteLine(colNumber);
                Console.WriteLine(direction);
            } while (CheckPlacement(colNumber, rowNumber, direction, length - 1) == false);  // length - 1 passed to align to array zero indexing

            switch (direction)
            {
                case 0: // North
                    facing = "North";
                    break;
                case 1: // East
                    facing = "East";
                    break;
                case 2: // South
                    facing = "South";
                    break;
                case 3: // West
                    facing = "West";
                    break;
            }

            Console.WriteLine("Placing " + name);
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GameAPIBaseAddress + "/api/placeship");
            request.Content = new System.Net.Http.StringContent("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': "+ rowNumber + ",'facing': '" + facing + "'}", Encoding.UTF8, "application/json");
            Console.WriteLine("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            Task t = Task.Run(async () =>
            {
                // Update the grid with placed ship
                for (int i = 0; i < length; i++)
                {
                    GameGrid[rowNumber, colNumber] = "(" + name + " - " +i + ")";

                    switch (direction)
                    {
                        case 0: // north
                            rowNumber = rowNumber + 1;
                            break;
                        case 1: // east
                            colNumber = colNumber - 1;
                            break;
                        case 2: // south
                            rowNumber = rowNumber - 1;
                            break;
                        case 3: // west
                            colNumber = colNumber + 1;
                            break;
                    }
                }

                HttpResponseMessage response = await client.SendAsync(request);
                Console.WriteLine("Call sucessfull = " + response.IsSuccessStatusCode);
                Console.WriteLine("Result: HTTP{0}", response.StatusCode);
                Console.WriteLine("Response success/error message = " + response.ReasonPhrase);

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response data = " + responseString);
                ShowGameGrid();  //show the game grid after the ship has been placed
                Console.WriteLine();
            });

            t.Wait();

        }

        private void PlaceD1()
        {
            int rowNumber = 0;
            int colNumber = 0;
            int direction = 0;
            string facing = "";

            string shipType = "Destroyer";
            int length = 4;
            string shipIndex = "1";
            string name = "D1";

            do
            {
                rowNumber = new Random().Next(0, 10);
                colNumber = new Random().Next(0, 10);
                direction = new Random().Next(0, 4);
                Console.WriteLine(rowNumber);
                Console.WriteLine(colNumber);
                Console.WriteLine(direction);
            } while (CheckPlacement(colNumber, rowNumber, direction, length - 1) == false);  // length - 1 passed to align to array zero indexing

            switch (direction)
            {
                case 0: // North
                    facing = "North";
                    break;
                case 1: // East
                    facing = "East";
                    break;
                case 2: // South
                    facing = "South";
                    break;
                case 3: // West
                    facing = "West";
                    break;
            }

            Console.WriteLine("Placing " + name);
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GameAPIBaseAddress + "/api/placeship");
            request.Content = new System.Net.Http.StringContent("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}", Encoding.UTF8, "application/json");
            Console.WriteLine("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            Task t = Task.Run(async () =>
            {
                // Update the grid with placed ship
                for (int i = 0; i < length; i++)
                {
                    GameGrid[rowNumber, colNumber] = "(" + name + " - " + i + ")";

                    switch (direction)
                    {
                        case 0: // north
                            rowNumber = rowNumber + 1;
                            break;
                        case 1: // east
                            colNumber = colNumber - 1;
                            break;
                        case 2: // south
                            rowNumber = rowNumber - 1;
                            break;
                        case 3: // west
                            colNumber = colNumber + 1;
                            break;
                    }
                }

                HttpResponseMessage response = await client.SendAsync(request);
                Console.WriteLine("Call sucessfull = " + response.IsSuccessStatusCode);
                Console.WriteLine("Result: HTTP{0}", response.StatusCode);
                Console.WriteLine("Response success/error message = " + response.ReasonPhrase);

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response data = " + responseString);
                ShowGameGrid();  //show the game grid after the ship has been placed
                Console.WriteLine();
            });

            t.Wait();

        }

        private void PlaceD2()
        {
            int rowNumber = 0;
            int colNumber = 0;
            int direction = 0;
            string facing = "";

            string shipType = "Destroyer";
            int length = 4;
            string shipIndex = "2";
            string name = "D2";

            do
            {
                rowNumber = new Random().Next(0, 10);
                colNumber = new Random().Next(0, 10);
                direction = new Random().Next(0, 4);
                Console.WriteLine(rowNumber);
                Console.WriteLine(colNumber);
                Console.WriteLine(direction);
            } while (CheckPlacement(colNumber, rowNumber, direction, length - 1) == false);  // length - 1 passed to align to array zero indexing

            switch (direction)
            {
                case 0: // North
                    facing = "North";
                    break;
                case 1: // East
                    facing = "East";
                    break;
                case 2: // South
                    facing = "South";
                    break;
                case 3: // West
                    facing = "West";
                    break;
            }

            Console.WriteLine("Placing " + name);
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GameAPIBaseAddress + "/api/placeship");
            request.Content = new System.Net.Http.StringContent("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}", Encoding.UTF8, "application/json");
            Console.WriteLine("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            Task t = Task.Run(async () =>
            {
                // Update the grid with placed ship
                for (int i = 0; i < length; i++)
                {
                    GameGrid[rowNumber, colNumber] = "(" + name + " - " + i + ")";

                    switch (direction)
                    {
                        case 0: // north
                            rowNumber = rowNumber + 1;
                            break;
                        case 1: // east
                            colNumber = colNumber - 1;
                            break;
                        case 2: // south
                            rowNumber = rowNumber - 1;
                            break;
                        case 3: // west
                            colNumber = colNumber + 1;
                            break;
                    }
                }

                HttpResponseMessage response = await client.SendAsync(request);
                Console.WriteLine("Call sucessfull = " + response.IsSuccessStatusCode);
                Console.WriteLine("Result: HTTP{0}", response.StatusCode);
                Console.WriteLine("Response success/error message = " + response.ReasonPhrase);

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response data = " + responseString);
                ShowGameGrid();  //show the game grid after the ship has been placed
                Console.WriteLine();
            });

            t.Wait();

        }

        private void PlaceF1()
        {
            int rowNumber = 0;
            int colNumber = 0;
            int direction = 0;
            string facing = "";

            string shipType = "Frigate";
            int length = 3;
            string shipIndex = "1";
            string name = "F1";

            do
            {
                rowNumber = new Random().Next(0, 10);
                colNumber = new Random().Next(0, 10);
                direction = new Random().Next(0, 4);
                Console.WriteLine(rowNumber);
                Console.WriteLine(colNumber);
                Console.WriteLine(direction);
            } while (CheckPlacement(colNumber, rowNumber, direction, length - 1) == false);  // length - 1 passed to align to array zero indexing

            switch (direction)
            {
                case 0: // North
                    facing = "North";
                    break;
                case 1: // East
                    facing = "East";
                    break;
                case 2: // South
                    facing = "South";
                    break;
                case 3: // West
                    facing = "West";
                    break;
            }

            Console.WriteLine("Placing " + name);
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GameAPIBaseAddress + "/api/placeship");
            request.Content = new System.Net.Http.StringContent("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}", Encoding.UTF8, "application/json");
            Console.WriteLine("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            Task t = Task.Run(async () =>
            {
                // Update the grid with placed ship
                for (int i = 0; i < length; i++)
                {
                    GameGrid[rowNumber, colNumber] = "(" + name + " - " + i + ")";

                    switch (direction)
                    {
                        case 0: // north
                            rowNumber = rowNumber + 1;
                            break;
                        case 1: // east
                            colNumber = colNumber - 1;
                            break;
                        case 2: // south
                            rowNumber = rowNumber - 1;
                            break;
                        case 3: // west
                            colNumber = colNumber + 1;
                            break;
                    }
                }

                HttpResponseMessage response = await client.SendAsync(request);
                Console.WriteLine("Call sucessfull = " + response.IsSuccessStatusCode);
                Console.WriteLine("Result: HTTP{0}", response.StatusCode);
                Console.WriteLine("Response success/error message = " + response.ReasonPhrase);

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response data = " + responseString);
                ShowGameGrid();  //show the game grid after the ship has been placed
                Console.WriteLine();
            });

            t.Wait();

        }

        private void PlaceF2()
        {
            int rowNumber = 0;
            int colNumber = 0;
            int direction = 0;
            string facing = "";

            string shipType = "Frigate";
            int length = 3;
            string shipIndex = "2";
            string name = "F2";

            do
            {
                rowNumber = new Random().Next(0, 10);
                colNumber = new Random().Next(0, 10);
                direction = new Random().Next(0, 4);
                Console.WriteLine(rowNumber);
                Console.WriteLine(colNumber);
                Console.WriteLine(direction);
            } while (CheckPlacement(colNumber, rowNumber, direction, length - 1) == false);  // length - 1 passed to align to array zero indexing

            switch (direction)
            {
                case 0: // North
                    facing = "North";
                    break;
                case 1: // East
                    facing = "East";
                    break;
                case 2: // South
                    facing = "South";
                    break;
                case 3: // West
                    facing = "West";
                    break;
            }

            Console.WriteLine("Placing " + name);
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GameAPIBaseAddress + "/api/placeship");
            request.Content = new System.Net.Http.StringContent("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}", Encoding.UTF8, "application/json");
            Console.WriteLine("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            Task t = Task.Run(async () =>
            {
                // Update the grid with placed ship
                for (int i = 0; i < length; i++)
                {
                    GameGrid[rowNumber, colNumber] = "(" + name + " - " + i + ")";

                    switch (direction)
                    {
                        case 0: // north
                            rowNumber = rowNumber + 1;
                            break;
                        case 1: // east
                            colNumber = colNumber - 1;
                            break;
                        case 2: // south
                            rowNumber = rowNumber - 1;
                            break;
                        case 3: // west
                            colNumber = colNumber + 1;
                            break;
                    }
                }

                HttpResponseMessage response = await client.SendAsync(request);
                Console.WriteLine("Call sucessfull = " + response.IsSuccessStatusCode);
                Console.WriteLine("Result: HTTP{0}", response.StatusCode);
                Console.WriteLine("Response success/error message = " + response.ReasonPhrase);

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response data = " + responseString);
                ShowGameGrid();  //show the game grid after the ship has been placed
                Console.WriteLine();
            });

            t.Wait();

        }

        private void PlaceF3()
        {
            int rowNumber = 0;
            int colNumber = 0;
            int direction = 0;
            string facing = "";

            string shipType = "Frigate";
            int length = 3;
            string shipIndex = "3";
            string name = "F3";

            do
            {
                rowNumber = new Random().Next(0, 10);
                colNumber = new Random().Next(0, 10);
                direction = new Random().Next(0, 4);
                Console.WriteLine(rowNumber);
                Console.WriteLine(colNumber);
                Console.WriteLine(direction);
            } while (CheckPlacement(colNumber, rowNumber, direction, length - 1) == false);  // length - 1 passed to align to array zero indexing

            switch (direction)
            {
                case 0: // North
                    facing = "North";
                    break;
                case 1: // East
                    facing = "East";
                    break;
                case 2: // South
                    facing = "South";
                    break;
                case 3: // West
                    facing = "West";
                    break;
            }

            Console.WriteLine("Placing " + name);
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GameAPIBaseAddress + "/api/placeship");
            request.Content = new System.Net.Http.StringContent("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}", Encoding.UTF8, "application/json");
            Console.WriteLine("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            Task t = Task.Run(async () =>
            {
                // Update the grid with placed ship
                for (int i = 0; i < length; i++)
                {
                    GameGrid[rowNumber, colNumber] = "(" + name + " - " + i + ")";

                    switch (direction)
                    {
                        case 0: // north
                            rowNumber = rowNumber + 1;
                            break;
                        case 1: // east
                            colNumber = colNumber - 1;
                            break;
                        case 2: // south
                            rowNumber = rowNumber - 1;
                            break;
                        case 3: // west
                            colNumber = colNumber + 1;
                            break;
                    }
                }

                HttpResponseMessage response = await client.SendAsync(request);
                Console.WriteLine("Call sucessfull = " + response.IsSuccessStatusCode);
                Console.WriteLine("Result: HTTP{0}", response.StatusCode);
                Console.WriteLine("Response success/error message = " + response.ReasonPhrase);

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response data = " + responseString);
                ShowGameGrid();  //show the game grid after the ship has been placed
                Console.WriteLine();
            });

            t.Wait();

        }

        private void PlaceS1()
        {
            int rowNumber = 0;
            int colNumber = 0;
            int direction = 0;
            string facing = "";

            string shipType = "Submarine";
            int length = 2;
            string shipIndex = "1";
            string name = "S1";

            do
            {
                rowNumber = new Random().Next(0, 10);
                colNumber = new Random().Next(0, 10);
                direction = new Random().Next(0, 4);
                Console.WriteLine(rowNumber);
                Console.WriteLine(colNumber);
                Console.WriteLine(direction);
            } while (CheckPlacement(colNumber, rowNumber, direction, length - 1) == false);  // length - 1 passed to align to array zero indexing

            switch (direction)
            {
                case 0: // North
                    facing = "North";
                    break;
                case 1: // East
                    facing = "East";
                    break;
                case 2: // South
                    facing = "South";
                    break;
                case 3: // West
                    facing = "West";
                    break;
            }

            Console.WriteLine("Placing " + name);
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GameAPIBaseAddress + "/api/placeship");
            request.Content = new System.Net.Http.StringContent("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}", Encoding.UTF8, "application/json");
            Console.WriteLine("{'shipType': '" + shipType + "','shipIndex': '" + shipIndex + "','name': '" + name + "','positionX': " + colNumber + ",'positionY': " + rowNumber + ",'facing': '" + facing + "'}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            Task t = Task.Run(async () =>
            {
                // Update the grid with placed ship
                for (int i = 0; i < length; i++)
                {
                    GameGrid[rowNumber, colNumber] = "(" + name + " - " + i + ")";

                    switch (direction)
                    {
                        case 0: // north
                            rowNumber = rowNumber + 1;
                            break;
                        case 1: // east
                            colNumber = colNumber - 1;
                            break;
                        case 2: // south
                            rowNumber = rowNumber - 1;
                            break;
                        case 3: // west
                            colNumber = colNumber + 1;
                            break;
                    }
                }

                HttpResponseMessage response = await client.SendAsync(request);
                Console.WriteLine("Call sucessfull = " + response.IsSuccessStatusCode);
                Console.WriteLine("Result: HTTP{0}", response.StatusCode);
                Console.WriteLine("Response success/error message = " + response.ReasonPhrase);

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response data = " + responseString);
                ShowGameGrid();  //show the game grid after the ship has been placed
                Console.WriteLine();
            });

            t.Wait();

        }

        #endregion

        #region Shooting

        private void Shoot()
        {
            int rowNumber = 0;
            int colNumber = 0;

            do
            {
                rowNumber = new Random().Next(0, 10);
                colNumber = new Random().Next(0, 10);
            } while (CheckShot(colNumber, rowNumber) == false);

            Console.WriteLine("Shooting at co-ordinates " + colNumber + "," + rowNumber);
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GameAPIBaseAddress + "/api/shoot");
            request.Content = new System.Net.Http.StringContent("{'positionX': " + colNumber + ",'positionY': " + rowNumber + "}", Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            Task t = Task.Run(async () =>
            {
                EnemyGameGrid[rowNumber, colNumber] = "Shot At";
                ShotsTaken++;
                Console.WriteLine("Number of shots fired: " + ShotsTaken);

                HttpResponseMessage response = await client.SendAsync(request);
                Console.WriteLine("Call sucessfull = " + response.IsSuccessStatusCode);
                Console.WriteLine("Result: HTTP{0}", response.StatusCode);
                Console.WriteLine("Response success/error message = " + response.ReasonPhrase);

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response data = " + responseString);
                Console.WriteLine();
            });

            t.Wait();

        }

        public bool CheckShot(int colNumber, int rowNumber)
        {
            // Existing shot at co-oridinates
            if (EnemyGameGrid[rowNumber, colNumber].Length > 3) { return false; }
            return true;
        }
        #endregion
    }
}