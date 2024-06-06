using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Management.Instrumentation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab3_Plane
{
    public partial class Plane : Form
    {
        public Plane()
        {
            InitializeComponent();
            textBox1_TextChanged(textBox1, null);
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8 && e.KeyChar != 44)
            {
                e.Handled = true;
            }
        }

        //set
        //double m0=77000;//Масса самолёта
        //double S_cr = 124.7; //площадь крыла
        //double x_cr = 25;// стрелковидность крыла по 1/4 хорд градус
        //double lambda_cr = 9.3; // удлинение крыла
        //double m_tcr = 0.35; //Относительная масса топлива в дву половинах крыла
        //double c0_cr = 0.12; //Относительная толщина крыла у борта фюзележа
        //double ck_cr = 0.08;//Относительная толщина крыла в конце крыла
        //double nu_cr = 0.58;// Сужение крыла в плане = b0/bk
        //double lambda_fu = 7.3;// удлинение фюзеляжа // 10
        //double m_gr = 22500; // Найбольшая масса грузов
        //double S_op = 37.4;// площаль оперения
        //double L_msh = 5500; // найбольшая возможная дальность беззпосадочного полёта
        //double H_st = 4.37; //габаритная высота стойки главной опоры шасси
        //double jk = 8; // число всех колес главных стоек шасси
        //double bk = 0.762; //ширина колес в метрах
        //double m_col = 1134; //масса колес
        //double P0i = 122.4; // стартовая тяга одного двигателя без форсажа
        //double m_trd = 6.5; //Степень двухконтурности ТРД
        //double H0=9.144; //начальная высота крейсерского полета км
        //double Hk = 11.582; //конечная высота крейсерского полета км
        //const
        double k1_cr = 1;//ресурс крыла
        double kcy_cr = 1;//1 если двигатели на крыле
        double k2_cr = 1.4;//тип крыла
        double k3_cr = 1.2;//герметезация бака
        bool k1_fu_bool = true; //true двигатели соеденные с крыло и false если на кормовой части самолёта
        double k2_fu = 0.0; // крепление стоек шасси
        double k3_fu = 0.004; //стойки убираються куда
        double k4_fu = 0.0;//каков перевоз багажа
        double k_mt = 0.95; //использования композитотов это 0,85, 1 если нет
        double k_msh_cx = 1.2; //схема главных стоек шасси
        double m_onsh = 0.1; // доля взлётной массы
        double k_sh_p = 1;// коефициент для расчёта числа стоек шасси
        double pika = 12; // степень повышения давления в компрессоре
        double k1_su = 0.95;//расположение крыльев
        double k2_su = 0.049;//тип воздухообразников
        double n_dv = 2;//число двигателей на самолёте
        double n_dv_rev = 2;//двигатели оборудываны реверсной тягой
        double kf_su = 1; // если у двигателей форсажная тяга
        double mt_pr = 0.006;//прочие массы
        double M = 0.95; // число М полёта
        const double G = 9.8;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double m0 = Convert.ToDouble(textBox1.Text); //Масса самолёта
                double S_cr = Convert.ToDouble(textBox2.Text);  //площадь крыла
                double x_cr = Convert.ToDouble(textBox3.Text); // стрелковидность крыла по 1/4 хорд градус
                double lambda_cr = Convert.ToDouble(textBox4.Text);  // удлинение крыла
                double m_tcr = Convert.ToDouble(textBox5.Text);  //Относительная масса топлива в дву половинах крыла
                double c0_cr = Convert.ToDouble(textBox6.Text);  //Относительная толщина крыла у борта фюзележа
                double ck_cr = Convert.ToDouble(textBox7.Text); //Относительная толщина крыла в конце крыла
                double nu_cr = Convert.ToDouble(textBox8.Text); // Сужение крыла в плане = b0/bk
                double lambda_fu = Convert.ToDouble(textBox9.Text); // удлинение фюзеляжа // 10
                double m_gr = Convert.ToDouble(textBox10.Text);  // Найбольшая масса грузов
                double S_op = Convert.ToDouble(textBox11.Text); // площаль оперения
                double L_msh = Convert.ToDouble(textBox12.Text);  // найбольшая возможная дальность беззпосадочного полёта
                double H_st = Convert.ToDouble(textBox13.Text);  //габаритная высота стойки главной опоры шасси
                double jk = Convert.ToDouble(textBox14.Text);  // число всех колес главных стоек шасси
                double bk = Convert.ToDouble(textBox15.Text);  //ширина колес в метрах
                double m_col = Convert.ToDouble(textBox16.Text);  //масса колес
                double P0i = Convert.ToDouble(textBox17.Text);  // стартовая тяга одного двигателя без форсажа
                double m_trd = Convert.ToDouble(textBox18.Text);  //Степень двухконтурности ТРД
                double H0 = Convert.ToDouble(textBox19.Text);  //начальная высота крейсерского полета км
                double Hk = Convert.ToDouble(textBox20.Text);  //конечная высота крейсерского полета км


                //----------------
                double p0_cr = (G * m0) / (10 * S_cr);//6.5
                double p0_op = (G * m0) / (10 * S_op);
                double np = 1.5 + (1685) / p0_cr * ((1 / Math.Cos(x_cr)) + 2 / lambda_cr);//6.51
                if (np < 3.45) { np = 3.45; }
                double phi_cr = 0.92 - 0.5 * m_tcr - 0.1 * kcy_cr; //6.7
                double u_cr = c0_cr / ck_cr;
                double mcr = ((7* k1_cr*np*phi_cr*lambda_cr*Math.Sqrt(m0))/(Math.Pow(10,4)*p0_cr* Math.Pow(c0_cr, 0.75) * Math.Pow(Math.Cos(x_cr), 1.5)))*((nu_cr+4)/(nu_cr+1))*(1-((u_cr-1)/(nu_cr+3)))*((4.5*k2_cr*k3_cr)/(p0_cr))+0.015; //6.5
                double d_fu = 1.52 * Math.Pow(m_gr, 1/3); //6.22
                double k1_fu=0, i_fu, k_p =0;
                if (d_fu > 5) { if (k1_fu_bool) { k1_fu = 3.63 - 0.333 * d_fu; } else { k1_fu = 4.56 - 0.441 * d_fu; } } else if (d_fu < 5) { k1_fu = 3.58-0.278 * d_fu; }
                if (d_fu >= 4) { i_fu = 0.743; } else { i_fu = 0.718; }
                
                double mfu = k1_fu*lambda_fu*Math.Pow(d_fu,2)*Math.Pow(m0, -i_fu)+k2_fu+k3_fu+k3_fu+k4_fu; // 6.13

                if (p0_op > 450) { k_p = 1.0; } else { k_p = 0.84; }
                double k_op_cx = (1.333-0.0032*S_op)/(1.295+0.0028*p0_op);// Для Т образного
                double mop = ((0.85*k_mt)/(m0))*k_p*k_op_cx*Math.Pow(p0_op,0.6)*Math.Pow(S_op, 1.16);//6.25

                double m_rpos = 0.91 * m0 * ((4.0) / (Math.Pow(10, -3) * L_msh + 4) + 0.38);
                if (m_rpos > m0) { m_rpos = m0; }
                double m_sel = H_st * (4.6 * Math.Pow(10, -3) * m_rpos * (1 - (m_onsh)) + 52.5);//6.32
                double m_cel = k_sh_p * (6.52 * Math.Pow(10, -3) * m_rpos * (1 - m_onsh) + 22); //6.33
                double m_osi = (1.44 * Math.Pow(10, -3) * m_rpos * (1 - m_onsh) + 5) * jk * bk;//6.34
                double xz = 0.594 + 0.31 * Math.Pow(10, -5) * m_rpos;// эта формула так как k_sh_p =1
                double msh_n = ((xz * k_msh_cx) / m0) * (m_sel + m_cel);//6.35
                double msh_g = (1/m0)*(0.93-0.64*Math.Pow(10, -6)*m_rpos)*(k_msh_cx*(m_sel+m_cel)+m_osi);//6.31
                double msh = msh_g + msh_n + (m_col/m0);//6.31?
                double mcon = mcr + mfu + mop + msh; //6.1

                op1.Text = "= " + Math.Round(mcon, 3);
                op2.Text = "= " + Math.Round(mcr, 3);
                op3.Text = "= " + Math.Round(np, 3);
                op4.Text = "= " + Math.Round(mfu, 3);
                op5.Text = "= " + Math.Round(mop, 3);
                op6.Text = "= " + Math.Round(k_op_cx, 3);
 
                op7.Text = "= " + Math.Round(msh, 3);
                op8.Text = "= " + Math.Round(msh, 3);
                op9.Text = "= " + Math.Round(msh, 3);
               

                //-----------------------------------

                double yot_dv = ((Math.Pow(10,-4)*Math.Pow(P0i,2)*(1.69+1.4*(pika-3)))/(Math.Pow(1+Math.Pow(m_trd,2),3/4)))+(100/P0i)+0.05*Math.Pow(m_trd,1/3)+0.06; //6.43
                double k_su = k1_su * (1 + 0.1 * n_dv_rev / n_dv) * (kf_su + (k2_su / yot_dv)) * Math.Pow((1.62 + 0.275 * Math.Pow(m_trd, 0.75)), 2);
                double m_su = k_su * yot_dv * P0i;
                op10.Text = "= " + Math.Round(m_su, 3);
                op11.Text = "= " + Math.Round(k_su, 3);
                op12.Text = "= " + Math.Round(yot_dv, 3);
                // ------------------
                double mt_mr = (0.0035 * H0 * (1 - 0.03 * m_trd)) / (1 - 0.004 * H0);
                double mt_cnp = 0.002 * Hk * (1 - 0.023 * Hk) * (1 - 0.003 * m_trd);
                double ci0 = 0.8 / (1 + 0.5 * Math.Sqrt(m_trd));
                double H = H0 / Hk / 2;
                double Cp = ci0 + ((0.4 * M) / (1 + 0.027 * H));
                double P = (P0i * m_trd) / (1 + m_tcr + m_gr / m0) * (1 - 0.035 * H / 1000);
                double K = G * m0 / P;
                double mt_nz = (0.9 * Cp) / (K);
                double mt = mt_mr + mt_cnp + mt_nz+mt_pr;
                op13.Text = "= " + Math.Round(mt, 3);
                op14.Text = "= " + Math.Round(mt_mr, 3);
                op15.Text = "= " + Math.Round(mt_cnp, 3);
                op16.Text = "= " + Math.Round(mt_nz, 10_);
            }
            catch
            {
                Console.WriteLine("Error with parameters");
            }
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }
    }
}
