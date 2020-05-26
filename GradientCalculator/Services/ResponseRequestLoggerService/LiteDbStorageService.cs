using GradientCalculator.Services.ResponseRequestLoggerService.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Services.ResponseRequestLoggerService
{
    public class LiteDbStorageService : IDisposable
    {
        private const string connectionString = @"LiteDbStorage.db";

        private LiteDatabase db = new LiteDatabase(connectionString);

        private LiteCollection<ResponseRequestLog> ResponseRequestLogItems
        {
            get
            {
                return db.GetCollection<ResponseRequestLog>("ResponseRequestLogItems");
            }
        }


        public LiteDbStorageService()
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

        public List<ResponseRequestLog> GetResponseRequestLogs(ResponseRequestLogType? type) 
        {
            if (type != null)
            {
                string typeStr = type.ToString();
                return this.ResponseRequestLogItems.Find(Query.EQ(nameof(ResponseRequestLog.Type), typeStr)).ToList();
            }
            else 
            {
                return this.ResponseRequestLogItems.FindAll().ToList();
            }
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
