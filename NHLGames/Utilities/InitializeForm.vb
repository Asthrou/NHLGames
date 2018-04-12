﻿Imports System.Globalization
Imports System.IO
Imports MetroFramework.Controls
Imports NHLGames.Controls
Imports NHLGames.Objects

Namespace Utilities
    Public Class InitializeForm
        Private ReadOnly Shared Form As NHLGamesMetro = NHLGamesMetro.FormInstance

        Public Async Shared Function VersionCheck() As Task(Of Boolean)

            Dim latestVersion As Version = Await Downloader.DownloadApplicationVersion()
            If latestVersion.Equals(New Version()) Then Return False

            If latestVersion > My.Application.Info.Version Then
                Form.lnkDownload.Text = String.Format(
                    NHLGamesMetro.RmText.GetString("msgNewVersionText"),
                    latestVersion.ToString())
                Form.lnkDownload.Width = 700
                Dim strChangeLog As String = Await Downloader.DownloadChangelog()
                InvokeElement.MsgBoxBlue(
                    String.Format(NHLGamesMetro.RmText.GetString("msgChangeLog"), latestVersion.ToString(), vbCrLf,
                                  vbCrLf, strChangeLog),
                    NHLGamesMetro.RmText.GetString("msgNewVersionAvailable"),
                    MessageBoxButtons.OK)
            End If

            Await AnnouncementCheck()
            Return True
        End Function

        Private Async Shared Function AnnouncementCheck() As Task(Of Boolean)
            Dim latestAnnouncement As String = Await Downloader.DownloadAnnouncement()
            If Not latestAnnouncement.Equals(String.Empty) Then
                InvokeElement.MsgBoxBlue(latestAnnouncement,
                                         NHLGamesMetro.RmText.GetString("msgAnnouncement"),
                                         MessageBoxButtons.OK)
            End If
            Return Not latestAnnouncement.Equals(String.Empty)
        End Function

        Public Shared Sub SetLanguage()
            Dim lstHostsFileActions = New String() { _
                                                       NHLGamesMetro.RmText.GetString("cbHostsTest"),
                                                       NHLGamesMetro.RmText.GetString("cbHostsAdd"),
                                                       NHLGamesMetro.RmText.GetString("cbHostsRemove"),
                                                       NHLGamesMetro.RmText.GetString("cbHostsView"),
                                                       NHLGamesMetro.RmText.GetString("cbHostsEntry"),
                                                       NHLGamesMetro.RmText.GetString("cbHostsLocation")
                                                   }

            Dim lstStreamQualities = New String() { _
                                                      NHLGamesMetro.RmText.GetString("cbQualitySuperb60fps"),
                                                      NHLGamesMetro.RmText.GetString("cbQualitySuperb"),
                                                      NHLGamesMetro.RmText.GetString("cbQualityGreat"),
                                                      NHLGamesMetro.RmText.GetString("cbQualityGood"),
                                                      NHLGamesMetro.RmText.GetString("cbQualityNormal"),
                                                      NHLGamesMetro.RmText.GetString("cbQualityLow"),
                                                      NHLGamesMetro.RmText.GetString("cbQualityMobile")
                                                  }

            Dim lstLiveReplayPreferences = New String() { _
                                                            NHLGamesMetro.RmText.GetString("cbLiveReplayPuckDrop"),
                                                            NHLGamesMetro.RmText.GetString("cbLiveReplayGameTime"),
                                                            NHLGamesMetro.RmText.GetString("cbLiveReplayFeedStart")
                                                        }

            'Main
            Form.tabMenu.TabPages.Item(0).Text = NHLGamesMetro.RmText.GetString("tabGames")
            Form.tabMenu.TabPages.Item(1).Text = NHLGamesMetro.RmText.GetString("tabSettings")
            Form.tabMenu.TabPages.Item(2).Text = NHLGamesMetro.RmText.GetString("tabConsole")
            Form.tt.SetToolTip(Form.btnHelp, NHLGamesMetro.RmText.GetString("tipHelp"))

            Form.lblNoGames.Text = NHLGamesMetro.RmText.GetString("lblNoGames")
            Form.lblStatus.Text = String.Format(NHLGamesMetro.RmText.GetString("msgGamesFound"),
                                                NHLGamesMetro.FormInstance.flpGames.Controls.Count())

            'Games
            Form.tt.SetToolTip(Form.btnYesterday, NHLGamesMetro.RmText.GetString("tipDayLeft"))
            Form.tt.SetToolTip(Form.btnDate, NHLGamesMetro.RmText.GetString("tipCalendar"))
            Form.tt.SetToolTip(Form.btnTomorrow, NHLGamesMetro.RmText.GetString("tipDayRight"))
            Form.tt.SetToolTip(Form.btnRefresh, NHLGamesMetro.RmText.GetString("tipRefresh"))

            'Settings
            Dim minutesBehind = Form.tbLiveRewind.Value*5
            Form.lblGamePanel.Text = NHLGamesMetro.RmText.GetString("lblShowScores")
            Form.lblPlayer.Text = NHLGamesMetro.RmText.GetString("lblPlayer")
            Form.lblQuality.Text = NHLGamesMetro.RmText.GetString("lblQuality")
            Form.lblCdn.Text = NHLGamesMetro.RmText.GetString("lblCdn")
            Form.lblHostname.Text = NHLGamesMetro.RmText.GetString("lblHostname")
            Form.lblHosts.Text = NHLGamesMetro.RmText.GetString("lblHosts")
            Form.lblVlcPath.Text = NHLGamesMetro.RmText.GetString("lblVlcPath")
            Form.lblMpcPath.Text = NHLGamesMetro.RmText.GetString("lblMpcPath")
            Form.lblMpvPath.Text = NHLGamesMetro.RmText.GetString("lblMpvPath")
            Form.lblSlPath.Text = NHLGamesMetro.RmText.GetString("lblSlPath")
            Form.lblOutput.Text = NHLGamesMetro.RmText.GetString("lblOutput")
            Form.lblPlayerArgs.Text = NHLGamesMetro.RmText.GetString("lblPlayerArgs")
            Form.lblStreamerArgs.Text = NHLGamesMetro.RmText.GetString("lblStreamerArgs")
            Form.lblLanguage.Text = NHLGamesMetro.RmText.GetString("lblLanguage")
            Form.lblShowLiveTime.Text = NHLGamesMetro.RmText.GetString("lblShowLiveTime")
            Form.lblUseAlternateCdn.Text = NHLGamesMetro.RmText.GetString("lblAlternateCdn")
            Form.lblLiveReplay.Text = NHLGamesMetro.RmText.GetString("lblLiveReplay")
            Form.lblLiveRewind.Text = NHLGamesMetro.RmText.GetString("lblLiveRewind")
            Form.lblLiveRewindDetails.Text = String.Format(
                NHLGamesMetro.RmText.GetString("lblLiveRewindDetails"),
                minutesBehind, Now.AddMinutes(- minutesBehind).ToString("h:mm tt", CultureInfo. InvariantCulture))

            Form.lblGamePanel.Text = NHLGamesMetro.RmText.GetString("lblGamePanel")
            Form.lblShowFinalScores.Text = NHLGamesMetro.RmText.GetString("lblShowFinalScores")
            Form.lblShowLiveScores.Text = NHLGamesMetro.RmText.GetString("lblShowLiveScores")
            Form.lblShowSeriesRecord.Text = NHLGamesMetro.RmText.GetString("lblShowSeriesRecord")
            Form.lblShowTeamCityAbr.Text = NHLGamesMetro.RmText.GetString("lblShowTeamCityAbr")
            Form.lblShowTodayLiveGamesFirst.Text = NHLGamesMetro.RmText.GetString("lblShowTodayLiveGamesFirst")

            Form.cbStreamQuality.Items.Clear()
            Form.cbStreamQuality.Items.AddRange(lstStreamQualities)
            Form.cbStreamQuality.SelectedIndex = 0

            Form.cbHostsFileActions.Items.Clear()
            Form.cbHostsFileActions.Items.AddRange(lstHostsFileActions)
            Form.cbHostsFileActions.SelectedIndex = 0

            Form.cbLiveReplay.Items.Clear()
            Form.cbLiveReplay.Items.AddRange(lstLiveReplayPreferences)
            Form.cbLiveReplay.SelectedIndex = 0

            Form.tt.SetToolTip(Form.lnkGetVlc, NHLGamesMetro.RmText.GetString("tipGetVlc"))
            Form.tt.SetToolTip(Form.lnkGetMpc, NHLGamesMetro.RmText.GetString("tipGetMpc"))
            Form.tt.SetToolTip(Form.btnHostsFileActions, NHLGamesMetro.RmText.GetString("tipHostsExecuteAction"))
            Form.tt.SetToolTip(Form.btnMPCPath, NHLGamesMetro.RmText.GetString("tipBrowse"))
            Form.tt.SetToolTip(Form.btnMpvPath, NHLGamesMetro.RmText.GetString("tipBrowse"))
            Form.tt.SetToolTip(Form.btnMPCPath, NHLGamesMetro.RmText.GetString("tipBrowse"))
            Form.tt.SetToolTip(Form.btnstreamerPath, NHLGamesMetro.RmText.GetString("tipBrowse"))
            Form.tt.SetToolTip(Form.btnOutput, NHLGamesMetro.RmText.GetString("tipBrowse"))

            Form.lblModules.Text = NHLGamesMetro.RmText.GetString("lblModules")
            Form.lblModulesDesc.Text = NHLGamesMetro.RmText.GetString("lblModulesDesc")
            Form.lblSpotify.Text = NHLGamesMetro.RmText.GetString("lblSpotify")
            Form.lblSpotifyDesc.Text = NHLGamesMetro.RmText.GetString("lblSpotifyDesc")
            Form.lblOBS.Text = NHLGamesMetro.RmText.GetString("lblObs")
            Form.lblOBSDesc.Text = NHLGamesMetro.RmText.GetString("lblObsDesc")
            Form.lblObsAdEndingHotkey.Text = NHLGamesMetro.RmText.GetString("lblObsAdEndingHotkey")
            Form.lblObsAdStartingHotkey.Text = NHLGamesMetro.RmText.GetString("lblObsAdStartingHotkey")
            Form.chkSpotifyForceToStart.Text = NHLGamesMetro.RmText.GetString("chkSpotifyForceToStart")
            Form.chkSpotifyPlayNextSong.Text = NHLGamesMetro.RmText.GetString("chkSpotifyPlayNextSong")
            Form.chkSpotifyAnyMediaPlayer.Text = NHLGamesMetro.RmText.GetString("chkSpotifyAnyMediaPlayer")

            'Console
            Form.btnCopyConsole.Text = NHLGamesMetro.RmText.GetString("btnCopyConsole")
            Form.btnClearConsole.Text = NHLGamesMetro.RmText.GetString("btnClearConsole")

            'Calendar
            Form.flpCalendarPanel.Controls.Clear()
            Form.flpCalendarPanel.Controls.Add(New CalendarControl())

            'RecordList
            Form.flpRecordList.Controls.Clear()
            Form.flpRecordList.Controls.Add(New OneRecordControl())
            Form.flpRecordList.Controls.Add(New OneRecordControl())
            Form.btnRecord.Text = Form.flpRecordList.Controls.Count
        End Sub

        Public Shared Sub SetWindow()
            Dim windowSize = Split(ApplicationSettings.Read (Of String)(SettingsEnum.LastWindowSize, "990;655"), ";")
            Form.Width = If (windowSize.Length = 2, Convert.ToInt32(windowSize(0)), 990)
            Form.Height = If (windowSize.Length = 2, Convert.ToInt32(windowSize(1)), 655)
        End Sub

        Public Shared Sub SetSettings()
            Dim lstLanguages = New String() { _
                                                NHLGamesMetro.RmText.GetString("cbEnglish"),
                                                NHLGamesMetro.RmText.GetString("cbFrench")
                                            }

            Form.cbLanguage.Items.Clear()
            Form.cbLanguage.Items.AddRange(lstLanguages)
            Form.cbLanguage.SelectedIndex = 0

            Form.lblVersion.Text = String.Format("v {0}.{1}.{2}",
                                                 My.Application.Info.Version.Major,
                                                 My.Application.Info.Version.Minor,
                                                 My.Application.Info.Version.Build)

            Form.txtMPCPath.Text = GetApplication(SettingsEnum.MpcPath, PathFinder.GetPathOfMpc())
            Form.txtVLCPath.Text = GetApplication(SettingsEnum.VlcPath, PathFinder.GetPathOfVlc())
            Form.txtMpvPath.Text = GetApplication(SettingsEnum.MpvPath, Path.Combine(Application.StartupPath, "mpv\mpv.exe"))
            Form.txtStreamerPath.Text = GetApplication(SettingsEnum.StreamerPath, Path.Combine(Application.StartupPath, "livestreamer\livestreamer.exe"))

            Form.tgShowFinalScores.Checked = ApplicationSettings.Read (Of Boolean)(SettingsEnum.ShowScores, False)
            Form.tgShowLiveScores.Checked = ApplicationSettings.Read (Of Boolean)(SettingsEnum.ShowLiveScores, False)
            Form.tgShowSeriesRecord.Checked = ApplicationSettings.Read (Of Boolean)(SettingsEnum.ShowSeriesRecord, False)
            Form.tgShowTeamCityAbr.Checked = ApplicationSettings.Read (Of Boolean)(SettingsEnum.ShowTeamCityAbr, False)
            Form.tgShowLiveTime.Checked = ApplicationSettings.Read (Of Boolean)(SettingsEnum.ShowLiveTime, False)
            Form.tgShowTodayLiveGamesFirst.Checked = ApplicationSettings.Read (Of Boolean)(SettingsEnum.ShowTodayLiveGamesFirst, False)

            Dim playersPath = New String() {Form.txtMpvPath.Text, Form.txtMPCPath.Text, Form.txtVLCPath.Text}
            Dim watchArgs = ApplicationSettings.Read (Of GameWatchArguments)(SettingsEnum.DefaultWatchArgs, Nothing)

            If ValidWatchArgs(watchArgs, playersPath, Form.txtStreamerPath.Text) Then
                Player.RenewArgs(True)
                watchArgs = ApplicationSettings.Read (Of GameWatchArguments)(SettingsEnum.DefaultWatchArgs, New GameWatchArguments)
            End If

            PopulateComboBox(Form.cbServers, SettingsEnum.SelectedServer, settingsenum.ServerList, String.Empty)
            Common.SetRedirectionServerInApp()

            BindWatchArgsToForm(watchArgs)

            Dim adDetectionConfigs = ApplicationSettings.Read (Of AdDetectionConfigs)(SettingsEnum.AdDetection, Nothing)

            If adDetectionConfigs Is Nothing Then
                AdDetection.Renew(True)
                adDetectionConfigs = ApplicationSettings.Read (Of AdDetectionConfigs)(SettingsEnum.AdDetection, New AdDetectionConfigs)
            End If

            BindAdDetectionConfigsToForm(adDetectionConfigs)

            Form.lblNoGames.Location = New Point(((Form.tabGames.Width - Form.lblNoGames.Width)/2), Form.tabGames.Height/2)
            Form.spnLoading.Location = New Point(((Form.tabGames.Width - Form.lblNoGames.Width)/2) + 40, (Form.tabGames.Height/2) - 20)

            Form.spnLoading.Value = NHLGamesMetro.SpnLoadingValue
            Form.spnLoading.Maximum = NHLGamesMetro.spnLoadingMaxValue
            Form.spnStreaming.Value = NHLGamesMetro.SpnStreamingValue
            Form.spnStreaming.Maximum = NHLGamesMetro.spnStreamingMaxValue
            Form.lblDate.Text = DateHelper.GetFormattedDate(NHLGamesMetro.GameDate)

            NHLGamesMetro.LabelDate = Form.lblDate
            NHLGamesMetro.GamesDownloadedTime = Now
        End Sub

        Private Shared Function GetApplication(varPath As SettingsEnum, currentPath As String)
            Dim savedPathFromConfig = ApplicationSettings.Read (Of String)(varPath, String.Empty)
            Dim currentPathIfFound As String = currentPath

            If File.Exists(savedPathFromConfig) Then Return savedPathFromConfig

            If File.Exists(currentPathIfFound) Then
                ApplicationSettings.SetValue(varPath, currentPathIfFound)
                Return currentPathIfFound
            Else
                ApplicationSettings.SetValue(varPath, String.Empty)
                Return String.Empty
            End If
        End Function

        Private Shared Sub PopulateComboBox(cb As MetroComboBox, selectedItem As SettingsEnum, items As SettingsEnum, defaultValue As String)
            Dim cbItemsFromConfig = ApplicationSettings.Read (Of String)(items, defaultValue)

            cb.Items.AddRange(cbItemsFromConfig.Split(";"))

            cb.SelectedItem = ApplicationSettings.Read (Of String)(selectedItem, String.Empty)
            If cb.SelectedItem Is Nothing Then
                cb.SelectedItem = cb.Items(0)
            End If
        End Sub

        Private Shared Function ValidWatchArgs(watchArgs As GameWatchArguments, playersPath As String(), streamerPath As String) As Boolean
            If watchArgs Is Nothing Then Return True

            Dim hasPlayerSet As Boolean = playersPath.Any(Function(x) x = watchArgs.PlayerPath)
            If Not watchArgs.streamerPath.Equals(streamerPath) Then Return True
            If Not hasPlayerSet Then
                watchArgs.PlayerType = PlayerTypeEnum.None
                watchArgs.StreamerType = StreamerTypeEnum.None
                watchArgs.PlayerPath = String.Empty
                watchArgs.StreamerPath = String.Empty
                Form.rbMPC.Enabled = False
                Form.rbVLC.Enabled = False
                Form.rbMpv.Enabled = False
                Return True
            End If

            Return False
        End Function

        Private Shared Sub BindWatchArgsToForm(watchArgs As GameWatchArguments)
            If watchArgs IsNot Nothing Then
                Form.cbStreamQuality.SelectedIndex = CType(watchArgs.Quality, Integer)

                Form.tgAlternateCdn.Checked = watchArgs.Cdn = CdnTypeEnum.L3C

                Form.tbLiveRewind.Value = If (watchArgs.StreamLiveRewind Mod 5 = 0, watchArgs.StreamLiveRewind/5, 1)

                Form.rbMpv.Checked = watchArgs.PlayerType = PlayerTypeEnum.Mpv AndAlso Form.rbMpv.Enabled
                Form.rbVLC.Checked = watchArgs.PlayerType = PlayerTypeEnum.Vlc AndAlso Form.rbVLC.Enabled
                Form.rbMPC.Checked = watchArgs.PlayerType = PlayerTypeEnum.Mpc AndAlso Form.rbMPC.Enabled

                If Form.rbVLC.Checked AndAlso watchArgs.PlayerPath <> Form.txtVLCPath.Text Then
                    Player.RenewArgs()
                ElseIf Form.rbMPC.Checked AndAlso watchArgs.PlayerPath <> Form.txtMPCPath.Text Then
                    Player.RenewArgs()
                ElseIf Form.rbMpv.Checked AndAlso watchArgs.PlayerPath <> Form.txtMpvPath.Text Then
                    Player.RenewArgs()
                End If

                Form.tgPlayer.Checked = watchArgs.UseCustomPlayerArgs
                Form.txtPlayerArgs.Enabled = watchArgs.UseCustomPlayerArgs
                Form.txtPlayerArgs.Text = watchArgs.CustomPlayerArgs

                Form.tgStreamer.Checked = watchArgs.UseCustomStreamerArgs
                Form.txtStreamerArgs.Enabled = watchArgs.UseCustomStreamerArgs
                Form.txtStreamerArgs.Text = watchArgs.CustomStreamerArgs

                Form.txtOutputArgs.Text = watchArgs.PlayerOutputPath
                Form.txtOutputArgs.Enabled = watchArgs.UseOutputArgs
                Form.tgOutput.Checked = watchArgs.UseOutputArgs
            End If
        End Sub

        Private Shared Sub BindAdDetectionConfigsToForm(configs As AdDetectionConfigs)
            If configs IsNot Nothing Then

                form.tgModules.Checked = configs.IsEnabled

                form.chkSpotifyForceToStart.Checked = configs.EnabledSpotifyForceToOpen
                form.chkSpotifyPlayNextSong.Checked = configs.EnabledSpotifyPlayNextSong
                Form.chkSpotifyAnyMediaPlayer.Checked = configs.EnabledSpotifyAndAnyMediaPlayer

                form.txtAdKey.Text = configs.EnabledObsAdSceneHotKey.Key
                form.chkAdCtrl.Checked = configs.EnabledObsAdSceneHotKey.Ctrl
                form.chkAdAlt.Checked = configs.EnabledObsAdSceneHotKey.Alt
                form.chkAdShift.Checked = configs.EnabledObsAdSceneHotKey.Shift

                form.txtGameKey.Text = configs.EnabledObsGameSceneHotKey.Key
                form.chkGameCtrl.Checked = configs.EnabledObsGameSceneHotKey.Ctrl
                form.chkGameAlt.Checked = configs.EnabledObsGameSceneHotKey.Alt
                form.chkGameShift.Checked = configs.EnabledObsGameSceneHotKey.Shift

                form.tgSpotify.Checked = configs.EnabledSpotifyModule
                form.tgOBS.Checked = configs.EnabledObsModule

            End If
        End Sub
    End Class
End Namespace
