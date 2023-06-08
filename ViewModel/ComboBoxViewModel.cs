using DGSWManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DGSWManager.ViewModel
{
    public class ComboBoxViewModel
    {
        public ObservableCollection<ComboBoxModel> EduOffice { get; set; }
        public ComboBoxViewModel() 
        {
            EduOffice = new ObservableCollection<ComboBoxModel>
            {
                new ComboBoxModel() { EduOfficeName = "서울특별시교육청", EduOfficeCode = "B10"},
                new ComboBoxModel() { EduOfficeName = "부산광역시교육청", EduOfficeCode = "C10"},
                new ComboBoxModel() { EduOfficeName = "대구광역시교육청", EduOfficeCode = "D10"},
                new ComboBoxModel() { EduOfficeName = "인천광역시교육청", EduOfficeCode = "E10"},
                new ComboBoxModel() { EduOfficeName = "광주광역시교육청", EduOfficeCode = "F10"},
                new ComboBoxModel() { EduOfficeName = "대전광역시교육청", EduOfficeCode = "G10"},
                new ComboBoxModel() { EduOfficeName = "울산광역시교육청", EduOfficeCode = "H10"},
                new ComboBoxModel() { EduOfficeName = "세종특별자치시교육청", EduOfficeCode = "I10"},
                new ComboBoxModel() { EduOfficeName = "경기도교육청", EduOfficeCode = "J10"},
                new ComboBoxModel() { EduOfficeName = "강원도교육청", EduOfficeCode = "K10"},
                new ComboBoxModel() { EduOfficeName = "충청북도교육청", EduOfficeCode = "M10"},
                new ComboBoxModel() { EduOfficeName = "충청남도교육청", EduOfficeCode = "N10"},
                new ComboBoxModel() { EduOfficeName = "전라북도교육청", EduOfficeCode = "P10"},
                new ComboBoxModel() { EduOfficeName = "전라남도교육청", EduOfficeCode = "Q10"},
                new ComboBoxModel() { EduOfficeName = "경상북도교육청", EduOfficeCode = "R10"},
                new ComboBoxModel() { EduOfficeName = "경상남도교육청", EduOfficeCode = "S10"},
                new ComboBoxModel() { EduOfficeName = "제주특별자치도교육청", EduOfficeCode = "T10"},
            };
        }
    }
}
