using System;
using Gtk;
using Lab1CSharp.srv;
using UI = Gtk.Builder.ObjectAttribute;

namespace Lab1CSharp.GUI
{
    public class Login : Window
    {
        [UI] private Label _label = null;
        [UI] private Entry _usernameEntry = null;
        [UI] private Entry _passEntry = null;
        [UI] private Button _loginButton = null;
        [UI] private Calendar _calendar = null;

        private Application app = null;
        private Service srv = null;

        public Service Srv
        {
            get => srv;
            set => srv = value;
        }

        public Application App
        {
            get => app;
            set => app = value;
        }

        private string date;

        public Login(Service srv) : this(new Builder("Login.glade"))
        {
            this.Srv = srv;
        }

        private Login(Builder builder) : base(builder.GetRawOwnedObject("Login"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;

            _label.Text = "LOGIN";

            _usernameEntry.PlaceholderText = "Username";
            _passEntry.PlaceholderText = "Password";
            _passEntry.InvisibleChar = '*';
            _passEntry.Visibility = false;

            _loginButton.Clicked += LoginButtonEvent;
        }

        private void CalendarEvent(object sender, EventArgs e)
        {
            date = _calendar.Day.ToString() + "/" + _calendar.Month.ToString() + "/" + _calendar.Year.ToString();
            Console.WriteLine(date);
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        [UI] private MessageDialog msg = null;
        
        private void LoginButtonEvent(object sender, EventArgs a)
        {
            try
            {
                if (_usernameEntry.Text.Equals("") && _passEntry.Text.Equals(""))
                {
                    throw new Exception("Both fields are empty, you must complete them!");
                } else if (_usernameEntry.Text.Equals("")) {
                    throw new Exception("Username field is empty!");
                } else if (_passEntry.Text.Equals("")) {
                    throw new Exception("Password field is empty!");
                }

                if (srv.Login(_usernameEntry.Text.Trim(), _passEntry.Text.Trim()) == 0)
                {
                    var win = new Client(srv);
                    win.App = this.App;
                    win.LoginWindow = this;
                    win.ShowAll();
                    win.Title = srv.GetEmployeeByUser(_usernameEntry.Text.Trim()).Name;
                    _usernameEntry.Text = "";
                    _passEntry.Text = "";
                    this.Hide();

                }
                else if (srv.Login(_usernameEntry.Text.Trim(), _passEntry.Text.Trim()) == 1)
                {
                    throw new Exception("Incorrect password!");
                }
                else
                {
                    throw new Exception("Username doesn't exist!");
                }
            }
            catch (Exception e)
            {
                msg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.None, e.Message, "");
                // msg.ButtonPressEvent += (o, args) => msg.Hide();
                
                msg.ShowAll();
            }
        }
    }
}