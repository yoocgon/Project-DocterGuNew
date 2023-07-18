namespace KCureVDIDataBox
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            statusStrip = new StatusStrip();
            tsslLoginId = new ToolStripStatusLabel();
            tableLayoutPanel1 = new TableLayoutPanel();
            splitOuter = new SplitContainer();
            txtLog = new TextBox();
            splitBody = new SplitContainer();
            splitBodyContent = new SplitContainer();
            splitLeft = new SplitContainer();
            tlpClient = new TableLayoutPanel();
            label1 = new Label();
            txtClient = new TextBox();
            tvwClient = new TreeView();
            mnuPopup = new ContextMenuStrip(components);
            tsmiDownload = new ToolStripMenuItem();
            tsmiOpen = new ToolStripMenuItem();
            imageList1 = new ImageList(components);
            lvwClient = new ListView();
            colClientName = new ColumnHeader();
            colClientDateModified = new ColumnHeader();
            colClientType = new ColumnHeader();
            colClientSize = new ColumnHeader();
            splitRight = new SplitContainer();
            spltRightTop = new SplitContainer();
            tlpServerPrti = new TableLayoutPanel();
            txtServerPrti = new TextBox();
            lblServerPrti = new Label();
            tvwServerPrti = new TreeView();
            lvwServerPrti = new ListView();
            colServerName = new ColumnHeader();
            colServerDateModified = new ColumnHeader();
            colServerType = new ColumnHeader();
            colServerSize = new ColumnHeader();
            spltRightBottom = new SplitContainer();
            tlpServerCarry = new TableLayoutPanel();
            txtServerCarry = new TextBox();
            lblServerCarry = new Label();
            tvwServerCarry = new TreeView();
            lvwServerCarry = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            lvwStatus = new ListView();
            colStatusLocalFile = new ColumnHeader();
            colStatusRemoteFile = new ColumnHeader();
            colStatusSize = new ColumnHeader();
            colStatusStatus = new ColumnHeader();
            tlpCommands = new TableLayoutPanel();
            pnlCommandsLeft = new Panel();
            btnStop = new Button();
            prgSub = new ProgressBar();
            prgMain = new ProgressBar();
            pnlCommandsRight = new Panel();
            btnExit = new Button();
            pnlCommandsCenter = new Panel();
            btnRequestFile = new Button();
            statusStrip.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitOuter).BeginInit();
            splitOuter.Panel1.SuspendLayout();
            splitOuter.Panel2.SuspendLayout();
            splitOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitBody).BeginInit();
            splitBody.Panel1.SuspendLayout();
            splitBody.Panel2.SuspendLayout();
            splitBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitBodyContent).BeginInit();
            splitBodyContent.Panel1.SuspendLayout();
            splitBodyContent.Panel2.SuspendLayout();
            splitBodyContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitLeft).BeginInit();
            splitLeft.Panel1.SuspendLayout();
            splitLeft.Panel2.SuspendLayout();
            splitLeft.SuspendLayout();
            tlpClient.SuspendLayout();
            mnuPopup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitRight).BeginInit();
            splitRight.Panel1.SuspendLayout();
            splitRight.Panel2.SuspendLayout();
            splitRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spltRightTop).BeginInit();
            spltRightTop.Panel1.SuspendLayout();
            spltRightTop.Panel2.SuspendLayout();
            spltRightTop.SuspendLayout();
            tlpServerPrti.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spltRightBottom).BeginInit();
            spltRightBottom.Panel1.SuspendLayout();
            spltRightBottom.Panel2.SuspendLayout();
            spltRightBottom.SuspendLayout();
            tlpServerCarry.SuspendLayout();
            tlpCommands.SuspendLayout();
            pnlCommandsLeft.SuspendLayout();
            pnlCommandsRight.SuspendLayout();
            pnlCommandsCenter.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { tsslLoginId });
            statusStrip.Location = new Point(0, 831);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 21, 0);
            statusStrip.Size = new Size(1665, 26);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "StatusStrip";
            // 
            // tsslLoginId
            // 
            tsslLoginId.Name = "tsslLoginId";
            tsslLoginId.Size = new Size(46, 20);
            tsslLoginId.Text = "guest";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.Controls.Add(splitOuter, 0, 0);
            tableLayoutPanel1.Controls.Add(tlpCommands, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(1665, 831);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // splitOuter
            // 
            splitOuter.BorderStyle = BorderStyle.Fixed3D;
            splitOuter.Dock = DockStyle.Fill;
            splitOuter.Location = new Point(3, 4);
            splitOuter.Margin = new Padding(3, 4, 3, 4);
            splitOuter.Name = "splitOuter";
            splitOuter.Orientation = Orientation.Horizontal;
            // 
            // splitOuter.Panel1
            // 
            splitOuter.Panel1.Controls.Add(txtLog);
            // 
            // splitOuter.Panel2
            // 
            splitOuter.Panel2.Controls.Add(splitBody);
            splitOuter.Size = new Size(1659, 773);
            splitOuter.SplitterDistance = 83;
            splitOuter.SplitterWidth = 5;
            splitOuter.TabIndex = 1;
            // 
            // txtLog
            // 
            txtLog.Dock = DockStyle.Fill;
            txtLog.Location = new Point(0, 0);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(1655, 79);
            txtLog.TabIndex = 0;
            // 
            // splitBody
            // 
            splitBody.BorderStyle = BorderStyle.Fixed3D;
            splitBody.Dock = DockStyle.Fill;
            splitBody.Location = new Point(0, 0);
            splitBody.Margin = new Padding(3, 4, 3, 4);
            splitBody.Name = "splitBody";
            splitBody.Orientation = Orientation.Horizontal;
            // 
            // splitBody.Panel1
            // 
            splitBody.Panel1.Controls.Add(splitBodyContent);
            // 
            // splitBody.Panel2
            // 
            splitBody.Panel2.Controls.Add(lvwStatus);
            splitBody.Size = new Size(1659, 685);
            splitBody.SplitterDistance = 561;
            splitBody.SplitterWidth = 5;
            splitBody.TabIndex = 0;
            // 
            // splitBodyContent
            // 
            splitBodyContent.BorderStyle = BorderStyle.Fixed3D;
            splitBodyContent.Dock = DockStyle.Fill;
            splitBodyContent.Location = new Point(0, 0);
            splitBodyContent.Margin = new Padding(3, 4, 3, 4);
            splitBodyContent.Name = "splitBodyContent";
            // 
            // splitBodyContent.Panel1
            // 
            splitBodyContent.Panel1.Controls.Add(splitLeft);
            // 
            // splitBodyContent.Panel2
            // 
            splitBodyContent.Panel2.Controls.Add(splitRight);
            splitBodyContent.Size = new Size(1659, 561);
            splitBodyContent.SplitterDistance = 706;
            splitBodyContent.SplitterWidth = 6;
            splitBodyContent.TabIndex = 0;
            // 
            // splitLeft
            // 
            splitLeft.BorderStyle = BorderStyle.Fixed3D;
            splitLeft.Dock = DockStyle.Fill;
            splitLeft.Location = new Point(0, 0);
            splitLeft.Margin = new Padding(3, 4, 3, 4);
            splitLeft.Name = "splitLeft";
            splitLeft.Orientation = Orientation.Horizontal;
            // 
            // splitLeft.Panel1
            // 
            splitLeft.Panel1.Controls.Add(tlpClient);
            // 
            // splitLeft.Panel2
            // 
            splitLeft.Panel2.Controls.Add(lvwClient);
            splitLeft.Size = new Size(706, 561);
            splitLeft.SplitterDistance = 263;
            splitLeft.SplitterWidth = 5;
            splitLeft.TabIndex = 0;
            // 
            // tlpClient
            // 
            tlpClient.ColumnCount = 2;
            tlpClient.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 79F));
            tlpClient.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpClient.Controls.Add(label1, 0, 0);
            tlpClient.Controls.Add(txtClient, 1, 0);
            tlpClient.Controls.Add(tvwClient, 0, 1);
            tlpClient.Dock = DockStyle.Fill;
            tlpClient.Location = new Point(0, 0);
            tlpClient.Name = "tlpClient";
            tlpClient.RowCount = 2;
            tlpClient.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpClient.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpClient.Size = new Size(702, 259);
            tlpClient.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(73, 30);
            label1.TabIndex = 0;
            label1.Text = "Client:";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtClient
            // 
            txtClient.Dock = DockStyle.Fill;
            txtClient.Location = new Point(82, 3);
            txtClient.Name = "txtClient";
            txtClient.ReadOnly = true;
            txtClient.Size = new Size(617, 27);
            txtClient.TabIndex = 1;
            // 
            // tvwClient
            // 
            tlpClient.SetColumnSpan(tvwClient, 2);
            tvwClient.ContextMenuStrip = mnuPopup;
            tvwClient.Dock = DockStyle.Fill;
            tvwClient.FullRowSelect = true;
            tvwClient.HideSelection = false;
            tvwClient.ImageIndex = 0;
            tvwClient.ImageList = imageList1;
            tvwClient.Location = new Point(3, 34);
            tvwClient.Margin = new Padding(3, 4, 3, 4);
            tvwClient.Name = "tvwClient";
            tvwClient.SelectedImageIndex = 0;
            tvwClient.ShowLines = false;
            tvwClient.Size = new Size(696, 221);
            tvwClient.TabIndex = 0;
            // 
            // mnuPopup
            // 
            mnuPopup.ImageScalingSize = new Size(20, 20);
            mnuPopup.Items.AddRange(new ToolStripItem[] { tsmiDownload, tsmiOpen });
            mnuPopup.Name = "mnuPopup";
            mnuPopup.Size = new Size(160, 52);
            mnuPopup.Opening += mnuPopup_Opening;
            // 
            // tsmiDownload
            // 
            tsmiDownload.Name = "tsmiDownload";
            tsmiDownload.Size = new Size(159, 24);
            tsmiDownload.Text = "다운로드(&D)";
            tsmiDownload.Click += tsmiDownload_Click;
            // 
            // tsmiOpen
            // 
            tsmiOpen.Name = "tsmiOpen";
            tsmiOpen.Size = new Size(159, 24);
            tsmiOpen.Text = "열기(&O)";
            tsmiOpen.Click += tsmiOpen_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth16Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "disk");
            imageList1.Images.SetKeyName(1, "file");
            imageList1.Images.SetKeyName(2, "folder");
            imageList1.Images.SetKeyName(3, "folder_light");
            // 
            // lvwClient
            // 
            lvwClient.Columns.AddRange(new ColumnHeader[] { colClientName, colClientDateModified, colClientType, colClientSize });
            lvwClient.ContextMenuStrip = mnuPopup;
            lvwClient.Dock = DockStyle.Fill;
            lvwClient.FullRowSelect = true;
            lvwClient.LargeImageList = imageList1;
            lvwClient.Location = new Point(0, 0);
            lvwClient.Margin = new Padding(3, 4, 3, 4);
            lvwClient.Name = "lvwClient";
            lvwClient.Size = new Size(702, 289);
            lvwClient.SmallImageList = imageList1;
            lvwClient.TabIndex = 0;
            lvwClient.UseCompatibleStateImageBehavior = false;
            lvwClient.View = View.Details;
            // 
            // colClientName
            // 
            colClientName.Text = "이름";
            colClientName.Width = 300;
            // 
            // colClientDateModified
            // 
            colClientDateModified.Text = "수정일";
            colClientDateModified.Width = 100;
            // 
            // colClientType
            // 
            colClientType.Text = "유형";
            colClientType.Width = 100;
            // 
            // colClientSize
            // 
            colClientSize.Text = "크기";
            colClientSize.TextAlign = HorizontalAlignment.Right;
            // 
            // splitRight
            // 
            splitRight.BorderStyle = BorderStyle.Fixed3D;
            splitRight.Dock = DockStyle.Fill;
            splitRight.Location = new Point(0, 0);
            splitRight.Margin = new Padding(3, 4, 3, 4);
            splitRight.Name = "splitRight";
            splitRight.Orientation = Orientation.Horizontal;
            // 
            // splitRight.Panel1
            // 
            splitRight.Panel1.Controls.Add(spltRightTop);
            // 
            // splitRight.Panel2
            // 
            splitRight.Panel2.Controls.Add(spltRightBottom);
            splitRight.Size = new Size(947, 561);
            splitRight.SplitterDistance = 263;
            splitRight.SplitterWidth = 5;
            splitRight.TabIndex = 1;
            // 
            // spltRightTop
            // 
            spltRightTop.BackColor = SystemColors.Control;
            spltRightTop.Dock = DockStyle.Fill;
            spltRightTop.ForeColor = SystemColors.ControlText;
            spltRightTop.Location = new Point(0, 0);
            spltRightTop.Name = "spltRightTop";
            // 
            // spltRightTop.Panel1
            // 
            spltRightTop.Panel1.Controls.Add(tlpServerPrti);
            // 
            // spltRightTop.Panel2
            // 
            spltRightTop.Panel2.Controls.Add(lvwServerPrti);
            spltRightTop.Size = new Size(943, 259);
            spltRightTop.SplitterDistance = 486;
            spltRightTop.TabIndex = 0;
            // 
            // tlpServerPrti
            // 
            tlpServerPrti.ColumnCount = 2;
            tlpServerPrti.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 124F));
            tlpServerPrti.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpServerPrti.Controls.Add(txtServerPrti, 1, 0);
            tlpServerPrti.Controls.Add(lblServerPrti, 0, 0);
            tlpServerPrti.Controls.Add(tvwServerPrti, 0, 1);
            tlpServerPrti.Dock = DockStyle.Fill;
            tlpServerPrti.Location = new Point(0, 0);
            tlpServerPrti.Name = "tlpServerPrti";
            tlpServerPrti.RowCount = 2;
            tlpServerPrti.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpServerPrti.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpServerPrti.Size = new Size(486, 259);
            tlpServerPrti.TabIndex = 1;
            // 
            // txtServerPrti
            // 
            txtServerPrti.Dock = DockStyle.Fill;
            txtServerPrti.Location = new Point(127, 3);
            txtServerPrti.Name = "txtServerPrti";
            txtServerPrti.ReadOnly = true;
            txtServerPrti.Size = new Size(356, 27);
            txtServerPrti.TabIndex = 1;
            // 
            // lblServerPrti
            // 
            lblServerPrti.AutoSize = true;
            lblServerPrti.Dock = DockStyle.Fill;
            lblServerPrti.Location = new Point(3, 0);
            lblServerPrti.Name = "lblServerPrti";
            lblServerPrti.Size = new Size(118, 30);
            lblServerPrti.TabIndex = 0;
            lblServerPrti.Text = "참여기관전송:";
            lblServerPrti.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tvwServerPrti
            // 
            tlpServerPrti.SetColumnSpan(tvwServerPrti, 2);
            tvwServerPrti.ContextMenuStrip = mnuPopup;
            tvwServerPrti.Dock = DockStyle.Fill;
            tvwServerPrti.FullRowSelect = true;
            tvwServerPrti.HideSelection = false;
            tvwServerPrti.ImageIndex = 0;
            tvwServerPrti.ImageList = imageList1;
            tvwServerPrti.Location = new Point(3, 34);
            tvwServerPrti.Margin = new Padding(3, 4, 3, 4);
            tvwServerPrti.Name = "tvwServerPrti";
            tvwServerPrti.PathSeparator = "/";
            tvwServerPrti.SelectedImageIndex = 0;
            tvwServerPrti.ShowLines = false;
            tvwServerPrti.Size = new Size(480, 221);
            tvwServerPrti.TabIndex = 1;
            // 
            // lvwServerPrti
            // 
            lvwServerPrti.Columns.AddRange(new ColumnHeader[] { colServerName, colServerDateModified, colServerType, colServerSize });
            lvwServerPrti.ContextMenuStrip = mnuPopup;
            lvwServerPrti.Dock = DockStyle.Fill;
            lvwServerPrti.FullRowSelect = true;
            lvwServerPrti.Location = new Point(0, 0);
            lvwServerPrti.Margin = new Padding(3, 4, 3, 4);
            lvwServerPrti.Name = "lvwServerPrti";
            lvwServerPrti.Size = new Size(453, 259);
            lvwServerPrti.SmallImageList = imageList1;
            lvwServerPrti.TabIndex = 1;
            lvwServerPrti.UseCompatibleStateImageBehavior = false;
            lvwServerPrti.View = View.Details;
            // 
            // colServerName
            // 
            colServerName.Text = "이름";
            colServerName.Width = 200;
            // 
            // colServerDateModified
            // 
            colServerDateModified.Text = "수정일";
            colServerDateModified.Width = 100;
            // 
            // colServerType
            // 
            colServerType.Text = "유형";
            colServerType.Width = 100;
            // 
            // colServerSize
            // 
            colServerSize.Text = "크기";
            // 
            // spltRightBottom
            // 
            spltRightBottom.Dock = DockStyle.Fill;
            spltRightBottom.Location = new Point(0, 0);
            spltRightBottom.Name = "spltRightBottom";
            // 
            // spltRightBottom.Panel1
            // 
            spltRightBottom.Panel1.Controls.Add(tlpServerCarry);
            // 
            // spltRightBottom.Panel2
            // 
            spltRightBottom.Panel2.Controls.Add(lvwServerCarry);
            spltRightBottom.Size = new Size(943, 289);
            spltRightBottom.SplitterDistance = 486;
            spltRightBottom.TabIndex = 1;
            // 
            // tlpServerCarry
            // 
            tlpServerCarry.ColumnCount = 2;
            tlpServerCarry.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 124F));
            tlpServerCarry.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpServerCarry.Controls.Add(txtServerCarry, 1, 0);
            tlpServerCarry.Controls.Add(lblServerCarry, 0, 0);
            tlpServerCarry.Controls.Add(tvwServerCarry, 0, 1);
            tlpServerCarry.Dock = DockStyle.Fill;
            tlpServerCarry.Location = new Point(0, 0);
            tlpServerCarry.Name = "tlpServerCarry";
            tlpServerCarry.RowCount = 2;
            tlpServerCarry.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpServerCarry.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpServerCarry.Size = new Size(486, 289);
            tlpServerCarry.TabIndex = 1;
            // 
            // txtServerCarry
            // 
            txtServerCarry.Dock = DockStyle.Fill;
            txtServerCarry.Location = new Point(127, 3);
            txtServerCarry.Name = "txtServerCarry";
            txtServerCarry.ReadOnly = true;
            txtServerCarry.Size = new Size(356, 27);
            txtServerCarry.TabIndex = 1;
            // 
            // lblServerCarry
            // 
            lblServerCarry.AutoSize = true;
            lblServerCarry.Dock = DockStyle.Fill;
            lblServerCarry.Location = new Point(3, 0);
            lblServerCarry.Name = "lblServerCarry";
            lblServerCarry.Size = new Size(118, 30);
            lblServerCarry.TabIndex = 0;
            lblServerCarry.Text = "반입:";
            lblServerCarry.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tvwServerCarry
            // 
            tlpServerCarry.SetColumnSpan(tvwServerCarry, 2);
            tvwServerCarry.ContextMenuStrip = mnuPopup;
            tvwServerCarry.Dock = DockStyle.Fill;
            tvwServerCarry.FullRowSelect = true;
            tvwServerCarry.HideSelection = false;
            tvwServerCarry.ImageIndex = 0;
            tvwServerCarry.ImageList = imageList1;
            tvwServerCarry.Location = new Point(3, 34);
            tvwServerCarry.Margin = new Padding(3, 4, 3, 4);
            tvwServerCarry.Name = "tvwServerCarry";
            tvwServerCarry.PathSeparator = "/";
            tvwServerCarry.SelectedImageIndex = 0;
            tvwServerCarry.ShowLines = false;
            tvwServerCarry.Size = new Size(480, 251);
            tvwServerCarry.TabIndex = 1;
            // 
            // lvwServerCarry
            // 
            lvwServerCarry.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            lvwServerCarry.ContextMenuStrip = mnuPopup;
            lvwServerCarry.Dock = DockStyle.Fill;
            lvwServerCarry.FullRowSelect = true;
            lvwServerCarry.Location = new Point(0, 0);
            lvwServerCarry.Margin = new Padding(3, 4, 3, 4);
            lvwServerCarry.Name = "lvwServerCarry";
            lvwServerCarry.Size = new Size(453, 289);
            lvwServerCarry.SmallImageList = imageList1;
            lvwServerCarry.TabIndex = 1;
            lvwServerCarry.UseCompatibleStateImageBehavior = false;
            lvwServerCarry.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "이름";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "수정일";
            columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "유형";
            columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "크기";
            // 
            // lvwStatus
            // 
            lvwStatus.Columns.AddRange(new ColumnHeader[] { colStatusLocalFile, colStatusRemoteFile, colStatusSize, colStatusStatus });
            lvwStatus.Dock = DockStyle.Fill;
            lvwStatus.FullRowSelect = true;
            lvwStatus.Location = new Point(0, 0);
            lvwStatus.Margin = new Padding(3, 4, 3, 4);
            lvwStatus.Name = "lvwStatus";
            lvwStatus.Size = new Size(1655, 115);
            lvwStatus.TabIndex = 0;
            lvwStatus.UseCompatibleStateImageBehavior = false;
            lvwStatus.View = View.Details;
            // 
            // colStatusLocalFile
            // 
            colStatusLocalFile.Text = "로컬 파일";
            colStatusLocalFile.Width = 550;
            // 
            // colStatusRemoteFile
            // 
            colStatusRemoteFile.Text = "원격 파일";
            colStatusRemoteFile.Width = 450;
            // 
            // colStatusSize
            // 
            colStatusSize.Text = "크기";
            colStatusSize.TextAlign = HorizontalAlignment.Right;
            colStatusSize.Width = 100;
            // 
            // colStatusStatus
            // 
            colStatusStatus.Text = "상태";
            colStatusStatus.TextAlign = HorizontalAlignment.Right;
            colStatusStatus.Width = 100;
            // 
            // tlpCommands
            // 
            tlpCommands.ColumnCount = 3;
            tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 169F));
            tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tlpCommands.Controls.Add(pnlCommandsLeft, 0, 0);
            tlpCommands.Controls.Add(pnlCommandsRight, 2, 0);
            tlpCommands.Controls.Add(pnlCommandsCenter, 1, 0);
            tlpCommands.Dock = DockStyle.Fill;
            tlpCommands.Location = new Point(3, 784);
            tlpCommands.Name = "tlpCommands";
            tlpCommands.RowCount = 1;
            tlpCommands.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCommands.Size = new Size(1659, 44);
            tlpCommands.TabIndex = 2;
            // 
            // pnlCommandsLeft
            // 
            pnlCommandsLeft.Controls.Add(btnStop);
            pnlCommandsLeft.Controls.Add(prgSub);
            pnlCommandsLeft.Controls.Add(prgMain);
            pnlCommandsLeft.Dock = DockStyle.Fill;
            pnlCommandsLeft.Location = new Point(3, 3);
            pnlCommandsLeft.Name = "pnlCommandsLeft";
            pnlCommandsLeft.Size = new Size(739, 38);
            pnlCommandsLeft.TabIndex = 13;
            // 
            // btnStop
            // 
            btnStop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStop.Font = new Font("Wingdings 2", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            btnStop.ForeColor = SystemColors.Highlight;
            btnStop.Location = new Point(692, 0);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(44, 30);
            btnStop.TabIndex = 13;
            btnStop.Text = "Ò";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Visible = false;
            btnStop.Click += btnStop_Click;
            // 
            // prgSub
            // 
            prgSub.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            prgSub.Location = new Point(3, 2);
            prgSub.Name = "prgSub";
            prgSub.Size = new Size(682, 12);
            prgSub.TabIndex = 12;
            prgSub.Visible = false;
            // 
            // prgMain
            // 
            prgMain.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            prgMain.Location = new Point(3, 17);
            prgMain.Name = "prgMain";
            prgMain.Size = new Size(682, 12);
            prgMain.TabIndex = 12;
            prgMain.Visible = false;
            // 
            // pnlCommandsRight
            // 
            pnlCommandsRight.Controls.Add(btnExit);
            pnlCommandsRight.Dock = DockStyle.Fill;
            pnlCommandsRight.Location = new Point(917, 3);
            pnlCommandsRight.Name = "pnlCommandsRight";
            pnlCommandsRight.Size = new Size(739, 38);
            pnlCommandsRight.TabIndex = 17;
            // 
            // btnExit
            // 
            btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExit.DialogResult = DialogResult.Cancel;
            btnExit.Location = new Point(627, 0);
            btnExit.Margin = new Padding(4, 5, 4, 5);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(108, 30);
            btnExit.TabIndex = 17;
            btnExit.Text = "종료(&E)";
            btnExit.Click += btnExit_Click;
            // 
            // pnlCommandsCenter
            // 
            pnlCommandsCenter.Controls.Add(btnRequestFile);
            pnlCommandsCenter.Dock = DockStyle.Fill;
            pnlCommandsCenter.Location = new Point(748, 3);
            pnlCommandsCenter.Name = "pnlCommandsCenter";
            pnlCommandsCenter.Size = new Size(163, 38);
            pnlCommandsCenter.TabIndex = 18;
            // 
            // btnRequestFile
            // 
            btnRequestFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnRequestFile.DialogResult = DialogResult.Cancel;
            btnRequestFile.Location = new Point(0, 0);
            btnRequestFile.Margin = new Padding(4, 5, 4, 5);
            btnRequestFile.Name = "btnRequestFile";
            btnRequestFile.Size = new Size(163, 30);
            btnRequestFile.TabIndex = 16;
            btnRequestFile.Text = "반출 신청(&R)";
            btnRequestFile.Click += btnRequestFile_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1665, 857);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(statusStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            Margin = new Padding(6, 5, 6, 5);
            Name = "MainForm";
            Text = "MainForm";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            splitOuter.Panel1.ResumeLayout(false);
            splitOuter.Panel1.PerformLayout();
            splitOuter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitOuter).EndInit();
            splitOuter.ResumeLayout(false);
            splitBody.Panel1.ResumeLayout(false);
            splitBody.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitBody).EndInit();
            splitBody.ResumeLayout(false);
            splitBodyContent.Panel1.ResumeLayout(false);
            splitBodyContent.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitBodyContent).EndInit();
            splitBodyContent.ResumeLayout(false);
            splitLeft.Panel1.ResumeLayout(false);
            splitLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitLeft).EndInit();
            splitLeft.ResumeLayout(false);
            tlpClient.ResumeLayout(false);
            tlpClient.PerformLayout();
            mnuPopup.ResumeLayout(false);
            splitRight.Panel1.ResumeLayout(false);
            splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitRight).EndInit();
            splitRight.ResumeLayout(false);
            spltRightTop.Panel1.ResumeLayout(false);
            spltRightTop.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)spltRightTop).EndInit();
            spltRightTop.ResumeLayout(false);
            tlpServerPrti.ResumeLayout(false);
            tlpServerPrti.PerformLayout();
            spltRightBottom.Panel1.ResumeLayout(false);
            spltRightBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)spltRightBottom).EndInit();
            spltRightBottom.ResumeLayout(false);
            tlpServerCarry.ResumeLayout(false);
            tlpServerCarry.PerformLayout();
            tlpCommands.ResumeLayout(false);
            pnlCommandsLeft.ResumeLayout(false);
            pnlCommandsRight.ResumeLayout(false);
            pnlCommandsCenter.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsslLoginId;
        private TableLayoutPanel tableLayoutPanel1;
        private SplitContainer splitOuter;
        private SplitContainer splitBody;
        private SplitContainer splitBodyContent;
        private SplitContainer splitLeft;
        private TreeView tvwClient;
        private ListView lvwClient;
        private ColumnHeader colClientName;
        private ColumnHeader colClientDateModified;
        private ColumnHeader colClientType;
        private ColumnHeader colClientSize;
        private SplitContainer splitRight;
        private TreeView tvwServerPrti;
        private ListView lvwServerPrti;
        private ColumnHeader colServerName;
        private ColumnHeader colServerDateModified;
        private ColumnHeader colServerType;
        private ColumnHeader colServerSize;
        private ListView lvwStatus;
        private ColumnHeader colStatusLocalFile;
        private ColumnHeader colStatusRemoteFile;
        private ColumnHeader colStatusSize;
        private ColumnHeader colStatusStatus;
        private ContextMenuStrip mnuPopup;
        private ToolStripMenuItem tsmiDownload;
        private ImageList imageList1;
        private ToolStripMenuItem tsmiOpen;
        private TableLayoutPanel tlpCommands;
        private TextBox txtLog;
        private Panel pnlCommandsLeft;
        private Button btnStop;
        private ProgressBar prgSub;
        private ProgressBar prgMain;
        private Button btnRequestFile;
        private Panel pnlCommandsRight;
        private Button btnExit;
        private Panel pnlCommandsCenter;
        private TableLayoutPanel tlpClient;
        private Label label1;
        private TextBox txtClient;
        private TableLayoutPanel tlpServerPrti;
        private Label lblServerPrti;
        private TextBox txtServerPrti;
        private SplitContainer spltRightTop;
        private SplitContainer spltRightBottom;
        private TableLayoutPanel tlpServerCarry;
        private TextBox txtServerCarry;
        private Label lblServerCarry;
        private TreeView tvwServerCarry;
        private ListView lvwServerCarry;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
    }
}



