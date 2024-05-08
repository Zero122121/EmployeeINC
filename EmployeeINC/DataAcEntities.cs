using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeINC
{
    public partial class DataAcEntities1 : DbContext
    {
        private static DataAcEntities1 _context;


        public static DataAcEntities1 GetContext()
        {
            if (_context == null)
            {
                _context = new DataAcEntities1();
            }
            return _context;
        }

        public virtual DbSet<Отделы> Otdelss { get; set; }
        public virtual DbSet<Должности> dol { get; set; }
        public virtual DbSet<Сотрудники> sot { get; set; }
    }
}
