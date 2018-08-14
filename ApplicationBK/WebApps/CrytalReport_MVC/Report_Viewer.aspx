<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report_Viewer.aspx.cs" Inherits="WebApps.CrytalReport_MVC.Report_Viewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
<head>
    <title>.:: PATHLAWER ::.</title>
    <style type="text/css">
        .style2 {
            font-family: Verdana;
        }

        .auto-style1 {
            background-color: #ddd;
            color: #000;
            cursor: pointer;
            width: 10px;
        }

        .auto-style2 {
            background-color: #ddd;
            color: #000;
            cursor: pointer;
            width: 19px;
        }
    </style>
</head>
<%--<script src="../crystalreportviewers13/js/crviewer/crv.js"></script>--%>
<script src="../crystalreportviewers13/js/crviewer/crv.js"></script>
<body>
    <form id="Form1" method="post" runat="server">
        <table id="dgMain" cellspacing="0" cellpadding="3" rules="all" width="99%" align="center"
            border="0">
            <tr>
                <td>
                    <table class="discNavText" style="height: 60px" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr class="discNavHiliteRow" valign="top" style="border: 0px;">
                            <td class="discNavHilite" valign="middle" style="background: url(/Content/icons/m01.jpg);" colspan="6" height="2"></td>
                        </tr>
                        <tr class="discNavHiliteRow" valign="top" style="border: 0px;">
                            <td class="discNavHilite" style="background: red" valign="middle" colspan="6" height="36">
                                <span class="style2">.::: KẾT XUẤT ĐƠN :::.</span>
                                <asp:TextBox ID="TextBox1" runat="server" Width="48px" Visible="False"></asp:TextBox>
                                <asp:TextBox ID="TextBox2" runat="server" Width="47px" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="discNavHiliteRow" valign="top" style="border: 0px;">
                            <td class="discNavHilite" valign="middle" style="background: url(/Content/icons/m01.jpg);" colspan="6" height="2"></td>
                        </tr>
                        <tr class="discNavOtherRow" valign="middle">
                            <td class="discNavOther" valign="bottom" width="25%" height="24">ZOOM:&nbsp;
									<asp:DropDownList ID="cboZoom" runat="server" Width="120px" AutoPostBack="True" CssClass="TextNormal" OnSelectedIndexChanged="cboZoom_SelectedIndexChanged">
                                        <asp:ListItem Value="20">20%</asp:ListItem>
                                        <asp:ListItem Value="50">50%</asp:ListItem>
                                        <asp:ListItem Value="75">75%</asp:ListItem>
                                        <asp:ListItem Value="100" Selected="True">100%</asp:ListItem>
                                        <asp:ListItem Value="200">200%</asp:ListItem>
                                    </asp:DropDownList></td>
                            <td class="discNavOther" valign="bottom" width="25%" height="24">KHỔ GIẤY:&nbsp;
									<asp:DropDownList ID="cboOrientation" runat="server" Width="100px" CssClass="TextNormal">
                                        <asp:ListItem Value="1">--DỌC--</asp:ListItem>
                                        <asp:ListItem Value="2">--NGANG--</asp:ListItem>
                                    </asp:DropDownList></td>
                            <td class="discNavOther" height="24" valign="bottom" width="160">
                                <asp:ImageButton ID="cmdFirst" runat="server" BorderStyle="Double" ImageUrl="/Content/icons/undo.gif" OnClick="cmdFirst_Click"></asp:ImageButton>
                                <asp:ImageButton ID="cmdBack" runat="server" BorderStyle="Double" ImageUrl="/Content/icons/GoPrev.gif" OnClick="cmdBack_Click"></asp:ImageButton>
                                <asp:ImageButton ID="cmdNext" runat="server" BorderStyle="Double" ImageUrl="/Content/icons/GoNext.gif" OnClick="cmdNext_Click"></asp:ImageButton>
                                <asp:ImageButton ID="cmdLast" runat="server" BorderStyle="Double" ImageUrl="/Content/icons/redo.gif" OnClick="cmdLast_Click"></asp:ImageButton>
                            </td>
                            <td class="discNavOther" valign="bottom" width="80" height="24">KẾT&nbsp;XUẤT:&nbsp;
                            </td>
                            <td class="discNavOther" valign="bottom" height="24">
                                <asp:ImageButton ID="cmdExportPDF" runat="server" Width="14px" BorderStyle="Double" ImageUrl="/Content/icons/PDF.gif"
                                    Height="14px" OnClick="cmdExportPDF_Click"></asp:ImageButton>&nbsp;</td>
                            <td class="discNavOther" valign="bottom" width="90" height="24">
                                <asp:LinkButton ID="cmdExit" runat="server" OnClick="cmdExit_Click">ĐÓNG</asp:LinkButton></td>
                        </tr>
                    </table>
                    <hr width="98%" style="color: #000000">
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        &nbsp;<CR:CrystalReportViewer ID="rptViewer" runat="server" Width="350px" Height="50px" PageZoomFactor="100" DisplayToolbar="false"
                            SeparatePages="true" bestfixpage="false" DisplayPage="true" align="center"
                            HasRefreshButton="True" Visible="true" HasToggleGroupTreeButton="False" HasDrilldownTabs="False" ToolPanelView="None"></CR:CrystalReportViewer>
                    </p>
                    <p>&nbsp;</p>
                    <p>&nbsp;</p>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
