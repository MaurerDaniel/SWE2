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
        ObservableCollection<FotographerModel> _FotographersSource = new ObservableCollection<FotographerModel>();




        private PictureModel _selectedImg;

        private FotographerModel _selectedFotographers;

        private RelayCommand _editImgCommand;


        public RelayCommand EditImgCommand
        {
            get;
            set;
        }

        public RelayCommand EditFotographerCommand
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

        public ICommand AddFotographerCommand { get; private set; }

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

        private string _newName;
        public string NewName
        {
            get
            {
                return _newName;
            }
            set
            {
                Set(ref _newName, value);
            }
        }

        private string _newSurName;
        public string NewSurName
        {
            get
            {
                return _newSurName;
            }
            set
            {
                Set(ref _newSurName, value);
            }
        }

        private string _newDate;
        public string NewDate
        {
            get
            {
                return _newDate;
            }
            set
            {
                Set(ref _newDate, value);
            }
        }

        private string _newNotice;
        public string NewNotice
        {
            get
            {
                return _newNotice;
            }
            set
            {
                Set(ref _newNotice, value);
            }
        }

        private string _allnewName;
        public string AllNewName
        {
            get
            {
                return _allnewName;
            }
            set
            {
                Set(ref _allnewName, value);
            }
        }

        private string _allnewSurName;
        public string AllNewSurName
        {
            get
            {
                return _allnewSurName;
            }
            set
            {
                Set(ref _allnewSurName, value);
            }
        }

        private string _allnewDate;
        public string AllNewDate
        {
            get
            {
                return _allnewDate;
            }
            set
            {
                Set(ref _allnewDate, value);
            }
        }

        private string _allnewNotice;
        public string AllNewNotice
        {
            get
            {
                return _allnewNotice;
            }
            set
            {
                Set(ref _allnewNotice, value);
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

        public FotographerModel SelectedFotographers
        {
            get
            {
                return _selectedFotographers;
            }
            set
            {
                Set(ref _selectedFotographers, value);
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
        public ObservableCollection<FotographerModel> FotographersSource
        {
            get
            {
                return _FotographersSource;
            }
            set
            {
                Set(ref _FotographersSource, value);
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
            EditFotographerCommand = new RelayCommand(EditFotographer);
            AddFotographerCommand = new RelayCommand(AddFotographer);
            SearchImgCommand = new RelayCommand(SearchImage);
            //FotographersCmd = new RelayCommand(() => ShowFotographers());


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

            foreach (var item in fotographers)
            {
                FotographersSource.Add(item);
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

            LoadData();
            
            RaisePropertyChanged("SelectedImg");

        }

        public void EditFotographer()
        {
            int selectedIndex = FotographersSource.IndexOf(SelectedFotographers);

            // FotographersSource[selectedIndex] = NewImageModel;

            FotographerModel newFog = FotographersSource[selectedIndex];

            DateTime date = new DateTime();

            if (NewDate != null)
            {
                date = DateTime.Parse(NewDate);
            }


            if (!string.IsNullOrWhiteSpace(NewSurName) && NewSurName != FotographersSource[selectedIndex].Surname && !string.IsNullOrWhiteSpace(NewName)
                && date != FotographersSource[selectedIndex].Birthday && date < DateTime.Today)
            {
                newFog.Name = NewName;
                newFog.Surname = NewSurName;
                newFog.Birthday = date;
                newFog.Notes = NewNotice;
                SampleDataService.ChangeFog(selectedIndex, newFog);
            }

            LoadData();
            RaisePropertyChanged("FotographersSource");

        }

        public void AddFotographer()
        {
            // int selectedIndex = FotographersSource.IndexOf(SelectedFotographers);

            // FotographersSource[selectedIndex] = NewImageModel;

            FotographerModel newFog = new FotographerModel();

            DateTime date = DateTime.Parse(AllNewDate);


            if (!string.IsNullOrWhiteSpace(AllNewSurName) && !string.IsNullOrWhiteSpace(AllNewName)
                && date < DateTime.Today)
            {
                newFog.Name = AllNewName;
                newFog.Surname = AllNewSurName;
                newFog.Birthday = date;
                newFog.Notes = AllNewNotice;
                SampleDataService.AddFog(newFog);
            }

            LoadData();
            RaisePropertyChanged("FotographersSource");

        }


        //public void ShowFotographers()
        //{
        //    //passende Viewmodel erstellen
        //    FotographerViewViewModel model = new FotographerViewViewModel();
        //    View.DisplayAddNewView(model);
        //}

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

