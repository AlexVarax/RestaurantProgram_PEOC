using Microsoft.AspNetCore.Builder;
using Marten;
using Microsoft.Extensions.Configuration;

namespace PEOC_Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ComClients comClients = new ComClients();

            //comClients.ReconnectingÑlients();

            var builder = WebApplication.CreateBuilder();

            var ConnectString = builder.Configuration.GetConnectionString("DefaultConnection");
            ConnectString = "Server=localhost;Port=15001;Database=peoc-db;User Id=postgres;Password=12345678;Include Error Detail=true";

            builder.Services.AddMarten(options =>
            {
                options.Connection(ConnectString);
            });
            var app = builder.Build();


            MessageBox.Show(ConnectString.ToString());
        }
    }
}
