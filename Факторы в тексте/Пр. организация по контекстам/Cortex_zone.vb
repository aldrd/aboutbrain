Public Class Cortex_zone

    Public Cortex As New List(Of context)

    Public Memory As New List(Of memory_item)


    Dim N_Context As Integer

    Public N_bit As Integer  ' Разрядность бинарного кода описания
    Public N_1 As Integer ' Количество единиц в простом признаке

    Public N_bit_Reg As Integer = 32
    Public N_1_Reg As Integer = 4

    Public factor_count As Integer = 1
    Public interpretation_count As Integer = 1

    Public win_m As memory_item
    Public win_cont As context

    Public potencial_cont_avg As Double

    Public Sub New(N As Integer, N_bit_main As Integer, N_1_main As Integer)

        N_bit = N_bit_main
        N_1 = N_1_main
        N_Context = N

        For i = 0 To N_Context - 1

            Cortex.Add(New context(N_bit, N_bit_Reg) With {.context_ID = i})

        Next

    End Sub

    Public Function create_memory_element(ByRef bin As BitArray, ByRef context_ID As Integer,
                               ByRef mem As original_mem_item, ByVal F_id As Integer, ByVal key As BitArray) As memory_item

        Dim interp As New memory_item

        interp.context_ID = context_ID
        interp.bin = bin.Clone
        interp.original_mem = mem

        interpretation_count += 1
        create_memory_element = interp
        interp.id = interpretation_count

        interp.f_key = key

    End Function

    Public Sub make_interpretations(ByRef ID_set As List(Of Integer))

        For Each cont In Cortex

            cont.make_interpretation(ID_set)

        Next


    End Sub


    ' Функция совпадения бинарных массивов
    Public Function mult(ByRef b1 As BitArray, b2 As BitArray, ByRef r0 As Double, ByRef r1 As Double) As Double


        Dim s, s1, s2, ss As Integer

        For i = 0 To N_bit - 1

            s += b1(i) * b2(i)
            s1 -= CInt(b1(i))
            s2 -= CInt(b2(i))

        Next

        ss = Math.Sqrt(s1 * s2)

        'mult = s / N_bit
        If ss = 0 Then
            mult = 0
            r0 = 0
            r1 = 0
        Else
            mult = s / ss
            ' Уровень случайного совпадения
            'r0 = s1 * s2 / (N_bit * N_bit)
            r0 = (s1 * s2 / N_bit) / ss

            mult -= r0

            ' Уровень максимального совпадения

            'r1 = Math.Min(s1, s2) / N_bit
            r1 = Math.Min(s1, s2) / ss
        End If




    End Function

    Public Sub memory_add_with_ID(ByRef context_index As Integer, ByVal key As BitArray)

        Memory.Add(New memory_item With {
                     .bin = Cortex(context_index).bin.Clone,
                     .context_ID = context_index,
                     .f_key = key})

    End Sub

End Class
