using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace DataGrid
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loadGird();
        }
        SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=quanLyThuVien;Integrated Security=True");

        private bool IsMaximize = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void exit_btn_Click(object sender, RoutedEventArgs e)
        {
            Login lg = new Login();
            this.Close();
            lg.Show();
        }

        private void dataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DataRowView row_selected = dataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {   
                id_txt.Text = row_selected.Row["MaThe"].ToString();
                name_txt.Text = row_selected.Row["TenDocGia"].ToString();
                birthday_txt.Text = row_selected.Row["NgaySinh"].ToString();
                address_txt.Text = row_selected.Row["DiaChi"].ToString();
                if (row_selected.Row["GioiTinh"].ToString() == "Nam")
                {
                    male.IsChecked = true;
                }
                else
                {
                    female.IsChecked = true;
                }
                phone_txt.Text = row_selected.Row["SDT"].ToString();
                dateBegin_txt.Text = row_selected.Row["NgayCap"].ToString();
                dateEnd_txt.Text = row_selected.Row["NgayHetHan"].ToString();
            }
            else
                clearData();
        }
      
        public void loadGird()
        {
            
            SqlCommand cmd = new SqlCommand("select * from DOCGIA", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            dataGrid.ItemsSource = dt.DefaultView;
        }

        public void clearData()
        {
            id_txt.Clear();
            name_txt.Clear();
            birthday_txt.Clear();
            address_txt.Clear();
            phone_txt.Clear();
            dateBegin_txt.Clear();
            dateEnd_txt.Clear();
            male.IsChecked = false;
            female.IsChecked = false;
        }

        public bool isValid()
        {
            if (id_txt.Text == string.Empty)
            {
                MessageBox.Show("CHƯA ĐIỂN ID", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (name_txt.Text == string.Empty)
            {
                MessageBox.Show("CHƯA ĐIỀN TÊN ĐỘC GIẢ", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (birthday_txt.Text == string.Empty)
            {
                MessageBox.Show("CHƯA ĐIỀN NGÀY SINH", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (address_txt.Text == string.Empty)
            {
                MessageBox.Show("CHƯA ĐIỀN ĐỊA CHỈ", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (phone_txt.Text == string.Empty)
            {
                MessageBox.Show("CHƯA ĐIỀN SỐ ĐIỆN THOẠI", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (dateBegin_txt.Text == string.Empty)
            {
                MessageBox.Show("CHƯA ĐIỀN NGÀY CẤP", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (dateEnd_txt.Text == string.Empty)
            {
                MessageBox.Show("CHƯA ĐIỀN NGÀY HẾT HẠN", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (male.IsChecked == false && female.IsChecked == false)
            {
                MessageBox.Show("CHƯA CHỌN GIỚI TÍNH", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (isValid())
            {           
                {

                    SqlCommand cmd = new SqlCommand("INSERT INTO DOCGIA VALUES (@MaThe, @TenDocGia, @NgaySinh, @GioiTinh, @DiaChi, @SDT, @NgayCap, @NgayHetHan)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@MaThe", id_txt.Text);
                    cmd.Parameters.AddWithValue("@TenDocGia", name_txt.Text);
                    cmd.Parameters.AddWithValue("@NgaySinh", birthday_txt.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", address_txt.Text);
                    if (male.IsChecked == true)
                        cmd.Parameters.AddWithValue("@GioiTinh", "Nam");
                    if (female.IsChecked == true)
                        cmd.Parameters.AddWithValue("@GioiTinh", "Nữ");
                    cmd.Parameters.AddWithValue("@SDT", phone_txt.Text);
                    cmd.Parameters.AddWithValue("@NgayCap", dateBegin_txt.Text);
                    cmd.Parameters.AddWithValue("@NgayHetHan", dateEnd_txt.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();                       
                    con.Close();
                    loadGird();
                    MessageBox.Show("THÊM THÀNH CÔNG", "ĐÃ LƯU", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData();
                }
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM DOCGIA WHERE MaThe = " + id_txt.Text, con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("ĐÃ XÓA THÀNH CÔNG", "ĐÃ LƯU", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                loadGird();
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }         
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE DOCGIA SET TenDocGia = @TenDocGia, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, Diachi = @DiaChi,SDT = @SDT,NgayCap = @NgayCap,NgayHetHan = @NgayHetHan WHERE MaThe = " + id_txt.Text, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("TenDocGia", name_txt.Text);
                cmd.Parameters.AddWithValue("NgaySinh", birthday_txt.Text);
                if (male.IsChecked == true)
                    cmd.Parameters.AddWithValue("@GioiTinh", "Nam");
                if (female.IsChecked == true)
                    cmd.Parameters.AddWithValue("@GioiTinh", "Nữ");
                cmd.Parameters.AddWithValue("Diachi", address_txt.Text);
                cmd.Parameters.AddWithValue("SDT", phone_txt.Text);
                cmd.Parameters.AddWithValue("NgayCap", dateBegin_txt.Text);
                cmd.Parameters.AddWithValue("NgayHetHan", dateEnd_txt.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                loadGird();
                MessageBox.Show("SỬA THÀNH CÔNG", "ĐÃ LƯU", MessageBoxButton.OK, MessageBoxImage.Information);
                clearData();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}