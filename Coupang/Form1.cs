using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coupang.Controls;
using Coupang.Restaurants_Controls;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using Newtonsoft.Json;
using System.Net;

namespace Coupang
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.storeList.CheckBoxCheckedChanged += StoreList_SelectedValueChanged;
            Refresh_Timer.Start();
            Sync_Timer.Start();
        }

        private void StoreList_SelectedValueChanged(object sender, EventArgs e)
        {
            ShowSelectedStoreInfo();
            GetOrder(Order_Type.COMPLETED, FromDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"), ToDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
        }

        public void ShowSelectedStoreInfo()
        {
            int count = 0;
            for (int i = 1; i < storeList.CheckBoxItems.Count; i++)
            {
                if (storeList.CheckBoxItems[i].Checked) { count++; }
            }
            if (count == storeList.CheckBoxItems.Count - 1 || count == 0)
            {
                for (int j = 1; j < storeList.CheckBoxItems.Count; j++)
                {
                    storeList.CheckBoxItems[j].Checked = true;
                }
            }
            else
            {
                storeList.Items[0] = string.Format("{0}개매장 선택됨", count.ToString());
            }

        }

        //public string Store_Id;
        public string Account_Id;

        private void Form1_Load(object sender, EventArgs e)
        {
            //Get_stores();
            //Order_Notification test = new Order_Notification() {  orderId = "test" };
            //test.Show();
        }

        #region Store Control 
        public void Get_stores()
        {

            this.tabControl1.Enabled = false;

            Thread th = new Thread(new ThreadStart(() =>
            {
                Task<IRestResponse> tx = Task.Run(() => Helper_Class.Send_Request("https://pos-api.coupang.com/api/v2/auth/verify", Method.GET));

                tx.Wait();
                if (this.IsDisposed == false)
                {
                    this.Invoke(new Action(() => {
                        this.Restaurants.Controls.Clear();

                        if (!string.IsNullOrEmpty(tx.Result.Content))
                        {
                            JToken o = Helper_Class.Json_Responce(tx.Result.Content.ToString());

                            if (tx.Result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                if (o.SelectToken("code").ToString() == "SUCCESS")
                                {
                                    Account_Id = o["content"]["accountId"].ToString();
                                    foreach (JToken x in o["content"]["verifiedStoreList"])
                                    {

                                        Restaurants.Controls.Add(new Stores_Item()
                                        {
                                            R_ID = x.SelectToken("storeId").ToString(),
                                            R_name = x.SelectToken("storeName").ToString(),
                                            Size = new Size(Restaurants.Width - 27, 0)

                                        });
                                        string tempStoreName = x.SelectToken("storeName").ToString();
                                        if(tempStoreName.Length > 6)
                                        {
                                            tempStoreName = tempStoreName.Substring(0,5) + "...";
                                        }
                                        Stores_Item list = new Stores_Item()
                                        {
                                            R_ID = x.SelectToken("storeId").ToString(),
                                            R_name = tempStoreName
                                        };
                                        storeList.Items.Add(tempStoreName);
                                    }
                                    if (Restaurants.Controls.Count > 0)
                                    {
                                        Stores_Availabilities_Status();
                                        this.tabControl1.Enabled = true;
                                    }


                                    Connection_Helper.Web_Socket_Client();

                                }

                                Login_btn.Text = "로그아웃";
                            }
                            else
                            {
                                MessageBox.Show(this, o.SelectToken("message").ToString(), o.SelectToken("code").ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                                Login_frm frm = new Login_frm();
                                frm.ShowDialog(this);
                                Login_btn.Text = "로그인";
                            }

                            this.Status.Text = tx.Result.StatusDescription.ToString();
                        }
                        
                        Status.Text = tx.Result.ResponseStatus.ToString();
                        ShowSelectedStoreInfo();
                    }));
                }

            }));
            th.Start();
        }

        public void Stores_Availabilities_Status()
        {

            Thread th = new Thread(new ThreadStart(() =>
            {

                foreach (Stores_Item r in this.Restaurants.Controls)
                {
                    var QueryParameters = new List<Tuple<string, string>>();



                    QueryParameters.Add(new Tuple<string, string>("storeIds", r.R_ID));
                    QueryParameters.Add(new Tuple<string, string>("accountId", this.Account_Id));

                    Task<IRestResponse> tx = Task.Run(() => Helper_Class.Send_Request($"https://pos-api.coupang.com/api/v1/stores/availability?", Method.GET, QueryParameters));

                    tx.Wait();

                    if (!string.IsNullOrEmpty(tx.Result.Content))
                    {
                        JToken o = Helper_Class.Json_Responce(tx.Result.Content.ToString());
                        if (tx.Result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if (o["code"].ToString() == "SUCCESS")
                            {
                                if (o["content"].First()["storeId"].ToString() == r.R_ID)
                                {

                                    if (this.IsDisposed == false)
                                    {
                                        r.Invoke(new Action(() =>
                                        {
                                            r.R_status = o["content"].First()["openStatus"].ToString();
                                            r.openStatusText = o["content"].First()["openStatusText"].ToString();
                                            r._displayItemDTO = o["content"].First()["displayItemDTO"]["text"].ToString();
                                        }));

                                    }

                                }
                            }
                        }

                    }
                }



            }));
            th.Start();


        }


        #endregion

        #region Order Control
        public enum Order_Type { PENDING, PROCESSING, COMPLETED }


        public void GetOrder(Order_Type _order_type, string start_date = null, string end_date = null)
        {
            string storeIDs = string.Empty;
            if (_order_type == Order_Type.COMPLETED)
            {
                int count = 0;
                for (int i = 1; i < storeList.CheckBoxItems.Count; i++)
                {
                    if (storeList.CheckBoxItems[i].Checked == true)
                    {
                        if (count == 0)
                        {
                            storeIDs = ((Stores_Item)this.Restaurants.Controls[i - 1]).R_ID;
                        }
                        else
                        {
                            storeIDs += "," + ((Stores_Item)this.Restaurants.Controls[i - 1]).R_ID;
                        }
                        count++;
                    }
                }
            }
            else
            {
                int count = 0;
                foreach (Stores_Item r in this.Restaurants.Controls)
                {
                    if (count == 0)
                    {
                        storeIDs = r.R_ID;
                    }
                    else
                    {
                        storeIDs += "," + r.R_ID;
                    }
                    count++;
                }
            }

            var QueryParameters = new List<Tuple<string, string>>();

            QueryParameters.Add(new Tuple<string, string>("storeIds", storeIDs));
            QueryParameters.Add(new Tuple<string, string>("status", _order_type.ToString()));





            if (start_date != null && end_date != null)
            {
                QueryParameters.Add(new Tuple<string, string>("startDate", start_date));
                QueryParameters.Add(new Tuple<string, string>("endDate", end_date));
            }

            //MessageBox.Show(QueryParameters.ToString());

            Thread th = new Thread(new ThreadStart(() =>
            {
                Task<IRestResponse> tx = Task.Run(() => Helper_Class.Send_Request("https://pos-api.coupang.com/api/v2/stores/orders?", Method.GET, QueryParameters));
                //Task<IRestResponse> tx = Getrestaurants().RunSynchronously();
                tx.Wait();
                this.Invoke(new Action(() => {
                    //this.dataGridView1.Rows.Clear();
                    //MessageBox.Show(tx.Result.Content.ToString());

                    if (!string.IsNullOrEmpty(tx.Result.Content))
                    {


                        JToken o = Helper_Class.Json_Responce(tx.Result.Content.ToString());

                        //MessageBox.Show (tx.Result.Content.ToString());


                        if (tx.Result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if (o.SelectToken("code").ToString() == "SUCCESS")
                            {
                                if (_order_type == Order_Type.COMPLETED)
                                {
                                    Order_Details_pan.Controls.Clear();
                                }
                                else if (_order_type == Order_Type.PROCESSING)
                                {
                                    In_Progress_pan.Controls.Clear();
                                }
                                else if (_order_type == Order_Type.PENDING)
                                {
                                    New_Orders_pan.Controls.Clear();
                                }



                                foreach (JToken x in o["content"]["content"])
                                {
                                    Order_Item o_item = new Order_Item();
                                    o_item.Click += (ss, ee) => { Get_Order_Details(ss, ee, o_item.StoreID, o_item.orderId); };
                                    o_item.complete1.Click += (ss, ee) => { Packaging_Complete_Notify(ss, ee, o_item.StoreID, o_item.orderId); };
                                    o_item.complete2.Click += (ss, ee) => { CompleteDelivery(ss, ee, o_item.StoreID, o_item.orderId); };
                                    List<string> options = new List<string>();
                                    string menutext = "";
                                    JToken menus = x["items"];
                                    int count = 0;
                                    foreach (var menu in menus)
                                    {
                                        count++;
                                        foreach (var item in menu["itemOptions"])
                                        {
                                            options.Add(item["optionName"].ToString());
                                        }

                                        menutext += string.Format("{0}원·{1}({2})*{3}\n", menu["unitSalePrice"].ToString(), menu["name"].ToString(), string.Join(", ", options), menu["quantity"].ToString());
                                    }
                                    o_item.menulist.Text = string.Format("[메뉴 {0}개] ", count) + menutext;
                                    if (o_item.menulist.Text.Length > 40)
                                    {
                                        o_item.menulist.Text = o_item.menulist.Text.Substring(0, 35) + "..." + o_item.menulist.Text.Substring(o_item.menulist.Text.Length - 5, 5);
                                    }

                                    o_item.StoreID = x["storeId"].ToString();
                                    o_item.orderId = x["orderId"].ToString();

                                    o_item.abbrOrderId.Text = x["abbrOrderId"] != null ? x["abbrOrderId"].ToString() : "...";
                                    o_item.Order_Time.Text = x["orderedAt"]["dateTime"] != null ? Helper_Class.From_Unix_Timestamp(double.Parse(x["orderedAt"]["dateTime"].ToString())).ToString("hh:mm tt") : "...";
                                    o_item.note.Text = x["note"] != null ? x["note"].ToString() : "...";
                                    o_item.Size = new Size(Order_Details_pan.Width - 24, o_item.Height);

                                    o_item.O_status.Text = x["status"] != null ? x["status"].ToString() : "...";
                                    o_item.O_orderServiceType.Text = x["orderServiceType"] != null ? x["orderServiceType"].ToString() : "...";

                                    o_item.remainingTime.Text = (Math.Abs(int.Parse(x["state"]["preparationRemainingTime"].ToString())) / 60).ToString();

                                    if (x["orderServiceType"].ToString() == "DELIVERY")
                                    {
                                        o_item.label1.Text = "배정중";
                                        o_item.statusText.Text = "매장 이동중";
                                        o_item.label4.Text = "배달 파트너";
                                        o_item.complete2.Visible = false;
                                        o_item.complete1.Text = "준비완료";
                       
                                        if(x["state"]["statusText"].ToString() == "Ready")
                                        {
                                            o_item.complete1.Enabled = false;
                                            o_item.complete1.Text = "배달준비완료";
                                            o_item.remainingTime.Text = "0";
                                        }
                                        if (x["state"]["courierStatus"].ToString() == "COURIER_ASSIGNING")
                                        {
                                            o_item.label4.Visible = false;
                                            o_item.pickupTime.Visible = false;
                                            o_item.label6.Visible = false;
                                        }
                                        else if (x["state"]["courierStatus"].ToString() == "COURIER_ACCEPTED" || x["state"]["statusText"].ToString() == "Ready")
                                        {
                                            o_item.orderAssignStatus.Checked = true;
                                            o_item.label1.Text = "배정완료";
                                        }
                                        else if (x["state"]["courierStatus"].ToString() == "COURIER_ARRIVED")
                                        {
                                            o_item.orderAssignStatus.Checked = true;
                                            o_item.label1.Text = "배정완료";
                                            o_item.orderPrepareStatus.Checked = true;
                                            o_item.statusText.Text = "매장도착";
                                            o_item.remainingTime.Text = "0";
                                        }
                                    }
                                    else
                                    {

                                        if (x["state"]["statusText"].ToString() == "Assigned" || x["state"]["statusText"].ToString() == "Ready")
                                        {
                                            o_item.orderAssignStatus.Checked = true;
                                        }
                                        if (x["state"]["statusText"].ToString() == "Ready")
                                        {
                                            o_item.orderPrepareStatus.Checked = true;
                                            o_item.statusText.Text = "포장완료알림";
                                            o_item.remainingTime.Text = "0";
                                            o_item.complete1.Enabled = false;
                                        }
                                    }

                                    o_item.pickupTime.Text = x["state"]["estimatedPickUpTime"].ToString();
                                    if(int.Parse(x["state"]["preparationRemainingTime"].ToString()) >= 0)
                                    {
                                        if(int.Parse(x["state"]["preparationRemainingTime"].ToString())/60 < 4)
                                        {
                                            o_item.remainingTime.ForeColor = Color.Red;
                                        }
                                        else o_item.remainType.ForeColor = Color.Black;
                                        o_item.remainType.Text = "분 남음";
                                        
                                    }
                                    else
                                    {
                                        o_item.remainType.Text = "분 지연됨";
                                        o_item.remainType.ForeColor = Color.Red;
                                    }

                                    //o_item.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
                                    switch (o_item.O_status.Text)
                                    {
                                        case "COMPLETED":
                                            o_item.O_status.ForeColor = Color.MediumSpringGreen;
                                            break;

                                        case "CANCELLED":
                                            o_item.O_status.ForeColor = Color.DarkOrange;
                                            break;
                                    }
                                    if (_order_type == Order_Type.COMPLETED)
                                    {
                                        ShowAdditionalInfo(o_item, false);
                                        Order_Details_pan.Controls.Add(o_item);
                                    }
                                    else if (_order_type == Order_Type.PROCESSING)
                                    {
                                        ShowAdditionalInfo(o_item, true);
                                        In_Progress_pan.Controls.Add(o_item);    
                                    }
                                    else if (_order_type == Order_Type.PENDING)
                                    {
                                        ShowAdditionalInfo(o_item, false);
                                        New_Orders_pan.Controls.Add(o_item);
                                    }

                                }
                            }

                        }




                        this.Status.Text = tx.Result.StatusDescription.ToString();
                    }


                    //this.button3.Enabled = true;

                }));

            }));
            th.Start();


            //string test = string.Empty;
            //Thread thx = new Thread(new ThreadStart(() =>
            //{
            //    Task<IRestResponse> tx = Task.Run(() => Helper_Class.Send_Request($"https://api-gateway.coupang.com/v2/providers/openapi/apis/api/v4/vendors/218497/ordersheets/17JP00/history", Method.GET, null));
            //    //Task<IRestResponse> tx = Getrestaurants().RunSynchronously();
            //    tx.Wait();
            //    this.Invoke(new Action(() =>
            //    {
            //        test = tx.Result.Content;
            //    }));

                   

            //}));
            //thx.Start();


        }

        private void CompleteDelivery(object ss, EventArgs ee, string store_ID, string orderId)
        {
            Thread th = new Thread(new ThreadStart(() =>
            {
                Task<IRestResponse> tx = Task.Run(() => Helper_Class.Send_Request($"https://pos-api.coupang.com/api/v1/stores/{store_ID}/orders/{orderId}/customer-pickup/", Method.POST, null, "{}"));
                //Task<IRestResponse> tx = Getrestaurants().RunSynchronously();
                tx.Wait();
                if (!string.IsNullOrEmpty(tx.Result.Content))
                {
                    JToken o = Helper_Class.Json_Responce(tx.Result.Content.ToString());


                    if (tx.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (o.SelectToken("code").ToString() == "SUCCESS")
                        {
                            MessageBox.Show("Delivery completed successfully!");
                            GetOrder(Order_Type.PROCESSING);
                        }
                    }
                }

            }));
            th.Start();
        }

        private void Packaging_Complete_Notify(object ss, EventArgs ee, string store_ID, string orderId)
        {
            Thread th = new Thread(new ThreadStart(() =>
            {
                Task<IRestResponse> tx = Task.Run(() => Helper_Class.Send_Request($"https://pos-api.coupang.com/api/v1/stores/{store_ID}/orders/{orderId}/ready/", Method.POST, null, "{}"));
                //Task<IRestResponse> tx = Getrestaurants().RunSynchronously();
                tx.Wait();
                if (!string.IsNullOrEmpty(tx.Result.Content))
                {
                    JToken o = Helper_Class.Json_Responce(tx.Result.Content.ToString());


                    if (tx.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (o.SelectToken("code").ToString() == "SUCCESS")
                        {
                            MessageBox.Show("Successfully notified");
                            GetOrder(Order_Type.PROCESSING);
                        }
                    }
                }

            }));
            th.Start();
        }

        public void ShowAdditionalInfo(Order_Item item, bool state)
        {
            item.label1.Visible = state;
            item.orderAssignStatus.Visible = state;
            item.label2.Visible = state;
            item.statusText.Visible = state;
            item.label7.Visible = state;
            item.orderPrepareStatus.Visible = state;
            item.remainingTime.Visible = state;
            item.remainType.Visible = state;
            if(!state)
            {
                item.complete1.Visible = state;
                item.complete2.Visible = state;
                item.label4.Visible = state;
                item.label6.Visible = state;
                item.pickupTime.Visible = state;
            }  
        }


        public void Get_Order_Details(object sender, EventArgs e, string StoreID, string orderId)
        {

            Thread th = new Thread(new ThreadStart(() =>
            {
                Task<IRestResponse> tx = Task.Run(() => Helper_Class.Send_Request($"https://pos-api.coupang.com/api/v1/stores/{StoreID}/orders/{orderId}", Method.GET));
                Task<IRestResponse> tx1 = Task.Run(() => Helper_Class.Send_Request($"https://pos-api.coupang.com/api/v1/safe-number/{StoreID}/orders/{orderId}/customer", Method.GET));
                Task<IRestResponse> tx2 = Task.Run(() => Helper_Class.Send_Request($"https://pos-api.coupang.com/api/v1/safe-number/{StoreID}/orders/{orderId}/courier", Method.GET));
                //Task<IRestResponse> tx = Getrestaurants().RunSynchronously();
                tx.Wait();
                tx1.Wait();
                tx2.Wait();
                this.Invoke(new Action(() => {

                    if (!string.IsNullOrEmpty(tx.Result.Content))
                    {


                        JToken o = Helper_Class.Json_Responce(tx.Result.Content.ToString());

                        //MessageBox.Show (tx.Result.Content.ToString());


                        if (tx.Result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            Order_Viewer viewer = new Order_Viewer();
                            if (tx1.Result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                JToken r = Helper_Class.Json_Responce(tx1.Result.Content.ToString());
                                try
                                {
                                    viewer.customerPhone.Text = r["content"]["safeNumber"].ToString();
                                }
                                catch(Exception)
                                {
                                    viewer.customerPhone.Text = "없음";
                                }
                            }
                            if (tx2.Result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                JToken r = Helper_Class.Json_Responce(tx2.Result.Content.ToString());
                                try
                                {
                                    viewer.riderPhone.Text = r["content"]["safeNumber"].ToString();
                                }
                                catch (Exception) 
                                {
                                    viewer.riderPhone.Text = "없음";
                                }
                            }
                            viewer.Total.Text = o["content"]["salePrice"].ToString();
                            viewer.order_ID = o["content"]["orderId"].ToString();
                            viewer.orderServiceType = o["content"]["orderServiceType"].ToString();
                            viewer.store_ID = o["content"]["storeId"].ToString();
                            viewer.Order_No.Text = o["content"]["abbrOrderId"] != null ? o["content"]["abbrOrderId"].ToString() : "...";

                            viewer.status = o["content"]["status"] != null ? o["content"]["status"].ToString() : "..."; ;
                            viewer.O_status.Text = viewer.status;

                            viewer.customerOrderCount.Text = o["content"]["customerOrderCount"] != null ? o["content"]["customerOrderCount"].ToString() : "...";

                            viewer.Order_Time.Text = o["content"]["orderedAt"]["dateTime"] != null ? Helper_Class.From_Unix_Timestamp(double.Parse(o["content"]["orderedAt"]["dateTime"].ToString())).ToString("hh:mm tt") : "...";
                            int quantity = 0;
                            foreach (JToken I in o["content"]["items"])
                            {
                                FlowLayoutPanel Items = new FlowLayoutPanel();
                                Items.FlowDirection = FlowDirection.TopDown;
                                Items.MinimumSize = new Size(viewer.Order_Items.Width - 27, 25);


                                Panel pn = new Panel() { Size = new Size(Items.Width, 25) };
                                Label I_name = new Label() { Text = I["name"].ToString(), Location = new Point(16, 0), Width = 300 };
                                Label I_quantity = new Label() { Text = I["quantity"].ToString(), Location = new Point(350, 0) };
                                Label I_subTotalPrice = new Label() { TextAlign = ContentAlignment.MiddleRight, Text = I["unitSalePrice"].ToString(), Location = new Point(466, 0) };

                                pn.Controls.Add(I_name);
                                pn.Controls.Add(I_quantity);
                                pn.Controls.Add(I_subTotalPrice);

                                Items.Controls.Add(pn);

                                foreach (JToken I_options in I["itemOptions"])
                                {
                                    //MessageBox.Show("op");
                                    Panel pn1 = new Panel() { Size = new Size(Items.Width, 25) };
                                    Label I_optionName = new Label() { Text = I_options["optionName"].ToString(), Location = new Point(30, 0), Width = 300};
                                    Label I_optionQuantity = new Label() { Text = I_options["optionQuantity"].ToString(), Location = new Point(350, 0) };
                                    Label I_optionPrice = new Label() { TextAlign = ContentAlignment.MiddleRight, Text = "+" + I_options["optionPrice"].ToString(), Location = new Point(466, 0) };


                                    pn1.Controls.Add(I_optionName);
                                    pn1.Controls.Add(I_optionQuantity);
                                    pn1.Controls.Add(I_optionPrice);

                                    Items.Controls.Add(pn1);
                                }

                                Items.AutoSize = true;
                                Items.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                                viewer.Order_Items.Controls.Add(Items);
                                quantity++;
                            }
                            viewer.quantity.Text = quantity.ToString();
                            viewer.Show((Form1)Application.OpenForms["Form1"]);
                        }

                    }

                }));

            }));
            th.Start();

        }

        #endregion





        private void timer1_Tick(object sender, EventArgs e)
        {
            //this.button3.Enabled = false;
            //FromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);
            //ToDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1);


            //FromDate.Value = DateTime.Now.AddDays(-1);
            //ToDate.Value = DateTime.Now.AddDays(1);
            //MessageBox.Show(Connection_Helper.Connection_Socket.State.ToString());
            if (Connection_Helper.Connection_Socket.State == System.Net.WebSockets.WebSocketState.Open)
            {
                //MessageBox.Show(Connection_Helper.Connection_Socket.State.ToString());
                //Thread th = new Thread(new ThreadStart( () =>
                //{


                string message = "[\"SEND\\ndestination:/app/ping/stores\\ncontent-length:14\\n\\nsend HeartBeat\\u0000\"]";
                //MessageBox.Show(message);
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                Connection_Helper.Connection_Socket.SendAsync(new ArraySegment<byte>(messageBytes), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);


                if (this.tabControl1.SelectedTab.Text == "접수대기")
                {
                    GetOrder(Order_Type.PENDING);
                }


                //}));
                //th.Start();
            }
            else
            {
                Socket_Timer.Enabled = false;
            }

            //GetOrder((string)f);
            //GetOrder();


        }







        private void Login_Button_Click(object sender, EventArgs e)
        {


            if (Login_btn.Text == "로그인")
            {
                Login_frm frm = new Login_frm();
                frm.ShowDialog(this);

            }
            else
            {
                Thread th = new Thread(new ThreadStart(() => {
                    Task<IRestResponse> tx = Task.Run(() => Helper_Class.Send_Request("https://pos-api.coupang.com/api/v1/sign-out", Method.POST));
                    tx.Wait();

                    JToken o = Helper_Class.Json_Responce(tx.Result.Content.ToString());
                    if (!string.IsNullOrEmpty(tx.Result.Content) && o.SelectToken("code").ToString() == "SUCCESS")
                    {

                        if (File.Exists(Application.StartupPath + "\\Cookies.json") == true)
                        {
                            File.Delete(Application.StartupPath + "\\Cookies.json");
                        }
                        Cookies_Class.Cookies = null;

                        this.Invoke(new Action(async () =>
                        {
                            Login_btn.Text = "로그인";
                            Restaurants.Controls.Clear();
                            Order_Details_pan.Controls.Clear();
                            Socket_Timer.Enabled = false;

                            if (Connection_Helper.Connection_Socket.State == System.Net.WebSockets.WebSocketState.Open)
                            {


                                await Connection_Helper.Connection_Socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(
                                     "[\"DISCONNECT\\nreceipt: close - 2\\n\\n\\u0000\"]"
                                    )), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);

                                await Connection_Helper.Connection_Socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(
                                             "(3000) Go away!"
                                            )), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);

                            }

                            Application.Restart();

                        }));
                    }


                }));

                if (MessageBox.Show("정말 로그아웃 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    th.Start();
                }

            }
        }





        private void button7_Click(object sender, EventArgs e)
        {
            GetOrder(Order_Type.COMPLETED, FromDate.Value.ToString("yyyy-MM-dd") + "T00:00:00.000+02:00", ToDate.Value.ToString("yyyy-MM-dd") + "T23:59:59.999+02:00");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Restaurants.Controls.Count > 0)
            {
                switch (this.tabControl1.SelectedTab.Text)
                {
                    case "주문내역":
                        GetOrder(Order_Type.COMPLETED, FromDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"), ToDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
                        //GetOrder(((Stores_Item)this.Restaurants.Controls[0]).R_ID, Order_Type.COMPLETED, "2022-05-27" + "T00:00:00.000+02:00", "2022-05-27" + "T23:59:59.999+02:00");
                        break;

                    case "진행중":
                        GetOrder(Order_Type.PROCESSING);
                        break;

                    case "접수대기":
                        GetOrder(Order_Type.PENDING);
                        break;
                }
            }


        }

        private void FromDate_ValueChanged(object sender, EventArgs e)
        {
            if (this.Restaurants.Controls.Count > 0)
            {
                GetOrder(Order_Type.COMPLETED, FromDate.Value.ToString("yyyy-MM-dd") + "T00:00:00.000+02:00", ToDate.Value.ToString("yyyy-MM-dd") + "T23:59:59.999+02:00");
            }

        }

        private void ToDate_ValueChanged(object sender, EventArgs e)
        {
            if (this.Restaurants.Controls.Count > 0)
            {
                GetOrder(Order_Type.COMPLETED, FromDate.Value.ToString("yyyy-MM-dd") + "T00:00:00.000+02:00", ToDate.Value.ToString("yyyy-MM-dd") + "T23:59:59.999+02:00");
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.Restaurants.Controls.Count > 0)
            {
                FromDate.Value = FromDate.Value.AddDays(+1);
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Restaurants.Controls.Count > 0)
            {
                FromDate.Value = FromDate.Value.AddDays(-1);

            }
        }

        public void RefreshCookie()
        {
            Thread th = new Thread(new ParameterizedThreadStart((object f) =>
            {
                if (Cookies_Class.Cookies.GetCookies(new Uri("https://pos-api.coupang.com"))["device-id"] == null)
                {
                    Cookies_Class.Cookies.Add(new Cookie() { Domain = "coupang.com", Name = "device-id", Value = "3f4cdd2f-66d9-4296-b867-5eff57ae6af8aaa" });
                    Cookies_Class.Cookies.Add(new Cookie() { Domain = "coupang.com", Name = "version", Value = "1.10.2" });
                    Cookies_Class.Cookies.Add(new Cookie() { Domain = "coupang.com", Name = "app-type", Value = "COUPANG_POS" });
                }

                //const string public_key = @"MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAJzE1obe1GiSE6rqaIWjreZ8NmXur3dYgJPths2FDnNtN3Mwgl0ZNDc7RUZwgE4LZf9E2Tf3JmLpRIKXHGXhCyUCAwEAAQ==";


                JObject request_payload = new JObject();
                request_payload.Add("username", Coupang.Program.Global.username);
                request_payload.Add("password", Coupang.Program.Global.password); //Signin_Helper.Password_Genrator(public_key,this.password.Text));
                request_payload.Add("encrypt", false);


                Task<IRestResponse> tx = Task.Run(() => Helper_Class.Send_Request("https://pos-api.coupang.com/api/v2/auth/sign-in/user", Method.POST, null, request_payload.ToString()));
                tx.Wait();

                if (!string.IsNullOrEmpty(tx.Result.Content))
                {
                    StringReader reader = new StringReader(tx.Result.Content.ToString());
                    using (JsonReader jsonReader = new JsonTextReader(reader))
                    {
                        JsonSerializer Deserializer = new JsonSerializer();
                        var o = (JToken)Deserializer.Deserialize<JToken>(jsonReader);

                        if (o.SelectToken("code").ToString() == "SUCCESS")
                        {
                            JObject request_payload2 = new JObject();
                            JArray storeIdsArray = new JArray();

                            // Iterate through the stores
                            foreach (var store in o["content"]["stores"])
                            {
                                // Check if the store is integrated
                                if ((bool)store["integrated"])
                                {
                                    // If integrated, add the store ID to the storeIdsArray
                                    Coupang.Program.Global.isIntegrated = true;
                                    storeIdsArray.Add((int)store["storeId"]);
                                }
                                else
                                {
                                    Coupang.Program.Global.storeID = (int)store["storeId"];
                                }
                            }

                            JObject payload = new JObject();
                            payload["storeIds"] = storeIdsArray;
                            string url = "https://pos-api.coupang.com/api/v2/auth/sign-in/stores";
                            if (!Coupang.Program.Global.isIntegrated)
                            {
                                payload["storeId"] = Coupang.Program.Global.storeID;
                                url = "https://pos-api.coupang.com/api/v2/auth/sign-in/store";
                            }

                            // Create a JObject to hold the payload

                            request_payload2 = JObject.Parse(payload.ToString());
                            // Add the payload dictionary to request_payload2
                            Task<IRestResponse> tx2 = Task.Run(() => Helper_Class.Send_Request(url, Method.POST, null, request_payload2.ToString()));

                            tx2.Wait();

                            //this.Invoke(new Action(() => {
                            //    this.textBox1.Text = tx2.Result.Content.ToString();
                            //    //this.label3.Text = tx2.Result.StatusCode.ToString();

                            //}));

                            if (!string.IsNullOrEmpty(tx2.Result.Content))
                            {

                                StringReader reader2 = new StringReader(tx2.Result.Content.ToString());
                                using (JsonReader jsonReader2 = new JsonTextReader(reader2))
                                {
                                    JsonSerializer Deserializer2 = new JsonSerializer();
                                    var o2 = (JToken)Deserializer2.Deserialize<JToken>(jsonReader2);

                                    if (o2.SelectToken("code").ToString() == "SUCCESS")
                                    {
                                        Cookies_Class.Save_Cookies();
                                        this.Invoke(new Action(() =>
                                        {

                                            Task<IRestResponse> tx3 = Task.Run(() => Helper_Class.Send_Request("https://pos-api.coupang.com/api/v1/meta/cs-info", Method.GET, null, null));
                                            //Task<IRestResponse> tx4 = Task.Run(() => Helper_Class.Send_Request("https://pos-push.coupang.com/push/info?t=1653305147507", Method.GET, null, null));
                                            Get_stores();

                                        }));
                                    }

                                }
                            }
                        }
                        else
                        {
                            this.Invoke(new Action(() => {

                                MessageBox.Show(this, o.SelectToken("message").ToString(), o.SelectToken("code").ToString().Replace("_", " "), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));

                        }
                    }
                }
            }));
            th.Start();
        }

        private void Refresh_Timer_Tick(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(Coupang.Program.Global.username) && !string.IsNullOrEmpty(Coupang.Program.Global.password))
            {
                RefreshCookie();
            }        
        }

        private void Sync_Timer_Tick(object sender, EventArgs e)
        {
            GetOrder(Order_Type.PROCESSING);
        }
    }
}
