﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected void Start(object sender,EventArgs e)
        {
            var okService = DependencyService.Get<IOkLocationService>();
            okService.GetAddress("+2348038541905", "Olamide", "James");
        }
    }
}
