﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Web.Models;

namespace Capstone.Web.DAL
{
    public interface IBreweryDAL
    {
        IList<BreweryModel> GetAllBreweries();

        List<BreweryModel> SearchBreweries(SearchStringModel searchstring);

        BreweryModel GetBreweryDetail(int breweryId);
    }
}