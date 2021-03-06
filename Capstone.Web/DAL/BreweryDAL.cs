﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Capstone.Web.Models;
using System.Configuration;
using System.Text.RegularExpressions;
using GoogleMaps.LocationServices;

namespace Capstone.Web.DAL
{
    public class BreweryDAL : IBreweryDAL
    {
        private string connectionString;

        private string locationServiceApiKey = "AIzaSyBd0o2LU8lvSyx2etULu-bEEiSl7EKTJFM";

        public BreweryDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddBrewery(AddBreweryModel brewery)
        {
            SetBreweryCoords(brewery);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Brewery (UserId, BreweryName, BreweryAddress, BreweryCity, BreweryDistrict, BreweryCountry, BreweryPostalCode, History, YearFounded, HoursOfOperation, BreweryProfileImg, BreweryBackgroundImg, BreweryHeaderImg, Email, Phone, BreweryLatitude, BreweryLongitude)
                                                                  VALUES (@userId, @breweryName, @breweryAddress, @breweryCity, @breweryDistrict, @breweryCountry, @breweryPostalCode, @history, @yearFounded, @hoursOfOperation, @breweryProfileImg, @breweryBackgroundImg, @breweryHeaderImg, @email, @phone, @latitude, @longitude)", conn);

                    //MERGE INTO dbo.Brewery WITH(HOLDLOCK) AS target
                    //USING(SELECT * FROM dbo.Brewery WHERE Brewery.BreweryId = 10) AS source
                    //    ON target.BreweryId = source.BreweryId
                    //WHEN MATCHED THEN
                    //    UPDATE SET target.BreweryName = 'Hi i work'
                    //WHEN NOT MATCHED BY TARGET THEN
                    //    INSERT(BreweryName)
                    //    VALUES('this is when it doesnt match');

                    //--Update the row if it exists.
                    //    UPDATE Brewery
                    //SET BreweryName = 'timmy'
                    //WHERE BreweryId = 16
                    //-- Insert the row if the UPDATE statement failed.
                    //IF(@@ROWCOUNT = 0)
                    //BEGIN
                    //    INSERT INTO Brewery(BreweryName, BreweryAddress)
                    //    VALUES('not timmy', '111 E Balls St')

                    //    END

                    //SqlCommand cmd = new SqlCommand(@"UPDATE Brewery
                    //                                         SET target.UserId = @userId
                    //                                         SET target.BreweryName = @breweryName
                    //                                         SET target.BreweryAddress = @breweryAddress
                    //                                         SET target.BreweryCity = @breweryCity
                    //                                         SET target.BreweryDistrict = @breweryDistrict
                    //                                         SET target.BreweryCountry = @breweryCountry
                    //                                         SET target.BreweryPostalCode = @breweryPostalCode
                    //                                         SET target.History = @history
                    //                                         SET target.YearFounded = @yearFounded
                    //                                         SET target.HoursOfOperation, @hoursOfOperation
                    //                                         SET target.BreweryProfileImg = @breweryProfileImg
                    //                                         SET target.BreweryBackgroundImg = @breweryBackgroundImg
                    //                                         SET target.BreweryHeaderImg = @breweryHeaderImage
                    //                                         SET target.Email = @email
                    //                                         SET target.Phone = @phone
                    //                                         SET target.Latitude = @latitude
                    //                                         SET target.Longitude = @longitude
                    //                                    WHERE Brewery.BreweryName = @breweryName
                    //                                    @IF(@@ROWCOUNT = 0)
                    //                                        INSERT INTO Brewery (UserId, BreweryName, BreweryAddress, BreweryCity, BreweryDistrict, BreweryCountry, BreweryPostalCode, History, YearFounded, HoursOfOperation, BreweryProfileImg, BreweryBackgroundImg, BreweryHeaderImg, Email, Phone, BreweryLatitude, BreweryLongitude)
                    //                                                    VALUES (@userId, @breweryName, @breweryAddress, @breweryCity, @breweryDistrict, @breweryCountry, @breweryPostalCode, @history, @yearFounded, @hoursOfOperation, @breweryProfileImg, @breweryBackgroundImg, @breweryHeaderImg, @email, @phone, @latitude, @longitude)", conn);

                    //cmd.Parameters.AddWithValue("@userId", brewery.UserId);
                    //cmd.Parameters.AddWithValue("@breweryName", brewery.BreweryName);
                    //cmd.Parameters.AddWithValue("@breweryAddress", brewery.BreweryAddress);
                    //cmd.Parameters.AddWithValue("@breweryCity", brewery.BreweryCity);
                    //cmd.Parameters.AddWithValue("@breweryDistrict", brewery.BreweryDistrict);
                    //cmd.Parameters.AddWithValue("@breweryCountry", brewery.BreweryCountry);
                    //cmd.Parameters.AddWithValue("@breweryPostalCode", brewery.BreweryPostalCode);
                    //cmd.Parameters.AddWithValue("@history", brewery.History);
                    //cmd.Parameters.AddWithValue("@yearFounded", brewery.YearFounded);
                    //cmd.Parameters.AddWithValue("@hoursOfOperation", brewery.HoursOfOperation);
                    //cmd.Parameters.AddWithValue("@breweryProfileImg", brewery.BreweryProfileImg);
                    //cmd.Parameters.AddWithValue("@breweryBackgroundImg", brewery.BreweryBackgroundImg);
                    //cmd.Parameters.AddWithValue("@breweryHeaderImg", brewery.BreweryHeaderImage);
                    //cmd.Parameters.AddWithValue("@email", brewery.Email);
                    //cmd.Parameters.AddWithValue("@phone", brewery.Phone);
                    //cmd.Parameters.AddWithValue("@latitude", brewery.BreweryLatitude);
                    //cmd.Parameters.AddWithValue("@longitude", brewery.BreweryLongitude);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public IList<BreweryModel> GetAllBreweries()
        {
            List<BreweryModel> breweries = new List<BreweryModel>();
            // Use SQL Reader to get a list of all brewery models
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM Brewery", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        BreweryModel brewery = BreweryReader(reader);
                        breweries.Add(brewery);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return breweries;
        }

        public List<string> GetAllBreweryNames()
        {
            try
            {
                List<string> breweryNames = new List<string>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT Brewery.BreweryName FROM Brewery", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        breweryNames.Add(Convert.ToString(reader["BreweryName"]));
                    }
                    return breweryNames;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public BreweryModel GetBreweryDetail(int breweryId)
        {
            // Use SQL REader to get the details of a single brewery
            

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM Brewery WHERE BreweryId = @breweryId", conn);
                    cmd.Parameters.AddWithValue("@breweryId", breweryId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        BreweryModel brewery = BreweryReader(reader);
                        return brewery;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return null;
        }

        //DID NOT TEST THIS. 
        public void RemoveBrewery(int breweryId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"DELETE FROM Brewery
                                                      WHERE Brewery.BreweryId = @breweryId", conn);

                    cmd.Parameters.AddWithValue("@breweryId", breweryId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex); ;
            }
        }


        public List<BreweryModel> SearchBreweries(string searchString, string latitude, string longitude, decimal searchRadius)
        {
            Dictionary<string, BreweryModel> searchResults = new Dictionary<string, BreweryModel>();

            Regex reg = new Regex(@"(?:\s)");

            var searchParameters = reg.Split(searchString);

            try
            {
                for (int i = 0; i < searchParameters.Length; i++)
                {

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();


//DECLARE @user_lat DECIMAL(12, 9)
//DECLARE @user_lng DECIMAL(12, 9)
//SET @user_lat=39.963692 SET @user_lng=-75.139946

//DECLARE @orig geography = geography::Point(@user_lat, @user_lng, 4326);

//SELECT *, @orig.STDistance(geography::Point(Brewery.BreweryLatitude,  Brewery.BreweryLongitude, 4326)) AS distance

//FROM Brewery
//ORDER BY distance ASC
                        string searchTerm = searchParameters[i];
                        SqlCommand cmd = new SqlCommand(@"DECLARE @user_lat DECIMAL(12, 9)
                                                          DECLARE @user_lng DECIMAL(12, 9)
                                                          SET @user_lat=@latitude SET @user_lng=@longitude
                                                          DECLARE @orig geography = geography::Point(@user_lat, @user_lng, 4326);
                                                          SELECT *, @orig.STDistance(geography::Point(Brewery.BreweryLatitude, Brewery.BreweryLongitude, 4326)) AS distance
                                                          FROM Brewery
                                                          WHERE (BreweryName LIKE @brewery
                                                          OR Brewery.BreweryDistrict LIKE @district
                                                          OR Brewery.BreweryCity LIKE @city
                                                          OR Brewery.BreweryPostalCode LIKE @postal)
                                                          AND (@orig.STDistance(geography::Point(Brewery.BreweryLatitude, Brewery.BreweryLongitude, 4326)) < @searchRadius)", conn);

                        cmd.Parameters.AddWithValue("@brewery", $"%{searchTerm}%");
                        cmd.Parameters.AddWithValue("@district", $"%{searchTerm}%");
                        cmd.Parameters.AddWithValue("@city", $"%{searchTerm}%");
                        cmd.Parameters.AddWithValue("@postal", $"%{searchTerm}%");
                        cmd.Parameters.AddWithValue("@latitude", latitude);
                        cmd.Parameters.AddWithValue("@longitude", longitude);
                        cmd.Parameters.AddWithValue("@searchRadius", searchRadius);


                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            BreweryModel brewery = BreweryReader(reader);
                            if(!searchResults.ContainsKey(brewery.BreweryName))
                            {
                                searchResults.Add(brewery.BreweryName, brewery);
                            }
                        }
                    }                    
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

            var searchResultsList = searchResults.Values.ToList();

            return searchResultsList;
        }

        private BreweryModel BreweryReader(SqlDataReader reader)
        {
            return new BreweryModel()
            {
                BreweryId = Convert.ToInt32(reader["BreweryId"]),
                BreweryName = Convert.ToString(reader["BreweryName"]),
                BreweryAddress = Convert.ToString(reader["BreweryAddress"]),
                BreweryCity = Convert.ToString(reader["BreweryCity"]),
                BreweryPostalCode = Convert.ToString(reader["BreweryPostalCode"]),
                BreweryDistrict = Convert.ToString(reader["BreweryDistrict"]),
                BreweryCountry = Convert.ToString(reader["BreweryCountry"]),
                History = Convert.ToString(reader["History"]),
                YearFounded = Convert.ToInt32(reader["YearFounded"]),
                BreweryProfileImg = Convert.ToString(reader["BreweryProfileImg"]),
                BreweryBackgroundImg = Convert.ToString(reader["BreweryBackgroundImg"]),
                BreweryHeaderImage = Convert.ToString(reader["BreweryHeaderImg"]),
                HoursOfOperation = Convert.ToString(reader["HoursOfOperation"]),
                Email = Convert.ToString(reader["Email"]),
                Phone = Convert.ToString(reader["Phone"]),
                BreweryLatitude = Convert.ToDouble(reader["BreweryLatitude"]),
                BreweryLongitude = Convert.ToDouble(reader["BreweryLongitude"]),
            };
        }

        /// <summary>
        /// Helper Method that uses Google GeoCode API that gets and sets lat and long from an address
        /// - JV
        /// </summary>
        /// <param name="brewery">the brewery model being added</param>
        public void SetBreweryCoords(AddBreweryModel brewery)
        {
            string address = brewery.BreweryAddress + ", " + brewery.BreweryCity + ", " + brewery.BreweryDistrict + "," + brewery.BreweryPostalCode;

            var geoCoder = new GoogleLocationService(locationServiceApiKey);

            var breweryLocation = geoCoder.GetLatLongFromAddress(address);

            brewery.BreweryLatitude = breweryLocation.Latitude;
            brewery.BreweryLongitude = breweryLocation.Longitude;
        }

        public void SetBreweryOwner(AddBreweryModel brewery, string userName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT Users.UserId FROM Users
                                                    WHERE userName = @userName", conn);

                    cmd.Parameters.AddWithValue("@userName", userName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        brewery.UserId = Convert.ToString(reader["UserId"]);
                    }
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}


