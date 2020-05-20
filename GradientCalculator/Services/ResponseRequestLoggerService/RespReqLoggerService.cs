using GradientCalculator.Services.ResponseRequestLoggerService.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Services.ResponseRequestLoggerService
{
    public class RespReqLoggerService : IDisposable
    {
        private const string connectionString = @"ResponseRequestStorage.db";

        private LiteDatabase db = new LiteDatabase(connectionString);

        private LiteCollection<ResponseRequestLog> ResponseRequestLogItems
        {
            get
            {
                return db.GetCollection<ResponseRequestLog>("ResponseRequestLogItems");
            }
        }


        public RespReqLoggerService()
        {

        }

        public bool AddNewResponseRequestLog(ResponseRequestLog newLog) 
        {
            if (newLog == null)
            {
                return false;
            }

            this.ResponseRequestLogItems.Insert(newLog);

            return true;
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
