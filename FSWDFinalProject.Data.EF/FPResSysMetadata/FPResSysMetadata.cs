using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSWDFinalProject.Data.EF//.FPResSysMetadata
{
    #region Location
    [MetadataType(typeof(LocationMetadata))]
    public partial class Location { }
    public class LocationMetadata
    {
        //public int LocationId { get; set; }
        [Required(ErrorMessage = "*Location Required")]
        [StringLength(50, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        [Display(Name = "Location")]
        public string LocationName { get; set; }

        [Required(ErrorMessage = "*Address Required")]
        [StringLength(100, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*City Required")]
        [StringLength(100, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        public string City { get; set; }

        [Required(ErrorMessage = "*State Required")]
        [StringLength(2, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        public string State { get; set; }

        [Required(ErrorMessage = "*Zip Code Required")]
        [StringLength(5, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "*Limit Required")]
        [StringLength(15, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        [Display(Name = "Reservation Limit")]
        public string ReservationLimit { get; set; }
    }
    #endregion

    #region UserDetail
    [MetadataType(typeof(UserDetailsMetadata))]
    public partial class UserDetail { }
    public class UserDetailsMetadata
    {
        [Required(ErrorMessage = "*User ID is Required")]
        [StringLength(128, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "*First Name Required")]
        [StringLength(50, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*Last Name Required")]
        [StringLength(50, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*Phone Number Required")]
        [StringLength(15, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        [Display(Name = "Phone Number")]
        public string UserPhone { get; set; }

        [Display(Name = "Location ID")]
        [Required(ErrorMessage = "Location ID is required")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public Nullable<int> LocationId { get; set; }
    
    }
    #endregion

    #region FullName
    public partial class UserDetail
    {
        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
    #endregion

    #region UserAssets
    [MetadataType(typeof(UserAssetsMetadata))]
    public partial class UserAssets { }
    public class UserAssetsMetadata
    {
        //public int UserAssetId { get; set; }
        [Required(ErrorMessage = "* Vehicle Model is Required")]
        [StringLength(50, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        [Display(Name = "Vehicle Model")]
        public string AssetName { get; set; }

        [Required(ErrorMessage = "*User ID is Required")]
        [StringLength(128, ErrorMessage = "* Character limit reached. Please contact system administrator if more room is needed")]
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        [Display(Name = "Asset Photo")]
        public string AssetPhoto { get; set; }

        [UIHint("MultilineText")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        [Display(Name = "Special Notes")]
        public string SpecialNotes { get; set; }

        [Display(Name = "Date Added")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "[-N/A-]")]
        public System.DateTime DateAdded { get; set; }
        public Nullable<System.DateTime> AssetYear { get; set; }
    }
    #endregion

    #region Reservation
    [MetadataType(typeof(ReservationMetadata))]
    public partial class Reservations { }
    public class ReservationMetadata
    {
        //[Display(Name = "Reservation ID")]
        //[Required(ErrorMessage = "Reservation ID is required")]
        //public int ReservationId { get; set; }

        [Display(Name = "Asset ID")]
        [Required(ErrorMessage = "Asset ID is required")]
        public int UserAssetId { get; set; }

        [Display(Name = "Location ID")]
        [Required(ErrorMessage = "Location ID is required")]
        public int LocationId { get; set; }
        public System.DateTime ReservationDate { get; set; }
    }
    #endregion
}
