using Microsoft.Data.SqlClient;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdoHelloWorld
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();

        public DelegateCommand OnClickCommand { get; }

        public MainWindowViewModel()
        {
            OnClickCommand = new DelegateCommand(OnClickAsync);
        }

        private async void OnClickAsync()
        {
            MessageBox.Show("Click");

            string connectionString = "Server=PIXEL; Database=AdoExplanationDb; Trusted_Connection=True; Encrypt=false";

           using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //ожидает в текущем потоке пока выполнится другой поток
                await connection.OpenAsync();

                MessageBox.Show($"Connected: {connection.ClientConnectionId/*id сеанса*/}");

                SqlCommand command = new SqlCommand("SELECT * FROM Persons", connection);

               

                //получение специфичного класса для считывания данных типо потока
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    //Усли есть что считывать то ReadAsync() возвращает true иначе false и выход из блока кода
                    while (await reader.ReadAsync())
                    {
                        var person = new Person
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Age = reader.GetInt32(2),
                            Height = reader.GetDouble(3)
                        };
                        People.Add(person);
                    }
                }     
            }
           
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
