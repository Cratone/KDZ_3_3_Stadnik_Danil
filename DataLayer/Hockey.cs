using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;

namespace DataLayer
{
    public class Hockey
    {
        private string _id;
        private string _objectName;
        private string _nameWinter;
        private string _photoWinter;
        private string _admArea;
        private string _district;
        private string _address;
        private string _email;
        private string _webSite;
        private string _helpPhone;
        private string _helpPhoneExtension;
        private string _workingHoursWinter;
        private string _clarificationOfWorkingHoursWinter;
        private string _propertyType;
        private string _departamentalAffiliationType;
        private string _hasEquipmentRental;
        private string _equipmentRentalComments;
        private string _hasTechService;
        private string _techServiceComments;
        private string _hasDressingRoom;
        private string _hasEatery;
        private string _hasToilet;
        private string _hasWifi;
        private string _hasCashMachine;
        private string _hasFirstAidPost;
        private string _hasMusic;
        private string _usagePeriodWinter;
        private string _status;
        private string _dimensionsWinter;
        private string _lighting;
        private string _surfaceTypeWinter;
        private int _seats;
        private string _paid;
        private string _paidComments;
        private string _disabilityFriendly;
        private string _servicesWinter;
        private string _geoData;
        private string _geoDataCenter;
        private string _geoArea;
        
        [Name("global_id")]
        [JsonProperty("global_id")]
        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public string ObjectName
        {
            get => _objectName;
            set => _objectName = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string NameWinter
        {
            get => _nameWinter;
            set => _nameWinter = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string PhotoWinter
        {
            get => _photoWinter;
            set => _photoWinter = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string AdmArea
        {
            get => _admArea;
            set => _admArea = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string District
        {
            get => _district;
            set => _district = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Address
        {
            get => _address;
            set => _address = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Email
        {
            get => _email;
            set => _email = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string WebSite
        {
            get => _webSite;
            set => _webSite = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string HelpPhone
        {
            get => _helpPhone;
            set => _helpPhone = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string HelpPhoneExtension
        {
            get => _helpPhoneExtension;
            set => _helpPhoneExtension = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string WorkingHoursWinter
        {
            get => _workingHoursWinter;
            set => _workingHoursWinter = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string ClarificationOfWorkingHoursWinter
        {
            get => _clarificationOfWorkingHoursWinter;
            set => _clarificationOfWorkingHoursWinter = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string PropertyType
        {
            get => _propertyType;
            set => _propertyType = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string DepartamentalAffiliationType
        {
            get => _departamentalAffiliationType;
            set => _departamentalAffiliationType = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string HasEquipmentRental
        {
            get => _hasEquipmentRental;
            set => _hasEquipmentRental = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string EquipmentRentalComments
        {
            get => _equipmentRentalComments;
            set => _equipmentRentalComments = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string HasTechService
        {
            get => _hasTechService;
            set => _hasTechService = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string TechServiceComments
        {
            get => _techServiceComments;
            set => _techServiceComments = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string HasDressingRoom
        {
            get => _hasDressingRoom;
            set => _hasDressingRoom = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string HasEatery
        {
            get => _hasEatery;
            set => _hasEatery = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string HasToilet
        {
            get => _hasToilet;
            set => _hasToilet = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string HasWifi
        {
            get => _hasWifi;
            set => _hasWifi = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string HasCashMachine
        {
            get => _hasCashMachine;
            set => _hasCashMachine = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string HasFirstAidPost
        {
            get => _hasFirstAidPost;
            set => _hasFirstAidPost = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string HasMusic
        {
            get => _hasMusic;
            set => _hasMusic = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string UsagePeriodWinter
        {
            get => _usagePeriodWinter;
            set => _usagePeriodWinter = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Status
        {
            get => _status;
            set => _status = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string DimensionsWinter
        {
            get => _dimensionsWinter;
            set => _dimensionsWinter = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Lighting
        {
            get => _lighting;
            set => _lighting = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string SurfaceTypeWinter
        {
            get => _surfaceTypeWinter;
            set => _surfaceTypeWinter = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int Seats
        {
            get => _seats;
            set => _seats = value;
        }

        public string Paid
        {
            get => _paid;
            set => _paid = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string PaidComments
        {
            get => _paidComments;
            set => _paidComments = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string DisabilityFriendly
        {
            get => _disabilityFriendly;
            set => _disabilityFriendly = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string ServicesWinter
        {
            get => _servicesWinter;
            set => _servicesWinter = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        [Name("geoData")]
        [JsonProperty("geoData")]
        public string GeoData
        {
            get => _geoData;
            set => _geoData = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        [Name("geodata_center")]
        [JsonProperty("geodata_center")]
        public string GeoDataCenter
        {
            get => _geoDataCenter;
            set => _geoDataCenter = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        [Name("geoarea")]
        [JsonProperty("geoarea")]
        public string GeoArea
        {
            get => _geoArea;
            set => _geoArea = value ?? throw new ArgumentNullException(nameof(value));
        }

        [Ignore]
        public string this[string nameField]
        {
            get
            {
                return nameField switch
                {
                    "ObjectName" => ObjectName,
                    "NameWinter" => NameWinter,
                    "District" => District,
                    "HasDressingRoom" => HasDressingRoom,
                    _ => ""
                };
            }
        }
    }
}
