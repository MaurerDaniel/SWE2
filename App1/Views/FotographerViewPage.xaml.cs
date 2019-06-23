using System;

using App1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace App1.Views
{
    public sealed partial class FotographerViewPage : Page
    {
        private FotographerViewViewModel ViewModel
        {
            get { return ViewModelLocator.Current.FotographerViewViewModel; }
        }

        public FotographerViewPage()
        {
            InitializeComponent();
        }
    }
}
