Public Class Supervised_learning

    Friend c_space As Combinatorial_space

    Dim zone As Cortex_zone = Form1.zone

    Friend cont_source As context = zone.Cortex(0)
    Friend cont_target As context = zone.Cortex(1)

    Public Source_Text As New Info_Sourse_TXT(Form1.N_Context)

    Public lvl As Integer = 2

    Public count As Integer = 0

    Dim mem_on As Boolean = True ' включение запоминания
    Dim multi_on As Boolean = True ' множественное влияние кластеров рецепторов на выходные элементы

    Dim connections_rebuild_on = False

    'Dim L_bin As BitArray = Form1.concepts_str_to_bin("a:1")

    Private Sub Supervised_learnig_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        c_space = New Combinatorial_space(Form1.N_bit_main, Form1.N_1_main, Combinatorial_space.Learning_mode.supervised)


    End Sub

    ' 1 steps
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        t_step()

        show_step_info()

    End Sub

    Private Sub show_step_info()


        PictureBox4.Image = c_space.draw_mem()
        PictureBox4.Update()

        PictureBox1.Image = Form1.draw_bin(cont_source.bin)
        PictureBox1.Update()

        PictureBox2.Image = Form1.draw_bin(cont_target.bin)
        PictureBox2.Update()

        PictureBox3.Image = Form1.draw_bin(c_space.out_code)
        PictureBox3.Update()

        PictureBox6.Image = Form1.draw_bin(c_space.out_code_part)
        PictureBox6.Update()

        PictureBox5.Image = Form1.draw_dif(c_space.out_code, cont_target.bin)
        PictureBox5.Update()

        TextBox1.Text = c_space.obs_cl_info(Form1.concepts_str_to_bin)
        TextBox1.Update()


        Label9.Text = c_space.info()
        Label9.Update()




    End Sub

    Private Sub t_step()


        Source_Text.step_txt(5, 5)
        Label7.Text = Source_Text.txt
        Label7.Update()

        Label4.Text = c_space.internal_time
        Label4.Update()


        zone.make_interpretations(Source_Text.ID_set)

        c_space.activate_clasters(cont_source.bin, cont_target.bin)
        c_space.make_out_code_lvl(lvl)

        c_space.modify_clasters_F()

        c_space.consolidate_memory_sl()

        'If int_space.internal_time > 1 Then
        '    mem_on = False
        'End If


        If mem_on Then
            c_space.add_new_clusters(cont_source.bin, cont_target.bin)
        End If

        c_space.internal_time += 1


        If c_space.internal_time Mod 5 = 0 Then

            show_step_info()

        End If


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        For i = 1 To 100
            t_step()
        Next

    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click

        For i = 1 To 500
            t_step()
        Next

    End Sub


    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll

        lvl = TrackBar1.Value


        zone.make_interpretations(Source_Text.ID_set)

        c_space.make_out_code_lvl(lvl)

        show_step_info()

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        For k = 1 To 5

            For i = 1 To 1000
                t_step()
            Next

        Next

        mem_on = False

        TrackBar2.Value = 0
        TrackBar2.Update()

        For k = 1 To 5

            For i = 1 To 1000
                t_step()
            Next

        Next

        mem_on = True

        TrackBar2.Value = 1
        TrackBar2.Update()

    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll

        mem_on = (TrackBar2.Value = 1)

    End Sub

    Private Sub TrackBar3_Scroll(sender As Object, e As EventArgs) Handles TrackBar3.Scroll

        multi_on = (TrackBar3.Value = 1)

    End Sub



    ' Выбор случайного временного кластера для наблюдения
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        c_space.obs_cl_select(status_type.tmp)
        TextBox1.Text = c_space.obs_cl_info(Form1.concepts_str_to_bin)
        TextBox1.Update()

    End Sub

    Private Sub Button6_Click_1(sender As Object, e As EventArgs) Handles Button6.Click
        c_space.obs_cl_select(status_type.tmp, True)
        TextBox1.Text = c_space.obs_cl_info(Form1.concepts_str_to_bin)
        TextBox1.Update()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        c_space.obs_cl_select(status_type.permanent_1)
        TextBox1.Text = c_space.obs_cl_info(Form1.concepts_str_to_bin)
        TextBox1.Update()
    End Sub


End Class