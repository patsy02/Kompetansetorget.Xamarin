﻿using KompetansetorgetXamarin.Controllers;
using KompetansetorgetXamarin.Controls;
using KompetansetorgetXamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KompetansetorgetXamarin.DAL;
using Xamarin.Forms;
using KompetansetorgetXamarin.Utility;

namespace KompetansetorgetXamarin.Views
{

    public partial class CarouselVarsler : BaseCarouselPage
    {
        string defaultLogo = "http://kompetansetorget.uia.no/extension/kompetansetorget/design/kompetansetorget/images/logo-virksomhet.jpg";
        VMVarselSettings LISTINIT = new VMVarselSettings();
        ObservableCollection<Varsel> varsler = new ObservableCollection<Varsel>();
        ICommand refreshCommand;
        string p0title = "Dine varsler";
        string p1title = "Velg fagområder";
        public static bool pullList = true;


        public CarouselVarsler()
        {
            InitializeComponent();
            GetStudentNotificationsPref();
            AddData();
            VarselList.ItemsSource = varsler;
            this.Title = p0title;
            StillingerSettings.ItemsSource = LISTINIT.varslerSettings;
            //OppgaverEmner.ItemsSource = LISTINIT.coursesSettings;
            VarselList.IsPullToRefreshEnabled = true;
            VarselList.IsRefreshing = false;
            VarselList.RefreshCommand = RefreshCommand;
            //stillingSwitch.Toggled += stillingToggle;
            //oppgaveSwitch.Toggled += oppgaveToggle;
            //varselSwitch.Toggled += varselToggle;
            varsler.Add(new Varsel("TEST", "TEST", "http://adila.prosjekt.uia.no/files/2015/02/UiA1.png","asd","asd","www.google.com"));

            PopupMenu();

        }

        void PopupMenu() {
            VarselList.ItemSelected += (sender, e) =>
            {
                Varsel d = (Varsel)e.SelectedItem;
                DisplayAlert("ListView Item select", d.Text + d.Uuid + d.Webpage + " Selected", "Ok");
            };
        }


        void OnClick(object sender, EventArgs e)
        {
            ToolbarItem tbi = (ToolbarItem)sender;
            this.DisplayAlert("Selected!", tbi.Text, "OK");
        }

        void Sorter_OnTapped(object sender, EventArgs e)
        {
            this.DisplayAlert("Selected!", "Fagområder get", "OK");
            bool alphabeticallyFirst = false;
            Sort();
        }

        void SaveSettings(object sender, EventArgs e)
        {
            SaveToggle();
            LISTINIT.SaveSettings();
            LISTINIT.PostToServer();

            this.DisplayAlert("Innstillinger lagret!", "Oppdatér for å få en ny liste.", "OK");
        }

        void Sort()
        {

        }

        void SwipeRight(object sender, EventArgs e)
        {
            ToolbarItem tbi = (ToolbarItem)sender;
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                if (CurrentPage == this.Children[0])
                {
                    this.CurrentPage = this.Children[1];
                }
                else if (CurrentPage == this.Children[1])
                {
                    this.CurrentPage = this.Children[2];
                }
                else if (CurrentPage == this.Children[2])
                {
                    this.CurrentPage = this.Children[3];
                }
            });
        }

        void SwipeLeft(object sender, EventArgs e)
        {
            ToolbarItem tbi = (ToolbarItem)sender;
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                if (CurrentPage == this.Children[3])
                {
                    this.CurrentPage = this.Children[2];
                }
                else if (CurrentPage == this.Children[2])
                {
                    this.CurrentPage = this.Children[1];
                }
                else if (CurrentPage == this.Children[1])
                {
                    this.CurrentPage = this.Children[0];
                }
            });
        }




        /// <summary>
        /// Handles refresh behaviour on button click
        /// </summary>
        void Refresh_OnTapped(object sender, EventArgs e)
        {
            //pullList = true;
            ExcecuteRefreshCommand();
        }

        private ICommand RefreshCommand
        {
            get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExcecuteRefreshCommand())); }
        }

        //private ICommand RefreshNewCommand
        //{
        //    get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExcecuteRefreshCommand())); }
        //}

        /// <summary>
        /// Refreshes the list
        /// </summary>
        async Task ExcecuteRefreshCommand()
        {
            //if (pullList == false)
            //{
            //}
            //else if (pullList == true)
            //{
            SaveToggle();
            LISTINIT.SaveSettings();
            LISTINIT.PostToServer();
            varsler.Clear();
            VarselList.ItemsSource = null;
            AddData();
            VarselList.ItemsSource = varsler;
            VarselList.IsRefreshing = false;
            //}
        }

        //Alters title on carouselpage by contentpage
        public new event EventHandler CurrentPageChanged;
        protected override void OnCurrentPageChanged()
        {
            int prevPage = 0;
            EventHandler changed = CurrentPageChanged;
            if (changed != null)
                changed(this, EventArgs.Empty);
            if (CurrentPage == this.Children[0])
            {
                this.Title = p0title;

                if (prevPage == 1)
                {
                    LISTINIT.SaveSettings();
                    LISTINIT.PostToServer();
                    SaveToggle();
                }
                prevPage = 0;

            }
            else if (CurrentPage == this.Children[1])
            {
                prevPage = 1;
                this.Title = p1title;
            }
            //add in case of more pages
            //else if (CurrentPage == this.Children[2]) {
            //    this.Title = p2title;
            //} else if (CurrentPage == this.Children[3]) {
            //    this.Title = p3title;
            //}
        }

        protected override void OnDisappearing()
        {
            LISTINIT.SaveSettings(); //saves the settings when pressing the up button/leaving the page
            LISTINIT.PostToServer();
            SaveToggle();
        }

        protected override bool OnBackButtonPressed() //behaviour of HARDWARE back button, not the up button.
        {
            //var p0 = this.Children[0];
            //var p1 = this.Children[1];

            if (CurrentPage.SendBackButtonPressed()) return true;

            //if (CurrentPage == p1)
            //{
            //    this.CurrentPage = p0;
            //    listInit.SaveSettings();
            //}
            //else if (CurrentPage == p0)
            //{
            //    return false;
            //}
            //listInit.SaveSettings();
            return true;
        }

        void SwitchToggle(object sender, ToggledEventArgs e)
        {
            SaveToggle();
        }

        void SaveToggle()
        {
            NotificationsController nc = new NotificationsController();
            bool allNotifications = varselSwitch.IsToggled;
            bool jobNotifications = stillingSwitch.IsToggled;
            bool projectNotification = oppgaveSwitch.IsToggled;
            nc.UpdateStudentsNotificationsPref(allNotifications, projectNotification, jobNotifications);
        }

        void GetStudentNotificationsPref()
        {
            DbStudent dbStudent = new DbStudent();
            Student student = dbStudent.GetStudent();
            if (student != null)
            {
                oppgaveSwitch.IsToggled = student.receiveProjectNotifications;
                stillingSwitch.IsToggled = student.receiveJobNotifications;
                varselSwitch.IsToggled = student.receiveNotifications;
            }
            else {
                oppgaveSwitch.IsToggled = false;
                stillingSwitch.IsToggled = false;
                varselSwitch.IsToggled = false;
            }
        }

        public void getFilter()
        { }

        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("ListView Item select", mi.Text + " Selected", "Ok");

            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
            // varsel.Webpage
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");

            if (true)
            {

                //varsler.Remove(varsel);
                DbNotification dbNotification = new DbNotification();
                if ("varsel.Type" == "job")
                {
                    dbNotification.DeleteNotificationBasedOnJob("varsel.uuid");
                }
                else
                {
                    dbNotification.DeleteNotificationBasedOnProject("varsel.Uuid");
                }
            }
        }


        public void AddData()
        {
            //if (pullList == false)
            //{
            //}
            //else if (pullList == true)
            //{
                System.Diagnostics.Debug.WriteLine("ViktorTestView - NotificationsFromDb_OnClicked: Initiated");
                NotificationsController nc = new NotificationsController();
                List<Advert> notifications = nc.GetNotificationList();
                
                System.Diagnostics.Debug.WriteLine(
                    "ViktorTestView - NotificationsFromDb_OnClicked: notifications.Count = " + notifications.Count);



                foreach (var n in notifications)
                {
                    if (n is Job)
                    {
                        Job job = (Job)n;

                        if (job.companies != null && job.companies[0].logo != null)
                        {
                            string logo = job.companies[0].logo;
                            string varselText = "Ny stilling fra " + job.companies[0].name + "!";
                            string published = "Publisert " + DateTimeHandler.MakeDateTimeString(job.published);
                            varsler.Add(new Varsel(varselText, published, logo, job.uuid, "job", job.webpage));
                            // DO spesific Job code
                            //long date = job.expiryDate; // Will work
                            System.Diagnostics.Debug.WriteLine("job.title = " + job.title);
                            System.Diagnostics.Debug.WriteLine("job.companies.logo = " + job.companies[0].logo);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("job.companies = null");
                        }
                        System.Diagnostics.Debug.WriteLine("job.expiryDate = " + job.expiryDate);

                    }
                    else if (n is Project)
                    {
                        // Do spesific Project  code.
                        Project project = (Project)n;
                        if (project.companies != null && project.companies[0].logo != null)
                        {
                            string logo = project.companies[0].logo;
                            string varselText = "Ny oppgave fra " + project.companies[0].name + "!";
                            string published = "Publisert " + DateTimeHandler.MakeDateTimeString(project.published);
                            varsler.Add(new Varsel(varselText, published, logo, project.uuid, "project", project.webpage));

                            System.Diagnostics.Debug.WriteLine("project.title = " + project.title);

                            System.Diagnostics.Debug.WriteLine("project.companies.logo = " + project.companies[0].logo);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("project.companies = null");
                        }
                        System.Diagnostics.Debug.WriteLine("project.companies.logo = " + project.companies[0].logo);
                        System.Diagnostics.Debug.WriteLine("project.published = " + project.published);
                    }
                    if (!Authenticater.Authorized)
                    {
                        GoToLogin();
                    }
                    if (varsler != null)
                    {
                        System.Diagnostics.Debug.WriteLine("GetJobsBasedOnFilter: jobs.Count(): " +
                                                          notifications.Count());
                    }
                //    pullList = false;
                //}
            }
        }
    }
}
