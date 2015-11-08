<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="bankprov.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta charset="utf-8" />
            <title>JE-Banken - Kompetensportal</title>

            <link rel ="stylesheet" href="StyleSheet.css" />
                    <link rel="stylesheet" media="screen and (max-width: 900px)" href="responsiv.css" />

        </head>
    <body>
       <div class="container">
            <div class="header">
                <img src="jebanken.jpg" alt="JE-Banken logo" />
            </div>

            <div class="nav">
                <ul class="clearfix">
                    <asp:Label ID="LabelInloggad" runat="server" Text=""></asp:Label>
                    <li><a href="#">Logga ut</a></li>
                </ul>
            </div>
           
            <form id ="form1" runat="server">
                <div class="sektioner clearfix">
                    <h1>Kompetensportal</h1>
                    <div class="infotext">
                             <asp:Label ID="LabelKompetensportal" runat="server" Text="Välkommen till JE-Bankens kompetensportal. Här kan du enkelt svara på frågor om Volvobilar, skidåkning och Bamsetidningar och på ett snabbt och smidigt sätt erhålla ett bevis på din kompetens inom dessa områden."></asp:Label>                                 
                    </div>


                    <div class="sektion">
                        <div class="sektioncentrera">
                            <asp:Button ID="btnGorProv" runat="server" Text="Gör Provet" onclick="btnGorProv_Click" />
                            <asp:Label ID="LabelEjInloggad" runat="server" Text="Label"></asp:Label>
                            <br />
                            <asp:Label ID="Labelfornam" runat="server" Text="Förnamn på den anställda"></asp:Label>
                            <asp:TextBox ID="TextBoxanvandare" runat="server" Height="16px" Width="76px"></asp:TextBox>
                        </div>
                    </div>

                    <asp:Repeater ID="Repeater1" runat="server">

                        <HeaderTemplate>
                            <h3>Fyll i efterfrågat antal svar per fråga</h3>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <div class="poster">
                                <div class="fraga">
                                    <table>
                                        <tr>
                                            <td>
                                                <p><%# Eval("fragestallning") %></p> 
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>                                               
                                                 <asp:Label ID="LabelInfo" runat="server"><%#Eval ("Info") %></asp:Label>  
                                            </td>
                                        </tr>
                                    </table>
                                        </div>
                                    <div class="svarsalternativ">                        
                                        <table>
                                            <tr>
                                                <td><asp:CheckBox ID="CheckBoxA" runat="server" /><asp:Label ID="LabelA" runat="server"><%#Eval ("svarsalternativa") %></asp:Label></td>
                                                <td><asp:CheckBox ID="CheckBoxB" runat="server" /><asp:Label ID="LabelB" runat="server"><%#Eval ("svarsalternativb") %></asp:Label></td>
                                            </tr>
                                             <tr>
                                                <td><asp:CheckBox ID="CheckBoxC" runat="server" /><asp:Label ID="LabelC" runat="server"><%#Eval ("svarsalternativc") %></asp:Label></td>                            
                                                <td><asp:CheckBox ID="CheckBoxD" runat="server" /><asp:Label ID="LabelD" runat="server"><%#Eval ("svarsalternativd") %></asp:Label></td>
                                            </tr>                                        
                                        </table>
                                    </div> 
                                </div>
                        </ItemTemplate>

                        <FooterTemplate></FooterTemplate>

                    </asp:Repeater>

        
                    <div class="sektion">
                        
                        <div class="svarsalternativ">
                           

                        </div>
                        <div class="info"></div>
                        <div class="sektioncentrera sektionprovlamnain">                            
                            <asp:Button ID="btnSeResultat" runat="server" Text="Se dina tidigare resultat" />
                            <asp:Button ID="btnLamnain" runat="server" Text="Lämna in"  onclick="btnLamnain_Click" />

                        </div>
                    </div>

                    <div class="sektion">
                        <div class="sektioncentrera">                            
                            <asp:Button ID="btnSeResultatAnstallda" runat="server" Text="Se anställdas resultat" />
                        </div>
                    </div>

                </div>
             </form>

            <div class="footer">
                <table>
                    <tr>
                        <td>
                            <p>
                                JE-Banken
                                <br />
                                Rikemansgatan 4b
                                <br />
                                54 678 Östersund
                            </p>
                        </td>
                    </tr>          
                </table>       
            </div>

            </div>
    </body>
</html>
