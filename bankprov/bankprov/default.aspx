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
                            <h3><asp:Literal ID="LiteralKategori" runat="server"></asp:Literal></h3>
                            <asp:Button ID="btnGorProv" runat="server" Text="Gör Provet" onclick="btnGorProv_Click" />
                            <asp:Label ID="LabelEjInloggad" runat="server" Text="Label"></asp:Label>
                        </div>
                    </div>
        
                    <div class="sektion sektionfraga">
                        <div class="fraga">
                            <asp:Literal ID="LiteralFraga" runat="server"></asp:Literal>
                        </div>
                        <div class="svarsalternativ">
                            
                            <asp:RadioButton ID="RadioButtonA" runat="server" GroupName="radio" /><asp:Label ID="LabelA" runat="server" Text="Label"></asp:Label>
                            <asp:RadioButton ID="RadioButtonB" runat="server" GroupName="radio" /><asp:Label ID="LabelB" runat="server" Text="Label"></asp:Label>
                            <br />
                            <asp:RadioButton ID="RadioButtonC" runat="server" GroupName="radio" /><asp:Label ID="LabelC" runat="server" Text="Label"></asp:Label>
                            <asp:RadioButton ID="RadioButtonD" runat="server" GroupName="radio" /><asp:Label ID="LabelD" runat="server" Text="Label"></asp:Label>
                        </div>
                        <div class="info"></div>
                        <div class="sektioncentrera">                            
                            <asp:Button ID="btnSeResultat" runat="server" Text="Se dina tidigare resultat" />
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
