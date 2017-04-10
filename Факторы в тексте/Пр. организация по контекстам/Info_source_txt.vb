Public Class Info_Sourse_TXT

    Dim source_text As String

    Dim Global_Pos As Integer ' текущая позиция

    Dim Local_pos As Integer ' Кольцевой идентификатор времени (позиции)    

    Public txt As String

    Dim Info_set As New List(Of Info_item)

    Public ID_set As New List(Of Integer)

    Dim First_L As Char = "a"c
    Dim Last_L As Char = "z"c

    Dim N_circle_id As Integer

    Public Sub New(N As Integer)

        source_text = IO.File.ReadAllText("text.txt", System.Text.Encoding.GetEncoding(65001))

        Global_Pos = 0
        Local_pos = 0

        N_circle_id = N

    End Sub


    Public Function get_next_info() As Info_item

        get_next_info = New Info_item

        Dim ch As Char = source_text(Global_Pos).ToString.ToLower

        get_next_info.pos = Local_pos


        If (ch >= First_L And ch <= Last_L) Then

            get_next_info.ch = ch

        Else
            get_next_info.ch = "_"

        End If

        get_next_info.ID = Form1.concepts_str_to_ID(get_next_info.ch & ":" & Local_pos)


        Local_pos += 1
        If Local_pos = N_circle_id Then Local_pos = 0

        Global_Pos += 1
        If Global_Pos = source_text.Length Then Global_Pos = 0



    End Function

    Public Sub step_txt(ByRef step_length As Integer, N_Frame As Integer)

        Dim S(N_circle_id - 1) As Char
        For i = 0 To N_circle_id - 1
            S(i) = "_"
        Next


        For i = 1 To step_length


            Dim info As New Info_item
            info = get_next_info()

            Info_set.Add(info)

            If Info_set.Count > N_Frame Then

                Info_set.RemoveAt(0)

            End If

        Next

        For Each inf In Info_set
            S(inf.pos) = inf.ch
        Next

        txt = New String(S)



        ID_set.Clear()

        For Each inf In Info_set

            If inf.ch <> "_" Then
                ID_set.Add(inf.ID)
            End If
        Next

    End Sub

    Public Sub text_to_info(text As String)

        If text.Length > 25 Then text = text.Substring(0, 25)

        txt = text

        ID_set.Clear()

        Dim ch As Char


        For pos = 0 To text.Length - 1

            ch = text(pos)

            If (ch >= First_L And ch <= Last_L) Then

                ID_set.Add(Form1.concepts_str_to_ID(ch & ":" & pos))

            End If

        Next

    End Sub

End Class