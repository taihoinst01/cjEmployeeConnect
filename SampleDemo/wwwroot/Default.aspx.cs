using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSODecodeCJW;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string KeyStr = "CJWKEY";
        string encryptedText = "8evVae2ekt7WtC2umaHAqYVyhf2W9eNA";

        CryptoDotNet cdn = new CryptoDotNet();
        string PlainText = cdn.Decrypt(encryptedText, KeyStr);

        Response.Write(PlainText);

    }
}