using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using WpfAppUniversityApi.Models;
using System.Net.Http.Headers;


namespace WpfAppUniversityApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();


        public MainWindow()
        {
            client.BaseAddress = new Uri("https://localhost:44355/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
           InitializeComponent();
        }

        private void btnLoadStudents_Click(object sender, RoutedEventArgs e)
        {
            this.GetStudents();
        }

        private async void GetStudents()
        {
            var response = await client.GetStringAsync("students");
            var students =JsonConvert.DeserializeObject<List<Student>>(response);
            dgStudent.DataContext = students;
        }

        private async void SaveStudent(Student student)
        {
            await client.PostAsJsonAsync("student", student);
        }

        private async void UpdateStudent(Student student)
        {
            await client.PutAsJsonAsync("student"+ student.StudentId, student);
        }

        private async void DeleteStudent(int studentId)
        {
            await client.DeleteAsync("student/" + studentId);
        }

        private void btnSaveStudent_Click(object sender, RoutedEventArgs e)
        {
            var student = new Student()
            {
                StudentId = Convert.ToInt32(txtStudentId.Text),
                Name = txtName.Text,
                Roll = txtRoll.Text,
            };
            if(student.StudentId == 0)
            {
                this.SaveStudent(student);
                //MessageBox.Show("Student Saved");
            }
            else
            {
                this.UpdateStudent(student);
               // MessageBox.Show("Student Updated");
            }

            txtStudentId.Text = 0.ToString();
            txtName.Text = "";
            txtRoll.Text = "";
        }

        void btnEditStudent(object sender, RoutedEventArgs e)
        {
            Student student = ((FrameworkElement)sender).DataContext as Student;
            txtStudentId.Text = student.StudentId.ToString();
            txtName.Text = student.Name;
            txtRoll.Text = student.Roll;
        }

        void btnDeleteStudent(object sender, RoutedEventArgs e)
        {
            Student student = ((FrameworkElement)sender).DataContext as Student;
            this.DeleteStudent(student.StudentId);

        }
    }
    
}
