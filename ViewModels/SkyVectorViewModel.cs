using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZoaInfoTool.Models;

namespace ZoaInfoTool.ViewModels
{
    public partial class SkyVectorViewModel : ObservableObject
    {
        [ObservableProperty]
        private string departureAirport;

        [ObservableProperty]
        private string arrivalAirport;

        [ObservableProperty]
        private string route;

        [ObservableProperty]
        private string url;

        public SkyVectorViewModel()
        {
            Url = Constants.SkyVectorBaseUrl;
        }

        [RelayCommand]
        private void MakeUrl()
        {
            Url = string.Join(" ", new string[] { Constants.SkyVectorBaseUrl, departureAirport, route, arrivalAirport });
        }
    }
}
