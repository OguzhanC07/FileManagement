using FileManagement.Business.Concrete;
using FileManagement.Business.Interfaces;
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
    public class FolderServiceTest
    {
        private readonly FolderManager _sut;
        private readonly Mock<IGenericDal<Folder>> _folderRepoMock = new Mock<IGenericDal<Folder>>();
        private readonly Mock<IFolderDal> _folderDalMock = new Mock<IFolderDal>();
        public FolderServiceTest()
        {
            _sut = new FolderManager(_folderDalMock.Object, _folderRepoMock.Object);
        }

        [Fact]
        public async Task GetFolderById_ShouldReturnFolder_WhenFolderExist()
        {
            var random = new Random();
            int folderId = random.Next(1, 100);
            var folderName = "test filename";

            var mockFolder = new Folder
            {
                Id = folderId,
                FolderName = folderName,
                FileGuid = Guid.NewGuid(),
                Size = 500,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                AppUserId = 1,
            };

            _folderRepoMock.Setup(x => x.GetById(folderId)).ReturnsAsync(mockFolder);

            //Act
            var folder = await _sut.GetById(folderId);

            //Assert
            Assert.Equal(folderId, folder.Id);
            Assert.Equal(folderName, mockFolder.FolderName);
        }

        [Fact]
        public async Task GetFolderById_ShouldReturnNothing_WhenFolderDoesNotExits()
        {
            //Arrange
            var random = new Random();
            int folderId = random.Next(1, 100);
            _folderRepoMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var file = await _sut.GetById(folderId);

            //Assert
            Assert.Null(file);
        }

        [Fact]
        public async Task AddFolders_ShouldReturnNothing_WhenUserExist()
        {
            //Arrange
            var random = new Random();
            int folderId = random.Next(1, 100);
            var folderName = "test filename";

            var mockFolder = new Folder
            {
                Id = folderId,
                FolderName = folderName,
                FileGuid = Guid.NewGuid(),
                Size = 500,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                AppUserId = 1,
            };

            _folderRepoMock.Setup(x => x.AddAsync(It.IsAny<Folder>())).Verifiable();

            //Act
            await _sut.AddAsync(mockFolder);

            //Assert
            _folderRepoMock.Verify(x => x.AddAsync(It.IsAny<Folder>()), Times.Once());
        }

        [Fact]
        public async Task UpdateFolderById_ShouldReturnNothing_WhenFolderExists()
        {
            //Arrange
            var random = new Random();
            int folderId = random.Next(1, 100);
            var folderName = "test filename";

            var mockFolder = new Folder
            {
                Id = folderId,
                FolderName = folderName,
                FileGuid = Guid.NewGuid(),
                Size = 500,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                AppUserId = 1,
            };
            _folderRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Folder>())).Verifiable();
            _folderRepoMock.Setup(x => x.AddAsync(It.IsAny<Folder>())).Verifiable();

            //Act
            await _sut.AddAsync(mockFolder);
            mockFolder.FolderName = "test foldername2";
            await _sut.UpdateAsync(mockFolder);

            //Arrange
            _folderRepoMock.Verify(x => x.AddAsync(It.IsAny<Folder>()), Times.Once());
            _folderRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Folder>()), Times.Once());
        }

        [Fact]
        public async Task DeleteFolderById_ShouldReturnNothing_WhenFolderExists()
        {
            //Arrange
            var random = new Random();
            int folderId = random.Next(1, 100);
            var folderName = "test filename";

            var mockFolder = new Folder
            {
                Id = folderId,
                FolderName = folderName,
                FileGuid = Guid.NewGuid(),
                Size = 500,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                AppUserId = 1,
            };
            _folderRepoMock.Setup(x => x.RemoveAsync(It.IsAny<Folder>())).Verifiable();
            _folderRepoMock.Setup(x => x.AddAsync(It.IsAny<Folder>())).Verifiable();

            //Act
            await _sut.AddAsync(mockFolder);
            await _sut.RemoveAsync(mockFolder);

            //Arrange
            _folderRepoMock.Verify(x => x.AddAsync(It.IsAny<Folder>()), Times.Once());
            _folderRepoMock.Verify(x => x.RemoveAsync(It.IsAny<Folder>()), Times.Once());
        }

        [Fact]
        public async Task GetAllSubFoldersWithFolderId_ShouldReturnListOfFolders_WhenFolderExists()
        {
            List<Folder> mockData = new List<Folder>{
                new Folder
                {
                    Id = 1,
                    FolderName = "test",
                    CreatedAt = DateTime.Now,
                    Size = 500,
                    FileGuid = Guid.NewGuid(),
                    AppUserId = 1,
                    IsDeleted = false,
                    ParentFolderId = null,
                    InverseParentFolder = new List<Folder>
                    {
                        new Folder{Id=6,FolderName="InverseSub",AppUserId=1,FileGuid=Guid.NewGuid(),CreatedAt=DateTime.Now.AddMinutes(1),ParentFolderId=1,IsDeleted=false,Size=800}
                    }
                }
            };
            //Arrange
            _folderDalMock.Setup(x => x.GetAllSubFolders(It.IsAny<int>())).ReturnsAsync(mockData);
            var expected = mockData.Count;
            //Act
            var actual = await _sut.GetAllSubFolders(1);
            //Assert
            Assert.Equal(expected, actual.Count);
        }


        [Fact]
        public async Task GetFoldersByUserId_ShouldReturnListOfFolders_WhenUserExist()
        {
            //Arrange
            Mock<IFolderService> folderServiceMock = new Mock<IFolderService>();
            var expected = GetSampleFolder();
            folderServiceMock.Setup(x => x.GetFoldersByUserId(It.IsAny<int>())).ReturnsAsync(expected);
            _folderRepoMock.Setup(x => x.GetAllByFilter(I => I.AppUserId == It.IsAny<int>() && I.IsDeleted == false && I.ParentFolderId == null)).ReturnsAsync(expected);


            //Act
            //throws exception for what ? 
            //this returns null beacuse _sut and _sut don't accept this setup above
            //how can i test this method ? 
            var actual = await _sut.GetFoldersByUserId(1); /* _sut.GetFoldersByUserId(1)*/

            //Assert 
            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].FolderName, actual[i].FolderName);
                Assert.Equal(expected[i].Size, actual[i].Size);
            }
        }

        [Fact]
        public async Task GetSubFoldersByFolderId_ShouldReturnListOfFolders_WhenFolderExists()
        {
            //Arrange
            Mock<IFolderService> folderServiceMock = new Mock<IFolderService>();
            var expected = GetSampleFolder();
            folderServiceMock.Setup(x => x.GetSubFoldersByFolderId(It.IsAny<int>())).ReturnsAsync(expected);

            //Act
            var actual = await folderServiceMock.Object.GetSubFoldersByFolderId(1);

            //Assert
            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].FolderName, actual[i].FolderName);
            }
        }

        [Fact]
        public async Task FindFolderById_ShouldReturnFolder_WhenFolderExists()
        {
            //Arrange
            Mock<IFolderService> folderServiceMock = new Mock<IFolderService>();
            Folder mockFolder = new Folder { Id = 1, FolderName = "test", CreatedAt = DateTime.Now, Size = 500, FileGuid = Guid.NewGuid(), AppUserId = 1, IsDeleted = false, ParentFolderId = null };
            folderServiceMock.Setup(x => x.FindFolderById(It.IsAny<int>())).ReturnsAsync(mockFolder);
            
            //Act
            var actual = await _sut.FindFolderById(1);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(mockFolder, actual);
        }

        private List<Folder> GetSampleFolder()
        {
            List<Folder> output = new List<Folder>
            {
                new Folder
                {
                    Id=1,
                    FolderName="test",
                    CreatedAt=DateTime.Now,
                    Size=500,
                    FileGuid=Guid.NewGuid(),
                    AppUserId=1,
                    IsDeleted=false,
                    ParentFolderId=null,
                    InverseParentFolder= new List<Folder>
                    {
                        new Folder{Id=6,FolderName="InverseSub",AppUserId=1,FileGuid=Guid.NewGuid(),CreatedAt=DateTime.Now.AddMinutes(1),ParentFolderId=1,IsDeleted=false,Size=800}
                    }
                },
                new Folder
                {
                    Id=2,
                    FolderName="test2",
                    CreatedAt=DateTime.Now.AddSeconds(1),
                    Size=600,
                    FileGuid=Guid.NewGuid(),
                    AppUserId=1,
                    IsDeleted=false,
                    ParentFolderId=1
                },
                new Folder
                {
                    Id=3,
                    FolderName="test3",
                    CreatedAt=DateTime.Now.AddSeconds(2),
                    Size=700,
                    FileGuid=Guid.NewGuid(),
                    AppUserId=1,
                    IsDeleted=false,
                    ParentFolderId=null
                },
                new Folder
                {
                    Id=4,
                    FolderName="test4",
                    CreatedAt=DateTime.Now,
                    Size=800,
                    FileGuid=Guid.NewGuid(),
                    AppUserId=1,
                    IsDeleted=false,
                    ParentFolderId=null
                },
            };

            return output;
        }

    }
}
