using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BatchDataAccessLibrary.Models
{
    public class GapInTimeReasons
    {
        public int GapInTimeReasonsId { get; set; }
        [MaxLength(50)]
        public string Material1 { get; set; }
        [MaxLength(50)]
        public string Material2 { get; set; }
        public string Reason { get; set; }
    }
}
