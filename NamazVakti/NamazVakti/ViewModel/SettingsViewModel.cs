using NamazVakti.Models;
using NamazVakti.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace NamazVakti.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Ulke> Countries { get; set; }
        public ObservableCollection<Sehir> Cities { get; set; }
        public ObservableCollection<Ilce> Towns { get; set; }
      
        private readonly NamazApiService namazApi;
        private readonly MainViewModel mainViewModel;

        internal void CountryIndexChanged(EventArgs e)
        {
            var cities = namazApi.GetCities(SelectedCountry.UlkeID);

            Cities.Clear();
            cities.ForEach(c => Cities.Add(c));

            Towns.Clear();
        }

        internal void CityIndexChanged(EventArgs e)
        {
            var towns = namazApi.GetTowns(SelectedCity.SehirID);

            Towns.Clear();
            towns.ForEach(t => Towns.Add(t));

            var cityCenter = Towns.FirstOrDefault(t => t.IlceAdi == SelectedCity.SehirAdi);

            SelectedTown = cityCenter;
        }

        public ICommand Save { get; set; }

        public SettingsViewModel(MainViewModel mainViewModel)
        {
            Countries = new ObservableCollection<Ulke>();
            Cities = new ObservableCollection<Sehir>();
            Towns = new ObservableCollection<Ilce>();
            namazApi = new NamazApiService();
            Save = new Command(SaveSettings);
            this.mainViewModel = mainViewModel;

            var settings = LocalSettings.GetCurrent();
            CurrentLocation = GetLocation(settings);
            AlertUser = settings.AlertUser;
            Interval = settings.Interval;
            LoadCurrents(settings.AbsolutePlace);
        }

        private string GetLocation(LocalSettings settings)
        {
            return settings.AbsolutePlace.Country.UlkeAdi + " > " + settings.AbsolutePlace.City.SehirAdi + " > " + settings.AbsolutePlace.Town.IlceAdi;
        }

        public void LoadCurrents(AbsolutePlace place)
        {
            var countries = namazApi.GetCountries();
            countries.ForEach(c => Countries.Add(c));

            if (!string.IsNullOrWhiteSpace(place.Country?.UlkeID)
                && Countries.Any())
            {
                var selectedCnt = Countries.FirstOrDefault(c => c.UlkeID == place.Country?.UlkeID);
                SelectedCountry = selectedCnt;

                var cities = namazApi.GetCities(selectedCnt.UlkeID);
                cities.ForEach(c => Cities.Add(c));

                if (!string.IsNullOrWhiteSpace(place.City?.SehirID))
                {
                    var selectedCity = Cities.FirstOrDefault(c => c.SehirID == place.City?.SehirID);
                    SelectedCity = selectedCity;

                    var towns = namazApi.GetTowns(selectedCity.SehirID);
                    towns.ForEach(i => Towns.Add(i));
                    if (!string.IsNullOrWhiteSpace(place.Town?.IlceID))
                    {
                        var selectedTown = Towns.FirstOrDefault(t => t.IlceID == place.Town?.IlceID);
                        SelectedTown = SelectedTown;
                    }
                    else
                    {
                        var cityCenter = towns.FirstOrDefault(t => t.IlceAdi == selectedCity.SehirAdi);
                        SelectedTown = cityCenter;
                    }
                }
            }
        }

        private async void SaveSettings(object obj)
        {
            var oldSettings = LocalSettings.GetCurrent();
            AbsolutePlace absoluteplace = null;
            if (SelectedTown == null || SelectedCity == null || SelectedCountry == null)
                absoluteplace = oldSettings.AbsolutePlace;
            else
                absoluteplace = new AbsolutePlace
                {
                    City = SelectedCity,
                    Country = SelectedCountry,
                    Town = SelectedTown
                };               
             
            var newSettings = new LocalSettings
            {
                AbsolutePlace=absoluteplace,
                AlertUser=AlertUser,
                Interval=Interval
            };

            

            if (GetLocation( oldSettings) != GetLocation(newSettings))
            {
                mainViewModel.DeletePrayerTimes();
            }

            LocalSettings.SaveSettings(newSettings);

            var navigation = App.Current.MainPage as NavigationPage;

            mainViewModel.ReStart();

            await navigation.PopAsync();
        }


        private string _currentLocation;
        public string CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                if (_currentLocation != value)
                {
                    _currentLocation = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _interval;
        public int Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                if (_interval != value)
                {
                    _interval = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _alertUser;
        public bool AlertUser
        {
            get
            {
                return _alertUser;
            }
            set
            {
                if (_alertUser != value)
                {
                    _alertUser = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Ulke _selectedCountry;
        public Ulke SelectedCountry
        {
            get
            {
                return _selectedCountry;
            }
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Sehir _selectedCity;
        public Sehir SelectedCity
        {
            get
            {
                return _selectedCity;
            }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Ilce _selectedTown;
        public Ilce SelectedTown
        {
            get
            {
                return _selectedTown;
            }
            set
            {
                if (_selectedTown != value)
                {
                    _selectedTown = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
