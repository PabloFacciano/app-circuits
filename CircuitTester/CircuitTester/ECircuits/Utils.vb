Imports CircuitTester.ECircuits

Public Module Utils

    ' Draw properties
    Public Const PIN_HEIGHT As Single = 10
    Public Const PIN_MARGIN As Single = 5
    Public Const WIRE_WIDTH As Single = 4

    Public DefaultComponents As EComponent() = {
        New EComponents.Battery,
        New EComponents.LED
    }

    Public Function IsPointInsideRect(ByVal point As PointF, ByVal rect As RectangleF) As Boolean
        Return (point.X >= rect.X AndAlso
        point.X <= (rect.X + rect.Width) AndAlso
        point.Y >= rect.Y AndAlso
        point.Y <= (rect.Y + rect.Height))
    End Function

    Public Function IsPointInsideRectWithAngle(ByVal point As PointF, ByVal rect As RectangleF, ByVal angle As Double) As Boolean
        ' Obtener el centro del rectángulo
        Dim center As New PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2)

        ' Calcular las coordenadas del punto relativas al centro del rectángulo
        Dim relativePoint As New PointF(point.X - center.X, point.Y - center.Y)

        ' Calcular el ángulo de rotación en radianes
        Dim rotationAngle As Double = angle * Math.PI / 180

        ' Aplicar la rotación inversa al punto
        Dim rotatedPoint As New PointF(
        relativePoint.X * Math.Cos(-rotationAngle) - relativePoint.Y * Math.Sin(-rotationAngle),
        relativePoint.X * Math.Sin(-rotationAngle) + relativePoint.Y * Math.Cos(-rotationAngle)
    )

        ' Verificar si el punto rotado está dentro del rectángulo original
        Return Math.Abs(rotatedPoint.X) <= rect.Width / 2 AndAlso Math.Abs(rotatedPoint.Y) <= rect.Height / 2
    End Function



    Public Function RotatePoint(ByVal originalPoint As PointF, ByVal centerPoint As PointF, ByVal angle As Single) As PointF
        Dim radians As Double = angle * Math.PI / 180.0
        Dim cosAngle As Double = Math.Cos(radians)
        Dim sinAngle As Double = Math.Sin(radians)

        Dim translatedX As Double = originalPoint.X - centerPoint.X
        Dim translatedY As Double = originalPoint.Y - centerPoint.Y

        Dim rotatedX As Double = translatedX * cosAngle - translatedY * sinAngle
        Dim rotatedY As Double = translatedX * sinAngle + translatedY * cosAngle

        Dim finalX As Double = rotatedX + centerPoint.X
        Dim finalY As Double = rotatedY + centerPoint.Y

        Return New Point(finalX, finalY)
    End Function


    Function IsPointInsideLine(ByVal point As PointF, ByVal pointA As PointF, ByVal pointB As PointF, ByVal thickness As Single) As Boolean
        ' Calcular la distancia desde el punto a la línea
        Dim distance As Double = DistanceFromPointToLine(point, pointA, pointB)

        ' Verificar si la distancia es menor o igual al grosor y si el punto está dentro de los extremos de la línea
        If distance <= thickness AndAlso IsPointInsideLineEndpoints(point, pointA, pointB) Then
            Return True
        Else
            Return False
        End If
    End Function

    Function DistanceFromPointToLine(ByVal point As PointF, ByVal pointA As PointF, ByVal pointB As PointF) As Double
        ' Calcular las diferencias en las coordenadas X e Y
        Dim diffX As Single = pointB.X - pointA.X
        Dim diffY As Single = pointB.Y - pointA.Y

        ' Calcular el producto cruzado entre los vectores formados por los puntos
        Dim crossProduct As Single = (point.X - pointA.X) * diffY - (point.Y - pointA.Y) * diffX

        ' Calcular la longitud de la línea AB
        Dim lineLength As Double = Math.Sqrt(diffX * diffX + diffY * diffY)

        ' Calcular la distancia desde el punto a la línea dividiendo el producto cruzado por la longitud de la línea
        Dim distance As Double = Math.Abs(crossProduct) / lineLength

        Return distance
    End Function

    Function IsPointInsideLineEndpoints(ByVal point As PointF, ByVal pointA As PointF, ByVal pointB As PointF) As Boolean
        ' Verificar si el punto está dentro de los extremos de la línea
        If (point.X >= Math.Min(pointA.X, pointB.X) AndAlso point.X <= Math.Max(pointA.X, pointB.X)) AndAlso
       (point.Y >= Math.Min(pointA.Y, pointB.Y) AndAlso point.Y <= Math.Max(pointA.Y, pointB.Y)) Then
            Return True
        Else
            Return False
        End If
    End Function




    Public Sub DrawTable(ByRef g As Graphics, color As Color, x As Single, y As Single, columns As Integer, rows As Integer, width As Single, height As Single)
        Using p As New Pen(color)
            Dim x1, x2, y1, y2 As Single

            ' Verticales
            For xx = 0 To columns
                x1 = x + (xx * width)
                x2 = x1
                y1 = y
                y2 = y + (rows * height)
                g.DrawLine(p, x1, y1, x2, y2)
            Next

            ' Horizontales
            For yy = 0 To rows
                x1 = x
                x2 = x + (columns * width)
                y1 = y + (yy * height)
                y2 = y1
                g.DrawLine(p, x1, y1, x2, y2)
            Next
        End Using
    End Sub



    'Public Function GetAmps(volts As Double, res As Double)
    '    If res = 0 Then Return 0
    '    Return volts / res
    'End Function
    'Public Function GetVolts(amps As Double, res As Double)
    '    Return amps * res
    'End Function
    'Public Function GetRes(volts As Double, amps As Double)
    '    If amps = 0 Then Return 0
    '    Return volts / amps
    'End Function



End Module
