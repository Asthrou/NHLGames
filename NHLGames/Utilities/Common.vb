﻿Imports System.IO
Imports System.Net
Imports System.Text
Imports NHLGames.My.Resources

Namespace Utilities
    Public Class Common
        Public Const UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36"

        Public Const Timeout = 10000

        Public Shared Function GetRandomString(intLength As Integer)
            Const s = "abcdefghijklmnopqrstuvwxyz0123456789"
            Dim r As New Random
            Dim sb As New StringBuilder

            For i = 1 To intLength
                Dim idx As Integer = r.Next(0, 35)
                sb.Append(s.Substring(idx, 1))
            Next

            Return sb.ToString()
        End Function

        Public Shared Function SetHttpWebRequest(address As String) As HttpWebRequest
            Dim defaultHttpWebRequest = CType(WebRequest.Create(New Uri(address)), HttpWebRequest)
            defaultHttpWebRequest.UserAgent = UserAgent
            defaultHttpWebRequest.Method = WebRequestMethods.Http.Head
            defaultHttpWebRequest.Proxy = WebRequest.DefaultWebProxy
            defaultHttpWebRequest.ContentType = "text/plain"
            defaultHttpWebRequest.CookieContainer = New CookieContainer()
            defaultHttpWebRequest.CookieContainer.Add(New Cookie("mediaAuth", GetRandomString(240), String.Empty, "nhl.com"))
            defaultHttpWebRequest.Timeout = Timeout
            defaultHttpWebRequest.Accept = "text/plain,text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8"
            defaultHttpWebRequest.Referer = "https://www.google.ca"
            defaultHttpWebRequest.Headers.Add("Origin", "https://wwww.google.ca")

            Return defaultHttpWebRequest
        End Function

        Public Shared Function SendWebRequest(address As String, Optional httpWebRequest As HttpWebRequest = Nothing) As Boolean
            If address Is Nothing AndAlso httpWebRequest Is Nothing Then Return False

            Dim myHttpWebRequest As HttpWebRequest
            Dim result = False

            If httpWebRequest Is Nothing Then
                myHttpWebRequest = SetHttpWebRequest(address)
            Else
                myHttpWebRequest = httpWebRequest
            End If

            Try
                Using myHttpWebResponse As HttpWebResponse = myHttpWebRequest.GetResponse()
                    result = (myHttpWebResponse.StatusCode = HttpStatusCode.OK)
                End Using
            Catch ex As Exception
            End Try

            Return result
        End Function

        Public Shared Async Function SendWebRequestAsync(address As String, Optional httpWebRequest As HttpWebRequest = Nothing) As Task(Of Boolean)
            If address Is Nothing AndAlso httpWebRequest Is Nothing Then Return False

            Dim myHttpWebRequest As HttpWebRequest
            Dim result = False

            If httpWebRequest Is Nothing Then
                myHttpWebRequest = SetHttpWebRequest(address)
            Else
                myHttpWebRequest = httpWebRequest
            End If

            Try
                Using myHttpWebResponse As HttpWebResponse = Await myHttpWebRequest.GetResponseAsync().ConfigureAwait(false)
                    result = (myHttpWebResponse.StatusCode = HttpStatusCode.OK)
                End Using
            Catch
            End Try

            Return result
        End Function

        Public Shared Async Function SendWebRequestAndGetContentAsync(address As String, Optional httpWebRequest As HttpWebRequest = Nothing) As Task(Of String)
            Dim content = New MemoryStream()
            Dim myHttpWebRequest As HttpWebRequest

            If httpWebRequest Is Nothing Then
                myHttpWebRequest = SetHttpWebRequest(address)
                myHttpWebRequest.Method = WebRequestMethods.Http.Get
            Else
                myHttpWebRequest = httpWebRequest
                myHttpWebRequest.Method = WebRequestMethods.Http.Get
            End If

            Try
                Using myHttpWebResponse As HttpWebResponse = Await myHttpWebRequest.GetResponseAsync().ConfigureAwait(false)
                    If myHttpWebResponse.StatusCode = HttpStatusCode.OK Then
                        Using reader As Stream = myHttpWebResponse.GetResponseStream()
                            reader.CopyTo(content)
                        End Using
                    End If
                End Using
            Catch
                content.Dispose()
                Return String.Empty
            End Try

            Dim result = Encoding.UTF8.GetString(content.ToArray())
            content.Dispose()

            Return result
        End Function

        Public Shared Sub GetLanguage()
            Dim lang = ApplicationSettings.Read (Of String)(SettingsEnum.SelectedLanguage, "English")

            If lang = NHLGamesMetro.RmText.GetString("cbEnglish") Then
                NHLGamesMetro.RmText = English.ResourceManager
            ElseIf lang = NHLGamesMetro.RmText.GetString("cbFrench") Then
                NHLGamesMetro.RmText = French.ResourceManager
            End If
        End Sub

        Public Async Shared Function CheckAppCanRun() As Task(Of Boolean)
            Dim errorMessage = String.Empty
            If NHLGamesMetro.ServerIp.Equals(String.Empty) Then
                errorMessage = "noGameServer"
            ElseIf Not Await SendWebRequestAsync("https://www.google.com") Then
                errorMessage = "noWebAccess"
            ElseIf Not Await InitializeForm.VersionCheck() Then
                errorMessage = "noAppServer"
            End If
            If Not errorMessage.Equals(String.Empty) Then
                FatalError(NHLGamesMetro.RmText.GetString(errorMessage))
                Console.WriteLine($"Status: {English.ResourceManager.GetString(errorMessage)}")
            End If
            Return errorMessage.Equals(String.Empty)
        End Function

        Public Shared Sub SetRedirectionServerInApp()
            NHLGamesMetro.HostName = NHLGamesMetro.FormInstance.cbServers.SelectedItem.ToString()
            HostsFile.SetServerIp()
            NHLGamesMetro.HostNameResolved = HostsFile.TestEntry()
            ApplicationSettings.SetValue(SettingsEnum.SelectedServer, NHLGamesMetro.FormInstance.cbServers.SelectedItem.ToString())
        End Sub

        Public Shared Sub CheckHostsFile
            If (HostsFile.TestEntry() = False) Then
                If HostsFile.EnsureAdmin() Then
                    If InvokeElement.MsgBoxBlue(NHLGamesMetro.RmText.GetString("msgHostnameSet"),
                                                NHLGamesMetro.RmText.GetString("msgAddHost"),
                                                MessageBoxButtons.YesNo) = DialogResult.Yes Then
                        HostsFile.AddEntry(False)
                    End If
                End If
            Else
                NHLGamesMetro.HostNameResolved = True
            End If
        End Sub

        Private Shared Sub FatalError(message As String)
            If InvokeElement.MsgBoxRed($"{message} {NHLGamesMetro.RmText.GetString("msgNotStarting")}",
                                       NHLGamesMetro.RmText.GetString("msgFailure"),
                                       MessageBoxButtons.YesNo) = DialogResult.Yes Then
                NHLGamesMetro.FormInstance.Close
            End If
        End Sub
    End Class
End Namespace
