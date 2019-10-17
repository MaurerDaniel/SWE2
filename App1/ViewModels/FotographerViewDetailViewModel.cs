using System;
using System.Linq;

using App1.Core.Models;
using App1.Core.Services;

using GalaSoft.MvvmLight;

namespace App1.ViewModels
{
    public class FotographerViewDetailViewModel : ViewModelBase
    {
        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public FotographerViewDetailViewModel()
        {
        }

        public void Initialize(long orderId)
        {
            var data = DataService.GetContentGridData();
            Item = data.First(i => i.OrderId == orderId);
        }
    }
}
