Imports System.ComponentModel
Imports CircuitTester.ECircuits.EComponents

Namespace ECircuits
    <TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))>
    <EditorBrowsable(EditorBrowsableState.Always)>
    Public MustInherit Class EComponent
        Implements ICloneable

        <Category("Component")>
        Public Property ID As String
        <Category("Component")>
        Public Property Name As String
        <Category("Component")>
        Public Property Type As EComponentType
        <Category("Component")>
        Public Property IsExploding As Boolean ' Indica si explota durante la ejecución
        <Category("Component")>
        Public Property HasWarning As String ' Indica si hay una advertencia
        <Category("Component")>
        Public Property HasError As String ' Indica si hay un error

        <Category("Design")>
        Public Property ShowName As Boolean = True
        <Category("Design")>
        Public Property TextFont As Font = SystemFonts.DefaultFont
        <Category("Design")>
        Public Property TextColor As Color = SystemColors.ControlText
        <Category("Design")>
        Public Property TextRotate As Boolean = True
        <Category("Design")>
        Public Property IsSelected As Boolean
        <Category("Design")>
        Public Property MainColor As Color
        <Category("Design")>
        Public Property Shape As EComponentShape
        <Category("Design")>
        Public MustOverride ReadOnly Property Text As String

        <Category("Location")>
        Public Property Rect As RectangleF
        <Category("Location")>
        Public Property Angle As Single = 0

        <TypeConverterAttribute(GetType(System.ComponentModel.ExpandableObjectConverter))>
        <EditorBrowsable(EditorBrowsableState.Always)>
        <Category("Electricity")>
        Public Property PinOut As EComponentPinout()

        Public MustOverride Sub Init()
        Public MustOverride Sub Run()
        Public MustOverride Function GetTextLocation(ByRef g As Graphics) As PointF
        Public MustOverride Function Copy() As Object

        Public Overridable Function Clone() As Object Implements ICloneable.Clone
            Dim b = DirectCast(MemberwiseClone(), EComponent)
            b.ID = System.Guid.NewGuid.ToString()

            Dim newPinout As New List(Of EComponentPinout)
            For Each c In Me.PinOut
                Dim cc = c.Clone()
                newPinout.Add(cc)
            Next
            b.PinOut = newPinout.ToArray

            Return b
        End Function

        ' Enums
        Public Enum EComponentType
            PowerSource
            PowerNeeded
            Logical
        End Enum
        Public Enum EComponentShape
            Square ' Pines en 4 bordes
            Rect ' Pines en 2 bordes
            OneSide ' Pines en 1 borde
            Table ' Breadboard (pines en forma de tabla)
            Custom
        End Enum


        Public Sub New()
            Me.ID = System.Guid.NewGuid.ToString()
        End Sub

        Public Overridable Function GetAbsolutePinLocation(index As Integer) As PointF
            Dim relativePoint = GetRelativePinLocation(index)

            relativePoint.X += Rect.X
            relativePoint.Y += Rect.Y

            Dim controlCenter As New PointF(
                Rect.X + (Rect.Width / 2),
                Rect.Y + (Rect.Height / 2)
            )

            Dim absolutePoint = RotatePoint(relativePoint, controlCenter, Me.Angle)

            Return absolutePoint
        End Function
        Public Overridable Function GetRelativePinLocation(Index As Integer) As PointF
            Dim x As Single = -(PIN_HEIGHT / 2)
            Dim y As Single = 0

            If Shape = EComponentShape.OneSide Then
                y += PIN_MARGIN + ((PIN_HEIGHT + PIN_MARGIN) * Index)
                x += Rect.Width

            ElseIf Shape = EComponentShape.Rect Then
                If Index >= (PinOut.Count / 2) Then
                    Index -= (PinOut.Count / 2)
                    x += Rect.Width
                End If
                y += PIN_MARGIN + ((PIN_HEIGHT + PIN_MARGIN) * Index)

            End If

            x += (PIN_HEIGHT / 2)
            y += (PIN_HEIGHT / 2)

            Return New PointF(x, y)
        End Function




        Public Overridable Sub Draw(ByRef g As Graphics)

            Dim xx, yy As Single
            xx = Rect.X + Rect.Width / 2
            yy = Rect.Y + Rect.Height / 2

            Dim x0, y0 As Single
            x0 = -Rect.Width / 2
            y0 = -Rect.Height / 2

            g.TranslateTransform(xx, yy)
            g.RotateTransform(Angle)

            Dim newRect As New RectangleF(x0, y0, Rect.Width, Rect.Height)

            Dim SolidBrush As New SolidBrush(MainColor)


            ' Selected Border
            ' 64 = 25%
            If IsSelected Then
                SolidBrush.Color = Color.FromArgb(128, MainColor)
                Dim borderRect As New Rectangle(
                    newRect.X - PIN_MARGIN * 2,
                    newRect.Y - PIN_MARGIN * 2,
                    newRect.Width + (PIN_MARGIN * 4),
                    newRect.Height + (PIN_MARGIN * 4)
                )
                g.FillRectangle(SolidBrush, borderRect)
            End If

            ' Background
            SolidBrush.Color = MainColor
            g.FillRectangle(SolidBrush, newRect)

            ' Pinout
            Dim PinLocation As PointF, PinRect As RectangleF
            Dim PinSize As New SizeF(PIN_HEIGHT, PIN_HEIGHT)

            For PinNumber As Integer = 0 To PinOut.Count - 1
                PinLocation = GetRelativePinLocation(PinNumber)
                PinLocation.X -= (PIN_HEIGHT / 2) - x0
                PinLocation.Y -= (PIN_HEIGHT / 2) - y0
                PinRect = New RectangleF(PinLocation, PinSize)

                SolidBrush.Color = SystemColors.ControlDark
                g.FillEllipse(SolidBrush, PinRect)

                SolidBrush.Color = SystemColors.ControlDarkDark
                g.DrawEllipse(New Pen(SolidBrush.Color), PinRect)
            Next


            SolidBrush.Dispose()

            g.RotateTransform(-Angle)
            g.TranslateTransform(-xx, -yy)
        End Sub
    End Class
End Namespace