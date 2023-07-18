# K-CURE Data Access

# AboutBox.cs

### partial class AboutBox : Form
현재 프로그램의 경로 정보를 표시

#### private string GetLocation()
현재 프로그램의 경로(실행 파일, 설정 파일, 로그 파일) 정보를 반환

#### public string AssemblyTitle
제목

#### public string AssemblyVersion
버전

#### public string AssemblyDescription
설명

#### public string AssemblyProduct
제품

#### public string AssemblyCopyright
저작권

#### public string AssemblyCompany
회사

# CApi.cs

### public class CApi
HTTP 요청을 통해 원격 서버와 상호 작용하는 API 함수
로그인, 파일 반출 용 API에서 공통으로 사용하는 부분을 따로 떼어냄

#### public CApi()
API Key, Value를 App.config에서 읽어와 설정

#### internal async Task<(bool, string, T)> Send<T>(string url, string jsonContent) where T : new()
API 호출에 공통으로 사용되는 함수

#### internal async Task<(bool, string, LoginResult)> Login(string id, string password)
로그인

#### internal async Task<(bool, string, AplyResponse)> Aply(string dataAplyNo, string userId, List<string> cryOutFiles)
파일 반출

# CCommon.cs

### public enum ConfigAppKeys
App.config 파일에서 사용된 키 목록

### public class CCommon
공통으로 사용되는 함수

##### public const string ConfigXmlUser = "ConfigUser.xml";
사용자의 선택에 따라 변경되는 설정 파일명

##### public const string ConfigXmlApp = "ConfigApp.xml";
배포시에 확정되어 변경 불가능한 설정 파일명

##### public const string USER_AU006 = "AU006";
일반 연구자

##### public const string REVIEWER_AU005 = "AU005";
심의 의원

#### public static string LoginId { get; set; }
로그인 아이디

#### public static bool useLocalhost { get; set; }
로그인 시에 localhost 사용을 선택했는 지 여부

#### public static LoginResult LoginResult { get; set; } = new LoginResult();
로그인 성공 시에 받아온 데이터

#### private static CXmlConfig _XmlConfigUser;
사용자용 설정 파일 객체

#### public static string GetDefaultFullPath()
반출 신청에 사용될 기본 폴더를 생성

#### public static string ReplaceVar(string value)
App.config의 값에 있는 {id}와 {date} 변수를 실제 값으로 치환

#### public static List<DriveInfo> GetDrives()
고정 드라이브 목록을 반환

#### public static string GetConfigUserFullPath()
ConfigUser.xml의 전체 경로를 반환

#### public static void WriteLog(TextBox txtLog, string text)
TextBox에 로그를 기록하고, 로그 파일에도 기록함.

#### public static CXmlConfig XmlConfigUser
ConfigUser.xml 파일 객체를 반환

#### public static T GetAppSetting<T>(ConfigAppKeys configAppKey, T defaultValue)
App.config 파일에서 키에 해당하는 값을 타입에 맞게 변환해서 반환

#### public static string CreateZip(string localDirectory)
파일을 압축하고 압축한 파일의 전체 경로를 반환

#### public static bool IsDebugMode
현재 Debug Mode인지, Release Mode인지를 확인함.
주의할 것은 DLL 안에 이 함수가 있다면 호출하는 실행파일이
Debug Mode라도 Debug Mode가 아닌 것으로 리턴함.

# CFtpClient.cs

### public class CFtpClient
FTP용 라이브러리인 FluentFTP와 SFTP용 라이브러리인 Renci.SshNet을 동시에 사용하기 위한 래퍼 클래스

#### private FtpClient? ftpClient;
FluentFTP용 FTP 클라이언트

#### private SftpClient? sftpClient;
Renci.SshNet용 SFTP 클라이언트

#### public CFtpClient(string host, string user, string pass, int port, Encoding encoding)
클래스 초기화 (pass 값이 없다면 SFTP로 인정)

#### public bool DirectoryExists(string path)
폴더가 존재하는 지 여부를 반환 (SshNet은 해당 기능이 존재하지 않으므로 에러가 발생하는 지 여부로 알아냄)

#### public bool FileExists(string path)
파일이 존재하는 지 여부를 반환 (SshNet은 폴더와 파일의 구분이 없으므로 DirectoryExists를 추가로 호출함)

#### public bool CreateDirectory(string path)
폴더와 하위 폴더를 생성 (SshNet은 해당 기능이 존재하지 않으므로 재귀적으로 폴더를 생성함)

#### public void DeleteDirectory(string path)
폴더와 하위 폴더를 삭제 (SshNet은 해당 기능이 존재하지 않으므로 재귀적으로 폴더를 삭제함)

#### public void DeleteFile(string path)
파일 삭제

#### public void SetWorkingDirectory(string path)
현재 경로를 설정

#### public FtpListItem[] GetListing(string path, FtpListOption option)
path의 하위 폴더와 파일 목록 전체를 반환 (SshNet은 해당 기능이 존재하지 않으므로 재귀적으로 폴더와 파일 목록을 가져옴)

#### private FtpListItem? GetFtpListItem(SftpFile item)
SshNet인 경우 .과 ..을 제외한 폴더와 파일을 FtpListItem 형식으로 변환해서 반환

#### public FtpStatus DownloadFile(string localPath, string remotePath, FtpLocalExists localExists, Action<FtpProgress> progress)
파일 다운로드 (SshNet은 다운로드할 폴더가 없으면 자동 생성하는 기능, 파일이 이미 존재하면 어떤 선택을 할 지의 기능이 존재하지 않으므로 직접 구현)

#### public FtpStatus UploadFile(string localPath, string remotePath, FtpRemoteExists remoteExists, Action<FtpProgress> progress)
파일 업로드 (SshNet은 파일이 이미 존재하면 어떤 선택을 할 지의 기능, 이어쓰기 기능이 존재하지 않으므로 직접 구현)

#### private static string GetPrivateKeyFullPath()
SFTP에서 필요한 Private Key를 반환

#### private void CreateDirectoryRecursively(string path)
SshNet은 폴더를 재귀적으로 생성하는 기능이 없으므로 직접 구현

#### private void DeleteDirectoryRecursively(string path)
SshNet은 폴더를 재귀적으로 삭제하는 기능이 없으므로 직접 구현

#### private void DownloadFileResumable(string remotePath, Stream localStream, Action<long> progress)
SshNet은 다운로드 시 이어쓰기 기능이 없으므로 직접 구현

#### private void UploadFileResumable(string remotePath, Stream localStream, Action<long> progress)
SshNet은 다운로드 시 이어쓰기 기능이 없으므로 직접 구현

# ChooseFtpExists.cs

### partial class ChooseFtpExists : Form
FTP 파일 전송 시에 파일이 이미 존재할 때의 처리 방법을 선택하는 폼

#### public string FullPath { get; set; } = "";
이미 존재하는 파일의 전체 경로

#### public FtpLocalExists LocalExists { get; private set; } = FtpLocalExists.Overwrite;
로컬 파일이 이미 존재할 때의 처리 방법

#### public FtpRemoteExists RemoteExists
원격지 파일이 이미 존재할 때의 처리 방법

#### public bool ApplyToAll { get; set; } = false;
이후 모든 파일에 대해 동일한 처리 방법을 적용할 지 여부

#### private void btnOk_Click(object sender, EventArgs e)
처리 방법을 적용

#### private void btnCancel_Click(object sender, EventArgs e)
취소

#### private void RestoreSettings()
이전 다운로드/업로드 시에 선택한 처리 방법을 복원해서 적용

#### private void SaveSettings()
이후 다운로드/업로드 시에 표시할 선택 방법을 XML에 저장

#### private FtpLocalExists GetLocalExists()
선택한 처리 방법을 가져옴

# CUiClient.cs

### public class CUiClient
로컬 파일용 Helper

#### public CUiClient(TextBox txtClient, TreeView tvwClient, ListView lvwClient) {
관련된 모든 컨트롤을 초기화 시에 메모리에 저장하고 재사용하기 위해 지역 변수에 저장, 이벤트 지정

#### private void tvwClient_BeforeExpand(object? sender, TreeViewCancelEventArgs e)
노드를 펼치기 전에 임시 노드인 _KEY_TEMP를 제거하고 자식 노드를 추가

#### private void tvwClient_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
TreeView의 폴더 노드를 선택하면 해당 폴더 안의 하위 폴더와 파일 목록을 ListView에 추가

#### private void tvwClient_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
우클릭 시에도 해당 노드를 선택하기 위한 이벤트

#### private void lvwClient_DoubleClick(object? sender, EventArgs e)
ListView에서 하위 폴더를 더블클릭하면 TreeView에서도 해당 폴더를 선택하기

#### public void AddDrives(TreeView tvw)
고정 드라이브 목록을 TreeView에 추가

#### public void AddDirectories(TreeNode parent, string parentFullPath)
상위 폴더에 대한 하위 폴더 목록을 TreeView에 추가

#### public List<ListViewItem> AddDirectories(ListView lvw, string parentFullPath)
상위 폴더에 대한 하위 폴더 목록을 ListView에 추가

#### public void AddFiles(ListView lvw, string parentFullPath)
상위 폴더에 대한 파일 목록을 ListView에 추가

#### public List<DirectoryInfo> GetDirectories(string parentFullPath)
상위 폴더에 대한 하위 폴더 목록을 반환

#### public List<FileInfo> GetFiles(string parentFullPath)
상위 폴더에 대한 파일 목록을 반환

#### public void CreateDefaultFolder()
파일 반출에 사용할 폴더를 생성

#### public void SelectDefaultFolder()
파일 반출에 사용할 폴더를 선택

# CUiCommon.cs

### public class CUiCommon
UI에서 공통으로 사용하는 기능

#### public static void SelectTreeFolderByItem(TreeView tvw, ListViewItem item)
TreeView에서 선택한 폴더의 하위 폴더와 파일 목록이 ListView에 표시된 상황에서,
ListView의 특정 폴더를 선택했다면 TreeView의 폴더를 펼치고 해당 하위 폴더를 선택

# CUiServer.cs

### public class CUiServer
FTP 서버용 Helper (참여기관전송, 반입에서 공통 이용)

#### public CUiServer(bool forCarry, Form frm,
관련된 모든 컨트롤을 초기화 시에 메모리에 저장하고 재사용하기 위해 지역 변수에 저장, 이벤트 지정

#### private void tvwServer_BeforeExpand(object? sender, TreeViewCancelEventArgs e)
노드를 펼치기 전에 임시 노드인 _KEY_TEMP를 제거하고 자식 노드를 추가

#### private void tvwServer_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
TreeView의 폴더 노드를 선택하면 해당 폴더 안의 하위 폴더와 파일 목록을 ListView에 추가

#### private void tvwServer_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
우클릭 시에도 해당 노드를 선택하기 위한 이벤트

#### private void lvwServer_DoubleClick(object? sender, EventArgs e)
ListView에서 하위 폴더를 더블클릭하면 TreeView에서도 해당 폴더를 선택하기

#### private CFtpClient GetFtpClient()
FTP용 클라이언트 객체를 생성하고 반환

#### private Encoding GetEncoding(string encoding)
encoding 문자열에 따른 Encoding 객체를 반환

#### public void AddRoot(TreeView tvw)
참여기관전송, 반입 여부에 따라 달라지는 루트 폴더를 반환 (없다면 생성)

#### private void AddListItems(TreeNode parent, string parentFullPath, FtpObjectType type)
TreeView의 특정 노드의 하위 폴더와 파일 목록 노드를 추가
(폴더인 경우엔 임시 노드를 추가해서 BeforeExpand 이벤트가 일어나게 함)

#### private List<ListViewItem> AddListItems(ListView lvw, string parentFullPath, FtpObjectType type)
ListView의 특정 노드의 하위 폴더와 파일 목록 노드를 추가

#### private List<FtpListItem> GetListItems(string parentFullPath, FtpObjectType type)
parentFullPath의 하위 폴더와 파일 목록을 이름 순으로 반환
type에 해당하는 것만 반환

#### private List<FtpListItem> GetListItems(string parentFullPath)
parentFullPath의 하위 폴더와 파일 목록을 이름 순으로 반환

#### public (bool, long, int) DownloadFiles(string localDirectory, List<FtpListItem> ftpItems, FtpLocalExists? localExistsAll = null)
파일을 다운로드 (폴더인 경우엔 재귀 호출 사용)

#### public (bool, List<FileInfo>) UploadFiles(string remoteDirectory, DirectoryInfo di, FileInfo[] files)
파일을 업로드

#### public void DeleteFiles(string remoteDirectory)
FTP 폴더의 모든 하위 폴더와 파일을 재귀적으로 삭제

#### private (FtpLocalExists, FtpLocalExists?) GetLocalExists(string localFullPath, FtpLocalExists? localExistsAll)
다운로드할 파일이 로컬에 존재한다면 무엇을 할지 선택하고 그 결과를 localExistsAll에 저장
(localExistsAll에 값이 있다면 모든 파일에 대해 동일한 작업을 수행)

#### private (FtpRemoteExists, FtpRemoteExists?) GetRemoteExists(string remoteFullPath, FtpRemoteExists? remoteExistsAll)
업로드할 파일이 원격지에 존재한다면 무엇을 할지 선택하고 그 결과를 remoteExistsAll에 저장
(remoteExistsAll에 값이 있다면 모든 파일에 대해 동일한 작업을 수행)

#### public void Start()
파일 업로드가 시작되었으므로 UI를 변경하고 _stop을 false로 설정

#### public void Stop()
파일 업로드가 중지되었으므로 UI를 변경하고 _stop을 true로 설정

#### public string GetDataAplcNo(out string msg)
파일 반출에 사용될 키인 데이터신청번호를 반환

# LoginBox.cs

### partial class LoginBox : Form
로그인 박스

#### private void LoginBox_Load(object sender, EventArgs e)
로그인 박스가 표시될 때 설정을 불러오고 포커스를 이동

#### private void LoginBox_MouseClick(object sender, MouseEventArgs e)
우클릭 시에 어플리케이션의 정보를 표시

#### private void chkUseLocalhost_CheckedChanged(object sender, EventArgs e)
API 호출 주소를 localhost을 이용할 지 여부 지정

#### private async void btnOk_Click(object sender, EventArgs e)
로그인 시도하고 성공하면 DialogResult.OK를 반환

#### private void btnCancel_Click(object sender, EventArgs e)
로그인 취소

#### private void tmrFocusPassword_Tick(object sender, EventArgs e)
폼이 로드된 후 비밀번호 텍스트박스에 포커스를 이동하기 위해 타이머를 사용

#### private bool IsValid(out string id, out string password, out bool useLocalhost, out string msg)
로그인 전에 유효성 검사 (Debug 모드일 때만 자동으로 비밀번호 지정)

#### private void RestoreSettings()
설정 불러와서 적용

#### private void SaveSettings()
설정 저장

# MainForm.cs

### public partial class MainForm : Form
메인 폼

#### private CUiClient uiClient;
로컬

#### private CUiServer uiServerPrti;
참여기관전송

#### private CUiServer uiServerCarry;
반입

#### private void MainForm_Load(object sender, EventArgs e)
폼이 로드될 때 필요한 초기 설정을 실행

#### private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
폼이 종료되기 전 설정을 저장

#### private void btnStop_Click(object sender, EventArgs e)
파일의 다운로드/업로드를 중지

#### private async void btnRequestFile_Click(object sender, EventArgs e)
반출 신청

#### private void btnExit_Click(object sender, EventArgs e)
종료 단추

#### private void mnuPopup_Opening(object sender, CancelEventArgs e)
팝업메뉴를 열기 전 이벤트 (팝업메뉴의 하위 메뉴의 Visible 속성을 설정)

#### private void tsmiDownload_Click(object sender, EventArgs e)
팝업메뉴에서 다운로드를 선택하면 선택한 파일 또는 폴더 안의 파일을 다운로드

#### private void tsmiOpen_Click(object sender, EventArgs e)
팝업메뉴에서 열기를 선택하면 선택된 파일 또는 폴더를 탐색기에서 열기

#### private bool IsValid(out DirectoryInfo di, out string msg)
반출 신청 전 유효성 검사

#### private FileInfo[] GetFilesToRequest(DirectoryInfo di)
반출 신청 대상 파일을 리턴함 (설정에 따라 압축파일이나 전체 파일로 구분)

#### private void DeleteZipFileInDefaultDirectory(DirectoryInfo di)
반출 신청용 기본 압축 파일을 삭제

#### private bool SetVisibleBySource(Control? ctl)
팝업 메뉴의 하위 메뉴를 현재 선택한 컨트롤에 따라 보이거나 숨김

#### private void RestoreSettings()
UI 변경 정보를 XML 파일에서 읽어와 적용

#### private void SaveSettings()
UI 변경 정보를 XML 파일에 저장

# Program.cs

#### [STAThread]
The main entry point for the application.

# AplyRequest.cs

### internal class AplyRequest
파일 반출 요청 파라미터

# AplyResponse.cs

### internal class AplyResponse: BaseResponse
파일 반출 결과 데이터

# BaseResponse.cs

### public class BaseResponse
로그인, 파일 반출에 공통으로 사용되는 결과 데이터

# LoginRequest.cs

### internal class LoginRequest
로그인 요청 파라미터

# LoginResponse.cs

#### Raw,
경로의 원본 사용

#### TrimBaseFolder,
경로의 앞에서 nasBaseFolder를 제거

#### PrefixBaseFolder,
경로의 앞에 nasBaseFolder를 붙임

### public class LoginResult
로그인 결과 데이터

#### public bool HasAuthCode(string AuthCode)
AuthCode가 <see cref="applyResult"/>에 있는지 여부를 반환

#### public string GetUrlDirectory(UrlDirectoryTypes type, string url)
type에 의해 url을 가공하여 반환

#### public List<Data> GetUserValidDatas(bool forCarry)
참여기관전송, 반입 여부에 따라 <see cref="applyResult"/>에서 <see cref="Data"/>를 반환

#### public List<string> GetUserRootDirs(bool forCarry)
참여기관전송, 반입 여부에 따라 FTP의 모든 루트 경로를 반환

#### public List<string> GetDataAplcNos(bool forCarry)
참여기관전송, 반입 여부에 따라 데이터신청번호를 반환

#### public string GetAplyUrl(string dataAplcNo, string loginId)
반출 신청 시 파일이 업로드될 경로를 반환

### public class ApplyResult
로그인 성공 후 가져올 Model

### public class Data
로그인 성공 후 가져올 하위 Model

# XmlDocToMd.cs

### public class XmlDocToMd
XML로 된 주석을 Markdown으로 변환

#### public XmlDocToMd(String rootFolder) {
객체를 생성할 때 .cs를 포함하는 폴더를 지정

#### public string Convert()
.cs 파일의 주석과 1번째 줄의 코드를 읽어 Markdown을 변환한 문자열을 반환