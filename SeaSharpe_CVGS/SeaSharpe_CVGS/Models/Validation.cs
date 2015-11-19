using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeaSharpe_CVGS.Models
{
    public class Validation
    {
    }


    /// <summary>
    /// MetaData Validation for Review
    /// </summary>
    [MetadataType(typeof(ReviewMetadata))]
    public partial class Review
    {
        class ReviewMetadata
        {
            [Required(ErrorMessage = "Required")]
            public virtual Game Game { get; set; }

            [Required(ErrorMessage = "Required")]
            public virtual Member Author { get; set; }

            [Required(ErrorMessage = "Required")]
            public float Rating { get; set; }

            [StringLength(500, MinimumLength = 1, ErrorMessage = "Maximun lenght is 500 characters")]
            public string Subject { get; set; }

            [StringLength(4000, MinimumLength = 0, ErrorMessage = "Maximun lenght is 4000 characters")]
            public string Body { get; set; }
        }
    }

    /// <summary>
    /// MetaData Validation for Friendship
    /// </summary>
    [MetadataType(typeof(FriendshipMetadata))]
    public partial class Friendship
    {
        class FriendshipMetadata
        {
            [Required(ErrorMessage = "Required")]
            public bool IsFamilyMember { get; set; }
        }
    }

    /// <summary>
    /// MetaData Validation for Platform
    /// </summary>
    [MetadataType(typeof(PlatformMetadata))]
    public partial class Platform
    {
        class PlatformMetadata
        {
            [Required(ErrorMessage = "Required"), MaxLength(50), MinLength(1)]
            public string Name { get; set; }
        }
    }

    /// <summary>
    /// MetaData Validation for Category
    /// </summary>
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
        class CategoryMetadata
        {
            [Required(ErrorMessage = "Required"), MaxLength(50), MinLength(1)]
            public string Name { get; set; }
        }
    }

    /// <summary>
    /// MetaData Validation for Event 
    /// </summary>
    [MetadataType(typeof(EventMetadata))]
    public partial class Event
    {
        class EventMetadata
        {
            //[Required(ErrorMessage = "Event Id is required")]
            //[Range(4000000, 4999999, ErrorMessage = "Event Id must be between 4,000,000 and 4,999,999")]
            //public int Id { get; set; }

            [Required]
            public virtual Employee Employee { get; set; }
            
            [MinLength(0), MaxLength(2000)]
            public string Location { get; set; }

            [DateNotInThePast(ErrorMessage = "Date can't be on the past")]
            [Required(ErrorMessage = "Required")]
            public DateTime StartDate { get; set; }

            [DateNotBeforeStartDate(ErrorMessage = "End date can't be before than start date")]
            [Required(ErrorMessage = "Required")]
            public DateTime EndDate { get; set; }

            [MinLength(0), MaxLength(4000)]
            public string Description { get; set; }
            
            [Required, Range(0, int.MaxValue)]
            public int Capacity { get; set; }
        }
    }




}