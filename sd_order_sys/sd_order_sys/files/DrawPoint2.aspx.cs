﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sd_order_sys.files
{
    public partial class DrawPoint2 : System.Web.UI.Page
    {
        public string hidFloorId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            hidFloorId = Request.QueryString["hidFloorId"];
        }
    }
}