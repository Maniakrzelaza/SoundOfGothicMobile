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
        public int RecordCoundTotal { get; set; }
        public int PageNumber { get; set; }
        public int RecordsOnPage { get; set; }
        public int FirstRecordOnPageNumber { get; set; }
        public int LastRecordOnPageNumber { get; set; } 
        public int DefaultPageSize { get; set; }
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