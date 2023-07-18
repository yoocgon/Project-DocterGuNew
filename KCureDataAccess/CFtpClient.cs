using FluentFTP;
using KCureVDIDataBox;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCureDataAccess
{
    /// <summary>
    /// FTP용 라이브러리인 FluentFTP와 SFTP용 라이브러리인 Renci.SshNet을 동시에 사용하기 위한 래퍼 클래스
    /// </summary>
    public class CFtpClient
    {
        /// <summary>
        /// FluentFTP용 FTP 클라이언트
        /// </summary>
        private FtpClient? ftpClient;
        /// <summary>
        /// Renci.SshNet용 SFTP 클라이언트
        /// </summary>
        private SftpClient? sftpClient;

        private const string MSG_SFTP_NOT_EXISTS = "SFtp client not exists.";
        private const string MSG_BOTH_NOT_EXISTS = "Both Ftp and Sftp client not exists.";

        /// <summary>
        /// 클래스 초기화 (pass 값이 없다면 SFTP로 인정)
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="port"></param>
        /// <param name="encoding"></param>
        public CFtpClient(string host, string user, string pass, int port, Encoding encoding)
        {
            if (!string.IsNullOrEmpty(CCommon.LoginResult.nasPw))
            {
                var client = new FtpClient(host, user, pass, port);
                client.Encoding = encoding;
                client.Connect();
                this.ftpClient = client;
            }
            else
            {
                var pkFullPath = GetPrivateKeyFullPath();
                Log("pkFullPath", pkFullPath);

                var connectionInfo = new ConnectionInfo(host, port, user,
                                            new PrivateKeyAuthenticationMethod(
                                                user,
                                                new PrivateKeyFile(pkFullPath)
                                            ));

                var client = new SftpClient(connectionInfo);
                client.Connect();
                this.sftpClient = client;
            }
        }

        /// <summary>
        /// 폴더가 존재하는 지 여부를 반환 (SshNet은 해당 기능이 존재하지 않으므로 에러가 발생하는 지 여부로 알아냄)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool DirectoryExists(string path)
        {
            if (ftpClient != null)
            {
                return ftpClient.DirectoryExists(path);
            }
            else if (sftpClient != null)
            {
                bool dirExists = false;
                try
                {
                    string dirWorking = sftpClient.WorkingDirectory;
                    if (dirWorking == path)
                    {
                        dirExists = true;
                    } else
                    {
                        sftpClient.ChangeDirectory(path);
                        dirExists = true;
                        sftpClient.ChangeDirectory(dirWorking);
                    }
                }
                catch (SftpPathNotFoundException) { }

                return dirExists;
            } 
            else
            {
                throw new Exception(MSG_BOTH_NOT_EXISTS);
            }
        }

        /// <summary>
        /// 파일이 존재하는 지 여부를 반환 (SshNet은 폴더와 파일의 구분이 없으므로 DirectoryExists를 추가로 호출함)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool FileExists(string path)
        {
            if (ftpClient != null)
            {
                return ftpClient.FileExists(path);
            }
            else if (sftpClient != null)
            {
                bool exists = sftpClient.Exists(path);
                if (!exists) return false;

                return !this.DirectoryExists(path);
            }
            else
            {
                throw new Exception(MSG_BOTH_NOT_EXISTS);
            }
        }

        /// <summary>
        /// 폴더와 하위 폴더를 생성 (SshNet은 해당 기능이 존재하지 않으므로 재귀적으로 폴더를 생성함)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool CreateDirectory(string path)
        {
            if (ftpClient != null)
            {
                return ftpClient.CreateDirectory(path);
            }
            else if (sftpClient != null)
            {
                CreateDirectoryRecursively(path);
                return true;
            }
            else
            {
                throw new Exception(MSG_BOTH_NOT_EXISTS);
            }
        }

        /// <summary>
        /// 폴더와 하위 폴더를 삭제 (SshNet은 해당 기능이 존재하지 않으므로 재귀적으로 폴더를 삭제함)
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="Exception"></exception>
        public void DeleteDirectory(string path)
        {
            if (ftpClient != null)
            {
                ftpClient.DeleteDirectory(path);
            }
            else if (sftpClient != null)
            {
                if (DirectoryExists(path))
                {
                    DeleteDirectoryRecursively(path);
                }
            }
            else
            {
                throw new Exception(MSG_BOTH_NOT_EXISTS);
            }
        }
        
        /// <summary>
        /// 파일 삭제
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="Exception"></exception>
        public void DeleteFile(string path)
        {
            if (ftpClient != null)
            {
                ftpClient.DeleteFile(path);
            }
            else if (sftpClient != null)
            {
                sftpClient.DeleteFile(path);
            }
            else
            {
                throw new Exception(MSG_BOTH_NOT_EXISTS);
            }
        }

        /// <summary>
        /// 현재 경로를 설정
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="Exception"></exception>
        public void SetWorkingDirectory(string path)
        {
            if (ftpClient != null)
            {
                ftpClient.SetWorkingDirectory(path);
            }
            else if (sftpClient != null)
            {
                sftpClient.ChangeDirectory(path);
            }
            else
            {
                throw new Exception(MSG_BOTH_NOT_EXISTS);
            }
        }

        /// <summary>
        /// <paramref name="path"/>의 하위 폴더와 파일 목록 전체를 반환 (SshNet은 해당 기능이 존재하지 않으므로 재귀적으로 폴더와 파일 목록을 가져옴)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public FtpListItem[] GetListing(string path, FtpListOption option)
        {
            Log("parent path", path);

            if (ftpClient != null)
            {
                return ftpClient.GetListing(path, option);
            }
            else if (sftpClient != null)
            {
                List<FtpListItem> listItems = new List<FtpListItem>();

                if (option == FtpListOption.Auto || option == FtpListOption.AllFiles)
                {
                    foreach (var item in sftpClient.ListDirectory(path))
                    {
                        FtpListItem? listItem = GetFtpListItem(item);
                        if (listItem == null) continue;

                        Log("file", listItem.ToString());
                        listItems.Add(listItem);
                    }
                }
                else if (option == FtpListOption.Recursive)
                {
                    Stack<IEnumerable<SftpFile>> stacks = new Stack<IEnumerable<SftpFile>>();
                    stacks.Push(sftpClient.ListDirectory(path));
                    while (stacks.Count > 0)
                    {
                        IEnumerable<SftpFile> listCur = stacks.Pop();
                        foreach (SftpFile item in listCur)
                        {
                            FtpListItem? listItem = GetFtpListItem(item);
                            if (listItem == null) continue;

                            Log("file", listItem.ToString());
                            listItems.Add(listItem);

                            if (item.IsDirectory)
                            {
                                stacks.Push(sftpClient.ListDirectory(item.FullName));
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception($"sftpClient does not support {option}");
                }

                return listItems.ToArray();
            }
            else
            {
                throw new Exception(MSG_BOTH_NOT_EXISTS);
            }
        }
        /// <summary>
        /// SshNet인 경우 .과 ..을 제외한 폴더와 파일을 FtpListItem 형식으로 변환해서 반환
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private FtpListItem? GetFtpListItem(SftpFile item)
        {
            if (item.Name == "." || item.Name == "..") return null;

            if (item.IsDirectory)
            {
                FtpObjectType objectType = FtpObjectType.Directory;
                FtpListItem listItem = new FtpListItem(item.Name, item.Length, objectType, item.LastWriteTime);
                listItem.FullName = item.FullName;
                return listItem;
            }
            else
            {
                FtpObjectType objectType = item.IsSymbolicLink ? FtpObjectType.Link : FtpObjectType.File;
                FtpListItem listItem = new FtpListItem(item.Name, item.Length, objectType, item.LastWriteTime);
                listItem.FullName = item.FullName;
                return listItem;
            }
        }

        /// <summary>
        /// 파일 다운로드 (SshNet은 다운로드할 폴더가 없으면 자동 생성하는 기능, 파일이 이미 존재하면 어떤 선택을 할 지의 기능이 존재하지 않으므로 직접 구현)
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="remotePath"></param>
        /// <param name="localExists"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public FtpStatus DownloadFile(string localPath, string remotePath, FtpLocalExists localExists, Action<FtpProgress> progress)
        {
            if (ftpClient != null)
            {
                return ftpClient.DownloadFile(localPath, remotePath, localExists, FtpVerify.None, progress);
            }
            else if (sftpClient != null)
            {
                string dir = Path.GetDirectoryName(localPath) ?? "";
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                FileMode fileMode = FileMode.Create;
                if (localExists == FtpLocalExists.Overwrite)
                {
                    if (File.Exists(localPath))
                    {
                        File.Delete(localPath);
                    }
                }
                else if (localExists is FtpLocalExists.Resume)
                {
                    fileMode = FileMode.Append;
                }
                else if (localExists == FtpLocalExists.Skip)
                {
                    if (File.Exists(localPath))
                    {
                        return FtpStatus.Skipped;
                    }
                }
                using (Stream localStream = new FileStream(localPath, fileMode))
                {
                    try
                    {
                        Action<long> progressS = delegate (long TransferredBytes)
                        {
                            FtpProgress ftpProgress = new FtpProgress(0, 
                                TransferredBytes, 0, TimeSpan.Zero,
                                localPath, remotePath, new FtpProgress(1, 0)
                                );
                            progress(ftpProgress);
                        };
                        DownloadFileResumable(remotePath, localStream, progressS);
                        return FtpStatus.Success;
                    }
                    catch (Exception)
                    {
                        return FtpStatus.Failed;
                    }
                }
            }
            else
            {
                throw new Exception(MSG_BOTH_NOT_EXISTS);
            }
        }

        /// <summary>
        /// 파일 업로드 (SshNet은 파일이 이미 존재하면 어떤 선택을 할 지의 기능, 이어쓰기 기능이 존재하지 않으므로 직접 구현)
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="remotePath"></param>
        /// <param name="remoteExists"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public FtpStatus UploadFile(string localPath, string remotePath, FtpRemoteExists remoteExists, Action<FtpProgress> progress)
        {
            if (ftpClient != null)
            {
                return ftpClient.UploadFile(localPath, remotePath, remoteExists, false, FtpVerify.None, progress);
            }
            else if (sftpClient != null)
            {
                if (remoteExists == FtpRemoteExists.Overwrite)
                {
                    if (FileExists(remotePath))
                    {
                        sftpClient.DeleteFile(remotePath);
                    }
                }
                else if (remoteExists is FtpRemoteExists.Resume)
                {
                    // Do nothing
                }
                else if (remoteExists == FtpRemoteExists.Skip)
                {
                    if (FileExists(remotePath))
                    {
                        return FtpStatus.Skipped;
                    }
                }
                
                using (Stream localStream = new FileStream(localPath, FileMode.Open))
                {
                    try
                    {
                        Action<long> progressS = delegate (long TransferredBytes)
                        {
                            FtpProgress ftpProgress = new FtpProgress(0,
                                TransferredBytes, 0, TimeSpan.Zero,
                                localPath, remotePath, new FtpProgress(1, 0)
                                );
                            progress(ftpProgress);
                        };
                        UploadFileResumable(remotePath, localStream, progressS);
                        Log("ftp status", "success");
                        return FtpStatus.Success;
                    }
                    catch (Exception)
                    {
                        Log("ftp status", "failed");
                        return FtpStatus.Failed;
                    }
                }
            }
            else
            {
                Log("ftp status", "exception");
                throw new Exception(MSG_BOTH_NOT_EXISTS);
            }
        }

        /// <summary>
        /// SFTP에서 필요한 Private Key를 반환
        /// </summary>
        /// <returns></returns>
        private static string GetPrivateKeyFullPath()
        {
            string fullPathApp = Application.ExecutablePath;
            string dirApp = Path.GetDirectoryName(fullPathApp) ?? "";
            string pkFullPath = Path.Combine(dirApp, "kcure_svr.pem");
            if (File.Exists(pkFullPath))
            {
                return pkFullPath;
            }

            byte[] contents = KCureDataAccess.Properties.Resources.kcure_svr_pem;
            File.WriteAllBytes(pkFullPath, contents);
            return pkFullPath;
        }

        /// <summary>
        /// SshNet은 폴더를 재귀적으로 생성하는 기능이 없으므로 직접 구현
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="Exception"></exception>
        /// <remarks>
        /// https://stackoverflow.com/a/36565854/2958717
        /// </remarks>
        private void CreateDirectoryRecursively(string path)
        {
            if (sftpClient == null)
            {
                throw new Exception(MSG_SFTP_NOT_EXISTS);
            }

            string current = "";

            if (path[0] == '/')
            {
                path = path[1..];
            }

            while (!string.IsNullOrEmpty(path))
            {
                int p = path.IndexOf('/');
                current += '/';
                if (p >= 0)
                {
                    current += path[..p];
                    path = path[(p + 1)..];
                }
                else
                {
                    current += path;
                    path = "";
                }

                try
                {
                    SftpFileAttributes attrs = sftpClient.GetAttributes(current);
                    if (!attrs.IsDirectory)
                    {
                        throw new Exception("not directory");
                    }
                }
                catch (SftpPathNotFoundException)
                {
                    sftpClient.CreateDirectory(current);
                }
            }
        }

        /// <summary>
        /// SshNet은 폴더를 재귀적으로 삭제하는 기능이 없으므로 직접 구현
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="Exception"></exception>
        /// <remarks>
        /// https://stackoverflow.com/a/36568767
        /// </remarks>
        private void DeleteDirectoryRecursively(string path)
        {
            if (sftpClient == null)
            {
                throw new Exception(MSG_SFTP_NOT_EXISTS);
            }

            foreach (SftpFile file in sftpClient.ListDirectory(path))
            {
                if ((file.Name == ".") || (file.Name == "..")) continue;

                if (file.IsDirectory)
                {
                    DeleteDirectoryRecursively(file.FullName);
                }
                else
                {
                    sftpClient.DeleteFile(file.FullName);
                }
            }

            sftpClient.DeleteDirectory(path);
        }

        /// <summary>
        /// SshNet은 다운로드 시 이어쓰기 기능이 없으므로 직접 구현
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localStream"></param>
        /// <param name="progress"></param>
        /// <exception cref="Exception"></exception>
        private void DownloadFileResumable(string remotePath, Stream localStream, Action<long> progress)
        {
            if (sftpClient == null)
            {
                throw new Exception(MSG_SFTP_NOT_EXISTS);
            }

            using (SftpFileStream remoteStream = sftpClient.Open(remotePath, FileMode.Open))
            {
                byte[] buffer = new byte[409600];
                int bytesRead;

                if (localStream.Length > 0)
                {
                    remoteStream.Seek(localStream.Length, SeekOrigin.Begin);
                }

                bytesRead = 1;
                while (bytesRead > 0)
                {
                    bytesRead = remoteStream.Read(buffer, 0, 409600);
                    if (bytesRead > 0)
                    {
                        localStream.Write(buffer, 0, bytesRead);
                        progress(remoteStream.Position);
                    }
                }

                localStream.Close();
            }
        }

        /// <summary>
        /// SshNet은 다운로드 시 이어쓰기 기능이 없으므로 직접 구현
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localStream"></param>
        /// <param name="progress"></param>
        /// <exception cref="Exception"></exception>
        private void UploadFileResumable(string remotePath, Stream localStream, Action<long> progress)
        {
            if (sftpClient == null)
            {
                throw new Exception(MSG_SFTP_NOT_EXISTS);
            }

            Log("status", "remoteStream.Open");
            using (SftpFileStream remoteStream = sftpClient.Open(remotePath, FileMode.Append, FileAccess.Write))
            {
                byte[] buffer = new byte[409600];
                int bytesRead;

                if (remoteStream.Length > 0)
                {
                    localStream.Seek(remoteStream.Length, SeekOrigin.Begin);
                }

                bytesRead = 1;
                while (bytesRead > 0)
                {
                    bytesRead = localStream.Read(buffer, 0, 409600);
                    if (bytesRead > 0)
                    {
                        remoteStream.Write(buffer, 0, bytesRead);
                        progress(localStream.Position);
                    }
                }

                remoteStream.Close();
                Log("status", "remoteStream.Close");
            }
        }

        public void Log(string category, string log)
        {
            Console.WriteLine($"\nDEBUG>>> ({category}) \n{log}");
        }
    }
}
