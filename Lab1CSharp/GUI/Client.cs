using System;
using Gdk;
using GLib;
using Gtk;
using Lab1CSharp.Domain;
using Lab1CSharp.srv;
using Application = Gtk.Application;
using DateTime = System.DateTime;
using UI = Gtk.Builder.ObjectAttribute;
using Window = Gtk.Window;

namespace Lab1CSharp.GUI
{
    public class Client : Window
    {
        [UI] private TreeView _tree = null;
        [UI] private Calendar _calendar = null;
        [UI] private SpinButton _spinButton = null;
        [UI] private Button _buttonBuyTicket = null;
        [UI] private Button _buttonLogout = null;
        [UI] private ListStore _showsListStore = null;
        [UI] private CellRendererText cellView0 = null;
        [UI] private CellRendererText cellView1 = null;
        [UI] private CellRendererText cellView2 = null;
        [UI] private CellRendererText cellView3 = null;
        [UI] private TreeViewColumn idColumn = null;
        [UI] private TreeViewColumn artistColumn = null;
        [UI] private TreeViewColumn ticketColumn = null;
        [UI] private TreeViewColumn dateColumn = null;
        [UI] private Entry _clientEntry = null;


        private Login _loginWindow = null;
        private Application app = null;
        private Service srv = null;

        private bool dateSelected = false;
        private DateTime date = DateTime.Today;

        private Show show = null;
        
        public Application App
        {
            get => app;
            set => app = value;
        }

        public Service Srv
        {
            get => srv;
            set => srv = value;
        }

        public Login LoginWindow
        {
            get => _loginWindow;
            set => _loginWindow = value;
        }

        public Client(Service srv) : this(new Builder("Client.glade"))
        {
            this.srv = srv;

            this.ReloadTable();
        }

        private Client(Builder builder) : base(builder.GetRawOwnedObject("Client"))
        {
            
            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;
            _buttonLogout.Clicked += ButtonLogoutOnClicked;
            _buttonBuyTicket.Clicked += ButtonBuyClicked;
            _clientEntry.PlaceholderText = "Client";
            
            this.InitTreeview();
            
            _spinButton.SetIncrements(1.0, 0.0);
            
            _calendar.DaySelectedDoubleClick += (sender, args) =>
            {
                dateSelected = true;
                string month;
                if (_calendar.Month <= 9) month = "0" + (_calendar.Month+1);
                else month = (_calendar.Month+1).ToString();
                string day;
                if (_calendar.Day <= 9) day = "0" + _calendar.Day;
                else day = _calendar.Day.ToString();
                date = DateTime.Parse(_calendar.Year + "-" + month + "-" + day);
                Console.WriteLine(date);
                this.ReloadTable();
            };

            _calendar.NextMonth += (sender, args) => { dateSelected = false; this.ReloadTable(); };
            _calendar.NextYear += (sender, args) => { dateSelected = false; this.ReloadTable(); };
            _calendar.PrevMonth += (sender, args) => { dateSelected = false; this.ReloadTable(); };
            _calendar.PrevYear += (sender, args) => { dateSelected = false; this.ReloadTable(); };

            _tree.Selection.Changed += (sender, args) =>
            {
                var x = _tree.Selection.GetSelectedRows();
                
                if (x.Length!=0)
                {
                    TreeIter z = new TreeIter();
                    var y = _tree.Model.GetIter(out z, x[0]);
                    show = srv.FindOneShow(long.Parse(_showsListStore.GetValue(z, 0).ToString()));
                    if (show.TicketNumber!=0)
                    {
                        _spinButton.SetRange(1.0, show.TicketNumber*1.0);
                        _spinButton.Value = 1.0;
                    }
                    else
                    {
                        _spinButton.SetRange(0.0, 0.0);
                        _spinButton.Value = 0.0;
                    }
                }
                else
                {
                    _spinButton.SetRange(0.0, 0.0);
                    _spinButton.Value = 0.0;
                    show = null;
                }
                Console.WriteLine(show);
            };

            dateSelected = false;
        }

        private void ButtonBuyClicked(object sender, EventArgs e)
        {
            if (show == null)
            {
                var msg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.None,
                    "You first have to select a show!");
                msg.ShowAll();
            }
            else
            {
                if (_clientEntry.Text.Equals(""))
                {
                    var msg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.None,
                        "Client field must be completed!");
                    msg.ShowAll();
                }
                else
                {
                    try
                    {
                        var trans = srv.BuyTicket(show, Convert.ToInt32(_spinButton.Value), _clientEntry.Text);
                        var msg = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.None,
                            "Transaction number is : "+trans.Id +"\nTransaction details : - Client : "+ trans.Client+
                                                                       "\n                      - Ticket number : "+trans.TicketNumber+
                                                                       "\n                      - Date : "+trans.Date);
                        msg.ShowAll();
                        this.ReloadTable();
                    }
                    catch (Exception ex)
                    {
                        var msg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.None,
                            ex.Message);
                        msg.ShowAll();
                    }
                }
                Console.WriteLine(show);
            }
        }

        private void ButtonLogoutOnClicked(object sender, EventArgs e)
        {
            this.Hide();
            _loginWindow.ShowAll();
        }

        private void InitTreeview()
        {
            cellView0 = new CellRendererText();
            idColumn = new TreeViewColumn("ID", cellView0);
            idColumn.AddAttribute(cellView0, "text", 0);
            idColumn.AddAttribute(cellView0, "background-rgba", 4);
            
            cellView1 = new CellRendererText();
            artistColumn = new TreeViewColumn("Artist", cellView1);
            artistColumn.AddAttribute(cellView1, "text", 1);
            artistColumn.AddAttribute(cellView1, "background-rgba", 4);
            
            cellView2 = new CellRendererText();
            ticketColumn = new TreeViewColumn("Ticket", cellView2);
            ticketColumn.AddAttribute(cellView2, "text", 2);
            ticketColumn.AddAttribute(cellView2, "background-rgba", 4);

            cellView3 = new CellRendererText();
            dateColumn = new TreeViewColumn("Date", cellView3);
            dateColumn.AddAttribute(cellView3, "text", 3);
            dateColumn.AddAttribute(cellView3, "background-rgba", 4);

            
            var t = new CellRendererText();
            TreeViewColumn c = new TreeViewColumn("",t);
            c.AddAttribute(t, "background-rgba", 4);

            _tree.AppendColumn(idColumn);
            _tree.AppendColumn(artistColumn);
            _tree.AppendColumn(ticketColumn);
            _tree.AppendColumn(dateColumn);
            _tree.AppendColumn(c);

            _showsListStore = new ListStore(typeof(long), typeof(string), typeof(string), typeof(string), typeof(RGBA));
            _tree.Model = _showsListStore;
        }

        private void ReloadTable()
        {
            if (dateSelected == false)
            {
                _showsListStore.Clear();
                foreach (var allListShow in srv.GetAllListShows())
                {
                    if (allListShow.TicketNumber!=0)
                    {
                        var col = new RGBA();
                        _showsListStore.InsertWithValues(-1, allListShow.Id, 
                            allListShow.Artist.Name,
                            allListShow.TicketNumber.ToString(),
                            allListShow.Date.Day + "/" + allListShow.Date.Month + "/" + allListShow.Date.Year, col);
                    }
                    else
                    {
                        var col = new RGBA();
                        col.Parse("#FF0000");

                        _showsListStore.InsertWithValues(-1, allListShow.Id,
                            allListShow.Artist.Name,
                            allListShow.TicketNumber.ToString(),
                            allListShow.Date.Day + "/" + allListShow.Date.Month + "/" + allListShow.Date.Year, col);
                    }
                }
            }
            else
            {
                _showsListStore.Clear();
                foreach (var allListShow in srv.FilterShowsByDate(date))
                {
                    if (allListShow.TicketNumber!=0)
                    {
                        var col = new RGBA();
                        
                        _showsListStore.InsertWithValues(-1, allListShow.Id,
                            allListShow.Artist.Name,
                            allListShow.TicketNumber.ToString(),
                            allListShow.Date.Day + "/" + allListShow.Date.Month + "/" + allListShow.Date.Year, col);
                    }
                    else
                    {
                        var col = new RGBA();
                        col.Parse("#FF0000");
                        
                        _showsListStore.InsertWithValues(-1, allListShow.Id, 
                            allListShow.Artist.Name,
                            allListShow.TicketNumber.ToString(),
                            allListShow.Date.Day + "/" + allListShow.Date.Month + "/" + allListShow.Date.Year, col);
                    }
                }
            }

            show = null;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
    }
}