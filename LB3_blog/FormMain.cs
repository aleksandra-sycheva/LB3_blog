
using LB3_blog.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace LB3_blog
{
    public partial class FormMain : Form
    {
        // Создается экземпляр контекста данных, который будет использоваться 
        // для загрузки и отслеживания изменений о пользователях
        private BlogContext? db;
        public FormMain()
        {
            InitializeComponent();
        }

        // Метод OnLoad вызывается при загрузке формы
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.db = new BlogContext();

            // Метод Load расширения используется для загрузки всех пользователей из базы данных в BlogContext базу данных
            this.db.Users.Load();
            this.dataGridViewUsers.DataSource = db.Users.Local.ToBindingList();
        }

        // Метод OnClosing вызываетяс при закрытии формы
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this.db?.Dispose();
            this.db = null;
        }

        private void DataGridViewUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (this.db != null)
            {
                var user = (User)this.dataGridViewUsers.CurrentRow.DataBoundItem;

                if (user != null)
                {
                    this.db.Entry(user).Collection(e => e.Posts).Load();
                    this.dataGridViewPosts.DataSource = user.Posts;
                }
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            // Этот код вызывает SaveChanges BlogContext объект,
            // который сохраняет все изменения, внесенные в базу данных
            this.db!.SaveChanges();

            this.dataGridViewUsers.Refresh();
            this.dataGridViewPosts.Refresh();
        }
    }
}
