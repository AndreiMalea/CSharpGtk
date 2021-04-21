using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace Lab1CSharp.GUI
{
    class MainWindow : Window
    {
        [UI] private Label _label1 = null;
        [UI] private Button _button1 = null;
        [UI] private TreeView tree = null;

        private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _button1.Clicked += Button1_Clicked;
            
            // Create a column for the artist name
            var cellView = new CellRendererText ();
            TreeViewColumn artistColumn = new TreeViewColumn ("Artist", cellView);
            artistColumn.AddAttribute(cellView, "text",0);
            
            // Create a column for the song title
            var cellView1 = new CellRendererText ();
            TreeViewColumn songColumn = new TreeViewColumn ("Song Title", cellView1);
            songColumn.AddAttribute(cellView1, "text",1);
 
            // Add the columns to the TreeView
            tree.AppendColumn(artistColumn);
            
            tree.AppendColumn (songColumn);
 
            // Create a model that will hold two strings - Artist Name and Song Title
            ListStore musicListStore = new ListStore (typeof (string), typeof (string));
            // Assign the model to the TreeView
            tree.Model = musicListStore;

            musicListStore.InsertWithValues(0, 678, 876);
            musicListStore.InsertWithValues(0, 567, 765.567);
            musicListStore.InsertWithValues(0, 456, 654.456);
            musicListStore.InsertWithValues(0, 345, 543.345);
            musicListStore.InsertWithValues(0, 234, 432.234);
            musicListStore.InsertWithValues(0, 123, 321.123);

            tree.CursorChanged += OnCursorChanged;
        }

        private void OnCursorChanged(object sender, EventArgs a)
        {
            TreeSelection sel = tree.Selection;

            ITreeModel model;
            TreeIter iter;

            var x =sel.GetSelectedRows();
            Console.WriteLine(x.Length);
            
            if (sel.GetSelected(out model, out iter))
            {
                Console.WriteLine(model.GetValue(iter, 0)+" "+model.GetValue(iter,1));
            }
        }
        
        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            // Application.Quit();
            // this.Hide();
        }

        private void Button1_Clicked(object sender, EventArgs a)
        {
            _counter++;
            _label1.Text = "Hello World! This button has been clicked " + _counter + " time(s).";
        }

        
    }
}