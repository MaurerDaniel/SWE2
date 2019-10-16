using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

using App1.Core.Models;
using App1.Core.Services;
using App1.Helpers;
using App1.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Microsoft.Toolkit.Uwp.UI.Animations;

using Windows.UI.Xaml.Controls;

namespace App1.ViewModels
{
    public class ImageGalleryViewModel : ViewModelBase
    {
        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

        public const string ImageGallerySelectedIdKey = "ImageGallerySelectedIdKey";

        //ObservableCollection<SampleImage> _source;
        ObservableCollection<PictureModel> _ImageSource = new ObservableCollection<PictureModel>();



        private PictureModel _selectedImg;

        private RelayCommand _editImgCommand;


        public RelayCommand EditImgCommand
        {
            get;
            set;
        }

        public RelayCommand SearchImgCommand
        {
            get;
            set;
        }

        public ICommand FotographersCmd { get; private set; }


        private string _newOrt;
        public string NewOrt
        {
            get
            {
                return _newOrt;
            }
            set
            {
                Set(ref _newOrt, value);
            }
        }

        private string _newLand;
        public string NewLand
        {
            get
            {
                return _newLand;
            }
            set
            {
                Set(ref _newLand, value);
            }
        }

        private string _inputText;

        public string InputText
        {
            get
            {
                return _inputText;
            }
            set
            {
                Set(ref _inputText, value);
            }
        }

        private PictureModel _newImageModel;

        public PictureModel NewImageModel
        {
            get
            {
                return _selectedImg;
            }
            set
            {
                Set(ref _newImageModel, value);
            }
        }
        public PictureModel SelectedImg
        {
            get
            {
                return _selectedImg;
            }
            set
            {
                Set(ref _selectedImg, value);
            }
        }

        public ObservableCollection<PictureModel> ImageSource
        {
            get
            {
                return _ImageSource;
            }
            set
            {
                Set(ref _ImageSource, value);
            }
        }

        private ObservableCollection<PictureModel> _wholeSource = new ObservableCollection<PictureModel>();
        public ObservableCollection<PictureModel> WholeSource
        {
            get
            {
                return _wholeSource;
            }
            set
            {
                Set(ref _wholeSource, value);
            }
        }

        //public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand<ItemClickEventArgs>(OnsItemSelected));

        public ImageGalleryViewModel()
        {
            // TODO WTS: Replace this with actual data from DB
            //ImageSource = SampleDataService.GetGalleryTempData();
            //Source = SampleDataService.GetGallerySampleData();
            LoadData();
            EditImgCommand = new RelayCommand(EditImg);
            SearchImgCommand = new RelayCommand(SearchImage);
            FotographersCmd = new RelayCommand(() => ShowFotographers());


        }

        public void LoadData()
        {
            ImageSource.Clear();
            WholeSource.Clear();

            var fotographers = SampleDataService.GetFotographerData();

            var pics = SampleDataService.GetImageData();
            foreach (var item in pics)
            {
                //Console.WriteLine(item.ToString());

                var a = fotographers.Where(n => n.ID == item.Owner.ID).FirstOrDefault();

                item.Owner = (FotographerModel)a;

                ImageSource.Add(item);
                WholeSource.Add(item);
            }
            if (ImageSource.Count != 0)
            {
                SelectedImg = ImageSource[0];
            }
        }

        public void SearchImage()
        {
            if (InputText != null)
            {
                ImageSource.Clear();

                var res = WholeSource.Where(t =>
                    t.Name.Contains(InputText) ||
                    t.Owner.Name.Contains(InputText) ||
                    t.Owner.Surname.Contains(InputText) ||
                    t.EXIF.Breite.Contains((InputText)) ||
                    t.EXIF.Hoehe.Contains((InputText)) ||
                    t.IPTC.Land.Contains(InputText) ||
                    t.IPTC.Ort.Contains(InputText))
                    .ToList();
                foreach (var item in res)
                {
                    ImageSource.Add(item);
                }
                if (ImageSource.Count != 0)
                {
                    SelectedImg = ImageSource[0];
                }
            }
        }

        public void EditImg()
        {
            int selectedIndex = ImageSource.IndexOf(SelectedImg);

            ImageSource[selectedIndex] = NewImageModel;

            PictureModel newPic = ImageSource[selectedIndex];

            if (!string.IsNullOrWhiteSpace(NewLand) && NewLand != ImageSource[selectedIndex].IPTC.Land)
            {
                newPic.IPTC.Land = NewLand;
                SampleDataService.ChangeIPTC(selectedIndex, newPic);
            }
            if (!string.IsNullOrWhiteSpace(NewOrt) && NewOrt != ImageSource[selectedIndex].IPTC.Ort)
            {
                newPic.IPTC.Ort = NewOrt;
                SampleDataService.ChangeIPTC(selectedIndex, newPic);
            }


            RaisePropertyChanged("SelectedImg");
        }

        public void ShowFotographers()
        {
            //passende Viewmodel erstellen
            FotographerViewViewModel model = new FotographerViewViewModel();
            View.DisplayAddNewView(model);
        }

        //private void OnsItemSelected(ItemClickEventArgs args)
        //{MasterDetailsViewState
        //    var selected = args.ClickedItem as PictureModel;
        //    ImagesNavigationHelper.AddImageId(ImageGallerySelectedIdKey, selected.Id);
        //    NavigationService.Frame.SetListDataItemForNextConnectedAnimation(selected);
        //    NavigationService.Navigate(typeof(ImageGalleryDetailViewModel).FullName, selected.Id);
        //}



        //protected void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //        handler(this, e);
        //}

        //protected void OnPropertyChanged(string propertyName)
        //{
        //    OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
    }
}

