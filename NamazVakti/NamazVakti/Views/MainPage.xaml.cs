﻿using NamazVakti.Services;
using NamazVakti.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NamazVakti.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        public MainViewModel viewModel;

        public MainPage ()
		{          
            viewModel = new MainViewModel();
            BindingContext = viewModel;

            InitializeComponent();         
        }
    }
}