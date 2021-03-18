using FileManagement.Business.Concrete;
using FileManagement.DataAccess;
using FileManagement.DataAccess.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileManagement.Test
{
    public class FileServiceTest
    {
        private readonly FileManager _sut;
        private readonly Mock<IGenericDal<File>> _fileRepoMock = new Mock<IGenericDal<File>>();

        public FileServiceTest()
        {
            _sut = new FileManager(_fileRepoMock.Object);
        }

        [Fact]
        public async Task GetFileById_ShouldReturnFile_WhenFileExists()
        {
            //Arrange
            var random = new Random();
            int fileId = random.Next(1, 100);
            var fileName = "test filename";
            var mockFile = new File
            {
                Id = fileId,
                FileName = fileName,
                FileGuid = Guid.NewGuid().ToString(),
                Size = 500,
                UploadedAt = DateTime.Now,
                IsActive = true
            };
            _fileRepoMock.Setup(x => x.GetById(fileId)).ReturnsAsync(mockFile);

            //Act
            var file = await _sut.GetById(fileId);

            //Assert
            Assert.Equal(fileId, file.Id);
            Assert.Equal(fileName, mockFile.FileName);
        }

        [Fact]
        public async Task GetFileById_ShouldReturnNothing_WhenFileDoesNotExists()
        {
            //Arrange
            var random = new Random();
            int fileId = random.Next(1, 100);
            _fileRepoMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var file = await _sut.GetById(fileId);

            //Assert
            Assert.Null(file);
        }


        [Fact]
        public async Task AddFile_ShouldReturnNothing_WhenFolderExist()
        {
            //Arrange
            var random = new Random();
            int fileId = random.Next(1, 100);
            var fileName = "test filename";
            var mockFile = new File
            {
                Id = fileId,
                FileName = fileName,
                FileGuid = Guid.NewGuid().ToString(),
                Size = 500,
                UploadedAt = DateTime.Now,
                IsActive = true,
                FolderId = random.Next(1, 100)
            };

            var mockFileList = new List<File>();
            mockFileList.Add(mockFile);

            _fileRepoMock.Setup(x => x.AddAsync(It.IsAny<File>())).Verifiable();
            _fileRepoMock.Setup(x => x.GetAll()).ReturnsAsync(mockFileList);

            //Act
            await _sut.AddAsync(mockFile);
            var actual = await _sut.GetAll();
            //Assert
            Assert.Single(actual);
            _fileRepoMock.Verify(mock => mock.AddAsync(It.IsAny<File>()), Times.Once());
        }

        [Fact]
        public async Task UpdateFile_ShouldReturnNothing_WhenFileExists()
        {
            //Arrange
            var random = new Random();
            int fileId = random.Next(1, 100);
            var fileName = "test filename";
            var mockFile = new File
            {
                Id = fileId,
                FileName = fileName,
                FileGuid = Guid.NewGuid().ToString(),
                Size = 500,
                UploadedAt = DateTime.Now,
                IsActive = true,
                FolderId = random.Next(1, 100)
            };
            _fileRepoMock.Setup(x => x.AddAsync(It.IsAny<File>())).Verifiable();
            _fileRepoMock.Setup(x => x.UpdateAsync(It.IsAny<File>())).Verifiable();

            //Act
            await _sut.AddAsync(mockFile);
            mockFile.FileName = "testfilename2";
            await _sut.UpdateAsync(mockFile);

            //Assert
            _fileRepoMock.Verify(x => x.AddAsync(It.IsAny<File>()), Times.Once());
            _fileRepoMock.Verify(x => x.UpdateAsync(It.IsAny<File>()), Times.Once());
        }

        [Fact]
        public async Task DeleteFileByFileId_ShouldReturnNothing_IfFileExists()
        {
            //Arrange
            var random = new Random();
            int fileId = random.Next(1, 100);
            var fileName = "test filename";
            var mockFile = new File
            {
                Id = fileId,
                FileName = fileName,
                FileGuid = Guid.NewGuid().ToString(),
                Size = 500,
                UploadedAt = DateTime.Now,
                IsActive = true,
                FolderId = random.Next(1, 100)
            };
            _fileRepoMock.Setup(x => x.AddAsync(It.IsAny<File>())).Verifiable();
            _fileRepoMock.Setup(x => x.RemoveAsync(It.IsAny<File>())).Verifiable();

            //Act
            await _sut.AddAsync(mockFile);
            await _sut.RemoveAsync(mockFile);

            //Assert 
            _fileRepoMock.Verify(x => x.RemoveAsync(It.IsAny<File>()), Times.Once());
        }

        //[Fact]
        //public async Task GetFilesByFolderId_ShouldReturnListOfFiles_IfFolderExists()
        //{
        //    //Arrange
        //    //_fileRepoMock.Setup(x=>x.GetAllByFilter(x=>x.FolderId==1))
        //    //Act

        //    //Assert
        //}

        //private List<File> GenerateFakeFolders()
        //{
        //    return new List<File>
        //    {
        //        new File{Id=1,FileName="test name 1",FileGuid = Guid.NewGuid().ToString(), Size = 500 , UploadedAt = DateTime.Now, IsActive =true,FolderId=1 },
        //        new File{Id=1,FileName="test name 2",FileGuid = Guid.NewGuid().ToString(), Size = 600 , UploadedAt = DateTime.Now.AddSeconds(1), IsActive =true,FolderId=1 },
        //        new File{Id=1,FileName="test name 3",FileGuid = Guid.NewGuid().ToString(), Size = 700 , UploadedAt = DateTime.Now.AddSeconds(2), IsActive =true,FolderId=1 },
        //        new File{Id=1,FileName="test name 4",FileGuid = Guid.NewGuid().ToString(), Size = 800 , UploadedAt = DateTime.Now.AddSeconds(3), IsActive =true,FolderId=1 },
        //    };
        //}
    }
}
