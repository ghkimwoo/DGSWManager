using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGSWManager.Models
{
    public class CorpDartListModel
    {
        [PrimaryKey]
        public int Corp_code { get; set; }
        public string Corp_name { get; set; }
        public int Modify_date { get; set; }
    }
}
