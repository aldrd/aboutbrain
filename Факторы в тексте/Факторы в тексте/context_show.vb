Public Class context_show

    Dim Cont As Integer
    Dim lvl_quality As Integer
    Dim lvl_quantity As Integer

    Dim c_space As Combinatorial_space


    Private Sub context_show_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        c_space = Form1.UnSL.c_space

        Dim best_context As context = Form1.zone.Cortex(0)
        Dim max_lvl As Integer = 0


        For Each c In Form1.zone.Cortex

            c_space.make_profile(c.bin)
            c_space.find_main_lvl()

            If c_space.main_lvl > max_lvl Then
                max_lvl = c_space.main_lvl
                best_context = c
            End If

        Next



        Cont = best_context.context_ID
        TrackBar1.Maximum = Form1.N_Context - 1
        TrackBar1.Value = Cont

        Label1.Text = "Контекст " & Cont
        Label1.Update()


        c_space.make_profile(Form1.zone.Cortex(Cont).bin)
        c_space.find_main_lvl()

        lvl_quality = c_space.main_lvl

        TrackBar2.Maximum = c_space.N_bits_in_point - 1
        TrackBar2.Value = lvl_quality
        TrackBar2.Update()
        Label2.Text = "Уровень качества " & lvl_quality
        Label2.Update()

        lvl_quantity = 1
        TrackBar3.Maximum = 20
        TrackBar3.Value = lvl_quantity
        Label3.Text = "Уровень количества " & lvl_quantity
        Label3.Update()

        Make_interf()
        Show_interf()


    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll

        Cont = TrackBar1.Value
        Label1.Text = "Контекст " & Cont
        Label1.Update()

        c_space.make_profile(Form1.zone.Cortex(Cont).bin)
        c_space.find_main_lvl()

        lvl_quality = c_space.main_lvl
        TrackBar2.Value = lvl_quality
        Label2.Text = "Уровень качества " & lvl_quality

        Make_interf()
        Show_interf()

    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll

        lvl_quality = TrackBar2.Value
        Label2.Text = "Уровень качества " & lvl_quality
        Label2.Update()

        Make_interf()
        Show_interf()

    End Sub



    Private Sub TrackBar3_Scroll(sender As Object, e As EventArgs) Handles TrackBar3.Scroll

        lvl_quantity = TrackBar3.Value
        Label3.Text = "Уровень количества " & lvl_quantity
        Label3.Update()

        Make_interf()
        Show_interf()

    End Sub

    Private Sub Show_interf()


        PictureBox1.Image = c_space.draw_points_A()
        PictureBox1.Update()


        PictureBox2.Image = Form1.draw_bin(c_space.out_code)
        PictureBox2.Update()

    End Sub

    Private Sub Make_interf()


        c_space.make_profile(Form1.zone.Cortex(Cont).bin)

        c_space.make_points_A(lvl_quality, lvl_quantity)
        c_space.make_out_code_A()



    End Sub


End Class