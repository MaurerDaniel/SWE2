using System;
using System.Collections.ObjectModel;
using System.Linq;

using App1.Core.Models;
using App1.Core.Services;
using App1.Helpers;

using GalaSoft.MvvmLight;

using Windows.UI.Xaml.Navigation;

namespace App1.ViewModels
{
    public class ImageGalleryDetailViewModel : ViewModelBase
    {
        private object _selectedImage;
        private ObservableCollection<PictureModel> _source;

        public object SelectedImage
        {
            get => _selectedImage;
            set
            {
                Set(ref _selectedImage, value);
                ImagesNavigationHelper.UpdateImageId(ImageGalleryViewModel.ImageGallerySelectedIdKey, ((PictureModel)SelectedImage).Id);
            }
        }

        public ObservableCollection<PictureModel> Source
        {
            get => _source;
            set => Set(ref _source, value);
        }

        public ImageGalleryDetailViewModel()
        {
            // TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGalleryTempData();
        }

        public void Initialize(string selectedImageID, NavigationMode navigationMode)
        {
            if (!string.IsNullOrEmpty(selectedImageID) && navigationMode == NavigationMode.New)
            {
                SelectedImage = Source.FirstOrDefault(i => i.Id == selectedImageID);
            }
            else
            {
                selectedImageID = ImagesNavigationHelper.GetImageId(ImageGalleryViewModel.ImageGallerySelectedIdKey);
                if (!string.IsNullOrEmpty(selectedImageID))
                {
                    SelectedImage = Source.FirstOrDefault(i => i.Id == selectedImageID);
                }
            }
        }
    }
}
