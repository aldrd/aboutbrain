' Алексей Редозубов
Public Class Form1

    Friend SL As Supervised_learning
    Friend UnSL As Unsupervised_learning

    Public N_Context As Integer = 10
    Public Const N_Frame As Integer = 5

    Public N_bit_main As Integer = 256
    Public N_1_main As Integer = 8





    Dim X_C As Integer = N_Context
    Dim Y_C As Integer = 1

    Public zone As New Cortex_zone(N_Context, N_bit_main, N_1_main)

    Public Original_Memory As New List(Of original_mem_item)




    Dim Color_arr()() As Color


    Public concepts_str_to_bin As New Dictionary(Of String, BitArray)
    Public concepts_str_to_ID As New Dictionary(Of String, Integer)
    Public concepts_id_to_bin As New Dictionary(Of Integer, BitArray)
    Public concepts_id_to_str As New Dictionary(Of Integer, String)
    Public concepts_bin_to_str As New Dictionary(Of BitArray, String)
    Public concepts_bin_to_id As New Dictionary(Of BitArray, Integer)


    Dim interp_set_pic(N_Context - 1) As PictureBox

    Dim First_L As Char = "a"c
    Dim Last_L As Char = "z"c

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Set_Concepts_txt()

        Set_Invariants_txt(zone)

    End Sub


    Private Sub Set_Concepts_txt()

        Dim str1 As String
        Dim ch_set As New List(Of Char)

        Dim h1 As Integer



        ch_set.Add("_")
        For ch = Asc(First_L) To Asc(Last_L)
            ch_set.Add(Chr(ch))
        Next

        For pos = 0 To N_Context - 1

            For Each ch In ch_set

                Dim b1 As New BitArray(zone.N_bit)

                str1 = ch & ":" & pos.ToString

                b1 = get_random_bin(zone.N_bit, zone.N_1)
                h1 = b1.GetHashCode

                concepts_str_to_bin.Add(str1, b1)
                concepts_str_to_ID.Add(str1, h1)
                concepts_id_to_str.Add(h1, str1)
                concepts_bin_to_id.Add(b1, h1)

            Next


        Next


    End Sub

    Private Sub Set_Invariants_txt(ByRef zone As Cortex_zone)

        Dim str1, str2 As String
        Dim ch_set As New List(Of Char)

        Dim h1, h2 As ULong
        Dim b1 As New BitArray(zone.N_bit)
        Dim b2 As New BitArray(zone.N_bit)

        ch_set.Add("_")
        For ch = Asc(First_L) To Asc(Last_L)
            ch_set.Add(Chr(ch))
        Next

        For Each col In zone.Cortex

            For pos = 0 To N_Context - 1

                For Each ch In ch_set

                    str1 = ch & ":" & pos.ToString

                    b1 = get_bin_from_string(str1)
                    h1 = b1.GetHashCode


                    str2 = ch & ":" & ((pos + col.context_ID) Mod N_Context).ToString

                    b2 = get_bin_from_string(str2)
                    h2 = b2.GetHashCode

                    col.transforms.Add(h1, h2)

                Next


            Next
        Next


    End Sub




    Friend Function get_bin_from_string(str As String) As BitArray

        get_bin_from_string = Nothing

        If Not concepts_str_to_bin.TryGetValue(str, get_bin_from_string) Then

            get_bin_from_string = New BitArray(zone.N_bit)

            get_bin_from_string = get_random_bin(zone.N_bit, zone.N_1)

            concepts_str_to_bin.Add(str, get_bin_from_string)

            concepts_id_to_str.Add(get_bin_from_string.GetHashCode, str)


        End If

    End Function




    Public Function get_random_bin(ByRef N_bit As Integer, ByRef N_1 As Integer) As BitArray

        Dim h As Integer

        Do

            Dim k As Integer

            get_random_bin = New BitArray(N_bit)

            For i = 1 To N_1

                Do

                    k = Int(Rnd() * N_bit)

                Loop Until get_random_bin.Item(k) = False

                get_random_bin.Item(k) = True

            Next



            h = get_random_bin.GetHashCode

        Loop Until Not concepts_id_to_bin.ContainsKey(h)

        concepts_id_to_bin.Add(h, get_random_bin)

    End Function


    Public Function make_portrait_str(ByRef bin As BitArray) As String

        Dim b As Boolean
        Dim pos As Integer
        Dim ch As Char

        make_portrait_str = StrDup(N_Context, "_")

        For Each c In concepts_str_to_bin

            b = True

            For i = 0 To zone.N_bit - 1

                If c.Value(i) And Not bin(i) Then
                    b = False
                    Exit For
                End If

            Next

            If b Then

                pos = Val(c.Key.Split(":")(1))
                ch = c.Key.Chars(0)

                Dim a() As Char = make_portrait_str.ToCharArray
                a(pos) = ch
                make_portrait_str = New String(a)

            End If

        Next

    End Function




    Friend Function show_interpretation_bin() As Bitmap

        show_interpretation_bin = New Bitmap(N_Context * 4, zone.N_bit)

        Dim N As Integer

        For c = 0 To N_Context - 1


            For i = 0 To zone.N_bit - 1

                N = -zone.Cortex(c).bin(i) * 255
                For k = 0 To 2
                    show_interpretation_bin.SetPixel(c * 4 + k, i, Color.FromArgb(N, N, N))
                Next

                show_interpretation_bin.SetPixel(c * 4 + 3, i, Color.Gray)

            Next

        Next

    End Function


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        SL = New Supervised_learning

        SL.Show()


    End Sub


    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        UnSL = New Unsupervised_learning

        UnSL.Show()

    End Sub

    Friend Function draw_bin(ByRef bin As BitArray) As Bitmap

        Dim N As Integer = bin.Length


        draw_bin = New Bitmap(N, 5)

        For i = 0 To N - 1

            For j = 0 To 4

                If bin(i) Then
                    draw_bin.SetPixel(i, j, Color.White)
                Else
                    draw_bin.SetPixel(i, j, Color.Black)
                End If

            Next

        Next

    End Function

    Friend Shared Function draw_dif(bin1 As BitArray, bin2 As BitArray) As Bitmap

        Dim N As Integer = bin1.Length


        draw_dif = New Bitmap(N, 5)

        For i = 0 To N - 1

            For j = 0 To 4

                If bin1(i) Then

                    If bin2(i) Then
                        draw_dif.SetPixel(i, j, Color.Black)
                    Else
                        draw_dif.SetPixel(i, j, Color.Red)
                    End If

                Else
                    If bin2(i) Then
                        draw_dif.SetPixel(i, j, Color.Blue)
                    Else
                        draw_dif.SetPixel(i, j, Color.Black)
                    End If
                End If

            Next

        Next

    End Function




End Class




Public Class context

    Dim N_bit As Integer
    Dim N_bit_reg As Integer

    ' Правила контекстного преобразования
    Public transforms As New Dictionary(Of Integer, Integer)

    Public bin As BitArray

    Public context_ID As Integer
    Public ID_set_local As New List(Of String)

    Friend A_part As Integer


    Public Sub New(N_bit_main, N_bit_reg_main)

        N_bit = N_bit_main
        N_bit_reg = N_bit_reg_main
        bin = New BitArray(N_bit)

    End Sub

    Public Sub make_interpretation(ByRef id_set As List(Of Integer))

        ' Делаем суммарное битовое описание c учетом трактовки в контексте

        bin.SetAll(False)
        ID_set_local.Clear()

        For Each id In id_set

            bin.Or(Form1.concepts_id_to_bin(transforms(id)))
            ID_set_local.Add(Form1.concepts_id_to_str(transforms(id)))

        Next

    End Sub



End Class

Public Class Info_item


    Public ID As Integer
    Public ch As Char
    Public pos As Integer


End Class




Public Class original_mem_item

    Public source_id_set As List(Of Integer)

    Public txt As String


End Class

Public Class memory_item

    Public original_mem As original_mem_item
    Public bin As BitArray
    Public f_key As BitArray
    Public context_ID As Integer
    Public T_creat As Integer
    Public id As Integer
    Public A As Double ' Активность в момент запоминания

    Public Activity_in_context(Form1.N_Context - 1) As Double

End Class