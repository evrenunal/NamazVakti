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

        public ICommand Save { get; set; }

        public SettingsViewModel()
        {
            Countries = new ObservableCollection<Ulke>();
            Cities = new ObservableCollection<Sehir>();
            Towns = new ObservableCollection<Ilce>();
            namazApi = new NamazApiService();
            Save = new Command(SaveSettings);

            var currentSettings = LocalSettings.GetCurrent();
            LoadCurrents(currentSettings);
        }

        private void LoadCurrents(LocalSettings currentSettings)
        {
            var countries = namazApi.GetCountries();
            countries.ForEach(c => Countries.Add(c));

            if (!string.IsNullOrWhiteSpace(currentSettings.AbsolutePlace.Country?.UlkeID))
            {
                var selectedCnt = Countries.FirstOrDefault(c => c.UlkeID == currentSettings.AbsolutePlace.Country?.UlkeID);
                SelectedCountry = selectedCnt;

                var cities = namazApi.GetCities(selectedCnt.UlkeID);
                cities.ForEach(c => Cities.Add(c));
                if (!string.IsNullOrWhiteSpace(currentSettings.AbsolutePlace.City?.SehirID))
                {
                    var selectedCity = Cities.FirstOrDefault(c => c.SehirID == currentSettings.AbsolutePlace.City?.SehirID);
                    SelectedCity = selectedCity;

                    var towns = namazApi.GetTowns(selectedCity.SehirID);
                    towns.ForEach(i => Towns.Add(i));
                    if (!string.IsNullOrWhiteSpace(currentSettings.AbsolutePlace.Town?.IlceID))
                    {
                        var selectedTown = Towns.FirstOrDefault(t => t.IlceID == currentSettings.AbsolutePlace.Town?.IlceID);

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

            if (SelectedTown == null || SelectedCity == null || SelectedCountry == null)
                return;
             
            var settings = new LocalSettings
            {
                AbsolutePlace=new AbsolutePlace
                {
                    City=SelectedCity,
                    Country=SelectedCountry,
                    Town=SelectedTown
                },
                AlertUser=AlertUser,
                Interval=Interval
            };

            App.Current.Properties["settings"] = settings;

          await  App.Current.SavePropertiesAsync();
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
