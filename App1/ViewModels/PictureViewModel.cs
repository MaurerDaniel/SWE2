using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1.Core.Models;
using Windows.Storage;
using Windows.Storage.FileProperties;
using System.Collections.ObjectModel;

namespace App1.ViewModels
{
    class PictureViewModel : ViewModelBase
    {


        public PictureViewModel()
        {

        }

        //private void SaveImageProperties

        private async void GetImageProperties(StorageFile imageFile)
        {
            ImageProperties props = await imageFile.Properties.GetImagePropertiesAsync();
            //ILogger log = (ILogger)obj;

            //TODO implement Logging
            string title = props.Title;
            try
            {
                if (title == null)
                {
                    throw new Exception("Format does not support, or image does not contain Title property");
                }

                DateTimeOffset date = props.DateTaken;
                if (date == null)
                {
                    throw new Exception("Format does not support, or image does not contain DateTaken property");
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
