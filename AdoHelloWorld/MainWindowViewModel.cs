﻿using Microsoft.Data.SqlClient;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdoHelloWorld
{
    
    /// <summary>
    /// INotifyPropertyChanged связывает между собой вьюху, модель и вьюмодель
    /// </summary>
    
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();

        public Person NewPerson { get; set; } = new Person();

        public DelegateCommand OnClickCommand { get; }

        public MainWindowViewModel()
        {
            OnClickCommand = new DelegateCommand(OnClickAsync);
        }

       
        private async void OnClickAsync()
        {
            MessageBox.Show("Click");

            //string connectionString = "Server=PIXEL; Database=AdoExplanationDb; Trusted_Connection=True; Encrypt=false";
            string connectionString = "Server=S-DEV-01; Database=AdoExplanationDb; Trusted_Connection=True; Encrypt=false";
            //string connectionString= "Data Source=S-DEV-01;Initial Catalog=AdoDB;Integrated Security=True;Pooling=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //ожидает в текущем потоке пока выполнится другой поток
                await connection.OpenAsync();

                MessageBox.Show($"Connected: {connection.ClientConnectionId/*id сеанса*/}");

                //SqlCommand command = new SqlCommand($"SELECT * FROM Persons", connection);



                ////получение специфичного класса для считывания данных типо потока
                //using (SqlDataReader reader = await command.ExecuteReaderAsync())
                //{
                //    //Усли есть что считывать то ReadAsync() возвращает true иначе false и выход из блока кода
                //    while (await reader.ReadAsync())
                //    {
                //        var person = new Person
                //        {
                //            Id = reader.GetInt32(0),
                //            Name = reader.GetString(1),
                //            Age = reader.GetInt32(2),
                //            Height = reader.GetDouble(3)
                //        };
                //        People.Add(person);
                //    }
                //}

                //SqlCommand command = new SqlCommand($"CREATE TABLE Users (Id INT PRIMARY KEY IDENTITY, Age INT NOT NULL, Name NVARCHAR(100) NOT NULL)", connection);

                //SqlCommand command = new SqlCommand("INSERT INTO Persons (Name, Age, Height) VALUES ('Владимир', 30, 190.1), ('Елена', 28, 165.5)", connection);

                //SqlCommand command = new SqlCommand("UPDATE Persons SET Age = 40 WHERE Name ='Елена'", connection);

                //SqlCommand command = new SqlCommand("DELETE FROM Persons WHERE Age >= 30", connection);

                //SqlCommand command = new SqlCommand($"INSERT INTO Persons (Name, Age, Height) VALUES ('{NewPerson.Name}', {NewPerson.Age}, 190.1)", connection);

                // Иньекция: x', 20, 20.0); DELETE FROM Persons --

                SqlCommand command = new SqlCommand($"INSERT INTO Persons (Name, Age, Height) VALUES (@name, @age, 190.1)", connection);

                //Создание параметра для имени
                SqlParameter nameParam = new SqlParameter("@name", NewPerson.Name);
                //Добавление параметра к команде
                command.Parameters.Add(nameParam);

                //Создание параметра для возраста
                SqlParameter ageParam = new SqlParameter("@age", NewPerson.Age);
                //Добавление параметра к команде
                command.Parameters.Add(ageParam);

                try
                {

                    /*ExecuteNonQueryAsync() - подходит для операций INSERT, UPDATE, DELETE, CREATE*/
                    //ExecuteNonQueryAsync() возвращает колличество измененных строк
                    var succes = await command.ExecuteNonQueryAsync();

                    if (succes > 0)
                    {
                        MessageBox.Show($"Succes: {succes}");
                    }
                    else
                    {
                        MessageBox.Show("Fail");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                /*ExecuteScalar()/ExecuteScalarAsync(): выполняет sql-выражение и возвращает одно скалярное значение, например, число. 
                 * Подходит для sql-выражения SELECT в паре с одной из встроенных функций SQL, как например, Min, Max, Sum, Count.*/
                //Команда выводит скалярное значение
                //SqlCommand command = new SqlCommand("SELECT MAX(Age) FROM Persons ",connection);

                //var res = await command.ExecuteScalarAsync();

                //MessageBox.Show($"Succes: {res}");

            }
           
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
