<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="bankprov.index" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta charset="utf-8" />
            <title>JE-Banken - Kompetensportal</title>

            <link rel ="stylesheet" href="StyleSheet.css" />    <!-- css-design ligger i StyleSheet.css-->
                    <link rel="stylesheet" media="screen and (max-width: 900px)" href="responsiv.css" />
            <!-- om vi använder en skärm som är mindre än 900 pixlar så läses designstruktur från responsiv.css -->

        </head>
    <body>
       <div class="container">
            <div class="header">
                <img src="jebanken.jpg" alt="JE-Banken logo" />
            </div>

            <div class="nav">
                <ul class="clearfix">
                    <asp:Label ID="LabelInloggad" runat="server" Text=""></asp:Label>       <!-- Namnet på den som är inloggad skrivs i en label -->
                    <li><a href="#">Logga ut</a></li>
                </ul>
            </div>
           
            <form id ="form1" runat="server">
                <div class="sektioner clearfix">    <!-- klassen sektioner ger vit bakgrundsfärg -->
                    <h1>Kompetensportal</h1>        <!--  h1 är centrerad med en mörkgrå färg -->
                    <div class="infotext">          <!-- klassen infotext fyller 70% av bredden, texten centrerad -->
                                                    <!-- Lite välkomsttext i en label-->
                             <asp:Label ID="LabelKompetensportal" runat="server" Text="Välkommen till JE-Bankens kompetensportal. Här kan du enkelt svara på frågor om Volvobilar, skidåkning och Bamsetidningar och på ett snabbt och smidigt sätt erhålla ett bevis på din kompetens inom dessa områden."></asp:Label>                                 
                    </div>


                    <div class="sektion">       <!-- klassen sektion fyller 90 % av bredden , vit bakgrundsfärg, flyter från vänster -->
                        <div class="sektioncentrera">   <!--  klass sektioncentrera visar innehållet som ett centrerat block med viss marginal -->
                            <asp:Button ID="btnGorProv" runat="server" Text="Gör Provet" onclick="btnGorProv_Click" />  
                            <!-- när man klickar på knappen "Gör provet" så körs metoden btnGorProv_Click i "default.aspx.cs"-->
                            <asp:Button ID="btnStartaprov" runat="server" Text="Starta Provet" onclick="btnStartaprov_Click" />
                            <asp:Label ID="LabelEjInloggad" runat="server" Text="Label"></asp:Label>    <!--  VAD GÖR DENNA??? -->
                            <br />
                            <asp:Label ID="Labelfornam" runat="server" Text="Förnamn på den anställda"></asp:Label>
                            <asp:TextBox ID="TextBoxanvandare" runat="server" Height="16px" Width="76px"></asp:TextBox>
                            <asp:Button ID="btnOk" runat="server" Text="OK" onclick="btnOK_Click" />    <!-- Kör metoden "btnOK_Click i default.aspx.cs-->
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="true" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" ></asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"></asp:ObjectDataSource>
                        </div>
                        
                    </div>


                    <asp:Repeater ID="Repeater1" runat="server">      <%-- Repeater som läser in frågor och ritar upp frågeformuläret. Se metod HamtaFragor() i "default.aspx.cs"--%>


                        <HeaderTemplate>                                        <!-- rubriken i repeatern -->
                        </HeaderTemplate>

                        <ItemTemplate>                                          <!-- De repeterade elementen i repeatern. I detta fall våra frågor med svarsrutor.-->
                            <div class="poster">        <!-- klassen "poster" ger typsnitt, en ram runt varje frågepost, viss marginal och bredd -->
                                <div class="fraga">     <!-- klassen "fraga" ger teckenstorlek och marginaler -->
                                    <table>             <!-- Varje fråga hanteras som en tabell med två rader och en kolumn-->
                                        <tr>
                                            <td>
                                                <p><%# Eval("fragestallning") %></p>    <!-- Eval hämtar upp innehållet i "fragestallning" från klassen fragor.cs. Dvs själva frågan -->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>                                               
                                                 <asp:Label ID="LabelInfo" runat="server"><%#Eval ("Info") %></asp:Label>  <!-- på rad två hämtas info om hur frågan skall besvaras som lagrats i "Info" i klassen "fragor.cs" -->
                                            </td>
                                        </tr>
                                    </table>
                                        </div>
                                    <div class="svarsalternativ">           <!-- klass "svarsalternativ" ger marginal och bredd -->             
                                        <table>                         <!-- Svarsalternativen hanteras som en tabell med två rader och två kolumner-->
                                            <tr>
                                                <td><asp:CheckBox ID="CheckBoxA" runat="server" /><asp:Label ID="LabelA" runat="server"><%#Eval ("svarsalternativa") %></asp:Label></td>
                                                <td><asp:CheckBox ID="CheckBoxB" runat="server" /><asp:Label ID="LabelB" runat="server"><%#Eval ("svarsalternativb") %></asp:Label></td>
                                                <!-- Varje svarsalternativ placeras i egen cell i tabellen med en checkbox och svarsalternativstexten som hämtas upp från klassen "fragor.cs" -->
                                            </tr>
                                             <tr>
                                                <td><asp:CheckBox ID="CheckBoxC" runat="server" /><asp:Label ID="LabelC" runat="server"><%#Eval ("svarsalternativc") %></asp:Label></td>                            
                                                <td><asp:CheckBox ID="CheckBoxD" runat="server" /><asp:Label ID="LabelD" runat="server"><%#Eval ("svarsalternativd") %></asp:Label></td>
                                            </tr>                                        
                                        </table>
                                    </div> 
                                </div>
                        </ItemTemplate>

                        <FooterTemplate></FooterTemplate>      <%--här kan vi lägga till någon avslutande text på repeatern om vi vill--%>        
                    </asp:Repeater>
        
                    <div class="sektion">                       <!-- klassen sektion fyller 90 % av bredden , vit bakgrundsfärg, flyter från vänster -->
                        <div class="svarsalternativ">          <!-- VAD HÄNDER I DEN HÄR DIV'en??? -->     

                        </div>
                        <div class="info"></div>        <!-- VAD HÄNDER I DEN HÄR DIV'en??? -->    
                        <div class="sektioncentrera sektionprovlamnain">         <!--  klass sektioncentrera visar innehållet som ett centrerat block med viss marginal -->  
                                                
                            <asp:Button ID="btnSeResultat" runat="server" Text="Se dina tidigare resultat" OnClick="btnSeResultat_Click" />
                            
                            <asp:Button ID="btnLamnain" runat="server" Text="Lämna in"  onclick="btnLamnain_Click" />   <!-- När man är klar med provet så klickar man påknappen och då körs metoden "btnLamnain_Click" i default.aspx.cs -->

                        </div>
                    </div>

                    <div class="sektion">
                        <div class="sektioncentrera">                        <!--  klass sektioncentrera visar innehållet som ett centrerat block med viss marginal -->            
                            <asp:Button ID="btnSeResultatAnstallda" runat="server" Text="Se anställdas resultat" OnClick="btnSeResultatAnstallda_Click" />
                            <asp:Button ID="btnStart" runat="server" Text="Åter till start" OnClick="btnStart_Click" />
                        </div>
                    </div>

                </div>
             </form>

            <div class="footer">        <!-- Sidfot på websidan -->
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
