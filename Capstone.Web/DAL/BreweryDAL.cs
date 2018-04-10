﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Capstone.Web.Models;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Capstone.Web.DAL
{
    public class BreweryDAL : IBreweryDAL
    {
        private string connectionString;

        public BreweryDAL(string connectionString)
        {
            this.connectionString = connectionString;
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

        public BreweryModel GetBreweryDetail()
        {
            // Use SQL REader to get the details of a single brewery
            BreweryModel brewery;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(/*@""*/);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        brewery = BreweryReader(reader);
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

        public IList<BreweryModel> SearchBreweries(SearchResultModel searchString)
        {
            //List<string> searchParameters = new List<string>();

            Regex reg = new Regex(@"(?:,\s)");

            var searchParameters = reg.Split(searchString.SearchString);


            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        BreweryModel brewery = new BreweryModel();


                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        private BreweryModel BreweryReader(SqlDataReader reader)
        {
            return new BreweryModel()
            {
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
                BreweryHeaderImage = Convert.ToString(reader["BreweryHeaderImg"])
            };
        }
    }
}


