<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="bankprov.index" %>

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
                    <li><a href="#">Logga ut</a></li>
                </ul>
            </div>
           
            <form id ="form1" runat="server">
                <div class="sektioner clearfix">
                    <h1>Kompetensportal</h1>
                    <div class="infotext">
                        <p>Välkommen till JE-Bankens kompetensportal. Här kan du enkelt svara på frågor om Volvobilar, skidåkning och Bamsetidningar och på ett snabbt och smidigt sätt erhålla ett bevis på din kompetens inom dessa områden.</p>               
                    </div>
                    <div class="sektion">
                        <div class="sektioncentrera">
                           <asp:Button ID="ButtonPersonal" runat="server" Text="Personal" OnClick="ButtonPersonal_Click" />
                            <asp:TextBox ID="TextBoxanvandare" runat="server" Height="16px" Width="96px"></asp:TextBox>
                        </div>
                    </div>
        
                    <div class="sektion sektion2">
                        <div class="sektioncentrera">
                            <asp:Button ID="ButtonProvledare" runat="server" Text="Provledare" OnClick="ButtonProvledare_Click" />
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
                                <br>
                                Rikemansgatan 4b
                                <br>
                                54 678 Östersund
                            </p>
                        </td>
                    </tr>          
                </table>       
            </div>

            </div>
    </body>
</html>
