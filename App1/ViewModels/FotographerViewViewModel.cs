using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using App1.Core.Models;
using App1.Core.Services;
using App1.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Microsoft.Toolkit.Uwp.UI.Animations;

namespace App1.ViewModels
{
    public class FotographerViewViewModel : ViewModelBase
    {
        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

        private ICommand _itemClickCommand;

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<SampleOrder>(OnItemClick));

        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return DataService.GetContentGridData();
            }
        }

        public FotographerViewViewModel()
        {
        }

        private void OnItemClick(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate(typeof(FotographerViewDetailViewModel).FullName, clickedItem.OrderId);
            }
        }
    }
}
