Public Class Unsupervised_learning


    Public N_Context As Integer
    Public N_Frame As Integer

    Public c_space As Combinatorial_space

    Public Source_Text As New Info_Sourse_TXT(Form1.N_Context)

    Dim zone As Cortex_zone = Form1.zone

    Private Sub Unsupervised_learning_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        c_space = New Combinatorial_space(Form1.N_bit_main, Form1.N_1_main, Combinatorial_space.Learning_mode.unsupervised)


        N_Context = Form1.N_Context
        N_Frame = Form1.N_Frame

    End Sub


    ' Накопление памяти
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        For i = 1 To 30

            Source_Text.step_txt(N_Frame / 2, N_Frame)

            Label1.Text = Source_Text.txt
            Label1.Update()
            Label2.Text = i
            Label2.Update()

            Dim best_context As context = Nothing

            detect_best_context(best_context)

            Form1.show_interpretation_bin()

            c_space.add_new_clusters(best_context.bin)

            c_space.consolidate_memory_sl()

            c_space.activate_clasters(best_context.bin)

            show_step_info()

            c_space.internal_time += 1



        Next

    End Sub

    ' Шаг с запоминанием
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        Source_Text.step_txt(N_Frame / 2, N_Frame)

        Label1.Text = Source_Text.txt
        Label1.Update()

        Dim best_context As context = Nothing

        detect_best_context(best_context)

        Form1.show_interpretation_bin()

        c_space.add_new_clusters(best_context.bin)

        c_space.consolidate_memory_sl()

        c_space.activate_clasters(best_context.bin)

        c_space.make_out_code_unsl(best_context.bin)

        show_step_info()

        c_space.internal_time += 1

    End Sub


    'Public Sub draw_int_space()



    '    'Label7.Text = "Активность точек"
    '    'Label7.Update()

    '    'PictureBox1.Image = c_space.draw_points()
    '    'PictureBox1.Update()

    '    'Label8.Text = "Активность выхода"

    '    'PictureBox2.Image = Form1.draw_bin(c_space.out_code)
    '    'PictureBox2.Update()


    'End Sub

    Public Sub detect_best_context(ByRef best_context As context)


        zone.make_interpretations(Source_Text.ID_set)


        best_context = zone.Cortex(0)
        Dim max_part_lvl As Integer = 0


        For Each c In zone.Cortex

            c_space.make_profile(c.bin)

            c.A_part = c_space.part_lvl

            If c_space.part_lvl > max_part_lvl Then
                max_part_lvl = c_space.part_lvl
                best_context = c
            End If

        Next


    End Sub


    ' Шаг с запоминанием текста из строки ввода
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        Source_Text.text_to_info(TextBox5.Text)

        Label1.Text = Source_Text.txt
        Label1.Update()

        Dim best_context As context = Nothing

        detect_best_context(best_context)

        Form1.show_interpretation_bin()

        c_space.add_new_clusters(best_context.bin)

        c_space.consolidate_memory_sl()

        c_space.activate_clasters(best_context.bin)

        c_space.make_out_code_unsl(best_context.bin)

        show_step_info()

        c_space.internal_time += 1

    End Sub

    ' Шаг без запоминания
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Source_Text.text_to_info(TextBox5.Text)

        Label1.Text = Source_Text.txt
        Label1.Update()

        Dim best_context As context = Nothing

        detect_best_context(best_context)

        Form1.show_interpretation_bin()

        c_space.consolidate_memory_sl()

        c_space.activate_clasters(best_context.bin)

        c_space.make_out_code_unsl(best_context.bin)


        show_step_info()

    End Sub

    'Private Sub draw_mem()

    '    Label9.Text = "Плотность памяти"
    '    Label9.Update()

    '    PictureBox4.Image = c_space.draw_mem()
    '    PictureBox4.Update()


    'End Sub

    Private Sub show_step_info()

        PictureBox3.Image = Form1.show_interpretation_bin()
        PictureBox3.Update()

        PictureBox1.Image = c_space.draw_points_A()
        PictureBox1.Update()

        PictureBox4.Image = c_space.draw_mem()
        PictureBox4.Update()

        Label10.Text = c_space.N_cl_total & " (" & c_space.N_perm & ")"
        Label10.Update()

        PictureBox2.Image = Form1.draw_bin(c_space.out_code)
        PictureBox2.Update()


        Dim s As String = ""

        For Each col In zone.Cortex

            s &= "context " & col.context_ID & " - " & col.A_part & vbCrLf

        Next

        TextBox1.Text = s
        'TextBox1.Text = c_space.obs_cl_info(Form1.concepts_str_to_bin)
        TextBox1.Update()

        With c_space

            Label11.Text = "точные после cut+-b (" & .N_cut_ex & " , " & .N_cut_bad & " , " & .N_broken & ")   точные perm +- (" & .N_perm_ex & " , " & .N_perm_notex & ")"
            Label11.Update()

            Label12.Text = "потенциальные после cut+-b (" & .N_potenc_cut_ex & " , " & .N_potenc_cut_bad & " , " & .N_potenc_broken & ")   потенциальные new +- (" & .N_potenc_good & " , " & .N_potenc_bad & ")"
            Label12.Update()

            Label13.Text = "del g pg ok (ok_TO) (" & .N_good_del & " , " & .N_potenc_good_del & " , " & .N_ok_del & " (" & .N_ok_del_TO & ")" & " )"
            Label13.Update()

            Label14.Text = "ok " & .N_ok
            Label14.Update()

        End With



    End Sub

    ' Показ контекста
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click


        If Source_Text.ID_set.Count = 0 Then

            Source_Text.text_to_info(TextBox5.Text)

            Label1.Text = Source_Text.txt
            Label1.Update()


            Dim best_context As context = Nothing

            detect_best_context(best_context)

            show_step_info()

        End If


        Dim context_show = New context_show

        context_show.Show()




    End Sub





    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dim T As New BitArray(Form1.N_bit_main)

        Dim bit_set() As Integer = {10, 12, 24, 30, 32}


        For i = 1 To 5000

            Source_Text.step_txt(5, 5)

            Label1.Text = Source_Text.txt
            Label1.Update()
            Label2.Text = i
            Label2.Update()



            zone.make_interpretations(Source_Text.ID_set)

            PictureBox3.Image = Form1.show_interpretation_bin()
            PictureBox3.Update()

            For Each b In bit_set

                If Not zone.Cortex(0).bin(b) Then
                    GoTo m1
                End If

            Next

            MsgBox("collision on step " & i)

m1:

        Next

    End Sub

End Class