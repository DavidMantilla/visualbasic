Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging
Imports System.IO


Public Class Form1
    Dim clic As Boolean
    Dim pic, pic2 As Label
    Dim carpeta As String
    Dim transparencia As Integer
    Dim ARCHIVOS As ObjectModel.ReadOnlyCollection(Of String)
    Dim paginas As New List(Of String)
    Dim flujo As Stream
    Dim escritura As StreamWriter
    Dim lectura As StreamReader
    Private objLista, logo As WMPLib.IWMPPlaylist






    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        objLista = Form2.AxWindowsMediaPlayer1.newPlaylist("lista1", "lista.wpl")
        logo = Form2.AxWindowsMediaPlayer1.newPlaylist("logo", "")


        Label2.Visible = False



        Form2.AxWindowsMediaPlayer1.settings.setMode("AutoRewind", True)


        Dim i As Integer = 0

        clic = False

        Do While Screen.AllScreens.Length > i
            ToolStripComboBox1.Items.Add(Screen.AllScreens(i).DeviceName)
            i += 1
        Loop

        ToolStripComboBox1.SelectedIndex = UBound(Screen.AllScreens)



        Try

            Button2.Enabled = False

          
            Form2.Location = Screen.AllScreens(UBound(Screen.AllScreens)).Bounds.Location

            mostrar()


        Catch ex As Exception

        End Try

        TrackBar2.Visible = False


        Timer2.Enabled = True
        Timer2.Interval = 10
        Timer2.Start()

     


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)



    End Sub

   

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If (Form2.AxWindowsMediaPlayer1.playState) = 3 Then

            Form2.AxWindowsMediaPlayer1.Ctlcontrols.pause()


            Button2.BackgroundImage = My.Resources.Play_1_Normal_icon
        Else
            Form2.AxWindowsMediaPlayer1.Ctlcontrols.play()


            Button2.BackgroundImage = My.Resources.Pause_Hot_icon
        End If








    End Sub

  
    Private Sub ListFiles(ByVal folderPath As String)
        Dim i As Integer = 0
        Dim tipos() As String = {"*.avi", "*.mpg", "*.wmv", "*.mp4"}

        ARCHIVOS = My.Computer.FileSystem.GetFiles(folderPath, FileIO.SearchOption.SearchAllSubDirectories, tipos)
        If ARCHIVOS.Count > 0 Then
            FlowLayoutPanel1.Controls.Clear()
        End If


        For Each fileName As String In ARCHIVOS
            '   ListBox1.Items.Add()





           


            objLista.appendItem(Form2.AxWindowsMediaPlayer1.newMedia(fileName))




            ' Controls.Add(txt)
        Next
    End Sub
    Private Sub picClick(sender As Object, e As EventArgs)
        Dim txt As Label
        txt = TryCast(sender, Label)

        Dim str() As String = txt.Text.Split("_")

        Button2.BackgroundImage = My.Resources.Pause_Hot_icon


        
        Form2.AxWindowsMediaPlayer1.currentPlaylist = objLista

        videoenvivo()


        Form2.AxWindowsMediaPlayer1.Ctlcontrols.playItem(objLista.Item(str(0)))



     



        Timer1.Enabled = True
        Timer1.Start()

        Timer1.Interval = 1

        TrackBar2.Value = Form2.AxWindowsMediaPlayer1.settings.volume
        'Form2.AxWindowsMediaPlayer1.fullScreen = True



    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        Form2.AxWindowsMediaPlayer1.currentPlaylist.clear()

        FolderBrowserDialog1.ShowDialog()
        If Not FolderBrowserDialog1.SelectedPath = "" Then
            carpeta = FolderBrowserDialog1.SelectedPath
            ListFiles(carpeta)

            listareproduccion()
            Button2.Enabled = True
            Button2.BackgroundImage = My.Resources.Play_1_Normal_icon


            Timer1.Enabled = True
            Timer1.Start()

            Timer1.Interval = 1
        End If


      




    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form2.AxWindowsMediaPlayer1.Ctlcontrols.fastForward()
        Button2.BackgroundImage = My.Resources.Play_1_Normal_icon
        Form2.AxWindowsMediaPlayer1.Visible = True
        Form2.WebControl1.Visible = False
        videoenvivo()

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form2.AxWindowsMediaPlayer1.Ctlcontrols.fastReverse()
        Button2.BackgroundImage = My.Resources.Play_1_Normal_icon
        videoenvivo()

    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Form2.AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = TrackBar1.Value

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Try
            TrackBar1.Maximum = Form2.AxWindowsMediaPlayer1.currentMedia.duration
            TrackBar1.Value = Form2.AxWindowsMediaPlayer1.Ctlcontrols.currentPosition


            If (Form2.AxWindowsMediaPlayer1.playState = 3) Then
                Form2.AxWindowsMediaPlayer1.fullScreen = True
            End If


        Catch ex As Exception

        End Try

    End Sub

   

   
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)


    End Sub

    Private Sub FlowLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles FlowLayoutPanel1.Paint

    End Sub

    Private Sub Button7_Click_1(sender As Object, e As EventArgs) Handles Button7.Click
        Form2.AxWindowsMediaPlayer1.Ctlcontrols.stop()
        Button2.BackgroundImage = My.Resources.Play_1_Normal_icon

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If Not TrackBar2.Visible Then
            TrackBar2.Visible = True
        Else
            TrackBar2.Visible = False

        End If


    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If Not Form2.AxWindowsMediaPlayer1.settings.mute Then
            Form2.AxWindowsMediaPlayer1.settings.mute = True
            Button8.BackgroundImage = My.Resources.silencio
            TrackBar2.Value = 0


        Else
            Form2.AxWindowsMediaPlayer1.settings.mute = False
            TrackBar2.Value = 10
            Button8.BackgroundImage = My.Resources.sonido
        End If


    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        Form2.AxWindowsMediaPlayer1.settings.volume = TrackBar2.Value
        If TrackBar2.Value = 0 Then
            Button8.BackgroundImage = My.Resources.silencio
        Else
            Button8.BackgroundImage = My.Resources.sonido
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        If Form2.AxWindowsMediaPlayer1.settings.getMode("LOOP") Then
            Form2.AxWindowsMediaPlayer1.settings.setMode("LOOP", False)

        Else
            Form2.AxWindowsMediaPlayer1.settings.setMode("LOOP", True)

        End If


    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs)
     

    End Sub

    Private Sub Button11_Click_1(sender As Object, e As EventArgs) Handles Button11.Click
        Form2.AxWindowsMediaPlayer1.Ctlcontrols.next()
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Form2.AxWindowsMediaPlayer1.Ctlcontrols.previous()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Dim resultado As MsgBoxResult
        OpenFileDialog1.Filter = "videos(*.avi,*.mpg,*.wmv,*.mp4)|*.avi;*.mpg;*.wmv;*.mp4"
        OpenFileDialog1.ShowDialog()
        If (Not Form2.AxWindowsMediaPlayer1.currentPlaylist.count = 0) Then
            resultado = MsgBox("desea agregarlo a la lista de reproduccion actual", MsgBoxStyle.YesNo, "abrir")
        End If
        If resultado = MsgBoxResult.Yes Then

            objLista.appendItem(Form2.AxWindowsMediaPlayer1.newMedia(OpenFileDialog1.FileName))
        Else
            objLista.clear()
            objLista.appendItem(Form2.AxWindowsMediaPlayer1.newMedia(OpenFileDialog1.FileName))
        End If

        Button2.Enabled = True
        Button2.BackgroundImage = My.Resources.Play_1_Normal_icon


        Timer1.Enabled = True
        Timer1.Start()

        Timer1.Interval = 1
        Form2.AxWindowsMediaPlayer1.currentPlaylist = objLista
        listareproduccion()
        TrackBar2.Value = Form2.AxWindowsMediaPlayer1.settings.volume

    End Sub


    Sub listareproduccion()


        FlowLayoutPanel1.Controls.Clear()



        Dim i As Integer = 0
        While i < objLista.count
            pic = New Label

            With pic


                .Width = 100
                .Height = 100
                .Visible = True
                .ForeColor = Color.Black

                .TextAlign = ContentAlignment.BottomCenter
                .Font = New Font("Times new roman", 12)



                .Text = CStr(i) & "_" & objLista.Item(i).name



                Try
                    .Image = My.Resources.Moviesicon


                Catch ex As Exception

                End Try

                AddHandler pic.Click, AddressOf picClick


                FlowLayoutPanel1.Controls.Add(pic)
            End With

            i += 1

        End While

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        If Not (clic) Then


            mostrar()




            clic = True

        Else
            ToolStripButton1.Text = "Mostrar"
            clic = False
            Form2.Hide()

        End If
    End Sub

    Private Sub ToolStripComboBox1_Click(sender As Object, e As EventArgs) Handles ToolStripComboBox1.Click

    End Sub

    Sub web()
        flpWEB.Controls.Clear()


        Dim i As Integer = 0
        While i < paginas.Count
            pic2 = New Label

            With pic2


                .Width = 100
                .Height = 100
                .Visible = True
                .ForeColor = Color.Black

                .TextAlign = ContentAlignment.BottomCenter

                .Font = New Font("Times new roman", 12)
                .FlatStyle = FlatStyle.Standard


                .Text = i


                Try
                    .Image = My.Resources.urlS


                Catch ex As Exception

                End Try

                AddHandler pic2.Click, AddressOf pic2Click


                flpWEB.Controls.Add(pic2)
            End With

            i += 1

        End While
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        paginas.Add(TextBox1.Text)
        web()



    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If (TextBox1.Text.Split("/")(0) = "http:" Or TextBox1.Text.Split("/")(0) = "https:") Then
                WebControl1.Source = New Uri(TextBox1.Text)
            Else

                WebControl1.Source = New Uri("http://" & TextBox1.Text)
            End If










        Catch ex As Exception

        End Try


    End Sub




    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Sub pic2Click(sender As Object, e As EventArgs)
        Dim txt As Label
        txt = TryCast(sender, Label)

        WebControl1.Visible = False


        Form2.AxShockwaveFlash1.Visible = False
        Form2.AxShockwaveFlash1.Stop()
        Form2.AxWindowsMediaPlayer1.Visible = False
        Form2.AxWindowsMediaPlayer1.Ctlcontrols.stop()

        Form2.WebControl1.Show()

        Form2.WebControl1.Source = New Uri(paginas(txt.Text))





        Label2.Visible = True

    End Sub

  





    Sub mostrar()
        ToolStripButton1.Text = "Ocultar"
        Form2.ShowInTaskbar = False


        Form2.Show()
        Form2.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub TableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel1.Paint

    End Sub

    Private Sub GeckoWebBrowser1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

        If Form2.WebControl1.Visible Then
            WebControl1.Visible = False
            Label2.Visible = True
        ElseIf Not Form2.WebControl1.Visible Then
            WebControl1.Visible = True
            Label2.Visible = False


        End If
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        WebControl1.Reload(True)



    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        WebControl1.GoBack()


    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        WebControl1.GoForward()

    End Sub

    Private Sub SeleccionarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SeleccionarToolStripMenuItem.Click

        Try
            OpenFileDialog1.ShowDialog()
            Dim logo As String = OpenFileDialog1.FileName
            flujo = File.Create("logo.txt")
            escritura = New StreamWriter(flujo, System.Text.Encoding.Default)
            escritura.WriteLine(logo)
            escritura.Close()

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try


    End Sub

    Private Sub ToolStripButton2_ButtonClick(sender As Object, e As EventArgs) Handles ToolStripButton2.ButtonClick

        If (File.Exists("logo.txt")) Then
            Try
                videoenvivo()


                logo.clear()

                lectura = New StreamReader("logo.txt", System.Text.Encoding.Default)
                Form2.AxWindowsMediaPlayer1.settings.setMode("LOOP", True)
                Form2.AxWindowsMediaPlayer1.settings.autoStart = True


                logo.appendItem(Form2.AxWindowsMediaPlayer1.newMedia(lectura.ReadLine))

                Form2.AxWindowsMediaPlayer1.currentPlaylist = logo


                lectura.Close()

            Catch ex As Exception

            End Try

        End If
    End Sub

    Private Sub Awesomium_Windows_Forms_WebControl_ShowCreatedWebView(sender As Object, e As Awesomium.Core.ShowCreatedWebViewEventArgs) Handles WebControl1.ShowCreatedWebView

    End Sub

    Private Sub Awesomium_Windows_Forms_WebControl_TargetURLChanged(sender As Object, e As Awesomium.Core.UrlEventArgs) Handles WebControl1.TargetURLChanged
        TextBox1.Text = WebControl1.Source.ToString
    End Sub

    Sub videoenvivo()
        Form2.AxWindowsMediaPlayer1.Visible = True
        Form2.AxShockwaveFlash1.Visible = False
        Form2.AxShockwaveFlash1.Stop()
        Form2.WebControl1.Visible = False
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Form2.AxWindowsMediaPlayer1.Ctlcontrols.stop()
        Form2.WebControl1.Stop()




    End Sub

    Private Sub Video_Embebido1_Load(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        AxShockwaveFlash1.Movie = mostraryoutube()
        PictureBox1.Hide()

    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        Form2.AxShockwaveFlash1.Visible = True

        Form2.AxShockwaveFlash1.Movie = mostraryoutube()
        Form2.AxWindowsMediaPlayer1.Visible = False
        Form2.AxWindowsMediaPlayer1.Ctlcontrols.stop()

        Form2.AxShockwaveFlash1.Play()
        Form2.WebControl1.Hide()


    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)

    End Sub

  
    Function mostraryoutube() As String


        Dim video As String = Txtyoutube.Text.Split("=")(Txtyoutube.Text.Split("=").Count - 1)



        Return "http://www.youtube.com/v/" & video


    End Function

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub AxShockwaveFlash1_Enter(sender As Object, e As EventArgs) Handles AxShockwaveFlash1.Enter

    End Sub

    Private Sub Txtyoutube_TextChanged(sender As Object, e As EventArgs) Handles Txtyoutube.TextChanged

    End Sub

 

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)





    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs)



    End Sub
End Class
