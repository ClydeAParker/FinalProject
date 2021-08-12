using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSWDFinalProject.Data.EF//.FPResSysMetadata
{
    [MetadataType(typeof(LocationMetadata))]
    public partial class Location { }
    public class LocationMetadata
    {
        //public int LocationId { get; set; }
        [Required(ErrorMessage = "*Location Required")]
        [Display(Name = "Location")]
        public string LocationName { get; set; }
        [Required(ErrorMessage = "*Address Required")]
        [StringLength(50, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        public string Address { get; set; }
        [Required(ErrorMessage = "*City Required")]
        [StringLength(50, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        public string City { get; set; }
        [Required(ErrorMessage = "*State Required")]
        [StringLength(2, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        public string State { get; set; }
        [Required(ErrorMessage = "*Zip Code Required")]
        [StringLength(5, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "*Limit Required")]
        [Display(Name = "Reservation Limit")]
        public string ReservationLimit { get; set; }
    }


    public class UserDetailsMetadata
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserPhone { get; set; }
    }

    [MetadataType(typeof(UserDetailsMetadata))]
    public partial class UserDetails
    {
        //[Display(Name = "Full Name")]
        //public string FullName
        //{
        //    get { return FirstName + " " + LastName; }
        //}
    }

    [MetadataType(typeof(UserAssetsMetadata))]
    public partial class UserAssets { }
    public class UserAssetsMetadata
    {
        public int UserAssetId { get; set; }
        public string AssetName { get; set; }
        public string UserId { get; set; }
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string AssetPhoto { get; set; }
        [UIHint("MultilineText")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string SpecialNotes { get; set; }
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "[-N/A-]")]
        public System.DateTime DateAdded { get; set; }
        public Nullable<System.DateTime> AssetYear { get; set; }
    }

    [MetadataType(typeof(ReservationMetadata))]
    public partial class Reservation { }
    public class ReservationMetadata
    {
        public int ReservationId { get; set; }
        public int UserAssetId { get; set; }
        public int LocationId { get; set; }
        public System.DateTime ReservationDate { get; set; }
    }
}
