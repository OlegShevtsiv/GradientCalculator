using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Data
{
    public class LiteStorage : ILiteStorage
    {
        private LiteDatabase db;

        public LiteCollection<dynamic> MyProperty { get; set; }


        public void Dispose()
        {
            db.Dispose();
        }
    }
}
