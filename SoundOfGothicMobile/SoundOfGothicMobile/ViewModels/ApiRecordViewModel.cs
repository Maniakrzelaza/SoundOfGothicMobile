using SoundOfGothicMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SoundOfGothicMobile.ViewModels
{
	public class ApiRecordViewModel
    {
        public List<ApiRecord> ApiRecords { get; set; }
        public List<ApiRecord> Data { get; set; }
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; }
        }
        public ApiRecordViewModel()
        {
            ApiRecords = new List<ApiRecord>();
        }
        public ApiRecordViewModel(List<ApiRecord> records)
        {
            ApiRecords = new List<ApiRecord>();
            ApiRecords.Clear();
            ApiRecords.AddRange(records);
        }
    }
}