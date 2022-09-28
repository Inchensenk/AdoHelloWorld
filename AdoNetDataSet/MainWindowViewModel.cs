using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetDataSet
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public DelegateCommand ClickCommand { get; set; }
        public MainWindowViewModel()
        {
            ClickCommand = new DelegateCommand(OnClick);
        }

        private /*async*/ void OnClick()
        {
            string connectionString = "Server=S-DEV-01; Database=AdoExplanationDb; Trusted_Connection=True; Encrypt=false";

            string sql = "Select * FROM Persons";

            using (SqlConnection connection = new(connectionString))
            {
                //await connection.OpenAsync();

                SqlDataAdapter dataAdapter = new(sql, connection);

                DataSet dataSet = new();

                dataAdapter.Fill(dataSet);
            }
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
