Imports Inventor
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Linq
Imports System.Xml
Imports System.Reflection
Imports System.ComponentModel
Imports System.Collections
Imports System.Collections.Generic
Imports System.Windows
Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.Win32


Public Class Form1
    Dim inventorApp As Inventor.Application
    Dim assemblyDoc As AssemblyDocument = Nothing

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            inventorApp = Marshal.GetActiveObject("Inventor.Application")
        Catch ex As Exception
            inventorApp = CreateObject("Inventor.Application")
        End Try
        MessageBox.Show("Connected with Inventor")
        assemblyDoc = TryCast(inventorApp.Documents.Add(DocumentTypeEnum.kAssemblyDocumentObject, "", True), PartDocument)
        inventorApp.Visible = True
    End Sub

    Dim Directory As String
    Dim filename As String
    Public Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            If (ofd.ShowDialog() = DialogResult.OK) Then
                Directory = ofd.FileName
                Dim FolderPath As String = Directory
                Debug.WriteLine(Directory)
            End If
            filename = Directory
            Dim cmdMgr As CommandManager
            cmdMgr = inventorApp.CommandManager
            Call cmdMgr.PostPrivateEvent(6657, filename)
            cmdMgr.ControlDefinitions.Item("AssemblyPlaceComponentCmd").Execute()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Function faceMate(ByRef inventorApp As Inventor.Application)
        Try
            Dim count As Integer
            Dim oDoc As AssemblyDocument
            oDoc = inventorApp.ActiveDocument
            Dim oFace1 As Face
            oFace1 = inventorApp.CommandManager.Pick(SelectionFilterEnum.kPartFacePlanarFilter, "Select a cylindrical face")
            Dim oFace2 As Face
            oFace2 = inventorApp.CommandManager.Pick(SelectionFilterEnum.kPartFacePlanarFilter, "Select another cylindrical face")
            Dim omate As MateConstraint
            omate = oDoc.ComponentDefinition.Constraints.AddMateConstraint(oFace1, oFace2, CInt(Int(TextBox1.Text)))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function

    Function faceFlush(ByRef inventorApp As Inventor.Application)
        Try
            Dim oDoc As AssemblyDocument
            oDoc = inventorApp.ActiveDocument
            Dim oFace1 As Face
            oFace1 = inventorApp.CommandManager.Pick(SelectionFilterEnum.kPartFacePlanarFilter, "Select a cylindrical face")
            Dim oFace2 As Face
            oFace2 = inventorApp.CommandManager.Pick(SelectionFilterEnum.kPartFacePlanarFilter, "Select another cylindrical face")
            Dim omate As FlushConstraint
            omate = oDoc.ComponentDefinition.Constraints.AddFlushConstraint(oFace1, oFace2, CInt(Int(TextBox1.Text)))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            inventorApp = Marshal.GetActiveObject("Inventor.Application")
        Catch ex As Exception
            MessageBox.Show("Inventor not connected")
        End Try
        Try
            If Me.ComboBox1.SelectedIndex = 0 Then
                faceMate(inventorApp)
            End If
            If Me.ComboBox1.SelectedIndex = 1 Then
                faceFlush(inventorApp)
            End If
            If Me.ComboBox1.SelectedIndex = 2 Then
                If (Me.ComboBox2.Text = Nothing) Then
                    MessageBox.Show("Please Select Plane 1")
                Else
                    planeMate(inventorApp)
                End If
            End If
            If Me.ComboBox1.SelectedIndex = 3 Then
                If (Me.ComboBox3.Text = Nothing) Then
                    MessageBox.Show("Please Select Plane 2")
                Else
                    planeFlush(inventorApp)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Function planeMate(ByRef inventorApp As Inventor.Application)
        Try
            Dim oAsmCompDef As AssemblyComponentDefinition
            oAsmCompDef = inventorApp.ActiveDocument.ComponentDefinition
            Dim compOcc1 As ComponentOccurrence = oAsmCompDef.Occurrences.Item(1)
            Dim compOcc2 As ComponentOccurrence = oAsmCompDef.Occurrences.Item(2)
            Dim oWorkPlane1 As WorkPlane
            oWorkPlane1 = compOcc1.Definition.WorkPlanes(ComboBox2.Text)
            Dim oWorkPlane2 As WorkPlane
            oWorkPlane2 = compOcc2.Definition.WorkPlanes(ComboBox3.Text)
            Dim oproxyWorkPlane1 As WorkPlaneProxy = Nothing
            compOcc1.CreateGeometryProxy(oWorkPlane1, oproxyWorkPlane1)
            Dim oproxyWorkPlane2 As WorkPlaneProxy = Nothing
            compOcc2.CreateGeometryProxy(oWorkPlane2, oproxyWorkPlane2)
            Dim oAConstraint As MateConstraint
            oAConstraint = oAsmCompDef.Constraints.AddMateConstraint(oproxyWorkPlane1, oproxyWorkPlane2, CInt(Int(TextBox1.Text)))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function

    Function planeFlush(ByRef inventorApp As Inventor.Application)
        Try
            Dim oAsmCompDef As AssemblyComponentDefinition
            oAsmCompDef = inventorApp.ActiveDocument.ComponentDefinition
            Dim compOcc1 As ComponentOccurrence = oAsmCompDef.Occurrences.Item(1)
            Dim compOcc2 As ComponentOccurrence = oAsmCompDef.Occurrences.Item(2)
            Dim oWorkPlane1 As WorkPlane
            oWorkPlane1 = compOcc1.Definition.WorkPlanes(ComboBox2.Text)
            Dim oWorkPlane2 As WorkPlane
            oWorkPlane2 = compOcc2.Definition.WorkPlanes(ComboBox3.Text)
            Dim oproxyWorkPlane1 As WorkPlaneProxy = Nothing
            compOcc1.CreateGeometryProxy(oWorkPlane1, oproxyWorkPlane1)
            Dim oproxyWorkPlane2 As WorkPlaneProxy = Nothing
            compOcc2.CreateGeometryProxy(oWorkPlane2, oproxyWorkPlane2)
            Dim oAConstraint As FlushConstraint
            oAConstraint = oAsmCompDef.Constraints.AddFlushConstraint(oproxyWorkPlane1, oproxyWorkPlane2, CInt(Int(TextBox1.Text)))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function

End Class
