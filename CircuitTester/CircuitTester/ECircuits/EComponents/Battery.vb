Imports System.ComponentModel

Namespace ECircuits
    Namespace EComponents
        <TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Class Battery
            Inherits EComponent
            Implements EPowerSource

            <Category("Design")>
            Public Property ShowCapacity As Boolean = True

            <Category("Default Electricity")>
            Public Property DefaultVoltage As Double = 9
            <Category("Default Electricity")>
            Public Property DefaultCapacity As Double = 0.6

            Public Sub New()
                Me.Name = "Battery"
                Me.Type = EComponent.EComponentType.PowerSource
                Me.Shape = EComponent.EComponentShape.OneSide
                Me.PinOut = {
                  New EComponentPinout With {
                    .PinLabel = "Positivo (+)"
                  },
                  New EComponentPinout With {
                    .PinLabel = "Negativo (-)"
                  }
                }
                Me.MainColor = SystemColors.ControlDark
                Me.TextFont = SystemFonts.CaptionFont
            End Sub

            Public Overrides ReadOnly Property Text As String
                Get
                    Dim x As String = ""
                    If ShowName Then
                        x &= Me.Name
                    End If
                    If ShowCapacity Then
                        If x.Length > 0 Then x &= System.Environment.NewLine
                        x &= String.Format("({0} A)", Math.Round(Me.PinOut(0).Amperage, 2))
                    End If
                    Return x
                End Get
            End Property
            Public Overrides Function GetTextLocation(ByRef g As Graphics) As PointF
                ' Obtener el tamaño del texto
                Dim textSize As SizeF = g.MeasureString(Me.Text, Me.TextFont)
                ' Calcular las coordenadas para centrar el texto
                Dim textX As Single = -textSize.Width / 2
                Dim textY As Single = -textSize.Height / 2
                Return New PointF(textX, textY)
            End Function


            Public Overrides Function Copy() As Object
                Dim r = Me.Clone
                r.Name = "Copy of [" + Me.Name + "]"
                r.Rect = New RectangleF(Me.Rect.X + PIN_MARGIN + Rect.Width, Me.Rect.Y, Rect.Width, Rect.Height)
                Return r
            End Function

            Public Function HasPower() As Boolean Implements EPowerSource.HasPower
                Return (PinOut(1).Amperage > 0 And PinOut(1).Voltage > 0)
            End Function

            Public Overrides Sub Draw(ByRef g As Graphics)
                MyBase.Draw(g)

                ' TEXT
                If (ShowName Or ShowCapacity) And (TextColor.A > 0) Then
                    g.TranslateTransform(Me.Rect.X + Me.Rect.Width / 2, Me.Rect.Y + Me.Rect.Height / 2)
                    If TextRotate Then g.RotateTransform(Me.Angle)
                    Using brush As New SolidBrush(Me.TextColor)
                        Dim L = GetTextLocation(g)
                        g.DrawString(Text, Me.TextFont, brush, L.X, L.Y)
                    End Using
                    If TextRotate Then g.RotateTransform(-Me.Angle)
                    g.TranslateTransform(-Me.Rect.X - Me.Rect.Width / 2, -Me.Rect.Y - Me.Rect.Height / 2)
                End If
            End Sub

            Public Overrides Sub Init()
                PinOut(0).Voltage = DefaultVoltage
                PinOut(0).Amperage = DefaultCapacity
                PinOut(0).Resistance = 0
                PinOut(1).Voltage = DefaultVoltage
                PinOut(1).Amperage = DefaultCapacity
                PinOut(1).Resistance = 0
            End Sub

            Public Overrides Sub Run()
                ' Mover la electricidad dentro del componente
                PinOut(0).Voltage = PinOut(1).Voltage
                PinOut(0).Amperage = PinOut(1).Amperage
                PinOut(0).Resistance = PinOut(1).Resistance
            End Sub



        End Class

    End Namespace
End Namespace