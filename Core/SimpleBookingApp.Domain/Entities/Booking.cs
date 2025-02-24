using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBookingApp.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public DateTime DateFrom { get; set; }  // Başlangıç tarihi
        public DateTime DateTo { get; set; }  // Bitiş tarihi (örneğin 1 gün sonrası)
        public int BookedQuantity { get; set; }
        public Resource Resource { get; set; }

    }
}
