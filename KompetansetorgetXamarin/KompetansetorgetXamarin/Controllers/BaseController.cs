﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KompetansetorgetXamarin.DAL;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;

namespace KompetansetorgetXamarin.Controllers
{
    public abstract class BaseController
    {
        protected DbContext DbContext = DbContext.GetDbContext;
        protected SQLiteConnection Db;

        // Change this to http://kompetansetorget.uia.no/api/
        protected string Adress = "http://kompetansetorgetserver1.azurewebsites.net/api/";

        public BaseController()
        {
            Db = DbContext.Db;
        }

        /// <summary>
        /// Removes the milliseconds of a datetime, which is needed due to "lack" of precicion in SQLITE.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DateTime TrimMilliseconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
        }


        /*
        public T GetProjectByUuid(string uuid)
        {
            try
            {
                lock (DbContext.locker)
                {
                    return Db.GetWithChildren<T>(uuid);
                    //return Db.Get<Project>(uuid);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ProjectController - GetProjectByUuid(string uuid): Exception msg: " + e.Message);
                System.Diagnostics.Debug.WriteLine("ProjectController - GetProjectByUuid(string uuid): Stack Trace: \n" + e.StackTrace);
                System.Diagnostics.Debug.WriteLine("ProjectController - GetProjectByUuid(string uuid): End Of Stack Trace");
                return default(T);
            }
        }
        */
    }
}