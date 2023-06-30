Imports System.Buffers
Imports System.ComponentModel

Namespace ECircuits
    Namespace EComponents
        <TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Class LED
            Inherits EComponent

            <Category("Design")>
            Public Property Color As Color = Color.White
            <Category("Design")>
            Public Property LightOutput As Byte = 255
            <Category("Design")>
            Public Property ShowPowerUse As Boolean = True

            <Category("Default Electricity")>
            Public Property MaxVoltage As Double = 3.5 ' Volts
            <Category("Default Electricity")>
            Public Property RequiredVoltage As Double = 3.2 ' Volts
            <Category("Default Electricity")>
            Public Property PowerConsumption As Double = 0.02 ' Amperes

            <Category("Electricity")>
            Public ReadOnly Property PowerPerecentage As Single
                Get
                    Return Math.Max(Math.Min((LightOutput * 100) / 255, 100), 0)
                End Get
            End Property

            Public Overrides ReadOnly Property Text As String
                Get
                    Dim x As String = ""
                    If ShowName Then
                        x &= Me.Name
                    End If
                    If ShowPowerUse Then
                        If x.Length > 0 Then x &= System.Environment.NewLine
                        x &= "(" & Me.PowerConsumption & " A)"
                    End If
                    Return x
                End Get
            End Property
            Public Overrides Function GetTextLocation(ByRef g As Graphics) As PointF
                ' Obtener el tamaño del texto
                Dim textSize As SizeF = g.MeasureString(Me.Text, Me.TextFont)
                ' Calcular las coordenadas para centrar el texto
                Dim textX As Single = -textSize.Width / 2
                Dim textY As Single = (-Me.Rect.Height / 2) - textSize.Height - PIN_HEIGHT
                Return New PointF(textX, textY)
            End Function

            Public Sub New()
                Me.Name = "LED"
                Me.Type = EComponent.EComponentType.PowerNeeded
                Me.Shape = EComponent.EComponentShape.OneSide
                Me.PinOut = {
                    New EComponentPinout With {
                        .PinLabel = "Positive (+)"
                    },
                    New EComponentPinout With {
                        .PinLabel = "Negative (-)"
                    }
                }
                Me.MainColor = Color.DeepSkyBlue
            End Sub

            Private Const POSITIVE_PIN As Integer = 0
            Private Const NEGATIVE_PIN As Integer = 1

            Public Overrides Sub Init()

            End Sub
            Public Overrides Sub Run()

                ' Si se supera el maximo de voltaje, explota
                If PinOut(0).Voltage > MaxVoltage Then
                    IsExploding = True
                    HasError = "Max voltage: " & MaxVoltage & System.Environment.NewLine & "Current voltage: " & Math.Round(PinOut(0).Voltage, 3)
                Else
                    IsExploding = False
                    HasError = ""
                End If

                ' Mover la electricidad dentro del componente
                Dim remainingAmperage As Double = Math.Max(PinOut(0).Amperage - powerConsumption, 0)
                Dim remainingVoltage As Double = Math.Max(PinOut(0).Voltage - (PowerConsumption * remainingAmperage), 0)
                Dim remainingResistance As Double = RequiredVoltage / PowerConsumption

                ' Actualizar los valores de salida en PinOut(1)
                PinOut(1).Voltage = remainingVoltage
                PinOut(1).Amperage = remainingAmperage
                PinOut(1).Resistance = remainingResistance

                ' Ajustar iluminacion
                Dim lightIntensity As Byte = CByte(Math.Min(Math.Round((PinOut(0).Voltage / RequiredVoltage) * (PinOut(0).Amperage / PowerConsumption) * 255), 255))
                LightOutput = lightIntensity

            End Sub


            Public Overrides Sub Draw(ByRef g As Graphics)
                MyBase.Draw(g)

                'Dim innerLedRect As New RectangleF(Me.Rect.X + (PIN_HEIGHT / 2) + (PIN_HEIGHT / 3), (Me.Rect.Y + PIN_HEIGHT / 2) + (PIN_HEIGHT / 3), Me.Rect.Width - PIN_HEIGHT - ((PIN_HEIGHT / 3) * 2), Me.Rect.Height - PIN_HEIGHT - ((PIN_HEIGHT / 3) * 2))
                Dim padding As Single = (PIN_HEIGHT / 3) + (PIN_HEIGHT / 2)
                Dim innerLedRect As New RectangleF(Me.Rect.X + padding, Me.Rect.Y + padding, Me.Rect.Width - padding * 2, Me.Rect.Height - padding * 2)


                ' LIGHT
                If LightOutput > 0 Then
                    Using b As New SolidBrush(Color.FromArgb(LightOutput, Me.Color))
                        g.FillEllipse(b, innerLedRect)
                    End Using
                End If

                ' LIGHT BORDER
                If Me.Color.A > 0 Then
                    Using b As New Pen(Me.Color)
                        g.DrawEllipse(b, innerLedRect)
                    End Using
                End If

                ' TEXT
                If (ShowName Or ShowPowerUse) And (TextColor.A > 0) Then
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


            Public Overrides Function Copy() As Object
                Dim r = Me.Clone
                r.Name = "Copy of [" + Me.Name + "]"
                r.Rect = New RectangleF(Me.Rect.X + PIN_MARGIN + Rect.Width, Me.Rect.Y, Rect.Width, Rect.Height)
                Return r
            End Function

        End Class
    End Namespace
End Namespace
