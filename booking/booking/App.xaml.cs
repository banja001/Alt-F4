using booking.View;
using booking.View.Guest1;
using Egor92.MvvmNavigation;
using Localization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF.ViewModels.Guest1;

namespace booking
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHFqVk9rXVNbdV5dVGpAd0N3RGlcdlR1fUUmHVdTRHRcQlliTn9VdkNhWXlfd3Q=;Mgo+DSMBPh8sVXJ1S0d+X1hPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXpScURlWXlccHJWRGA=;ORg4AjUWIQA/Gnt2VFhhQlJNfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5Xd0JiX39YcnRTQmdc;MTc1MzU1M0AzMjMxMmUzMTJlMzMzOUlmUC95M0wvUDcreWFoeTJnNDdGdWxOZnBLVXNWY085dk5vZWVNNFB0cDg9;MTc1MzU1NEAzMjMxMmUzMTJlMzMzOVc5TTNMa2loZ0syam9qVWs0THY1WG50U1dVd0huRGREUHU1dHJGRUJrbUU9;NRAiBiAaIQQuGjN/V0d+XU9Hf1RDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31TckRkWHlednZVTmJYUQ==;MTc1MzU1NkAzMjMxMmUzMTJlMzMzOUphbGo5VlVEM0Ntb0p5eUY0TUNqanoreTAzaFpyU2ZVWnE5L0Jrc3B4UXM9;MTc1MzU1N0AzMjMxMmUzMTJlMzMzOW5RMFRaeVNsL3VPZCsxd3ZoazRSUWJVZmlMa0ZGc1A0M0VaWWlNSkdmT3c9;Mgo+DSMBMAY9C3t2VFhhQlJNfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5Xd0JiX39YcnRcQGdc;MTc1MzU1OUAzMjMxMmUzMTJlMzMzOVlHQTBicEtWWVdYVkJvc2IzMk5pMGt0UHR2MHN2eStNMUVXMFlKT1A3MUE9;MTc1MzU2MEAzMjMxMmUzMTJlMzMzOUFidkg1bjBBMkZPTEhoaXNGTWF4TTQxeVlWU1NNVFZlQyswNFBKdmNqZEk9;MTc1MzU2MUAzMjMxMmUzMTJlMzMzOUphbGo5VlVEM0Ntb0p5eUY0TUNqanoreTAzaFpyU2ZVWnE5L0Jrc3B4UXM9");
            
            
        }
        public string getCultureInfo()
        {
            return TranslationSource.Instance.CurrentCulture.Name;
        }
        public void ChangeLanguage(string lang)
        {
            TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo(lang);
        }
    }
}