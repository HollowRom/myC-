using System.Windows.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace macWindowTest
{
    public partial class FormMain : Form
    {
 
        public FormMain(ArrayList arrip_list)
        {
            InitializeComponent();
 
            this.listView1.View = View.Details;
            this.listView1.SmallImageList = this.imageList1;
 
            this.listView1.Columns.Add("列标题1", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("列标题2", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("列标题3", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("列标题4", 100, HorizontalAlignment.Left);
 
            
            this.listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
            for (int i = 0; i < imageList1.Images.Count; i++)   //添加10行数据
            {
                ListViewItem lvi = new ListViewItem();
 
                lvi.ImageIndex = i;     //通过与imageList绑定，显示imageList中第i项图标
                lvi.Text = "item" + i;
                lvi.SubItems.Add("第2列,第" + i + "行");
                lvi.SubItems.Add("第3列,第" + i + "行");
 
                this.listView1.Items.Add(lvi);
            }
 
            this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            
        }
 
        //Detail
        private void button1_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();  //先移除所有的项，避免重复添加
 
            this.listView1.View = View.Details;
            this.listView1.LargeImageList = this.imageList1;
 
            this.listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
            for (int i = 0; i < imageList1.Images.Count; i++)   //添加10行数据
            {
                ListViewItem lvi = new ListViewItem();
 
                lvi.ImageIndex = i;     //通过与imageList绑定，显示imageList中第i项图标
                lvi.Text = "item" + i;
                lvi.SubItems.Add("第2列,第" + i + "行");
                lvi.SubItems.Add("第3列,第" + i + "行");
 
                this.listView1.Items.Add(lvi);
            }
 
            this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。
        }
 
        //Large
        private void button2_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();  //先移除所有的项，避免重复添加
 
            this.listView1.View = View.LargeIcon;
            this.listView1.LargeImageList = this.imageList1;
 
            this.listView1.BeginUpdate();
            for (int i = 0; i < imageList1.Images.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = i;
                lvi.Text = "item" + i;
                this.listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate();
        }
 
        //Small
        private void button3_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();  //先移除所有的项，避免重复添加
 
            this.listView1.View = View.SmallIcon;
            this.listView1.SmallImageList = this.imageList1;
 
            this.listView1.BeginUpdate();
            for (int i = 0; i < imageList1.Images.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = i;
                lvi.Text = "item" + i;
                this.listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate();
        }
 
        //List
        private void button4_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();  //先移除所有的项，避免重复添加
 
            this.listView1.View = View.List;
            this.listView1.SmallImageList = this.imageList1;
 
            this.listView1.BeginUpdate();
            for (int i = 0; i < imageList1.Images.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = i;
                lvi.Text = "item" + i;
                this.listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate();
        } 
 
        private void button5_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();  //先移除所有的项，避免重复添加
 
            ListViewGroup man_group = new ListViewGroup();  //创建男生分组
            man_group.Header = "男生";  //设置组的标题。
            //man_group.Name = "man";   //设置组的名称。
            man_group.HeaderAlignment = HorizontalAlignment.Left;   //设置组标题文本的对齐方式。（默认为Left）
            ListViewGroup women_group = new ListViewGroup();  //创建女生分组
            women_group.Header = "女生";
 
            //women_group.Name = "women";
            women_group.HeaderAlignment = HorizontalAlignment.Center;   //组标题居中对齐
            this.listView1.Groups.Add(man_group);    //把男生分组添加到listview中
            this.listView1.Groups.Add(women_group);   //把男生分组添加到listview中 
            this.listView1.ShowGroups = true;  //记得要设置ShowGroups属性为true（默认是false），否则显示不出分组
 
            for (int i = 0; i < imageList1.Images.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
 
                lvi.ImageIndex = i;
                lvi.Text = "item"+i;
                lvi.ForeColor = Color.Blue;  //设置行颜色
                lvi.SubItems.Add("第2列,第"+i+"行");
                lvi.SubItems.Add("第3列,第"+i+"行");
 
                //前五项给man分组，后五项给women分组
                if (i < imageList1.Images.Count / 2)
                    man_group.Items.Add(lvi);   //分组添加子项
                else
                    women_group.Items.Add(lvi);
 
                this.listView1.Items.Add(lvi);
            }
        }
 
        private void button6_Click(object sender, EventArgs e)
        {
            //显示选中内容
            if (listView1.SelectedItems.Count != 0)
            {
                //清空原来的内容
                textBox1.Text = "";
 
                //SelectedItem是可以多选的，单选的话下标取0即可
                //多选的话，索引是从0开始
                for (int i = 0; i < listView1.SelectedItems[0].SubItems.Count; i++)
                {
                    textBox1.Text += listView1.SelectedItems[0].SubItems[i].Text;
                    textBox1.Text += ",";
                }
            }
            else
            {
                MessageBox.Show("未选中任何项");
            }
        }
    }
}