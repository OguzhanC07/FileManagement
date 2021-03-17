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
            _fileRepoMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(()=>null);

            //Act
            var file = await _sut.GetById(fileId);

            //Assert
            Assert.Null(file);
        }
    }
}
