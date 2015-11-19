using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeaSharpe_CVGS.Models
{
    public class Validation
    {

    }

    [MetadataType(typeof(EventMetadata))]
    public partial class Event
    {
        class EventMetadata
        {
            [Required(ErrorMessage = "Event Id is required")]
            [Range(4000000, 4999999, ErrorMessage = "Event Id must be between 4,000,000 and 4,999,999")]
            public int Id { get; set; }

            [Required(ErrorMessage = "Start Date is required")]
            public DateTime StartDate { get; set; }

            [Required(ErrorMessage = "End Date is required")]
            public DateTime EndDate { get; set; }
        }
    }
}